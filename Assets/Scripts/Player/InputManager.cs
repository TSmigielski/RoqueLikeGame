using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IMoveDirection
{
	public static InputManager Instance { get; private set; }
	Controls controls;

	public Vector2 MoveDirection { get; private set; }

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
	}

	private void OnDisable()
	{
		controls.Disable();
		controls.Player.Movement.performed -= OnMoveInput;
	}

	private void OnMoveInput(InputAction.CallbackContext ctx)
	{
		var value = ctx.ReadValue<Vector2>();
		if (value.magnitude > 1f)
		{
			value = value.normalized;
		}
		MoveDirection = value;
	}
}
