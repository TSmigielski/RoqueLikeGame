using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RoomType { Start, Empty, Multi, Destroyed }

public class Room : MonoBehaviour
{
	#region Custom Vector2's
	Vector2 v2_rightUp = new Vector2(1, 1);
	Vector2 v2_rightDown = new Vector2(1, -1);
	Vector2 v2_leftDown = new Vector2(-1, -1);
	Vector2 v2_leftUp = new Vector2(-1, 1);

	Vector2 v2Multi_TopLeft = new Vector2(-.5f, 1.5f);
	Vector2 v2Multi_TopRight = new Vector2(.5f, 1.5f);
	Vector2 v2Multi_RightTop = new Vector2(1.5f, .5f);
	Vector2 v2Multi_RightBottom = new Vector2(1.5f, -.5f);
	Vector2 v2Multi_BottomRight = new Vector2(.5f, -1.5f);
	Vector2 v2Multi_BottomLeft = new Vector2(-.5f, -1.5f);
	Vector2 v2Multi_LeftBottom = new Vector2(-1.5f, -.5f);
	Vector2 v2Multi_LeftTop = new Vector2(-1.5f, .5f);
	#endregion

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

	public bool PlayerVisited { get; set; } = false;

	public Dictionary<DoorSide, List<Room>> NeighbouringRooms { get; set; }

	public List<Door> doors = new List<Door>();
	public List<Wall> walls = new List<Wall>();
	private List<Room> multiRoom = new List<Room>();

	[HideInInspector] public Scene initialScene;

	private MinimapRoom _myMMRoom;
	public MinimapRoom MyMinimapRoom
	{
		get { return _myMMRoom; }
		set { _myMMRoom = value; value.MyRoom = this; }
	}

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

	private void Awake()
	{
		NeighbouringRooms = new Dictionary<DoorSide, List<Room>>();
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
			RoomController.Instance.OnPlayerEnterRoom(this);
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

	public void UpdateNeighbours()
	{
		if (Room_Type != RoomType.Multi)
		{
			if (GetOffsetRoom(Vector2.up))
			{
				NeighbouringRooms.Add(DoorSide.Top, new List<Room>() { GetOffsetRoom(Vector2.up) } );
			}
			if (GetOffsetRoom(Vector2.right))
			{
				NeighbouringRooms.Add(DoorSide.Right, new List<Room>() { GetOffsetRoom(Vector2.right) });
			}
			if (GetOffsetRoom(Vector2.down))
			{
				NeighbouringRooms.Add(DoorSide.Bottom, new List<Room>() { GetOffsetRoom(Vector2.down) });
			}
			if (GetOffsetRoom(Vector2.left))
			{
				NeighbouringRooms.Add(DoorSide.Left, new List<Room>() { GetOffsetRoom(Vector2.left) });
			}

			return;
		}

		else if (Dimension.x == RoomController.roomX * 2 && Dimension.y == RoomController.roomY * 2)
		{
			if (GetOffsetRoom(v2Multi_TopLeft))
			{
				if (NeighbouringRooms.ContainsKey(DoorSide.Top))
					NeighbouringRooms[DoorSide.Top].Add(GetOffsetRoom(v2Multi_TopLeft));
				else
					NeighbouringRooms.Add(DoorSide.Top, new List<Room>() { GetOffsetRoom(v2Multi_TopLeft) });
			}
			if (GetOffsetRoom(v2Multi_TopRight))
			{
				if (NeighbouringRooms.ContainsKey(DoorSide.Top))
					NeighbouringRooms[DoorSide.Top].Add(GetOffsetRoom(v2Multi_TopRight));
				else
					NeighbouringRooms.Add(DoorSide.Top, new List<Room>() { GetOffsetRoom(v2Multi_TopRight) });
			}

			if (GetOffsetRoom(v2Multi_RightTop))
			{
				if (NeighbouringRooms.ContainsKey(DoorSide.Right))
					NeighbouringRooms[DoorSide.Right].Add(GetOffsetRoom(v2Multi_RightTop));
				else
					NeighbouringRooms.Add(DoorSide.Right, new List<Room>() { GetOffsetRoom(v2Multi_RightTop) });
			}
			if (GetOffsetRoom(v2Multi_RightBottom))
			{
				if (NeighbouringRooms.ContainsKey(DoorSide.Right))
					NeighbouringRooms[DoorSide.Right].Add(GetOffsetRoom(v2Multi_RightBottom));
				else
					NeighbouringRooms.Add(DoorSide.Right, new List<Room>() { GetOffsetRoom(v2Multi_RightBottom) });
			}

			if (GetOffsetRoom(v2Multi_BottomRight))
			{
				if (NeighbouringRooms.ContainsKey(DoorSide.Bottom))
					NeighbouringRooms[DoorSide.Bottom].Add(GetOffsetRoom(v2Multi_BottomRight));
				else
					NeighbouringRooms.Add(DoorSide.Bottom, new List<Room>() { GetOffsetRoom(v2Multi_BottomRight) });
			}
			if (GetOffsetRoom(v2Multi_BottomLeft))
			{
				if (NeighbouringRooms.ContainsKey(DoorSide.Bottom))
					NeighbouringRooms[DoorSide.Bottom].Add(GetOffsetRoom(v2Multi_BottomLeft));
				else
					NeighbouringRooms.Add(DoorSide.Bottom, new List<Room>() { GetOffsetRoom(v2Multi_BottomLeft) });
			}

			if (GetOffsetRoom(v2Multi_LeftBottom))
			{
				if (NeighbouringRooms.ContainsKey(DoorSide.Left))
					NeighbouringRooms[DoorSide.Left].Add(GetOffsetRoom(v2Multi_LeftBottom));
				else
					NeighbouringRooms.Add(DoorSide.Left, new List<Room>() { GetOffsetRoom(v2Multi_LeftBottom) });
			}
			if (GetOffsetRoom(v2Multi_LeftTop))
			{
				if (NeighbouringRooms.ContainsKey(DoorSide.Left))
					NeighbouringRooms[DoorSide.Left].Add(GetOffsetRoom(v2Multi_LeftTop));
				else
					NeighbouringRooms.Add(DoorSide.Left, new List<Room>() { GetOffsetRoom(v2Multi_LeftTop) });
			}
		}

		else if (Dimension.x == RoomController.roomX * 2)
		{
			if (GetOffsetRoom(v2Multi_TopLeft - new Vector2(0, .5f)))
			{
				if (NeighbouringRooms.ContainsKey(DoorSide.Top))
					NeighbouringRooms[DoorSide.Top].Add(GetOffsetRoom(v2Multi_TopLeft - new Vector2(0, .5f)));
				else
					NeighbouringRooms.Add(DoorSide.Top, new List<Room>() { GetOffsetRoom(v2Multi_TopLeft - new Vector2(0, .5f)) });
			}
			if (GetOffsetRoom(v2Multi_TopRight - new Vector2(0, .5f)))
			{
				if (NeighbouringRooms.ContainsKey(DoorSide.Top))
					NeighbouringRooms[DoorSide.Top].Add(GetOffsetRoom(v2Multi_TopRight - new Vector2(0, .5f)));
				else
					NeighbouringRooms.Add(DoorSide.Top, new List<Room>() { GetOffsetRoom(v2Multi_TopRight - new Vector2(0, .5f)) });
			}

			if (GetOffsetRoom(Vector2.right + new Vector2(.5f, 0)))
			{
				NeighbouringRooms.Add(DoorSide.Right, new List<Room>() { GetOffsetRoom(Vector2.right + new Vector2(.5f, 0)) });
			}

			if (GetOffsetRoom(v2Multi_BottomRight + new Vector2(0, .5f)))
			{
				if (NeighbouringRooms.ContainsKey(DoorSide.Bottom))
					NeighbouringRooms[DoorSide.Bottom].Add(GetOffsetRoom(v2Multi_BottomRight + new Vector2(0, .5f)));
				else
					NeighbouringRooms.Add(DoorSide.Bottom, new List<Room>() { GetOffsetRoom(v2Multi_BottomRight + new Vector2(0, .5f)) });
			}
			if (GetOffsetRoom(v2Multi_BottomLeft + new Vector2(0, .5f)))
			{
				if (NeighbouringRooms.ContainsKey(DoorSide.Bottom))
					NeighbouringRooms[DoorSide.Bottom].Add(GetOffsetRoom(v2Multi_BottomLeft + new Vector2(0, .5f)));
				else
					NeighbouringRooms.Add(DoorSide.Bottom, new List<Room>() { GetOffsetRoom(v2Multi_BottomLeft + new Vector2(0, .5f)) });
			}

			if (GetOffsetRoom(Vector2.left - new Vector2(.5f, 0)))
			{
				NeighbouringRooms.Add(DoorSide.Left, new List<Room>() { GetOffsetRoom(Vector2.left - new Vector2(.5f, 0)) });
			}
		}

		else if (Dimension.y == RoomController.roomY * 2)
		{
			if (GetOffsetRoom(Vector2.up + new Vector2(0, .5f)))
			{
				NeighbouringRooms.Add(DoorSide.Top, new List<Room>() { GetOffsetRoom(Vector2.up + new Vector2(0, .5f)) });
			}

			if (GetOffsetRoom(v2Multi_RightTop - new Vector2(.5f, 0)))
			{
				if (NeighbouringRooms.ContainsKey(DoorSide.Right))
					NeighbouringRooms[DoorSide.Right].Add(GetOffsetRoom(v2Multi_RightTop - new Vector2(.5f, 0)));
				else
					NeighbouringRooms.Add(DoorSide.Right, new List<Room>() { GetOffsetRoom(v2Multi_RightTop - new Vector2(.5f, 0)) });
			}
			if (GetOffsetRoom(v2Multi_RightBottom - new Vector2(.5f, 0)))
			{
				if (NeighbouringRooms.ContainsKey(DoorSide.Right))
					NeighbouringRooms[DoorSide.Right].Add(GetOffsetRoom(v2Multi_RightBottom - new Vector2(.5f, 0)));
				else
					NeighbouringRooms.Add(DoorSide.Right, new List<Room>() { GetOffsetRoom(v2Multi_RightBottom - new Vector2(.5f, 0)) });
			}

			if (GetOffsetRoom(Vector2.down - new Vector2(0, .5f)))
			{
				NeighbouringRooms.Add(DoorSide.Bottom, new List<Room>() { GetOffsetRoom(Vector2.down - new Vector2(0, .5f)) });
			}

			if (GetOffsetRoom(v2Multi_LeftBottom + new Vector2(.5f, 0)))
			{
				if (NeighbouringRooms.ContainsKey(DoorSide.Left))
					NeighbouringRooms[DoorSide.Left].Add(GetOffsetRoom(v2Multi_LeftBottom + new Vector2(.5f, 0)));
				else
					NeighbouringRooms.Add(DoorSide.Left, new List<Room>() { GetOffsetRoom(v2Multi_LeftBottom + new Vector2(.5f, 0)) });
			}
			if (GetOffsetRoom(v2Multi_LeftTop + new Vector2(.5f, 0)))
			{
				if (NeighbouringRooms.ContainsKey(DoorSide.Left))
					NeighbouringRooms[DoorSide.Left].Add(GetOffsetRoom(v2Multi_LeftTop + new Vector2(.5f, 0)));
				else
					NeighbouringRooms.Add(DoorSide.Left, new List<Room>() { GetOffsetRoom(v2Multi_LeftTop + new Vector2(.5f, 0)) });
			}
		}

		else
		{
			Debug.LogError($"Something went horribly wrong! Room \"{gameObject.name}\" is not a \"not multi-room\" and does not have a valid size if it is a multi-room");
		}

		var tmp = new Dictionary<DoorSide, List<Room>>();
		foreach (var item in NeighbouringRooms.Keys)
		{
			tmp.Add(item, NeighbouringRooms[item].Distinct().ToList());
		}
		NeighbouringRooms = tmp;
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

		List<List<Vector2>> tests = new List<List<Vector2>>()
		{
			new List<Vector2>() { v2_rightUp, Vector2.right, Vector2.up },
			new List<Vector2>() { v2_rightDown, Vector2.right, Vector2.down },
			new List<Vector2>() { v2_leftDown, Vector2.left, Vector2.down },
			new List<Vector2>() { v2_leftUp, Vector2.left, Vector2.up },
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
				Room rm = GetOffsetRoom(testValue);
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
			Room rm = GetOffsetRoom(test);
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
		_offset.x *= RoomController.roomX;
		_offset.y *= RoomController.roomY;
		return RoomController.GetRoom(transform.position + _offset);
	}
}
