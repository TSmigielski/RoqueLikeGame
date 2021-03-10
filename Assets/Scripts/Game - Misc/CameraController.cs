using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	public static CameraController Instance { get; private set; } // Singleton
	public static Camera MainCamera { get; private set; } // Static camera variable is way faster than Camera.main

	public int CameraControlSelector { get; set; } // (1 == mouse controlled) ; (2 == stick/arrows controlled) ; (else == default)
	public Vector2 MousePosition { get; set; }
	public Vector2 LookDirection { get; set; }

	[SerializeField] Transform target;
	[SerializeField] float baseSpeed, transitionSpeed;

	private Vector3 newPos;
	private float actualSpeed, timer;
	private float camSize, aspect;

	private void Awake()
	{
		if (Instance == null)
			Instance = this;
		else
			Destroy(gameObject);

		MainCamera = GetComponentInChildren<Camera>();
	}

	private void Start()
	{
		InitializeCamera(); // todo - InitializeCamera again on resolution or aspect ratio change
	}

	private void Update()
	{
		timer += Time.deltaTime;

		if (timer > .2f)				   // Change camera speed
			actualSpeed = baseSpeed;	   // in transition.
		else                               // It is based on
			actualSpeed = transitionSpeed; // the timer reset.

		UpdatePosition();
	}

	public void InitializeCamera()
	{
		camSize = MainCamera.orthographicSize;
		aspect = MainCamera.aspect;
	}

	private void UpdatePosition()
	{
		if (target == null || RoomManager.PlayerRoom == null)
			return;

		if (CameraControlSelector == 1)
		{
			newPos = Vector3.Lerp(target.position, MainCamera.ScreenToWorldPoint(MousePosition), .3f);
		}
		else if (CameraControlSelector == 2)
		{
			newPos = Vector3.Lerp(target.position, (LookDirection * 10) + (Vector2)target.position, .3f);
		}
		else
		{
			newPos = target.position;
		}

		newPos.z = transform.position.z;

		// The following code involves a lot of math, I don't want to try to understand it, I just took it from the internet, gimme a break!
		newPos.x = Mathf.Clamp(newPos.x, RoomManager.PlayerRoom.transform.position.x - (RoomManager.PlayerRoom.Info.TrueSize.x / 2) + camSize * aspect, RoomManager.PlayerRoom.transform.position.x + (RoomManager.PlayerRoom.Info.TrueSize.x / 2) - camSize * aspect);
		newPos.y = Mathf.Clamp(newPos.y, RoomManager.PlayerRoom.transform.position.y - (RoomManager.PlayerRoom.Info.TrueSize.y / 2) + camSize, RoomManager.PlayerRoom.transform.position.y + (RoomManager.PlayerRoom.Info.TrueSize.y / 2) - camSize);

		transform.position = Vector3.Lerp(transform.position, newPos, actualSpeed * Time.deltaTime); // This is just a normal lerp (not the intended way to use it)
	}
}
