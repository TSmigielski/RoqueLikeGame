using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IEntityDirections
{
	public static InputManager Instance { get; private set; }
	Controls controls;

	public Vector2 WalkDirection { get; private set; }
	public Quaternion LookDirection { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			Destroy(gameObject);
		}

		controls = new Controls();
	}

	private void OnEnable()
	{
		controls.Enable();
		controls.Player.Movement.performed += OnMoveInput;
		controls.Player.LookDirection.performed += OnLookInput;
	}

	private void OnDisable()
	{
		controls.Disable();
		controls.Player.Movement.performed -= OnMoveInput;
		controls.Player.LookDirection.performed -= OnLookInput;
	}

	private void Update()
	{
		LookDirection = CalculateLookDirection(controls.Player.MousePosition.ReadValue<Vector2>()); // TODO - switch between mouse and other input
	}

	private void OnMoveInput(InputAction.CallbackContext ctx)
	{
		var value = ctx.ReadValue<Vector2>();
		if (value.magnitude > 1f)
		{
			value = value.normalized;
		}
		WalkDirection = value;
	}

	private void OnLookInput(InputAction.CallbackContext ctx)
	{
		var value = ctx.ReadValue<Vector2>();

		float angle = Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg;
		LookDirection = Quaternion.Euler(0, 0, angle);
	}

	private void OnMouseInput(InputAction.CallbackContext ctx)
	{
		LookDirection = CalculateLookDirection(ctx.ReadValue<Vector2>());
	}

	private Quaternion CalculateLookDirection(Vector2 direction)
	{
		Vector2 lookDirection = Camera.main.ScreenToWorldPoint(direction) - transform.position;
		float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
		return Quaternion.Euler(0, 0, angle);
	}
}
