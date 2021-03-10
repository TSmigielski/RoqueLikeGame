using UnityEngine;

public class Room : MonoBehaviour
{
	[Header("Info")]
	[SerializeField] RoomInfo _info;

	[Header("Main Components")]
	[SerializeField] BoxCollider2D roomTrigger;

	[Header("Prefabs")]
	[SerializeField] GameObject doorPrefab;

	[Header("Walls")]
	[SerializeField] Wall lW;
	[SerializeField] Wall tW;
	[SerializeField] Wall rW;
	[SerializeField] Wall bW;


	public RoomInfo Info
	{
		get { return _info; }
		private set { _info = value; }
	}
	public bool PlayerVisited { get; set; } = false;


	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
			RoomManager.OnPlayerEnterRoom(this);
	}

	public void InitializeRoom(RoomInfo _info)
	{
		Info = _info;
		name = Info.Name;
		transform.position = Info.CenterPoint;
		GetComponent<BoxCollider2D>().size = Info.TrueSize - new Vector2(2, 2); // Make the trigger collider the same size - walls

		BuildWalls();
		PlaceDoors();

		if (RoomManager.ActiveRooms.Count == LevelInstantiator.Instance.GeneratedRoomCount) // When the number of instantiated rooms will be equal to the number
			RoomManager.LevelReady = true;                                                  // of generated ones, tell RoomManger that room instantation is done
	}

	private void BuildWalls() // A lot of math to position the walls properly
	{
		lW.transform.position = new Vector3(Info.CenterPoint.x - (Info.TrueSize.x / 2) + .5f, Info.CenterPoint.y, 0);
		lW.corner.position = new Vector3(Info.CenterPoint.x - (Info.TrueSize.x / 2) + .5f, Info.CenterPoint.y + (Info.TrueSize.y / 2) - .5f, 0);

		rW.transform.position = new Vector3(Info.CenterPoint.x + (Info.TrueSize.x / 2) - .5f, Info.CenterPoint.y, 0);
		rW.corner.position = new Vector3(Info.CenterPoint.x + (Info.TrueSize.x / 2) - .5f, Info.CenterPoint.y - (Info.TrueSize.y / 2) + .5f, 0);

		tW.transform.position = new Vector3(Info.CenterPoint.x, Info.CenterPoint.y + (Info.TrueSize.y / 2) - .5f, 0);
		tW.corner.position = new Vector3(Info.CenterPoint.x + (Info.TrueSize.x / 2) - .5f, Info.CenterPoint.y + (Info.TrueSize.y / 2) - .5f, 0);

		bW.transform.position = new Vector3(Info.CenterPoint.x, Info.CenterPoint.y - (Info.TrueSize.y / 2) + .5f, 0);
		bW.corner.position = new Vector3(Info.CenterPoint.x - (Info.TrueSize.x / 2) + .5f, Info.CenterPoint.y - (Info.TrueSize.y / 2) + .5f, 0);

		lW.sR.size = new Vector2(1, Info.TrueSize.y - 2);
		lW.cL.size = new Vector2(.9f, Info.TrueSize.y);

		rW.sR.size = new Vector2(1, Info.TrueSize.y - 2);
		rW.cL.size = new Vector2(.9f, Info.TrueSize.y);

		tW.sR.size = new Vector2(Info.TrueSize.x - 2, 1);
		tW.cL.size = new Vector2(Info.TrueSize.x, .9f);

		bW.sR.size = new Vector2(Info.TrueSize.x - 2, 1);
		bW.cL.size = new Vector2(Info.TrueSize.x, .9f);
	}

	private void PlaceDoors()
	{
		foreach (var neighbour in Info.Neighbours)
		{
			foreach (var room in neighbour.Value)
			{
				if (!Info.IsBigRoom) // Place the doors next to neighbouring rooms at the position of the wall in that direction
				{
					switch (neighbour.Key)
					{
						case Direction.left:
							InstantiateDoor(lW.transform.position, neighbour.Key);
							continue;
						case Direction.right:
							InstantiateDoor(rW.transform.position, neighbour.Key);
							continue;
						case Direction.up:
							InstantiateDoor(tW.transform.position, neighbour.Key);
							continue;
						case Direction.down:
							InstantiateDoor(bW.transform.position, neighbour.Key);
							continue;
					}
				}
				else // More or less the same, but a lot of math and conditions needed to work properly with big rooms
				{
					switch (neighbour.Key)
					{ // todo - fix double door for the else statements
						case Direction.left:
							if (Info.CenterPoint.y == room.CenterPoint.y || !room.IsBigRoom || room.Size.y == 0)
								InstantiateDoor(new Vector3(lW.transform.position.x, room.CenterPoint.y, 0), neighbour.Key);
							else if (Info.Size.y == 0)
								InstantiateDoor(new Vector3(lW.transform.position.x, Info.CenterPoint.y, 0), neighbour.Key);
							else
								InstantiateDoor(new Vector3(lW.transform.position.x, (Info.CenterPoint.y + room.CenterPoint.y) / 2, 0), neighbour.Key);
							continue;
						case Direction.right:
							if (Info.CenterPoint.y == room.CenterPoint.y || !room.IsBigRoom || room.Size.y == 0)
								InstantiateDoor(new Vector3(rW.transform.position.x, room.CenterPoint.y, 0), neighbour.Key);
							else if (Info.Size.y == 0)
								InstantiateDoor(new Vector3(rW.transform.position.x, Info.CenterPoint.y, 0), neighbour.Key);
							else
								InstantiateDoor(new Vector3(rW.transform.position.x, (Info.CenterPoint.y + room.CenterPoint.y) / 2, 0), neighbour.Key);
							continue;
						case Direction.up:
							if (Info.CenterPoint.x == room.CenterPoint.x || !room.IsBigRoom || room.Size.x == 0)
								InstantiateDoor(new Vector3(room.CenterPoint.x, tW.transform.position.y, 0), neighbour.Key);
							else if (Info.Size.x == 0)
								InstantiateDoor(new Vector3(Info.CenterPoint.x, tW.transform.position.y, 0), neighbour.Key);
							else
								InstantiateDoor(new Vector3((Info.CenterPoint.x + room.CenterPoint.x) / 2, tW.transform.position.y, 0), neighbour.Key);
							continue;
						case Direction.down:
							if (Info.CenterPoint.x == room.CenterPoint.x || !room.IsBigRoom || room.Size.x == 0)
								InstantiateDoor(new Vector3(room.CenterPoint.x, bW.transform.position.y, 0), neighbour.Key);
							else if (Info.Size.x == 0)
								InstantiateDoor(new Vector3(Info.CenterPoint.x, bW.transform.position.y, 0), neighbour.Key);
							else
								InstantiateDoor(new Vector3(((Info.CenterPoint.x + room.CenterPoint.x) / 2), bW.transform.position.y, 0), neighbour.Key);
							continue;
					}
				}
			}
		}
	}

	private void InstantiateDoor(Vector3 _position, Direction _doorSide)
	{
		var doorGO = Instantiate(doorPrefab, transform);
		doorGO.transform.position = _position;
		var door = doorGO.GetComponent<Door>();

		switch (_doorSide)
		{
			case Direction.left:
				door.cL.size = new Vector2(.75f, 2);
				door.mask.transform.localScale = new Vector2(1.1f, 2) * 100;
				door.tpPoint.position += Vector3.left * door.tpDistance;
				break;
			case Direction.right:
				door.cL.size = new Vector2(.75f, 2);
				door.mask.transform.localScale = new Vector2(1.1f, 2) * 100;
				door.tpPoint.position += Vector3.right * door.tpDistance;
				break;
			case Direction.up:
				door.cL.size = new Vector2(2, .75f);
				door.mask.transform.localScale = new Vector2(2, 1.1f) * 100;
				door.tpPoint.position += Vector3.up * door.tpDistance;
				break;
			case Direction.down:
				door.cL.size = new Vector2(2, .75f);
				door.mask.transform.localScale = new Vector2(2, 1.1f) * 100;
				door.tpPoint.position += Vector3.down * door.tpDistance;
				break;
		}
	}
}
