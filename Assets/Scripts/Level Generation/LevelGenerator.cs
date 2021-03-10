using System.Collections.Generic;
using UnityEngine;

public static class LevelGenerator
{
	public static Dictionary<string, List<Vector2>> bigRoom_2x2_Tests = new Dictionary<string, List<Vector2>>
	{
		{ "UpRight", new List<Vector2> { Vector2.up, Vector2.right, new Vector2(1, 1) } },		//
		{ "DownRight", new List<Vector2> { Vector2.down, Vector2.right, new Vector2(1, -1) } }, // Predetermined tests to see if
		{ "DownLeft",new List<Vector2> { Vector2.down, Vector2.left, new Vector2(-1, -1) } },	// there is space for a big room
		{ "UpLeft",new List<Vector2> { Vector2.up, Vector2.left, new Vector2(-1, 1) } }			//
	};

	public static LevelGenerationData CurrentLevelData { get; private set; } // Information on how the level needs to be generated
	public static List<RoomInfo> CurrentLevel { get; private set; } = new List<RoomInfo>(); // The 'work in progress' level
	public static bool IsGeneratingLevel { get; set; } = false; // Self explanatory boolean

	public static List<RoomInfo> GenerateNewLevel(LevelGenerationData _data)
	{
		if (IsGeneratingLevel) // If some script requested a new level whilst one was already being made
		{					   // Return to avoid errors | todo - Wait for the current level to finish instead of returning
			Debug.LogError("Someone tried to generate a level whilst one was already being generated!");
			Debug.LogError("Returning null!");
			return null;
		}

		IsGeneratingLevel = true;
		CurrentLevelData = _data;

		LevelCrawler.ResetCrawledPositions(); // Make every position available
		CurrentLevel.Add(new RoomInfo("Starting Room", Vector2.zero, Vector2.zero, RoomType.StartingRoom)); // Add the starting room

		for (int i = 0; i < CurrentLevelData.numberOfCrawlers; i++) // Iterate over every crawler
		{
			int crawls;
			if (CurrentLevelData.minimumCrawls >= CurrentLevelData.maximumCrawls) // If the minimum is higher than the maximum,
				crawls = CurrentLevelData.minimumCrawls;						  // Just assign the minimum
			else																  // Else assign a random value between the min and max
				crawls = Random.Range(CurrentLevelData.minimumCrawls, CurrentLevelData.maximumCrawls + 1); // The +1 is here because the second number is exclusive

			LevelCrawler crawler; // Create the crawler
			if (i == 0) // If this is the first crawler, choose a random position adjacent to the starting room
			{
				crawler = new LevelCrawler(Vector2.zero); // Instantiate the crawler
				GenerateRoom(crawler, 0, 0); // Generate First Room
	}
			else // Instantiate the crawler at a random position that has been already visited
			{
				crawler = new LevelCrawler(CurrentLevel[Random.Range(0, CurrentLevel.Count)].Coordinates);
				if (GenerateRoom(crawler, i, 0)) // Generate the first room of this crawler
					break; // True means that a error happened, so break out
			}

			for (int j = 1; j < crawls; j++) // Iterate over every crawl of crawler 'i'
			{
				if (GenerateRoom(crawler, i, j))
					break; // True means that a error happened, so break out
			}
		}

		foreach (var room in CurrentLevel)
		{
			room.UpdateNeighbours(); // After the crawlers are finished, update the Neighbours list (dictionary) of every room
		}

		IsGeneratingLevel = false;
		return CurrentLevel;
	}

	public static RoomInfo GetRoom(Vector2 _position) // Returns the room at the passed coordinates, or null if there is no room there
	{
		foreach (var room in CurrentLevel) // Iterate over every (generated) room in current level
		{
			if (room.Coordinates == _position)
				return room;

			if (room.IsBigRoom) // If room is a big room, then check if passed coordinates match any of room's "subRooms"
			{
				if (room.Coordinates + room.Size == _position ||
					room.Coordinates + new Vector2(room.Size.x, 0) == _position ||
					room.Coordinates + new Vector2(0, room.Size.y) == _position)
				{
					return room;
				}
			}
		}

		return null; // If no room matched passed coordinates, return null
	}

	static bool GenerateRoom(LevelCrawler _crawler, int _crawlerID, int _crawlerCrawl)
	{
		if (_crawler.NewPosition() == Vector2.zero) // Moves the crawler to a new not-occupied position & If there is no free position, return;
		{
			Debug.LogWarning($"Crawler {_crawlerID} got himeself stuck, returning.");
			return true; // True for 'a error happened' | todo - help the crawlers get unstuck
		}
		var newRoom = new RoomInfo($"Room {_crawlerID}-{_crawlerCrawl}", _crawler.Position, CalculateBigRoom(_crawler), RoomType.RegularRoom);
		CurrentLevel.Add(newRoom); // Add the new room to the List of current rooms
		if (newRoom.IsBigRoom)
			CrawlBigRoom(newRoom, _crawler); // This method adds the subRooms to the crawled positions

		return false; // False for 'no errors occured'
	}

	static Vector2 CalculateBigRoom(LevelCrawler _crawler)
	{
		if (Random.Range(.001f, 99.999f) > CurrentLevelData.individualBigRoomChance) // 100 > 100 == false, (you could do >= for this)
			goto ReturnDefault;														 // But then 0 >= 0 == true, and that's not you want
		//  goto instead of return, so you wont need to change every return,
		//  in case you would want to do that.

		if (Random.Range(.001f, 99.999f) < CurrentLevelData.doubleOverQuadrupleBias) // same logic as above  ^
		{
			// Logic for 2 subRooms:
			List<Vector2> viableVSizes = new List<Vector2>(); // The logic of this (viableVSizes (V for Vector)) is pretty self explanatory if you will read the if's

			if (Random.Range(0, 2) == 0) // 50% for 0 and 1 ; 0 for a horizontal, and 1 for vertical room
			{
				if (LevelCrawler.CheckIfPositionIsFree(_crawler.Position + Vector2.up))
					viableVSizes.Add(Vector2.up);
				if (LevelCrawler.CheckIfPositionIsFree(_crawler.Position + Vector2.down))
					viableVSizes.Add(Vector2.down);

				if (viableVSizes.Count == 0) // If there are no viable sizes, just skip this room
					goto ReturnDefault;

				return viableVSizes[Random.Range(0, viableVSizes.Count)];
			}

			if (LevelCrawler.CheckIfPositionIsFree(_crawler.Position + Vector2.left))
				viableVSizes.Add(Vector2.left);
			if (LevelCrawler.CheckIfPositionIsFree(_crawler.Position + Vector2.right))
				viableVSizes.Add(Vector2.right);

			if (viableVSizes.Count == 0) // If there are no viable sizes, just skip this room
				goto ReturnDefault;

			return viableVSizes[Random.Range(0, viableVSizes.Count)];
		}

		// Logic for 4 subRooms:

		List<string> viableSizes = new List<string>(); // Same idea as above, but there are no diagonals for Vectors unfortunately, so I had to use strings

		foreach (var tests in bigRoom_2x2_Tests)
		{
			bool allPassed = true;
			foreach (var test in tests.Value)
			{
				if (!LevelCrawler.CheckIfPositionIsFree(_crawler.Position + test))
					allPassed = false;
			}
			if (allPassed)
				viableSizes.Add(tests.Key);
		}

		if (viableSizes.Count == 0) // If there are no viable sizes, just skip this room
			goto ReturnDefault;

		var chosenSize = viableSizes[Random.Range(0, viableSizes.Count)];
		switch (chosenSize) // Convert the chosen string to a Vector
		{
			case "UpRight":
				return new Vector2(1, 1);
			case "DownRight":
				return new Vector2(1, -1);
			case "DownLeft":
				return new Vector2(-1, -1);
			case "UpLeft":
				return new Vector2(-1, 1);
		}

		ReturnDefault:
		return Vector2.zero;
	}

	static void CrawlBigRoom(RoomInfo _room, LevelCrawler _crawler) // This method adds the subRooms to the crawled positions
	{
		if (_room.Size == new Vector2(1, 1))
		{
			foreach (var value in bigRoom_2x2_Tests["UpRight"])
			{
				LevelCrawler.CrawledPositions.Add(_room.Coordinates + value);
			}
		}
		else if (_room.Size == new Vector2(1, -1))
		{
			foreach (var value in bigRoom_2x2_Tests["DownRight"])
			{
				LevelCrawler.CrawledPositions.Add(_room.Coordinates + value);
			}
		}
		else if (_room.Size == new Vector2(-1, -1))
		{
			foreach (var value in bigRoom_2x2_Tests["DownLeft"])
			{
				LevelCrawler.CrawledPositions.Add(_room.Coordinates + value);
			}
		}
		else if (_room.Size == new Vector2(-1, 1))
		{
			foreach (var value in bigRoom_2x2_Tests["UpLeft"])
			{
				LevelCrawler.CrawledPositions.Add(_room.Coordinates + value);
			}
		}
		else if (_room.Size.x == -1)
		{
			LevelCrawler.CrawledPositions.Add(_room.Coordinates + Vector2.left);
		}
		else if (_room.Size.x == 1)
		{
			LevelCrawler.CrawledPositions.Add(_room.Coordinates + Vector2.right);
		}
		else if (_room.Size.y == 1)
		{
			LevelCrawler.CrawledPositions.Add(_room.Coordinates + Vector2.up);
		}
		else if (_room.Size.y == -1)
		{
			LevelCrawler.CrawledPositions.Add(_room.Coordinates + Vector2.down);
		}

		_crawler.Position += _room.Size;
	}
}
