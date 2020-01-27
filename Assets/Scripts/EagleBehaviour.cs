using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class EagleBehaviour : MonoBehaviour
{
	private enum EagleState { IDLE, ATTACK, TAKEOFF}

	public float Range = 0.0f;
	public float SpeedX = 0.0f;
	public float Radius = 0.0f;
	public float AccelerationY = 0.0f;
	public float DistanceTrigger = 0.0f;
	public float AttackSpeedX = 0.0f;
	public float AttackSpeedY = 0.0f;
	public float TakeOffSpeedY = 0.0f;

	public AudioClip DieClip;
	public AudioClip AttackClip;

	private Rigidbody2D selfRigidbody;
	private Animator selfAnimator;
	private AudioSource selfAudioSource;
	private Renderer selfRenderer;
	private float originalX;
	private float originalY;
	private float velocityY = 0.0f;
	private float currentTime = 0.0f;
	private int currentDirection = -1;
	private GameObject player;
	private EagleState state = EagleState.IDLE;

    void Start()
	{
		selfRigidbody = GetComponent<Rigidbody2D>();
		selfAnimator = GetComponent<Animator>();
		selfAudioSource = GetComponent<AudioSource>();
		originalX = transform.position.x;
		originalY = transform.position.y;
		player = GameObject.FindGameObjectWithTag("Player");
		selfRenderer = GetComponent<Renderer>();
	}

    void Update()
    {
		switch (state)
		{
			case EagleState.IDLE:
				if (Vector2.Distance(player.transform.position, transform.position) <= DistanceTrigger &&
					((transform.localScale.x > 0 && player.transform.position.x <= transform.position.x) ||
					(transform.localScale.x < 0 && player.transform.position.x >= transform.position.x)))
				{
					state = EagleState.ATTACK;
					selfAnimator.SetBool("Attack", true);
					if (selfRenderer.isVisible)
					{
						selfAudioSource.PlayOneShot(AttackClip);
					}
				}
				else
				{
					currentTime += Time.deltaTime;
					velocityY = Radius * Mathf.Sin(AccelerationY * currentTime);
				}
				break;
			case EagleState.ATTACK:
				transform.position = Vector2.Lerp(transform.position,
					new Vector2(player.transform.position.x, transform.position.y), AttackSpeedX * Time.deltaTime);

				if((player.transform.position.x < transform.position.x && transform.localScale.x < 0) ||
					(player.transform.position.x > transform.position.x && transform.localScale.x > 0))
				{
					transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
				}

				break;
			case EagleState.TAKEOFF:
				if (transform.position.y >= originalY)
				{
					state = EagleState.IDLE;
				}
				break;
		}
	}
	void FixedUpdate()
	{
		switch (state)
		{
			case EagleState.IDLE:
				if ((this.transform.position.x < originalX - Range && selfRigidbody.velocity.x < 0) ||
					(this.transform.position.x > originalX + Range && selfRigidbody.velocity.x > 0))
				{
					currentDirection *= -1;
					transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
				}

				selfRigidbody.velocity = new Vector2(currentDirection * SpeedX, velocityY);
				break;
			case EagleState.ATTACK:
				selfRigidbody.velocity = new Vector2(0, AttackSpeedY);
				break;
			case EagleState.TAKEOFF:
				selfRigidbody.velocity = new Vector2(0, TakeOffSpeedY);
				break;
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (state == EagleState.ATTACK)
		{
			state = EagleState.TAKEOFF;
			selfAnimator.SetBool("Attack", false);
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
			selfAnimator.SetBool("Die", true);
			GetComponent<Collider2D>().enabled = false;
			if (selfRenderer.isVisible)
			{
				selfAudioSource.PlayOneShot(DieClip);
			}
		}
	}
}
