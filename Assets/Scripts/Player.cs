using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class Player : MonoBehaviour {



	public GameObject grapplingHook;
	public float maxSpeed = 1f;
	private int rot = 0;
	public bool grounded = false;
	public Transform downCheck;
	public Transform upCheck;
	public Transform leftCheck;
	public Transform rightCheck;
	public Transform hookHand;
	public float groundRadius = 0.1f;
	


	public bool flipped = false;
	public int aim = 0;

	public LayerMask whatIsGround;

	private Rigidbody2D rb2D;
	private bool aHeld = false;
	private GameObject hookInstance;

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
		//Debug.Log (downCol.ToString() + upCol.ToString() + leftCol.ToString() + rightCol.ToString());


		bool nextGroundState = (downCol || upCol || leftCol || rightCol);

		float inputX = Input.GetAxis ("Horizontal");
		float inputY = Input.GetAxis ("Vertical");

		if (!grounded) {
			if (inputX < 0){
				if (flipped)
					flip ();
				rot = 180;
				transform.eulerAngles = new Vector3(0,0,rot);
				aim = 180;
			}
			if (inputX > 0){
				if (flipped)
					flip ();
				rot = 0;
				transform.eulerAngles = new Vector3(0,0,rot);
				aim = 0;
			}
			if (inputY < 0){
				if (flipped)
					flip ();
				rot = 270;
				transform.eulerAngles = new Vector3(0,0,rot);
				aim = 270;
			}
			if (inputY > 0){
				if (flipped)
					flip ();
				rot = 90;
				transform.eulerAngles = new Vector3(0,0,rot);
				aim = 90;
			}


			if (nextGroundState && !downCol) {
				latchToWall(upCol, leftCol, rightCol);
			}

			downCol = Physics2D.OverlapCircle (downCheck.position, groundRadius, whatIsGround);
			nextGroundState = (upCol || leftCol || rightCol);
			if (!downCol && nextGroundState){
				latchToWall(upCol, leftCol, rightCol);
			}


			if (!aHeld && Input.GetAxis("AButton") > 0 && hookInstance == null){
				aHeld = true;
				GameObject toInstantiate = grapplingHook;
				//Debug.Log (grapplingHook);
				hookInstance = Instantiate(toInstantiate, hookHand.transform.position, Quaternion.identity) as GameObject;
				Physics2D.IgnoreCollision(hookInstance.GetComponent<BoxCollider2D>(), GetComponent<BoxCollider2D>());

			}


		}


		if (nextGroundState != grounded) {
			if (!grounded){
				grounded = true;
			}
		}


		grounded = nextGroundState;

		if (grounded) {
			if (!aHeld){
				switch (rot){
				case 0:
					rb2D.velocity = new Vector2 (inputX * maxSpeed, rb2D.velocity.y);
					if (inputX < 0 && !flipped){
						flip ();
						aim = 180;
					}else if (inputX > 0 && flipped){
						flip ();
						aim = rot;
					}
					if (Input.GetAxis("AButton") > 0){
						rb2D.velocity = new Vector2 (0, 0.5f);
						aHeld = true;
					}
					break;
				case 180:
					rb2D.velocity = new Vector2 (inputX * maxSpeed, rb2D.velocity.y);
					if (inputX < 0 && flipped){
						flip ();
						aim = rot;
					}else if (inputX > 0 && !flipped){
						flip ();
						aim = 0;
					}
					if (Input.GetAxis("AButton") > 0){
						rb2D.velocity = new Vector2 (0, -0.5f);
						aHeld = true;
					}
					break;
				case 90:
					rb2D.velocity = new Vector2 (rb2D.velocity.x, inputY * maxSpeed);
					if (inputY < 0 && !flipped){
						flip ();
						aim = rot;
					}else if (inputY > 0 && flipped){
						flip ();
						aim = 270;
					}
					if (Input.GetAxis("AButton") > 0){
						rb2D.velocity = new Vector2 (-0.5f, 0);
						aHeld = true;
					}
					break;
				case 270:
					rb2D.velocity = new Vector2 (rb2D.velocity.x, inputY * maxSpeed);
					if (inputY < 0 && flipped){
						flip ();
						aim = 90;
					}else if (inputY > 0 && !flipped){
						flip ();
						aim = rot;
					}
					if (Input.GetAxis("AButton") > 0){
						rb2D.velocity = new Vector2 (0.5f, 0);
						aHeld = true;
					}
					break;
				}
			}
		}

		if (Input.GetAxis("AButton") <= 0){
			aHeld = false;
		}

	}

	void flip(){
		flipped = !flipped;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
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
