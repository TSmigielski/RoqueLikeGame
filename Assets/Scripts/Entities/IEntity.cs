using UnityEngine;

//	Those interfaces make it possible to use HandleEntityMovement for both the player and npc's

public interface IEntity
{
	int Speed { get; }
	Transform Body { get; }
	Transform Legs { get; }
	bool IsFrozen { get; set; }
}

public interface IEntityControls
{
	Vector2 WalkVector { get; }
	Quaternion LookVector { get; } // Yes, this quaternion is named a Vector, when I wrote this, it was late, okay?
}