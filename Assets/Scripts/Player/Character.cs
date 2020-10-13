using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "John Rambo", menuName = "Player/New Character")]
public class Character : ScriptableObject
{
	[Header("General")]
	public string firstName;
	public string lastName;
	public Sprite menuImage;

	[TextArea(1, 4)] public string description;

	[Header("Base Stats")]
	[Range(1, 10)] public int speed;
	[Range(1, 10)] public int inteligence;
	[Range(1, 10)] public int strength;
	[Range(1, 10)] public int endurance;
	[Range(1, 10)] public int immunity;
}
