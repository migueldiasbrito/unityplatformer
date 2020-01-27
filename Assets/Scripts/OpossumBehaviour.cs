using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(AudioSource))]
public class OpossumBehaviour : MonoBehaviour
{
	public float Speed = 0.0f;

	public AudioClip DieClip;
	public AudioClip GrowlClip;
	public float GrowlInterval = 0.0f;

	private Rigidbody2D selfRigidbody;
	private AudioSource selfAudioSource;
	private Renderer selfRenderer;
	private int currentDirection = -1;
	private float lastGrowl = 0;

	void Start()
	{
		selfRigidbody = GetComponent<Rigidbody2D>();
		selfAudioSource = GetComponent<AudioSource>();
		selfRenderer = GetComponent<Renderer>();
	}

	void Update()
	{
		if(selfRenderer.isVisible)
		{
			lastGrowl += Time.deltaTime;
			if(lastGrowl >= GrowlInterval)
			{
				selfAudioSource.PlayOneShot(GrowlClip);
				lastGrowl = 0;
			}
		}
		else
		{
			lastGrowl = GrowlInterval;
		}
	}

    void FixedUpdate()
    {
		selfRigidbody.velocity = new Vector2(currentDirection * Speed, selfRigidbody.velocity.y);
	}
	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Environment"))
		{
			currentDirection *= -1;
			transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		}
	}
	void OnTriggerExit2D(Collider2D col)
	{
		if (col.CompareTag("Platform"))
		{
			currentDirection *= -1;
			transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
		}

		if (col.GetComponent<StarBehaviour>())
		{
			GetComponent<Animator>().SetBool("Die", true);
			GetComponent<Collider2D>().enabled = false;
			if (selfRenderer.isVisible)
			{
				selfAudioSource.PlayOneShot(DieClip);
			}
		}
	}

	void Die()
	{
		Destroy(gameObject);
	}
}
