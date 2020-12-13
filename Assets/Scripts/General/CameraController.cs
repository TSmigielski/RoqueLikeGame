using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	public static CameraController Instance { get; private set; }
	public static Camera MainCamera { get; private set; }

	public Transform target;
	public float baseSpeed;
	private float actualSpeed, timer;

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
		MainCamera = GetComponent<Camera>();
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

	private void UpdatePosition()
	{
		var myR = PlayerController.MyRoom;

		if (target == null || myR == null)
			return;

		var newPos = target.position;
		newPos.z = transform.position.z;
		var camSize = MainCamera.orthographicSize;
		var aspect = MainCamera.aspect;

		newPos.x = Mathf.Clamp(newPos.x, myR.transform.position.x - (myR.Dimension.x / 2) + camSize * aspect, myR.transform.position.x + (myR.Dimension.x / 2) - camSize * aspect);
		newPos.y = Mathf.Clamp(newPos.y, myR.transform.position.y - (myR.Dimension.y / 2) + camSize, myR.transform.position.y + (myR.Dimension.y / 2) - camSize);

		transform.position = Vector3.Lerp(transform.position, newPos, actualSpeed * Time.deltaTime);
	}
}
