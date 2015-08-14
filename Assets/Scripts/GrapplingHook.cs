using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GrapplingHook : MonoBehaviour {
	
	public GameObject chainLink;
	List<GameObject> links;
	bool latched = false;
	public LayerMask whatIsGround;
	public float retractSpeed = 0.08f;

	int extend = 0;
	public float maxLength = 1f;
	float length = 0f;
	bool lockLength;

	private Rigidbody2D rb2D;

	// Use this for initialization
	void Awake () {
		rb2D = GetComponent<Rigidbody2D> ();
		extend = 1;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector2 playerPos = GameObject.FindWithTag ("Player").transform.position;
		length = Vector2.Distance (transform.position, playerPos);
		if (length >= maxLength && extend == 1) {
			extend = -1;
		}

		if (extend == -1) {
			if (length < 0.05f){
				Destroy(gameObject);
			}
			transform.position = Vector3.MoveTowards(transform.position, playerPos,retractSpeed);
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
