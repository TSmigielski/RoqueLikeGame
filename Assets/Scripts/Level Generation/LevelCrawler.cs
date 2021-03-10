using System.Collections.Generic;
using UnityEngine;

public enum Direction
{
	up = 0,
	right = 1,
	down = 2,
	left = 3
}

public class LevelCrawler
{
	public Vector2 Position { get; set; } // Current position of a crawler
	public static List<Vector2> CrawledPositions { get; set; } = new List<Vector2>(); // List of every position that has been visited by a crawler

	public LevelCrawler(Vector2 _startPosition)
	{
		Position = _startPosition;
		CrawledPositions.Add(Position);
		if (!CrawledPositions.Contains(Vector2.zero)) // If this has not been added already by another (the first one) crawler already,
			CrawledPositions.Add(Vector2.zero);       // Add a crawled position for the starting room's position.
	}

	public Vector2 NewPosition()
	{
		var triedNums = new List<int>();
		int num;

		NewNumber:
		num = Random.Range(0, 4); // Each number represents a 'direction' in Direction, the 4 is basically Direction.Length or Direction.Count

		if (triedNums.Contains(0) && triedNums.Contains(1) && triedNums.Contains(2) && triedNums.Contains(3)) // If every direction has been tried already
			return Vector2.zero;																			  // Return zero

		else if (triedNums.Contains(num)) // Else if only Direction[num] has been tried,
			goto NewNumber;               // Try to get a new direction

		triedNums.Add(num);

		var tmp = Direction2Vector((Direction)num) + Position; // Store the new position

		if (!CheckIfPositionIsFree(tmp)) // Check if the new position is occupied
			goto NewNumber;				 // If so, try to get a new position

		Position = tmp;					// Assign the new position to Position
		CrawledPositions.Add(Position); // Add the new Position to the List of visited positions
		return Position;				// And return the position
	}

	// The next 3 methods are very self explanatory, so no comments there.

	public static Vector2 Direction2Vector(Direction _direction)
	{
		switch(_direction)
		{
			case Direction.up:
				return Vector2.up;
			case Direction.right:
				return Vector2.right;
			case Direction.down:
				return Vector2.down;
			case Direction.left:
				return Vector2.left;
			default:
				return Vector2.zero;
		}
	}

	public static bool CheckIfPositionIsFree(Vector2 _position)
	{
		if (CrawledPositions.Contains(_position))
			return false;
		return true;
	}

	public static void ResetCrawledPositions()
	{
		CrawledPositions = new List<Vector2>();
	}
}
