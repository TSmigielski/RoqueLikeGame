using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapController : MonoBehaviour
{
	public static MinimapController Instance { get; set; }

	public GameObject roomPrefab;
	public GameObject container;
	public static List<MinimapRoom> minimapRooms = new List<MinimapRoom>();

	private void Awake()
	{
		if (Instance == null) Instance = this;
		else Destroy(gameObject);
	}

	public void UpdateMinimap()
	{
		var myR = PlayerController.MyRoom;

		if (myR.MyMMRoom != null)
		{
			myR.MyMMRoom.FocusRoom();
			return;
		}

		var mmR = Instantiate(roomPrefab, container.transform).GetComponent<MinimapRoom>();
		minimapRooms.Add(mmR);
		myR.MyMMRoom = mmR;
		mmR.FocusRoom();
	}
}
