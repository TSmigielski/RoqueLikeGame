using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RoomInfo
{
	public RoomInfo(string _name, Vector2 _coords)
	{
		name = _name;
		coordinates = _coords;
	}
	public string name;
	public Vector2 coordinates;
}

public class RoomController : MonoBehaviour
{
	public static RoomController Instance { get; private set; }

	public const float roomX = 36f, roomY = 20f;

	private string currentWorldName = "Level 0";
	private RoomInfo currentLoadRoomData;
	private Queue<RoomInfo> loadRoomQueue = new Queue<RoomInfo>();
	public static List<Room> loadedRooms = new List<Room>();
	private bool isLoadingRoom = false, isModifyingRoom = false;
	public static List<Room> rooms2Destroy = new List<Room>();

	public static bool AllRoomsReady { get; private set; }


	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Update()
	{
		if (!AllRoomsReady && !isModifyingRoom)
			StartCoroutine(ModifyRooms());

		if (isLoadingRoom || loadRoomQueue.Count == 0)
		{
			return;
		}

		isLoadingRoom = true;
		currentLoadRoomData = loadRoomQueue.Dequeue();
		StartCoroutine(LoadRoomRoutine(currentLoadRoomData));
	}

	private IEnumerator ModifyRooms()
	{
		if (loadedRooms.Count == LevelGenerator.spawnedRoomCount)
		{
			isModifyingRoom = true;
			yield return new WaitForSeconds(.5f);

			if (loadedRooms.Count != LevelGenerator.spawnedRoomCount)
				yield break;

			AllRoomsReady = true;

			for (int i = loadedRooms.Count - 1; i > 0; i--)
			{
				loadedRooms[i].ModifyRoom();
			}

			yield return new WaitForSeconds(1f);

			foreach (var rm in rooms2Destroy)
			{
				loadedRooms.Remove(rm);
				SceneManager.UnloadSceneAsync(rm.initialScene);
				Destroy(rm.gameObject);
			}

			foreach (var rm in loadedRooms)
			{
				rm.RemoveUnconnectedDoors();
			}
		}
		isModifyingRoom = false;
	}

	public void LoadRoom(string _name, Vector2 _coordinates)
	{
		if (Check4Room(_coordinates * new Vector2(roomX, roomY)))
			return;

		loadRoomQueue.Enqueue(new RoomInfo(_name, _coordinates));
	}

	private IEnumerator LoadRoomRoutine(RoomInfo _info)
	{
		string roomName = $"{currentWorldName}_{_info.name}";

		AsyncOperation loadRoom = SceneManager.LoadSceneAsync(roomName, LoadSceneMode.Additive);

		while(!loadRoom.isDone)
		{
			yield return null;
		}
	}

	public void RegisterRoom(Room _room)
	{
		if (Check4Room(currentLoadRoomData.coordinates * new Vector2(roomX, roomY)))
		{
			SceneManager.UnloadSceneAsync(_room.gameObject.scene);
			isLoadingRoom = false;
			return;
		}

		_room.transform.position = new Vector3(currentLoadRoomData.coordinates.x * roomX, currentLoadRoomData.coordinates.y * roomY, 0);
		_room.Coordinates = currentLoadRoomData.coordinates;
		_room.name = $"{currentWorldName}-{currentLoadRoomData.name} ({_room.Coordinates.x},{_room.Coordinates.y})";
		_room.transform.SetParent(transform);

		loadedRooms.Add(_room);

		isLoadingRoom = false;
	}

	public static bool Check4Room(Vector3 _position)
	{
		foreach (var cl in Physics2D.OverlapPointAll(_position))
		{
			if (cl.CompareTag("Room") && loadedRooms.Contains(cl.GetComponent<Room>()))
			{
				return true;
			}
		}

		return false;
	}

	public static Room GetRoom(Vector3 _position)
	{
		foreach (var cl in Physics2D.OverlapPointAll(_position))
		{
			if (cl.CompareTag("Room") && loadedRooms.Contains(cl.GetComponent<Room>()))
			{
				return cl.GetComponent<Room>();
			}
		}

		return null;
	}

	public void OnPlayerEnterRoom(Room _room)
	{
		if (!AllRoomsReady && _room.Room_Type != RoomType.Start)
		{
			return;
		}
		PlayerController.CurrentRoom = _room;
		MinimapController.Instance.UpdateMinimap();

		if (!PlayerController.VisitedRooms.Contains(_room))
		{
			PlayerController.VisitedRooms.Add(_room);
		}
	}
}
