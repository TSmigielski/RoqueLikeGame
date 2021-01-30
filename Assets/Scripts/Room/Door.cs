using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DoorSide { Top, Bottom, Left, Right }

public class Door : MonoBehaviour
{
	public DoorSide doorSide;
	public Transform walls;

	public void PatchHorizontalDoor()
	{
		GameObject patch = new GameObject("Door Patch");
		patch.transform.position = transform.position;
		patch.transform.SetParent(walls);
		patch.transform.localScale = new Vector3(4f, 1.1f, 1f);

		var bx = patch.AddComponent<BoxCollider2D>();
		bx.usedByComposite = true;
		Destroy(gameObject);
	}

	public void PatchVerticalDoor()
	{
		GameObject patch = new GameObject("Door Patch");
		patch.transform.position = transform.position;
		patch.transform.SetParent(walls);
		patch.transform.localScale = new Vector3(1.1f, 4f, 1f);

		var bx = patch.AddComponent<BoxCollider2D>();
		bx.usedByComposite = true;
		Destroy(gameObject);
	}
}
