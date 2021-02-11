using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Room))]
public class PopulateRoom : MonoBehaviour
{
	#region Interiors
	[Header("Room Interiors")]
	[Tooltip("Make sure there are no duplicates in different arrays!")]
	public RoomInterior[] universal;
	public RoomInterior[] allDoors;
	[Space]
	public RoomInterior[] topDoors;
	public RoomInterior[] rightDoors;
	public RoomInterior[] bottomDoors;
	public RoomInterior[] leftDoors;
	[Space]
	public RoomInterior[] noTopDoors;
	public RoomInterior[] noRightDoors;
	public RoomInterior[] noBottomDoors;
	public RoomInterior[] noLeftDoors;
	[Space]
	public RoomInterior[] leftRightDoors;
	public RoomInterior[] topBottomDoors;
	[Space]
	public RoomInterior[] topRightDoors;
	public RoomInterior[] rightBottomDoors;
	public RoomInterior[] bottomLeftDoors;
	public RoomInterior[] leftTopDoors;
	#endregion

	Room myRoom;
	List<RoomInterior> availableRooms = new List<RoomInterior>();
	
	public Transform interiorParent;

	private void Awake()
	{
		myRoom = GetComponent<Room>();
		myRoom.MyPopulator = this;
		availableRooms.AddRange(universal);
	}

	public void InitializeRoomInterior()
	{
		var rooms = myRoom.NeighbouringRooms;

		if (rooms.Count == 4)
		{
			availableRooms.AddRange(allDoors);
		}
		else if (rooms.Count == 3)
		{
			if (!rooms.ContainsKey(DoorSide.Top))
				availableRooms.AddRange(noTopDoors);

			else if (!rooms.ContainsKey(DoorSide.Right))
				availableRooms.AddRange(noRightDoors);
			
			else if (!rooms.ContainsKey(DoorSide.Bottom))
				availableRooms.AddRange(noBottomDoors);
			
			else if (!rooms.ContainsKey(DoorSide.Left))
				availableRooms.AddRange(noLeftDoors);
		}
		else if (rooms.Count == 2)
		{
			if (rooms.ContainsKey(DoorSide.Left) && rooms.ContainsKey(DoorSide.Right))
				availableRooms.AddRange(leftRightDoors);

			else if (rooms.ContainsKey(DoorSide.Top) && rooms.ContainsKey(DoorSide.Bottom))
				availableRooms.AddRange(topBottomDoors);

			else if (rooms.ContainsKey(DoorSide.Top) && rooms.ContainsKey(DoorSide.Right))
				availableRooms.AddRange(topRightDoors);

			else if (rooms.ContainsKey(DoorSide.Right) && rooms.ContainsKey(DoorSide.Bottom))
				availableRooms.AddRange(rightBottomDoors);

			else if (rooms.ContainsKey(DoorSide.Bottom) && rooms.ContainsKey(DoorSide.Left))
				availableRooms.AddRange(bottomLeftDoors);

			else if (rooms.ContainsKey(DoorSide.Left) && rooms.ContainsKey(DoorSide.Top))
				availableRooms.AddRange(leftTopDoors);
		}
		else if (rooms.Count == 1)
		{
			if (rooms.ContainsKey(DoorSide.Top))
				availableRooms.AddRange(topDoors);

			else if (rooms.ContainsKey(DoorSide.Right))
				availableRooms.AddRange(rightDoors);

			else if (rooms.ContainsKey(DoorSide.Bottom))
				availableRooms.AddRange(bottomDoors);

			else if (rooms.ContainsKey(DoorSide.Left))
				availableRooms.AddRange(leftDoors);
		}
		else
		{
			Debug.LogError($"Room \"{gameObject.name}\" has a invalid amount of doors!");
			return;
		}

		InstantiateRandomInterior();
	}

	private void InstantiateRandomInterior()
	{
		Instantiate(availableRooms[Random.Range(0, availableRooms.Count)], interiorParent);
	}
}
