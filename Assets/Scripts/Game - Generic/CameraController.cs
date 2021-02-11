using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	public static CameraController Instance { get; private set; }
	public static Camera MainCamera { get; private set; }

	public int CameraControlSelector { get; set; } // (1 == mouse controlled) ; (2 == stick/arrows controlled) ; (else == default)
	public Vector2 MousePosition { get; set; }
	public Vector2 LookDirection { get; set; }

	public Transform target;
	Vector3 newPos;
	public float baseSpeed;
	private float actualSpeed, timer;

	float camSize, aspect;

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
		MainCamera = GetComponentInChildren<Camera>();
	}

	private void Start()
	{
		InitializeCamera();
	}

	private void Update()
	{
		timer += Time.deltaTime;

		if (timer > .2f)
		{
			actualSpeed = baseSpeed;
		}
		else
		{
			actualSpeed = 19f;
		}

		UpdatePosition();
	}

	public void InitializeCamera()
	{
		camSize = MainCamera.orthographicSize;
		aspect = MainCamera.aspect;
	}

	private void UpdatePosition()
	{
		if (target == null || PlayerController.CurrentRoom == null)
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

		newPos.x = Mathf.Clamp(newPos.x, PlayerController.CurrentRoom.transform.position.x - (PlayerController.CurrentRoom.Dimension.x / 2) + camSize * aspect, PlayerController.CurrentRoom.transform.position.x + (PlayerController.CurrentRoom.Dimension.x / 2) - camSize * aspect);
		newPos.y = Mathf.Clamp(newPos.y, PlayerController.CurrentRoom.transform.position.y - (PlayerController.CurrentRoom.Dimension.y / 2) + camSize, PlayerController.CurrentRoom.transform.position.y + (PlayerController.CurrentRoom.Dimension.y / 2) - camSize);

		transform.position = Vector3.Lerp(transform.position, newPos, actualSpeed * Time.deltaTime);
	}
}
