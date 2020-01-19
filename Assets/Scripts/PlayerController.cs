using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	public float Speed = 0.0f;

	private Rigidbody2D selfRigidbody2D;
	private float horizontalMovement = 0.0f;

    void Start()
    {
		selfRigidbody2D = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
		horizontalMovement = Input.GetAxis("Horizontal");
    }

	void FixedUpdate()
	{
		selfRigidbody2D.velocity = new Vector2(Speed * horizontalMovement, selfRigidbody2D.velocity.y);
	}
}
