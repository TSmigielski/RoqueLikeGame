using UnityEngine;

public interface IEntityDirections
{
	Vector2 WalkDirection { get; }
	Quaternion LookDirection { get; }
}

public interface IEntityInformation
{
	int Speed { get; }
	Transform Body { get; }
	Transform Legs { get; }
}