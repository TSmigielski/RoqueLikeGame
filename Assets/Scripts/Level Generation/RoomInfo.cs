using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum RoomType { StartingRoom, RegularRoom, StaircaseRoom, ElevatorRoom, ExitRoom }

[System.Serializable]
public class RoomInfo
{
	[SerializeField] private string _name;
	[SerializeField] private Vector2 _coordinates;
	[SerializeField] private Vector2 _size;
	[SerializeField] private Vector2 _centerPoint;
	[SerializeField] private Vector2 _totalSize;
	[SerializeField] private RoomType _roomType;
	[SerializeField] private bool _isBigRoom;

	public string Name
	{
		get { return _name; }
		private set { _name = value; }
	}
	public Vector2 Coordinates
	{
		get { return _coordinates; }
		private set { _coordinates = value; }
	}
	public Vector2 Size
	{
		get { return _size; }
		private set { _size = value; }
	}
	public Vector2 CenterPoint // Transform.position will be equal to this value
	{
		get { return _centerPoint; }
		private set { _centerPoint = value; }
	}
	public Vector2 TrueSize // This is the actual width and height of a room (in units)
	{
		get { return _totalSize; }
		private set { _totalSize = value; }
	}
	public RoomType RoomType
	{
		get { return _roomType; }
		private set { _roomType = value; }
	}
	public bool IsBigRoom
	{
		get { return _isBigRoom; }
		private set { _isBigRoom = value; }
	}
	public Dictionary<Direction, RoomInfo[]> Neighbours { get; private set; }

	// todo - information about enemies / what is inside this room

	public RoomInfo(string _name, Vector2 _coordinates, Vector2 _size, RoomType _roomType)
	{
		Name = _name;				//
		Coordinates = _coordinates; // Typical constructor stuff
		Size = _size;				//
		RoomType = _roomType;		//

		if (_size == Vector2.zero)  //
			IsBigRoom = false;      // If the size is 0,0 then
		else						// the room is regular sized
			IsBigRoom = true;		//

		TrueSize = LevelInstantiator.Instance.RoomSize; // Assign the provided size
		CenterPoint = Coordinates * TrueSize; // Multiplying the coordinates with the size gives you the center point

		if (IsBigRoom)
		{
			if (Size.x == 1)
				CenterPoint += new Vector2(TrueSize.x / 2, 0); //
			else if (Size.x == -1)								//
				CenterPoint -= new Vector2(TrueSize.x / 2, 0); //
			if (Size.y == 1)									// Figure out the center point of a big room
				CenterPoint += new Vector2(0, TrueSize.y / 2); //
			else if (Size.y == -1)								//
				CenterPoint -= new Vector2(0, TrueSize.y / 2); //

			if (Mathf.Abs(Size.x) == 1)
				TrueSize += new Vector2(TrueSize.x, 0); //
			if (Mathf.Abs(Size.y) == 1)					  // And the total size
				TrueSize += new Vector2(0, TrueSize.y); //
		}

		Neighbours = new Dictionary<Direction, RoomInfo[]>(); // Initialize the Dictionary of neighbours
	}

	public void UpdateNeighbours()
	{
		// This probably can be done better,
		// But it's good enough for now.

		// Basicically this is checking if there is a room next to this room.
		// For every possible position.

		// The same is true for every subRoom of big rooms.
		// Without the positions of other subRooms of the same room ofcourse.

		// subRoom:
		// A imaginery room of a big room (imagine 4 subRooms for a Size of 1,1)

		var up = new List<RoomInfo>();
		var down = new List<RoomInfo>();
		var left = new List<RoomInfo>();
		var right = new List<RoomInfo>();

		if (!IsBigRoom)
		{
			up.Add(LevelGenerator.GetRoom(Coordinates + Vector2.up));
			right.Add(LevelGenerator.GetRoom(Coordinates + Vector2.right));
			down.Add(LevelGenerator.GetRoom(Coordinates + Vector2.down));
			left.Add(LevelGenerator.GetRoom(Coordinates + Vector2.left));
		}
		else if (Size == new Vector2(1, 1))
		{
			up.Add(LevelGenerator.GetRoom(Coordinates + Vector2.up + Vector2.up));
			up.Add(LevelGenerator.GetRoom(Coordinates + Vector2.up + Vector2.up + Vector2.right));
			right.Add(LevelGenerator.GetRoom(Coordinates + Vector2.right + Vector2.right));
			right.Add(LevelGenerator.GetRoom(Coordinates + Vector2.right + Vector2.right + Vector2.up));
			down.Add(LevelGenerator.GetRoom(Coordinates + Vector2.down));
			down.Add(LevelGenerator.GetRoom(Coordinates + Vector2.down + Vector2.right));
			left.Add(LevelGenerator.GetRoom(Coordinates + Vector2.left));
			left.Add(LevelGenerator.GetRoom(Coordinates + Vector2.left + Vector2.up));
		}
		else if (Size == new Vector2(1, -1))
		{
			up.Add(LevelGenerator.GetRoom(Coordinates + Vector2.up));
			up.Add(LevelGenerator.GetRoom(Coordinates + Vector2.up + Vector2.right));
			right.Add(LevelGenerator.GetRoom(Coordinates + Vector2.right + Vector2.right));
			right.Add(LevelGenerator.GetRoom(Coordinates + Vector2.right + Vector2.right + Vector2.down));
			down.Add(LevelGenerator.GetRoom(Coordinates + Vector2.down + Vector2.down));
			down.Add(LevelGenerator.GetRoom(Coordinates + Vector2.down + Vector2.down + Vector2.right));
			left.Add(LevelGenerator.GetRoom(Coordinates + Vector2.left));
			left.Add(LevelGenerator.GetRoom(Coordinates + Vector2.left + Vector2.down));

		}
		else if (Size == new Vector2(-1, -1))
		{
			up.Add(LevelGenerator.GetRoom(Coordinates + Vector2.up));
			up.Add(LevelGenerator.GetRoom(Coordinates + Vector2.up + Vector2.left));
			right.Add(LevelGenerator.GetRoom(Coordinates + Vector2.right));
			right.Add(LevelGenerator.GetRoom(Coordinates + Vector2.right + Vector2.down));
			down.Add(LevelGenerator.GetRoom(Coordinates + Vector2.down + Vector2.down));
			down.Add(LevelGenerator.GetRoom(Coordinates + Vector2.down + Vector2.down + Vector2.left));
			left.Add(LevelGenerator.GetRoom(Coordinates + Vector2.left + Vector2.left));
			left.Add(LevelGenerator.GetRoom(Coordinates + Vector2.left + Vector2.left + Vector2.down));
		}
		else if (Size == new Vector2(-1, 1))
		{
			up.Add(LevelGenerator.GetRoom(Coordinates + Vector2.up + Vector2.up));
			up.Add(LevelGenerator.GetRoom(Coordinates + Vector2.up + Vector2.up + Vector2.left));
			right.Add(LevelGenerator.GetRoom(Coordinates + Vector2.right));
			right.Add(LevelGenerator.GetRoom(Coordinates + Vector2.right + Vector2.up));
			down.Add(LevelGenerator.GetRoom(Coordinates + Vector2.down));
			down.Add(LevelGenerator.GetRoom(Coordinates + Vector2.down + Vector2.left));
			left.Add(LevelGenerator.GetRoom(Coordinates + Vector2.left + Vector2.left));
			left.Add(LevelGenerator.GetRoom(Coordinates + Vector2.left + Vector2.left + Vector2.up));
		}
		else if (Size == new Vector2(1, 0))
		{
			up.Add(LevelGenerator.GetRoom(Coordinates + Vector2.up));
			up.Add(LevelGenerator.GetRoom(Coordinates + Vector2.right + Vector2.up));
			right.Add(LevelGenerator.GetRoom(Coordinates + Vector2.right + Vector2.right));
			down.Add(LevelGenerator.GetRoom(Coordinates + Vector2.down));
			down.Add(LevelGenerator.GetRoom(Coordinates + Vector2.right + Vector2.down));
			left.Add(LevelGenerator.GetRoom(Coordinates + Vector2.left));
		}
		else if (Size == new Vector2(-1, 0))
		{
			up.Add(LevelGenerator.GetRoom(Coordinates + Vector2.up));
			up.Add(LevelGenerator.GetRoom(Coordinates + Vector2.left + Vector2.up));
			right.Add(LevelGenerator.GetRoom(Coordinates + Vector2.right));
			down.Add(LevelGenerator.GetRoom(Coordinates + Vector2.down));
			down.Add(LevelGenerator.GetRoom(Coordinates + Vector2.left + Vector2.down));
			left.Add(LevelGenerator.GetRoom(Coordinates + Vector2.left + Vector2.left));
		}
		else if (Size == new Vector2(0, 1))
		{
			up.Add(LevelGenerator.GetRoom(Coordinates + Vector2.up + Vector2.up));
			right.Add(LevelGenerator.GetRoom(Coordinates + Vector2.right));
			right.Add(LevelGenerator.GetRoom(Coordinates + Vector2.up + Vector2.right));
			down.Add(LevelGenerator.GetRoom(Coordinates + Vector2.down));
			left.Add(LevelGenerator.GetRoom(Coordinates + Vector2.left));
			left.Add(LevelGenerator.GetRoom(Coordinates + Vector2.up + Vector2.left));
		}
		else if (Size == new Vector2(0, -1))
		{
			up.Add(LevelGenerator.GetRoom(Coordinates + Vector2.up));
			right.Add(LevelGenerator.GetRoom(Coordinates + Vector2.right));
			right.Add(LevelGenerator.GetRoom(Coordinates + Vector2.down + Vector2.right));
			down.Add(LevelGenerator.GetRoom(Coordinates + Vector2.down + Vector2.down));
			left.Add(LevelGenerator.GetRoom(Coordinates + Vector2.left));
			left.Add(LevelGenerator.GetRoom(Coordinates + Vector2.down + Vector2.left));
		}

		up = up.Distinct().ToList();				  //
		up = up.Where(x => x != null).ToList();		  //
		right = right.Distinct().ToList();			  //
		right = right.Where(x => x != null).ToList(); // Here we have some cleanup
		down = down.Distinct().ToList();			  // from the previous messy code
		down = down.Where(x => x != null).ToList();	  //
		left = left.Distinct().ToList();			  //
		left = left.Where(x => x != null).ToList();	  //

		if (up.Count > 0)
			Neighbours.Add(Direction.up, up.ToArray());		  //
		if (right.Count > 0)								  //
			Neighbours.Add(Direction.right, right.ToArray()); //
		if (down.Count > 0)									  // And some error checks
			Neighbours.Add(Direction.down, down.ToArray());	  //
		if (left.Count > 0)									  //
			Neighbours.Add(Direction.left, left.ToArray());	  //
	}
}
