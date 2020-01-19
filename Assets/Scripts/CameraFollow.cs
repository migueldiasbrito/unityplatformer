using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	private Vector2 velocity;
	private GameObject player;

	public float MinX;
	public float MinY;
	public float MaxX;
	public float MaxY;

	public float SmoothTimeX;
	public float SmoothTimeY;

	void Start()
    {
		player = GameObject.FindGameObjectWithTag("Player");
    }

    void FixedUpdate()
    {
		float x = Mathf.Min(MaxX, Mathf.Max(MinX,
			Mathf.SmoothDamp(transform.position.x, player.transform.position.x, ref velocity.x, SmoothTimeX)));
		float y = Mathf.Min(MaxY, Mathf.Max(MinY,
			Mathf.SmoothDamp(transform.position.y, player.transform.position.y, ref velocity.y, SmoothTimeY)));

		transform.position = new Vector3(x, y, transform.position.z);
	}
}
