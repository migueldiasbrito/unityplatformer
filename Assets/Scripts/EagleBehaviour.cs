using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EagleBehaviour : MonoBehaviour
{
	public float Range = 0.0f;
	public float SpeedX = 0.0f;
	public float Radius = 0.0f;
	public float AccelerationY = 0.0f;

	private Rigidbody2D selfRigidbody;
	private float originalX;
	private float velocityY = 0.0f;
	private float currentTime = 0.0f;
	private int currentDirection = -1;

    void Start()
	{
		selfRigidbody = GetComponent<Rigidbody2D>();
		originalX = transform.position.x;
    }

    void Update()
    {
		currentTime += Time.deltaTime;
		velocityY = Radius * Mathf.Sin(AccelerationY * currentTime);
	}
	void FixedUpdate()
	{
		if ((this.transform.position.x < originalX - Range && selfRigidbody.velocity.x < 0) ||
			(this.transform.position.x > originalX + Range && selfRigidbody.velocity.x > 0))
		{
			currentDirection *= -1;
		}

		selfRigidbody.velocity = new Vector2(currentDirection * SpeedX, velocityY);
	}
}
