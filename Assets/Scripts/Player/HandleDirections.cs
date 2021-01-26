using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IEntityDirections))]
[RequireComponent(typeof(IEntityInformation))]
public class HandleDirections : MonoBehaviour
{
	IEntityDirections controller;
	IEntityInformation myEntity;
	Rigidbody2D rb;

	public Vector2 Velocity { get; private set; }

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		controller = GetComponent<IEntityDirections>();
		myEntity = GetComponent<IEntityInformation>();
	}

	void FixedUpdate()
	{
		rb.AddForce(controller.WalkDirection * (myEntity.Speed * 333) * Time.fixedDeltaTime);
	}

	private void Update()
	{
		float angle = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;
		myEntity.Legs.rotation = Quaternion.Euler(0, 0, angle);

		myEntity.Body.rotation = controller.LookDirection;
	}

	private void LateUpdate()
	{
		if (rb.velocity.magnitude > myEntity.Speed)
		{
			rb.velocity = rb.velocity.normalized * myEntity.Speed;
		}
		Velocity = rb.velocity;
	}
}
