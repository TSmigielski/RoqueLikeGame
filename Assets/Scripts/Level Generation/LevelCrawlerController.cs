using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
	Up = 0,
	Left = 1,
	Down = 2,
	Right = 3
};

public class LevelCrawlerController : MonoBehaviour
{
	public static List<Vector2Int> positionsVisited = new List<Vector2Int>();
	public static readonly Dictionary<Direction, Vector2Int> directionMovementMap = new Dictionary<Direction, Vector2Int>()
	{
		{ Direction.Up, Vector2Int.up},
		{ Direction.Left, Vector2Int.left},
		{ Direction.Down, Vector2Int.down},
		{ Direction.Right, Vector2Int.right}
	};
	
	public static List<Vector2Int> GenerateLevel(LevelGenerationData _data)
	{
		List<LevelCrawler> levelCrawlers = new List<LevelCrawler>();

		for (int i = 0; i < _data.numberOfCrawlers; i++)
		{
			levelCrawlers.Add(new LevelCrawler(Vector2Int.zero));
		}

		positionsVisited.Add(Vector2Int.zero);

		foreach (var crawler in levelCrawlers)
		{
			for (int i = 0; i < Random.Range(_data.iterationMinMax.x, _data.iterationMinMax.y); i++)
			{
				Vector2Int newPos;
				bool freeSpaces = false;

				foreach (var direction in directionMovementMap)
				{
					if (positionsVisited.Contains(crawler.Move(direction.Key)))
						freeSpaces = true;
				}

				if (!freeSpaces)
					break;

				do
				{
					newPos = crawler.RandomMove();
				} while (positionsVisited.Contains(newPos));

				positionsVisited.Add(newPos);
			}
		}

		return positionsVisited;
	}
}
