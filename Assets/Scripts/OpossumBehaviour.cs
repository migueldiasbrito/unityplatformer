using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class OpossumBehaviour : MonoBehaviour
{
	public float Speed = 0.0f;

	private Rigidbody2D selfRigidbody;
	private int currentDirection = -1;
	void Start()
	{
		selfRigidbody = GetComponent<Rigidbody2D>();
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
	}

	void Die()
	{
		Destroy(gameObject);
	}
}
