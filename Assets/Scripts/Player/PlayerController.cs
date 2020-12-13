using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IMoveSpeed
{
	#region Stats
	[Header("Stats")]
	[SerializeField] private string _firstName;
	public string FirstName
	{
		get { return _firstName; }
		set { _firstName = value; }
	}

	[SerializeField] private string _lastName;
	public string LastName
	{
		get { return _lastName; }
		set { _lastName = value; }
	}

	[SerializeField] private Sprite _menuImage;
	public Sprite MenuImage
	{
		get { return _menuImage; }
		set { _menuImage = value; }
	}

	[SerializeField] private string _description;
	public string Description
	{
		get { return _description; }
		set { _description = value; }
	}

	[SerializeField] private int _speed;
	public int Speed
	{
		get { return _speed; }
		set { _speed = value; }
	}

	[SerializeField] private int _inteligence;
	public int Inteligence
	{
		get { return _inteligence; }
		set { _inteligence = value; }
	}

	[SerializeField] private int _strength;
	public int Strength
	{
		get { return _strength; }
		set { _strength = value; }
	}

	[SerializeField] private int _endurance;
	public int Endurance
	{
		get { return _endurance; }
		set { _endurance = value; }
	}

	[SerializeField] private int _immunity;
	public int Immunity
	{
		get { return _immunity; }
		set { _immunity = value; }
	}
	#endregion

	public static List<Room> VisitedRooms { get; set; }
	public static Room MyRoom { get; set; }

	[Header("Other")]
	public Animator animator;
	public Transform legs, body, head;
	public float maxBodyRotation = 30f, maxHeadRotation = 80f;
	HandleMovement movement;
	Vector2 velocity;

	private void Awake()
	{
		movement = GetComponent<HandleMovement>();
		VisitedRooms = new List<Room>();
	}

	private void Start()
	{
		InitializeCharacter();
	}

	private void Update()
	{
		velocity = movement.Velocity;

		animator.SetFloat("VelocityMag", velocity.magnitude);
		animator.SetFloat("Walk Speed", velocity.magnitude / 3f);

		RotatePlayer();
	}

	private void RotatePlayer()
	{
		if (velocity.magnitude > .01f) //Legs
		{
			Vector2 moveDir = (Vector2)transform.position + velocity - (Vector2)transform.position;
			float angle1 = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg + 90f;
			legs.rotation = Quaternion.Euler(0f, 0f, angle1);
		}

		//Vector2 mouseDir = transform.position - Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
		//float angle2 = Mathf.Atan2(mouseDir.y, mouseDir.x) * Mathf.Rad2Deg - 270f;
		//body.eulerAngles = new Vector3(0f, 0f, angle2);
		//body.localRotation = Quaternion.Euler(0f, 0f, Mathf.Clamp(body.localRotation.eulerAngles.z - 180f, -maxBodyRotation, maxBodyRotation));;
	}

	public void InitializeCharacter()
	{
		FirstName = MyData.Instance.MyCharacter.firstName;
		LastName = MyData.Instance.MyCharacter.lastName;
		Description = MyData.Instance.MyCharacter.description;
		Speed = MyData.Instance.MyCharacter.speed;
		Inteligence = MyData.Instance.MyCharacter.inteligence;
		Strength = MyData.Instance.MyCharacter.strength;
		Endurance = MyData.Instance.MyCharacter.endurance;
		Immunity = MyData.Instance.MyCharacter.immunity;
	}
}
