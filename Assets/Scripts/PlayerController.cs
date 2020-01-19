using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
	public float Speed = 0.0f;
	public float JumpForce = 0.0f;

	public Transform FeetRectangleTopLeft;
	public Transform FeetRectangleBottomRight;
	public LayerMask GroundLayer;

	private Rigidbody2D selfRigidbody;
	private float horizontalMovement = 0.0f;
	private bool isGrounded = false;
	private bool canJump = false;

    void Start()
    {
		selfRigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
		horizontalMovement = Input.GetAxis("Horizontal");

		if(Input.GetAxis("Jump") > 0.1f && isGrounded)
		{
			canJump = true;
		}
    }

	void FixedUpdate()
	{
		isGrounded = Physics2D.OverlapArea(FeetRectangleTopLeft.position, FeetRectangleBottomRight.position,
			GroundLayer);

		if (canJump)
		{
			canJump = false;
			isGrounded = false;
			selfRigidbody.AddForce(new Vector2(0, 1) * JumpForce, ForceMode2D.Impulse);
		}

		selfRigidbody.velocity = new Vector2(Speed * horizontalMovement, selfRigidbody.velocity.y);
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("House"))
		{
			SceneManager.LoadScene("SampleScene");
		}

		if (col.CompareTag("Death"))
		{
			SceneManager.LoadScene("SampleScene");
		}
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.transform.CompareTag("Death"))
		{
			SceneManager.LoadScene("SampleScene");
		}
	}
}
