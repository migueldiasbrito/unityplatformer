using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarBehaviour : MonoBehaviour
{
	public float TTL;
	public float Speed;
	public float RotationSpeed;

	private float timeLeft;
	private Rigidbody2D selfRigidbody;

	void Start()
	{
		timeLeft = TTL;
		selfRigidbody = GetComponent<Rigidbody2D>();
	}

    void Update()
    {
		timeLeft -= Time.deltaTime;

		if(timeLeft <= 0)
		{
			Destroy(gameObject);
			return;
		}

		transform.Rotate(Vector3.forward * Time.deltaTime * RotationSpeed);
	}

	void FixedUpdate()
	{
		selfRigidbody.velocity = new Vector2(Speed, 0);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.CompareTag("Enemy"))
		{
			col.gameObject.GetComponent<Animator>().SetBool("Die", true);
			col.enabled = false;

			Destroy(gameObject);
		}
	}
}
