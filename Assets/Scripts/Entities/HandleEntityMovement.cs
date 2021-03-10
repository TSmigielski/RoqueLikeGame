using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(IEntityControls))]
[RequireComponent(typeof(IEntity))]
public class HandleEntityMovement : MonoBehaviour
{
	IEntity myEntity;
	IEntityControls myEntityController;
	Rigidbody2D rb;

	public Vector2 Velocity { get; private set; }

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		myEntity = GetComponent<IEntity>();
		myEntityController = GetComponent<IEntityControls>();
	}

	void FixedUpdate()
	{
		if (myEntity.IsFrozen)
			return;

		rb.AddForce(myEntityController.WalkVector * (myEntity.Speed * 333) * Time.fixedDeltaTime);
	}

	private void Update()
	{
		float angle = Mathf.Atan2(Velocity.y, Velocity.x) * Mathf.Rad2Deg;
		myEntity.Legs.rotation = Quaternion.Euler(0, 0, angle);

		myEntity.Body.rotation = myEntityController.LookVector;
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
