using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MinimapController : MonoBehaviour // todo - Add 'per room' scalability and minimap zoom
{
	public static MinimapController Instance { get; private set; } // Singleton

	[SerializeField] GameObject minimapRoomPrefab;
	[SerializeField] Transform minimapRoomsParent;
	[Space]
	[SerializeField] Color notVisitedRoomColor;
	[SerializeField] Color visitedRoomColor;
	[SerializeField] Color currentRoomColor;
	[SerializeField] [Min(0)] float padding;

	public static List<MinimapRoom> MiniMapRooms { get; private set; } = new List<MinimapRoom>();

	private void Awake()
	{
		if (Instance == null)	 //
			Instance = this;	 // Singleton
		else					 //
			Destroy(gameObject); //

		SetColors();
	}

	public void InitializeMinimap()
	{
		foreach (var room in RoomManager.ActiveRooms)
		{
			var mR = Instantiate(minimapRoomPrefab, minimapRoomsParent);
			var rT = mR.GetComponent<RectTransform>();
			rT.sizeDelta = room.Info.TrueSize - new Vector2(padding, padding);
			rT.transform.localPosition = room.Info.CenterPoint;
			var script = mR.GetComponent<MinimapRoom>();
			script.Coordinates = room.Info.Coordinates;
			MiniMapRooms.Add(script);
		}
		UpdateMinimap();
	}

	public void UpdateMinimap()
	{
		foreach (var room in MiniMapRooms)
		{
			room.UpdateRoom();
		}

		minimapRoomsParent.localPosition = Vector3.zero - (Vector3)RoomManager.PlayerRoom.Info.CenterPoint; // Position current minimap room to the center
	}																										// Together with every other ofcourse

	public void SetColors()
	{
		MinimapRoom.NotVisitedColor = notVisitedRoomColor;
		MinimapRoom.VisitedColor = visitedRoomColor;
		MinimapRoom.CurrentColor = currentRoomColor;
	}

	public static MinimapRoom GetMinimapRoom(Vector2 _coordinates)
	{
		foreach (var room in MiniMapRooms)
		{
			if (room.Coordinates == _coordinates)
				return room;
		}
		return null;
	}
}
