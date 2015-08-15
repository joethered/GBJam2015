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
			transform.eulerAngles = new Vector3(0,0,0);
			break;
		case 90:
			rb2D.velocity = new Vector2 (rb2D.velocity.x, extendSpeed);
			transform.eulerAngles = new Vector3(0,0,90);
			break;
		case 180:
			rb2D.velocity = new Vector2 (-extendSpeed, rb2D.velocity.y);
			transform.eulerAngles = new Vector3(0,0,180);
			break;
		case 270:
			rb2D.velocity = new Vector2 (rb2D.velocity.x, -extendSpeed);
			transform.eulerAngles = new Vector3(0,0,270);
			break;
		}


	}
	
	// Update is called once per frame
	void FixedUpdate () {
		length = Vector2.Distance (transform.position, player.transform.position);
		if ((length >= maxLength && extend == 1) || playerScript.grounded) {
			extend = -1;
		}
		//Debug.Log(extend);
		if (extend == -1) {
			if (length < 0.08f){
				Destroy(gameObject);
			}
			transform.position = Vector3.MoveTowards(transform.position, player.transform.position,retractSpeed);
		}

		BoxCollider2D collider = GetComponent<BoxCollider2D> ();
		if (extend != 0 && collider.IsTouchingLayers() && !playerScript.grounded){
			latchPoint = transform.position;
			extend = 0;
			lockedLength = length;
		}

		if (extend == 0) {
			transform.position = latchPoint;
			//Debug.Log("latchPoint: " + latchPoint.x);
			Rigidbody2D prb2D = player.GetComponent<Rigidbody2D>();

			Vector2 nextPos = (Vector2)player.transform.position + (Vector2)prb2D.velocity * Time.deltaTime;
			//Debug.Log ("player: " + player.transform.position.x + "   nextPos: " + nextPos.x + "   vel: " + 
			//           prb2D.velocity.x + ", " + prb2D.velocity.y  + "    dt: " + Time.deltaTime);
			float nextLength = Vector2.Distance (nextPos, latchPoint);
			//Debug.Log ("nextLength: " + nextLength + "   lockedLength: " + lockedLength);
			if (nextLength > lockedLength){
				//Debug.Log("pos diff: " + (nextPos - latchPoint).x + "   " + (nextPos - latchPoint).normalized.x);
				nextPos = (nextPos - latchPoint).normalized * lockedLength + latchPoint;
				//Debug.Log("nextPos: " + nextPos.x);
				prb2D.velocity = ((Vector2)nextPos - (Vector2)player.transform.position) / Time.deltaTime;

				
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
