using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GrapplingHook : MonoBehaviour {
	
	public GameObject chainLink;
	List<GameObject> links;
	bool latched = false;
	public LayerMask whatIsGround;
	Rigidbody2D rb2D;
	int extend = 0;
	public float maxLength = 1f;
	float length = 0f;

	// Use this for initialization
	void Start () {
		rb2D = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

	}

	public void setVelocity(float xVel, float yVel){
		rb2D.velocity = new Vector2 (xVel, yVel);
	}

	public Vector2 getVelocity(){
		return rb2D.velocity;
	}


}
