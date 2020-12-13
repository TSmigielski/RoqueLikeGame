using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MinimapRoom : MonoBehaviour
{
	[SerializeField] private Color _roomColor = default;
	public Color DefaultRoomColor
	{
		get { return _roomColor; }
	}

	[SerializeField] private Color _visitedRoomColor = default;
	public Color VisitedRoomColor
	{
		get { return _visitedRoomColor; }
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
		image.color = VisitedRoomColor;
	}

	public void FocusRoom()
	{
		var rs = MinimapController.minimapRooms;

		foreach (var r in rs)
		{
			r.image.color = DefaultRoomColor;
		}
		image.color = VisitedRoomColor;

		foreach (var r in rs)
		{
			r.transform.localPosition = new Vector2(RoomController.roomX, RoomController.roomY) * r.MyRoom.Coordinates;
		}

		Vector2 total = Vector2.zero;
		foreach (var r in rs)
		{
			total += (Vector2)r.transform.localPosition;
		}
		total /= rs.Count;

		foreach (var r in rs)
		{
			r.transform.localPosition -= (Vector3)total;
		}
	}
}
