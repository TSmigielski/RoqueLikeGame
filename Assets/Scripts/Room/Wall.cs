using UnityEngine;

public class Wall : MonoBehaviour
{
	// This script has 2 purposes,
	// Hold the 3 public components (from below),
	// For easy access from the Room script,
	// And to know it's side,
	// To safe the Room script from making 1 extra calculation.

	[SerializeField] private Direction _side;
	public Direction Side
	{
		get { return _side; }
		set { _side = value; }
	}

	public SpriteRenderer sR;
	public BoxCollider2D cL;
	public Transform corner;
}
