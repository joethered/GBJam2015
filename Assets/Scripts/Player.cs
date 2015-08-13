using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {



	public GameObject grapplingHook;
	public float maxSpeed = 1f;
	public int rot = 0;
	public bool grounded = false;
	public Transform downCheck;
	public Transform upCheck;
	public Transform leftCheck;
	public Transform rightCheck;
	public float groundRadius = 0.1f;


	public bool flipped = false;

	public LayerMask whatIsGround;

	private Rigidbody2D rb2D;


	// Use this for initialization
	void Start () {
		rb2D = GetComponent<Rigidbody2D> ();
		rb2D.velocity = new Vector2 (rb2D.velocity.x, -1f);
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		bool downCol = Physics2D.OverlapCircle (downCheck.position, groundRadius, whatIsGround);
		bool upCol = Physics2D.OverlapCircle (upCheck.position, groundRadius, whatIsGround);
		bool leftCol = Physics2D.OverlapCircle (leftCheck.position, groundRadius, whatIsGround);
		bool rightCol = Physics2D.OverlapCircle (rightCheck.position, groundRadius, whatIsGround);
		Debug.Log (downCol.ToString() + upCol.ToString() + leftCol.ToString() + rightCol.ToString());


		bool nextGroundState = (downCol || upCol || leftCol || rightCol);

		float inputX = Input.GetAxis ("Horizontal");
		float inputY = Input.GetAxis ("Vertical");

		if (!grounded) {
			if (inputX < 0){
				rot = 180;
				transform.eulerAngles = new Vector3(0,0,rot);
			}
			if (inputX > 0){
				rot = 0;
				transform.eulerAngles = new Vector3(0,0,rot);
			}
			if (inputY < 0){
				rot = 270;
				transform.eulerAngles = new Vector3(0,0,rot);
			}
			if (inputY > 0){
				rot = 90;
				transform.eulerAngles = new Vector3(0,0,rot);
			}


			if (nextGroundState && !downCol) {
				latchToWall(upCol, leftCol, rightCol);
			}

			downCol = Physics2D.OverlapCircle (downCheck.position, groundRadius, whatIsGround);
			nextGroundState = (upCol || leftCol || rightCol);
			if (!downCol && nextGroundState){
				latchToWall(upCol, leftCol, rightCol);
			}



			if (Input.GetAxis("AButton") > 0 && GameObject.Find("GrapplingHook") != null){
				GameObject toInstantiate = grapplingHook;
				GameObject instance = Instantiate(toInstantiate, new Vector3(transform.position.x, transform.position.y, 0f), Quaternion.identity) as GameObject;
				GrapplingHook gHook = instance.GetComponent<GrapplingHook>();
				gHook.setVelocity(rb2D.velocity.x + 1, rb2D.velocity.y);
			}


		}


		if (nextGroundState != grounded) {
			if (!grounded){
				grounded = true;
			}
		}


		grounded = nextGroundState;

		if (grounded) {
			switch (rot){
			case 0:
				rb2D.velocity = new Vector2 (inputX * maxSpeed, rb2D.velocity.y);
				if (Input.GetAxis("AButton") > 0)
					rb2D.velocity = new Vector2 (0, 0.5f);
				break;
			case 180:
				rb2D.velocity = new Vector2 (inputX * maxSpeed, rb2D.velocity.y);
				if (Input.GetAxis("AButton") > 0)
					rb2D.velocity = new Vector2 (0, -0.5f);
				break;
			case 90:
				rb2D.velocity = new Vector2 (rb2D.velocity.x, inputY * maxSpeed);
				if (Input.GetAxis("AButton") > 0)
					rb2D.velocity = new Vector2 (-0.5f, 0);
				break;
			case 270:
				rb2D.velocity = new Vector2 (rb2D.velocity.x, inputY * maxSpeed);
				if (Input.GetAxis("AButton") > 0)
					rb2D.velocity = new Vector2 (0.5f, 0);
				break;
			}
		}
	}

	void Flip(){

	}

	private void latchToWall(bool upCol, bool leftCol, bool rightCol){
		if (upCol) {
			switch (rot){
			case 0:
				rot = 180;
				transform.eulerAngles = new Vector3(0,0,rot);
				rb2D.velocity = new Vector2 (rb2D.velocity.x, 1f);
				break;
			case 180:
				rot = 0;
				transform.eulerAngles = new Vector3(0,0,rot);
				rb2D.velocity = new Vector2 (rb2D.velocity.x, -1f);
				break;
			case 90:
				rot = 270;
				transform.eulerAngles = new Vector3(0,0,rot);
				rb2D.velocity = new Vector2 (-1f, rb2D.velocity.x);
				break;
			case 270:
				rot = 90;
				transform.eulerAngles = new Vector3(0,0,rot);
				rb2D.velocity = new Vector2 (1f, rb2D.velocity.x);
				break;
			}
			
		}else if (leftCol){
			switch (rot){
			case 0:
				rot = 270;
				transform.eulerAngles = new Vector3(0,0,rot);
				break;
			case 180:
				rot = 90;
				transform.eulerAngles = new Vector3(0,0,rot);
				break;
			case 90:
				rot = 0;
				transform.eulerAngles = new Vector3(0,0,rot);
				break;
			case 270:
				rot = 180;
				transform.eulerAngles = new Vector3(0,0,rot);
				break;
			}
		}else if (rightCol){
			switch (rot){
			case 0:
				rot = 90;
				transform.eulerAngles = new Vector3(0,0,rot);
				break;
			case 180:
				rot = 270;
				transform.eulerAngles = new Vector3(0,0,rot);
				break;
			case 90:
				rot = 180;
				transform.eulerAngles = new Vector3(0,0,rot);
				break;
			case 270:
				rot = 0;
				transform.eulerAngles = new Vector3(0,0,rot);
				break;
			}
		}
		
	}
	
	
	
}
