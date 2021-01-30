using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyData : MonoBehaviour
{
	public static MyData Instance { get; private set; }

	[SerializeField] private Character _myCharacter;
	public Character MyCharacter
	{
		get { return _myCharacter; }
		set { _myCharacter = value; }
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			Destroy(gameObject);
		}
	}
}
