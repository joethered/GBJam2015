using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

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

		bool nextGroundState = (downCol || upCol || leftCol || rightCol);

		if (!grounded) {
			if (nextGroundState && !downCol) {
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
		if (nextGroundState != grounded) {
			if (!grounded){
				grounded = true;
			}
		}




		float inputX = Input.GetAxis ("Horizontal");
		float inputY = Input.GetAxis ("Vertical");
		Debug.Log (rb2D.velocity);
		if (grounded) {
			rb2D.velocity = new Vector2 (inputX * maxSpeed, rb2D.velocity.y);
			rb2D.velocity = new Vector2 (rb2D.velocity.x, inputY * maxSpeed);
			grounded = false;
		}
	}

	void Flip(){

	}



}
