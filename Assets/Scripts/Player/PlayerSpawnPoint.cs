using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawnPoint : MonoBehaviour
{
	private void Start()
	{
		var player = FindObjectOfType<PlayerController>();
		if (player == null)
		{
			Debug.LogWarning("No player detected, this probably isn't the main scene, or something went horribly wrong!");
			return;
		}

		player.transform.position = transform.position;
	}
}
