using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
	public static MinimapController Instance { get; set; }

	public GameObject roomPrefab;
	public Transform minimapPlain;
	public static List<MinimapRoom> minimapRooms = new List<MinimapRoom>();

	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

	public void UpdateMinimap()
	{
		var myRoom = PlayerController.CurrentRoom; // Var for easy access to player's current room

		foreach (var rArray in myRoom.NeighbouringRooms.Values) // Create a room for each of the neighbours if it doesn't exist already
		{
			foreach (var r in rArray)
			{
				if (r.MyMinimapRoom == null)
					CreateNewMinimapRoom(r);
			}
		}

		if (myRoom.MyMinimapRoom != null) // If the room already exists on the minimap, just center it and return
		{
			myRoom.MyMinimapRoom.CenterRoom(minimapRooms, minimapPlain);
			return;
		}

		// Else: create a new room and center it

		CreateNewMinimapRoom(myRoom).CenterRoom(minimapRooms, minimapPlain);
	}

	private MinimapRoom CreateNewMinimapRoom(Room myRoom)
	{
		var newMinimapRoom = Instantiate(roomPrefab, minimapPlain.transform).GetComponent<MinimapRoom>();
		newMinimapRoom.transform.localPosition = new Vector2(RoomController.roomX, RoomController.roomY) * myRoom.Coordinates;
		minimapRooms.Add(newMinimapRoom);
		myRoom.MyMinimapRoom = newMinimapRoom;
		return newMinimapRoom;
	}
}
