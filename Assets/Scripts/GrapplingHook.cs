using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GrapplingHook : MonoBehaviour {
	
	public GameObject chainLink;
	List<GameObject> links;
	public LayerMask whatIsGround;
	public float retractSpeed = 0.08f;
	public float extendSpeed = 3f;

	int extend = 0;
	public float maxLength = 1f;
	float length = 0f;
	float lockedLength = 0f;
	private Vector2 latchPoint;

	private Rigidbody2D rb2D;
	GameObject player;
	Player playerScript;

	// Use this for initialization
	void Awake () {
		rb2D = GetComponent<Rigidbody2D> ();
		extend = 1;
		player = GameObject.Find("Player");
		playerScript = player.GetComponent<Player> ();


		switch (playerScript.aim) {
		case 0:
			rb2D.velocity = new Vector2 (extendSpeed, rb2D.velocity.y);

			break;
		case 90:
			rb2D.velocity = new Vector2 (rb2D.velocity.x, extendSpeed);
			break;
		case 180:
			rb2D.velocity = new Vector2 (-extendSpeed, rb2D.velocity.y);
			break;
		case 270:
			rb2D.velocity = new Vector2 (rb2D.velocity.x, -extendSpeed);
			break;
		}


	}
	
	// Update is called once per frame
	void FixedUpdate () {
		length = Vector2.Distance (transform.position, player.transform.position);
		if ((length >= maxLength && extend == 1) || playerScript.grounded) {
			extend = -1;
		}

		if (extend == -1) {
			if (length < 0.08f){
				Destroy(gameObject);
			}
			transform.position = Vector3.MoveTowards(transform.position, player.transform.position,retractSpeed);
		}

		CircleCollider2D collider = GetComponent<CircleCollider2D> ();
		if (extend != 0 && collider.IsTouchingLayers(9)){
			latchPoint = transform.position;
			extend = 0;
			lockedLength = length;
			Debug.Log("hi");
		}

		if (extend == 0) {
			transform.position = latchPoint;
			Debug.Log (latchPoint);
			if (length > lockedLength){
				Rigidbody2D prb2D = player.GetComponent<Rigidbody2D>();
				Vector2 force = new Vector2(player.transform.position.x - latchPoint.x, 
				                            player.transform.position.y - latchPoint.y);
				float velMag = rb2D.velocity.magnitude;
				force.Normalize();
				force *= velMag;
				prb2D.AddForce(force);
			}
		}


	}

	public void setVelocity(float xVel, float yVel){
		Debug.Log (rb2D);
		rb2D.velocity = new Vector2 (xVel, yVel);
	}

	public Vector2 getVelocity(){
		return rb2D.velocity;
	}


}
