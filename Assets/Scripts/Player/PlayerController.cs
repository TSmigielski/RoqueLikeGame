using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IEntityInformation
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

	#region Static Entity Information
	[Header("Static Information")]
	[SerializeField] private Transform _body;
	public Transform Body
	{
		get { return _body; }
		set { _body = value; }
	}
	[SerializeField] private Transform _legs;
	public Transform Legs
	{
		get { return _legs; }
		set { _legs = value; }
	}
	#endregion

	public static List<Room> VisitedRooms { get; set; }
	public static Room MyRoom { get; set; }

	[Header("Other")]
	public Animator animator;
	HandleDirections directions;
	Vector2 velocity;

	private void Awake()
	{
		directions = GetComponent<HandleDirections>();
		VisitedRooms = new List<Room>();
	}

	private void Start()
	{
		InitializeCharacter();
	}

	private void Update()
	{
		velocity = directions.Velocity;

		animator.SetFloat("VelocityMag", velocity.magnitude);
		animator.SetFloat("Walk Speed", velocity.magnitude / 5f);
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
