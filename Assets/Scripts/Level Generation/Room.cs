using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RoomType { Start, Empty, Multi, Destroyed }

public class Room : MonoBehaviour
{
	[SerializeField] private RoomType _roomType;
	public RoomType Room_Type
	{
		get { return _roomType; }
		set { _roomType = value; }
	}

	[SerializeField] private Vector2 _dimension;
	public Vector2 Dimension
	{
		get { return _dimension; }
		set { _dimension = value; }
	}

	[SerializeField] private Vector2 _coordinates;
	public Vector2 Coordinates
	{
		get { return _coordinates; }
		set { _coordinates = value; }
	}

	public List<Door> doors = new List<Door>();
	public List<Wall> walls = new List<Wall>();
	public List<Room> multiRoom = new List<Room>();

	[HideInInspector] public Scene initialScene;

	private void OnDrawGizmos/*Selected*/()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireCube(transform.position, new Vector3(Dimension.x, Dimension.y, 0));
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			RoomController.Instance.OnPlayerEnterRoom(this);
		}
	}

	private void Start()
	{
		initialScene = gameObject.scene;

		if (RoomController.Instance == null)
		{
			Debug.LogWarning("This scene contains this room only, it's likely you played the wrong scene.");
			return;
		}

		if (Room_Type == RoomType.Start)
		{
			CameraController.Instance.MyRoom = this;
		}

		foreach (var d in GetComponentsInChildren<Door>())
		{
			doors.Add(d);
		}
		foreach (var w in GetComponentsInChildren<Wall>())
		{
			walls.Add(w);
		}

		RoomController.Instance.RegisterRoom(this);
	}

	public void ModifyRoom()
	{
		multiRoom = MultiRoomCheck();

		if (multiRoom == null)
		{
			return;
		}

		if (multiRoom.Count == 4 && Random.Range(0f, 100f) < LevelGenerator.Instance.levelGenerationData.quadrupleRoomChance)
		{
			BuildMultiRoom();
		}
		else if (multiRoom.Count == 2 && Random.Range(0f, 100f) < LevelGenerator.Instance.levelGenerationData.doubleRoomChance)
		{
			BuildMultiRoom();
		}
		else if (multiRoom.Count != 2 && multiRoom.Count != 4)
		{
			Debug.LogError($"{multiRoom.Count} rooms is a wrong amount for a multi room! - {gameObject.name}");
			return;
		}
	}

	private void BuildMultiRoom()
	{
		LevelGenerator.spawnedRoomCount -= (multiRoom.Count - 1);

		List<Vector2> coords = new List<Vector2>();

		foreach (var rm in multiRoom)
		{
			coords.Add(rm.Coordinates);
			RoomController.rooms2Destroy.Add(rm);
			rm.Room_Type = RoomType.Destroyed;
		}

		if (coords.Count == 2)
		{
			if (Mathf.Abs(coords[0].x - coords[1].x) == 0f)
			{
				RoomController.Instance.LoadRoom("Empty 1x2", new Vector2(coords[0].x, (coords[0].y + coords[1].y) / 2));
			}
			else
			{
				RoomController.Instance.LoadRoom("Empty 2x1", new Vector2((coords[0].x + coords[1].x) / 2, coords[0].y));
			}
		}
		else if (coords.Count == 4)
		{
			var a = (coords[0] + coords[1]) / 2;
			var b = (coords[2] + coords[3]) / 2;
			var c = (a + b) / 2;

			RoomController.Instance.LoadRoom("Empty 2x2", c);
		}
	}

	private List<Room> MultiRoomCheck()
	{
		if (Room_Type != RoomType.Empty || doors.Count < 2)
			return null;

		List<Room> possibleCombo = new List<Room>();

		Vector2 rightUp = new Vector2(1, 1);
		Vector2 rightDown = new Vector2(1, -1);
		Vector2 leftDown = new Vector2(-1, -1);
		Vector2 leftUp = new Vector2(-1, 1);

		List<List<Vector2>> tests = new List<List<Vector2>>()
		{
			new List<Vector2>() { rightUp, Vector2.right, Vector2.up },
			new List<Vector2>() { rightDown, Vector2.right, Vector2.down },
			new List<Vector2>() { leftDown, Vector2.left, Vector2.down },
			new List<Vector2>() { leftUp, Vector2.left, Vector2.up },
		};

		bool doubleFirst = Random.Range(0f, 100f) > 50f ? true : false;
		if (doubleFirst)
		{
			goto Double;
		}

		Quadruple:
		foreach (var test in tests)
		{
			bool fail = false;
			possibleCombo.Add(this);
			foreach (var testValue in test)
			{
				Room rm = GetOffsetRoom(testValue * new Vector2(RoomController.roomX, RoomController.roomY));
				possibleCombo.Add(rm);
				if (rm == null || rm.Room_Type != RoomType.Empty)
				{
					possibleCombo.Clear();
					fail = true;
					break;
				}
			}

			if (!fail)
			{
				return possibleCombo;
			}

			if (doubleFirst && test == tests[tests.Count - 1])
			{
				return null;
			}
		}

		Double:
		Vector2[] doubleRoomTests =
		{
			Vector2.up,
			Vector2.down,
			Vector2.left,
			Vector2.right
		};
		
		foreach (var test in doubleRoomTests)
		{
			possibleCombo.Add(this);
			Room rm = GetOffsetRoom(test * new Vector2(RoomController.roomX, RoomController.roomY));
			possibleCombo.Add(rm);
			if (rm == null || rm.Room_Type != RoomType.Empty)
			{
				possibleCombo.Clear();
				continue;
			}
			return possibleCombo;
		}

		if (doubleFirst)
		{
			goto Quadruple;
		}

		return null;
	}

	public void RemoveUnconnectedDoors()
	{
		var doors2Remove = new List<Door>();

		foreach (var d in doors)
		{
			switch (d.doorSide)
			{
				case DoorSide.Top:
					if (!RoomController.Check4Room(d.transform.position + new Vector3(0f, 5f, 0f)))
					{
						doors2Remove.Add(d);
						d.PatchHorizontalDoor();
					}
					break;

				case DoorSide.Bottom:
					if (!RoomController.Check4Room(d.transform.position + new Vector3(0f, -5f, 0f)))
					{
						doors2Remove.Add(d);
						d.PatchHorizontalDoor();
					}
					break;

				case DoorSide.Left:
					if (!RoomController.Check4Room(d.transform.position + new Vector3(-5f, 0f, 0f)))
					{
						doors2Remove.Add(d);
						d.PatchVerticalDoor();
					}
					break;

				case DoorSide.Right:
					if (!RoomController.Check4Room(d.transform.position + new Vector3(5f, 0f, 0f)))
					{
						doors2Remove.Add(d);
						d.PatchVerticalDoor();
					}
					break;
			}
		}

		foreach (var d in doors2Remove)
		{
			doors.Remove(d);
		}
	}

	public Room GetOffsetRoom(Vector3 _offset)
	{
		return RoomController.GetRoom(transform.position + _offset);
	}

	public Vector3 GetRoomCenter()
	{
		return new Vector3(Coordinates.x * Dimension.x, Coordinates.y * Dimension.y);
	}
}
