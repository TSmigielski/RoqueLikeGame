using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IMoveDirection))]
[RequireComponent(typeof(IMoveSpeed))]
public class HandleMovement : MonoBehaviour
{
	IMoveDirection controller;
	IMoveSpeed speed;
	Rigidbody2D rb;

	public Vector2 Velocity { get; private set; }

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		controller = GetComponent<IMoveDirection>();
		speed = GetComponent<IMoveSpeed>();
	}

	void FixedUpdate()
	{
		rb.AddForce(controller.MoveDirection * (speed.Speed * 333) * Time.fixedDeltaTime);
	}

	private void LateUpdate()
	{
		if (rb.velocity.magnitude > speed.Speed)
		{
			rb.velocity = rb.velocity.normalized * speed.Speed;
		}
		Velocity = rb.velocity;
	}
}
