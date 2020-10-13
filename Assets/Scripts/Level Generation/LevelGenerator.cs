using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
	public LevelGenerationData levelGenerationData;
	private List<Vector2Int> levelRooms;

	public static int spawnedRoomCount;

	private void Start()
	{
		levelRooms = LevelCrawlerController.GenerateLevel(levelGenerationData);
		spawnedRoomCount = levelRooms.Count;
		SpawnRooms(levelRooms);
	}

	private void SpawnRooms(IEnumerable<Vector2Int> roomLocations)
	{
		RoomController.Instance.LoadRoom("Start", Vector2Int.zero);
		foreach (var roomLocation in roomLocations)
		{
			RoomController.Instance.LoadRoom("Empty", new Vector2Int(roomLocation.x, roomLocation.y));
		}
	}
}
