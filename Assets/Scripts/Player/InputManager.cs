using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour, IEntityControls
{
	public static InputManager Instance { get; private set; }
	Controls controls;

	bool mouseDetected = false;

	public Vector2 WalkVector { get; private set; }
	public Quaternion LookVector { get; private set; }

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
		controls.Player.MousePosition.performed += OnMouseInput;
		controls.Player.MinimapToggle.performed += OnMinimapToggle;
	}

	private void OnDisable()
	{
		controls.Disable();
		controls.Player.Movement.performed -= OnMoveInput;
		controls.Player.LookDirection.performed -= OnLookInput;
		controls.Player.MousePosition.performed -= OnMouseInput;
		controls.Player.MinimapToggle.performed -= OnMinimapToggle;
	}

	private void Update()
	{
		OnMouseInputLogic();

		if (!mouseDetected && controls.Player.LookDirection.ReadValue<Vector2>() == Vector2.zero)
		{
			CameraController.Instance.CameraControlSelector = 0;
			if (WalkVector != Vector2.zero)
			{
				var rotation = Quaternion.LookRotation(Vector3.forward, WalkVector);
				rotation.eulerAngles += new Vector3(0, 0, 90);
				LookVector = rotation;
			}
		}
	}

	private void OnMoveInput(InputAction.CallbackContext ctx)
	{
		var value = ctx.ReadValue<Vector2>();
		if (value.magnitude > 1f)
		{
			value = value.normalized;
		}
		WalkVector = value;
	}

	private void OnLookInput(InputAction.CallbackContext ctx)
	{
		mouseDetected = false;

		var value = ctx.ReadValue<Vector2>();

		float angle = Mathf.Atan2(value.y, value.x) * Mathf.Rad2Deg;
		LookVector = Quaternion.Euler(0, 0, angle);

		CameraController.Instance.CameraControlSelector = 2;
		CameraController.Instance.LookDirection = value;
	}

	private void OnMouseInput(InputAction.CallbackContext ctx)
	{
		mouseDetected = true;
	}

	private void OnMouseInputLogic()
	{
		if (!mouseDetected)
			return;

		var value = controls.Player.MousePosition.ReadValue<Vector2>();
		LookVector = CalculateLookDirection(value);

		CameraController.Instance.CameraControlSelector = 1;
		CameraController.Instance.MousePosition = value;
	}

	private Quaternion CalculateLookDirection(Vector2 direction)
	{
		Vector2 lookDirection = Camera.main.ScreenToWorldPoint(direction) - transform.position;
		float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
		return Quaternion.Euler(0, 0, angle);
	}

	private void OnMinimapToggle(InputAction.CallbackContext ctx)
	{
		//MinimapController.Instance.ToggleMinimap();
	}
}
