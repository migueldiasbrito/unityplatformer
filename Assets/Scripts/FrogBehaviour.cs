using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FrogBehaviour : MonoBehaviour
{
	public float IdleTime = 0.0f;
	public float JumpForce = 0.0f;
	public int MaxJumpsOnSameDirection = 0;

	public Transform FeetRectangleTopLeft;
	public Transform FeetRectangleBottomRight;
	public LayerMask GroundLayer;

	private float timeSinceLastAction = 0.0f;
	private Rigidbody2D selfRigidbody;
	private bool canJump = false;
	private int currentDirection = -1;
	private int jumpCount = 0;

	void Start()
    {
		selfRigidbody = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
		timeSinceLastAction += Time.deltaTime;

		if (timeSinceLastAction >= IdleTime)
		{
			canJump = true;
		}
    }

	void FixedUpdate()
	{
		if (canJump)
		{
			timeSinceLastAction = 0;
			canJump = false;
			
			jumpCount++;
			if (jumpCount > MaxJumpsOnSameDirection)
			{
				currentDirection *= -1;
				jumpCount = 1;
			}

			selfRigidbody.AddForce(new Vector2(currentDirection, 1) * JumpForce, ForceMode2D.Impulse);
		}
		else if(Physics2D.OverlapArea(FeetRectangleTopLeft.position, FeetRectangleBottomRight.position, GroundLayer))
		{
			selfRigidbody.velocity = Vector2.zero;
		}
	}
}
