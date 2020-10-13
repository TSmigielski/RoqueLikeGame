using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCrawler
{
	public Vector2Int Position { get; set; }

	public LevelCrawler(Vector2Int _startPos)
	{
		Position = _startPos;
	}

	public Vector2Int Move (Direction _direction)
	{
		Position += LevelCrawlerController.directionMovementMap[_direction];
		return Position;
	}

	public Vector2Int RandomMove()
	{
		Direction toMove = (Direction)Random.Range(0, LevelCrawlerController.directionMovementMap.Count);
		Position += LevelCrawlerController.directionMovementMap[toMove];
		return Position;
	}
}
