using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
	public float Speed = 0.0f;
	public float JumpForce = 0.0f;

	public Transform FeetRectangleTopLeft;
	public Transform FeetRectangleBottomRight;
	public LayerMask GroundLayer;

	public GameObject StarPrefab;
	public float RecoilTime = 0.0f;

	private Rigidbody2D selfRigidbody;
	private Animator selfAnimator;
	private float horizontalMovement = 0.0f;
	private bool isGrounded = false;
	private bool canJump = false;
	private float timeSinceLastFire = 0.0f;
	private bool isDead = false;

    void Start()
    {
		selfRigidbody = GetComponent<Rigidbody2D>();
		selfAnimator = GetComponent<Animator>();
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

			if (Input.GetAxis("Jump") > 0.1f && isGrounded)
			{
				canJump = true;
				selfAnimator.SetBool("JumpingUp", true);
			}

			timeSinceLastFire += Time.deltaTime;
			if (Input.GetAxis("Fire1") > 0.1f && timeSinceLastFire >= RecoilTime)
			{
				GameObject newStar = Instantiate(StarPrefab);
				newStar.transform.position = transform.position;
				newStar.GetComponent<StarBehaviour>().Speed *= (transform.localScale.x > 0) ? 1 : -1;
				timeSinceLastFire = 0.0f;
				selfAnimator.SetBool("Firing", true);
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
			SceneManager.LoadScene("SampleScene");
		}

		if (col.CompareTag("Death"))
		{
			selfAnimator.SetBool("Die", true);
		}
	}
	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.transform.CompareTag("Enemy"))
		{
			selfAnimator.SetBool("Die", true);
		}
	}

	void GameOver()
	{
		SceneManager.LoadScene("SampleScene");
	}
}
