using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class PlayerController : MonoBehaviour
{
	public float Speed = 0.0f;
	public float JumpForce = 0.0f;

	public Transform FeetRectangleTopLeft;
	public Transform FeetRectangleBottomRight;
	public LayerMask GroundLayer;

	public GameObject StarPrefab;
	public float RecoilTime = 0.0f;

	public AudioClip FireClip;
	public float FootstepInterval = 0.0f;
	public AudioClip Footstep1;
	public AudioClip Footstep2;
	public AudioClip Jump;
	public AudioClip Die;

	private Rigidbody2D selfRigidbody;
	private Animator selfAnimator;
	private AudioSource selfAudioSource;
	private float horizontalMovement = 0.0f;
	private bool isGrounded = false;
	private bool canJump = false;
	private float timeSinceLastFire = 0.0f;
	private bool isDead = false;
	private float footstepTimeCounter = 0.0f;
	private int footstepStepCounter = 0;

	void Start()
    {
		selfRigidbody = GetComponent<Rigidbody2D>();
		selfAnimator = GetComponent<Animator>();
		selfAudioSource = GetComponent<AudioSource>();
		timeSinceLastFire = RecoilTime;
    }

    void Update()
    {
		if (!isDead)
		{
			horizontalMovement = Input.GetAxis("Horizontal");

			selfAnimator.SetBool("Running", Mathf.Abs(horizontalMovement) > 0.1f);

			if ((horizontalMovement > 0.1f && transform.localScale.x < 0) ||
				(horizontalMovement < -0.1f && transform.localScale.x > 0))
			{
				transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
			}

			if (selfAnimator.GetBool("JumpingUp") && selfRigidbody.velocity.y < 0)
			{
				selfAnimator.SetBool("JumpingUp", false);
				selfAnimator.SetBool("JumpingDown", true);
			}

			if (selfAnimator.GetBool("JumpingDown") && isGrounded)
			{
				selfAnimator.SetBool("JumpingDown", false);
			}

			if (Input.GetAxis("Jump") > 0.1f && isGrounded && !selfAnimator.GetBool("JumpingUp"))
			{
				canJump = true;
				selfAnimator.SetBool("JumpingUp", true);
				selfAudioSource.PlayOneShot(Jump);
			}

			if (Mathf.Abs(horizontalMovement) > 0.1f && !selfAnimator.GetBool("JumpingUp") && !selfAnimator.GetBool("JumpingDown"))
			{
				footstepTimeCounter += Time.deltaTime;
				if (footstepTimeCounter >= FootstepInterval)
				{
					if (footstepStepCounter == 0)
					{
						selfAudioSource.PlayOneShot(Footstep1);
						footstepStepCounter = 1;
					}
					else
					{
						selfAudioSource.PlayOneShot(Footstep2);
						footstepStepCounter = 0;
					}

					footstepTimeCounter = 0;
				}
			}
			else
			{
				footstepTimeCounter = FootstepInterval;
				footstepStepCounter = 0;
			}

			timeSinceLastFire += Time.deltaTime;
			if (Input.GetAxis("Fire1") > 0.1f && timeSinceLastFire >= RecoilTime)
			{
				GameObject newStar = Instantiate(StarPrefab);
				newStar.transform.position = transform.position;
				newStar.GetComponent<StarBehaviour>().Speed *= (transform.localScale.x > 0) ? 1 : -1;
				timeSinceLastFire = 0.0f;
				selfAnimator.SetBool("Firing", true);
				selfAudioSource.PlayOneShot(FireClip);
			}
			else
			{
				selfAnimator.SetBool("Firing", false);
			}
		}
		else
		{
			horizontalMovement = 0;
			canJump = false;
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
			SceneManager.LoadScene("MenuScene");
		}

		if (col.CompareTag("Death"))
		{
			selfAnimator.SetBool("Die", true);
			selfAudioSource.PlayOneShot(Die);
		}
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.transform.CompareTag("Enemy"))
		{
			selfAnimator.SetBool("Die", true);
			selfAudioSource.PlayOneShot(Die);
			isDead = true;
		}
	}

	void GameOver()
	{
		SceneManager.LoadScene("MenuScene");
	}
}
