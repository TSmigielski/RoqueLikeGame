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

#if (UNITY_STANDALONE)
		controls.Player.MousePosition.performed += OnMouseInput;
#endif
	}

	private void OnDisable()
	{
		controls.Disable();
		controls.Player.Movement.performed -= OnMoveInput;
		controls.Player.LookDirection.performed -= OnLookInput;
		controls.Player.MousePosition.performed -= OnMouseInput;
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
		Vector2 mouseDir = Camera.main.ScreenToWorldPoint(ctx.ReadValue<Vector2>()) - transform.position;
		float angle = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg;
		LookDirection = Quaternion.Euler(0, 0, angle);
	}
}
