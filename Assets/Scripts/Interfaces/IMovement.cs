using UnityEngine;

public interface IMoveDirection
{
	Vector2 MoveDirection { get; }
}

public interface IMoveSpeed
{
	int Speed { get; }
}