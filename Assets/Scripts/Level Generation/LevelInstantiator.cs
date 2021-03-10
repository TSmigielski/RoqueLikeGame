using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelInstantiator : MonoBehaviour
{
	public static LevelInstantiator Instance { get; private set; } // Singleton

	[Header("Main")]
	public LevelGenerationData levelGenerationData;
	[SerializeField] [Min(5)] private Vector2 _roomSize; // The game supports any room size above 5
	[SerializeField] GameObject roomPrefab;
	[SerializeField] [Range(0, .333f)] float roomInitializationTime = .05f; // Set 0 for a 'pop' effect (every room starts existing at the same frame).
																		    // Anything above that will have a nice snake like expanding effect.
	[Header("Debug")]
	[SerializeField] List<RoomInfo> generatedLevelPreview = new List<RoomInfo>(); // In the case that rooms won't instantiate,
																				  // it helps to see their generated counterpart details
	public Vector2 RoomSize
	{
		get { return _roomSize; }
		private set { _roomSize = value; }
	}
	public int GeneratedRoomCount { get; private set; } // The amount of generated rooms

	private void Awake()
	{
		if (Instance == null)	 //
			Instance = this;	 // Singleton
		else					 //
			Destroy(gameObject); //
	}

	private void Start()
	{
		StartCoroutine(InstantiateRooms());
	}

	public IEnumerator InstantiateRooms()
	{
		var newLevel = LevelGenerator.GenerateNewLevel(levelGenerationData); // For now the generation is done in the same step as instantiation
		GeneratedRoomCount = newLevel.Count;
		generatedLevelPreview = newLevel;
		foreach (var roomInfo in newLevel)
		{
			var newRoom = Instantiate(roomPrefab, transform).GetComponent<Room>();
			RoomManager.ActiveRooms.Add(newRoom);
			newRoom.InitializeRoom(roomInfo);
			if (roomInitializationTime == 0)
				continue;
			else
				yield return new WaitForSeconds(roomInitializationTime);
		}
	}
}
