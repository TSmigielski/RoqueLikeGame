using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MinimapRoom : MonoBehaviour
{
	[SerializeField] private Color _unvisitedRoomColor = default;
	public Color UnvisitedRoomColor
	{
		get { return _unvisitedRoomColor; }
	}

	[SerializeField] private Color _visitedRoomColor = default;
	public Color VisitedRoomColor
	{
		get { return _visitedRoomColor; }
	}

	[SerializeField] private Color _currentRoomColo = default;
	public Color CurrentRoomColor
	{
		get { return _currentRoomColo; }
	}

	private Room _myRoom;
	public Room MyRoom
	{
		get { return _myRoom; }
		set
		{
			_myRoom = value;

			Vector2 size;

			if (_myRoom.Dimension.x == 36)
			{
				size.x = 34;
			}
			else
			{
				size.x = 70;
			}

			if (_myRoom.Dimension.y == 20)
			{
				size.y = 18;
			}
			else
			{
				size.y = 38;
			}

			GetComponent<RectTransform>().sizeDelta = size;
		}
	}

	public Image image;

	private void Awake()
	{
		image = GetComponent<Image>();
		image.color = UnvisitedRoomColor;
	}

	public void CenterRoom(List<MinimapRoom> rooms, Transform minimapRoomPlain)
	{
		foreach (var r in rooms) // Change the color of every room
		{
			if (r.MyRoom.PlayerVisited)
			{
				r.image.color = VisitedRoomColor;
			}
			else
			{
				r.image.color = UnvisitedRoomColor;
			}
		}
		image.color = CurrentRoomColor; // Change color of current room

		minimapRoomPlain.localPosition = new Vector3(-transform.localPosition.x, -transform.localPosition.y, 0); // Change position of the rooms plain
	}
}
