using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum WallSide { Top, Bottom, Left, Right }

public class Wall : MonoBehaviour
{
	public WallSide wallSide;
	public BoxCollider2D[] myColliders;
	public Door myDoor;

	public void DestroyWall()
	{
		foreach (var bx in myColliders)
		{
			Destroy(bx.gameObject);
		}

		foreach (var r in FindObjectsOfType<Room>())
		{
			if (r.walls.Contains(this))
			{
				r.doors.Remove(myDoor);
				break;
			}
		}

		Destroy(myDoor.gameObject);
		Destroy(gameObject);
	}
}
