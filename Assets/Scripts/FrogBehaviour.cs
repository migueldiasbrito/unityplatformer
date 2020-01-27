using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class FrogBehaviour : MonoBehaviour
{
	public float IdleTime = 0.0f;
	public float JumpForce = 0.0f;
	public int MaxJumpsOnSameDirection = 0;

	public Transform FeetRectangleTopLeft;
	public Transform FeetRectangleBottomRight;
	public LayerMask GroundLayer;

	public AudioClip DieClip;
	public AudioClip JumpClip;

	private float timeSinceLastAction = 0.0f;
	private Rigidbody2D selfRigidbody;
	private Animator selfAnimator;
	private Renderer selfRenderer;
	private AudioSource selfAudioSource;
	private bool canJump = false;
	private int currentDirection = -1;
	private int jumpCount = 0;

	void Start()
    {
		selfRigidbody = GetComponent<Rigidbody2D>();
		selfAnimator = GetComponent<Animator>();
		selfAudioSource = GetComponent<AudioSource>();
		selfRenderer = GetComponent<Renderer>();
	}

    void Update()
    {
		timeSinceLastAction += Time.deltaTime;

		if (timeSinceLastAction >= IdleTime)
		{
			canJump = true;
			if (selfRenderer.isVisible)
			{
				selfAudioSource.PlayOneShot(JumpClip);
			}
		}

		selfAnimator.SetFloat("Velocity", selfRigidbody.velocity.y);
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
				transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
				jumpCount = 1;
			}

			selfRigidbody.AddForce(new Vector2(currentDirection, 1) * JumpForce, ForceMode2D.Impulse);
			
			selfAnimator.SetBool("IsGrounded", false);
		}
		else if(Physics2D.OverlapArea(FeetRectangleTopLeft.position, FeetRectangleBottomRight.position, GroundLayer))
		{
			selfRigidbody.velocity = Vector2.zero;

			selfAnimator.SetBool("IsGrounded", true);
		}
	}

	void Die()
	{
		Destroy(gameObject);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.GetComponent<StarBehaviour>())
		{
			if (selfRenderer.isVisible)
			{
				selfAnimator.SetBool("Die", true);
			}
			GetComponent<Collider2D>().enabled = false;
			selfAudioSource.PlayOneShot(DieClip);
		}
	}
}
