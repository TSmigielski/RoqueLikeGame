using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class MinimapRoom : MonoBehaviour
{
	public static Color NotVisitedColor { get; set; }
	public static Color VisitedColor { get; set; }
	public static Color CurrentColor { get; set; }
	public Vector2 Coordinates { get; set; }

	private bool visited = false;
	private Image image;

	private void Awake()
	{
		image = GetComponent<Image>();
		image.color = NotVisitedColor;
		image.enabled = false;
	}

	public void UpdateRoom()
	{
		if (RoomManager.PlayerRoom.Info.Coordinates == Coordinates)
		{
			visited = true;
			image.color = CurrentColor;
			Reveal();
		}
		else if (visited)
			image.color = VisitedColor;
		else
			image.color = NotVisitedColor;
	}

	private void Reveal()
	{
		// For every neighbour of the room for this minimapRoom
		// Get the minimapRoom for that room (not confusing at all!)'
		// Then just enable the Image component of that room

		image.enabled = true;
		foreach (var roomArray in RoomManager.GetRoom_ByCoordinates(Coordinates).Info.Neighbours.Values)
		{
			foreach (var room in roomArray)
			{
				var mR = MinimapController.GetMinimapRoom(room.Coordinates);
				if (mR != null)
					MinimapController.GetMinimapRoom(room.Coordinates).image.enabled = true;
			}
		}
	}
}
