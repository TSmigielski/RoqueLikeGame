using UnityEngine;

//	Those interfaces make it possible to use HandleEntityMovement for both player and npc's

public interface IEntity
{
	int Speed { get; }
	Transform Body { get; }
	Transform Legs { get; }
}

public interface IEntityControls
{
	Vector2 WalkVector { get; }
	Quaternion LookVector { get; }
}