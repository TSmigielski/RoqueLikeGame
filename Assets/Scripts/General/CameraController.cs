using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
	public static CameraController Instance { get; private set; }
	public static Camera MainCamera { get; private set; }
	[SerializeField] private Room _room;
	public Room MyRoom
	{
		get { return _room; }
		set { _room = value; timer = 0f; }
	}

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
			actualSpeed = 18f;
		}

		UpdatePosition();
	}

	private void UpdatePosition()
	{
		if (target == null || MyRoom == null)
			return;

		var newPos = target.position;
		newPos.z = transform.position.z;

		newPos.x = Mathf.Clamp(newPos.x, MyRoom.GetRoomCenter().x - (MyRoom.Dimension.x / 3.95f), MyRoom.GetRoomCenter().x + (MyRoom.Dimension.x / 3.95f));
		newPos.y = Mathf.Clamp(newPos.y, MyRoom.GetRoomCenter().y - (MyRoom.Dimension.y / 4f), MyRoom.GetRoomCenter().y + (MyRoom.Dimension.y / 4f));

		transform.position = Vector3.Lerp(transform.position, newPos, actualSpeed * Time.deltaTime);
	}
}
