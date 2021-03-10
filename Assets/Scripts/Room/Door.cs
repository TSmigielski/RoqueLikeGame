using UnityEngine;

public class Door : MonoBehaviour
{
	public Transform tpPoint; // tp = teleport
	public BoxCollider2D cL; // cL = collider
	public SpriteMask mask;
	public float tpDistance = 2f; // perpendicular to the wall in the direction of neighbour

	static bool playerBeingMoved = false; // static bool to prevent other doors to take over
	bool movingPlayer = false; // local bool so the script will know when to go
	PlayerController player;
	float timer = 0f;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.CompareTag("Player") || playerBeingMoved)
			return;

		player = collision.GetComponent<PlayerController>();
		playerBeingMoved = true;
		movingPlayer = true;
		player.IsFrozen = true;
	}

	private void Update()
	{
		if (!movingPlayer)
			return;

		timer += Time.deltaTime;
		Mathf.Clamp(timer, 0f, 1f);

		player.transform.position = Vector3.Lerp(player.transform.position, tpPoint.position, timer);
		if (Vector3.Distance(player.transform.position, tpPoint.position) <= .1f)
		{
			movingPlayer = false;
			playerBeingMoved = false;
			player.IsFrozen = false;
			timer = 0;
		}
	}
}
