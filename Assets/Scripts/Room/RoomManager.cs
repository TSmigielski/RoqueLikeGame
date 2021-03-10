using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
	public static List<Room> ActiveRooms { get; set; }
	public static Room PlayerRoom { get; private set; }
	private static bool _levelReady;

	public static bool LevelReady
	{
		get { return _levelReady; }
		set
		{
			_levelReady = value;
			MinimapController.Instance.InitializeMinimap();
		}
	}


	private void Awake()
	{
		ActiveRooms = new List<Room>();
	}

	public static void OnPlayerEnterRoom(Room _room)
	{
		if (PlayerRoom == _room)
			return;
		PlayerRoom = _room;
		_room.PlayerVisited = true;

		MinimapController.Instance.UpdateMinimap();
	}

	public static Room GetRoom_ByCoordinates(Vector2 _coordinates)
	{
		foreach (var room in ActiveRooms) // Iterate over every active room
		{
			if (room.Info.Coordinates == _coordinates)
				return room;

			if (room.Info.IsBigRoom) // If room is a big room, then check if passed coordinates match any of room's "subRooms"
			{
				if (room.Info.Coordinates + room.Info.Size == _coordinates ||
					room.Info.Coordinates + new Vector2(room.Info.Size.x, 0) == _coordinates ||
					room.Info.Coordinates + new Vector2(0, room.Info.Size.y) == _coordinates)
				{
					return room;
				}
			}
		}

		return null;
	}
}
