using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {


	public static GameManager instance = null;
	public LevelBuilder levelScript;

	private int level = 1;


	// Use this for initialization
	void Awake () {
		if (instance == null)
			instance = this;
		else if (instance != null)
			Destroy (gameObject);

		DontDestroyOnLoad (gameObject);
		levelScript = GetComponent<LevelBuilder> ();
		InitGame ();
	}

	void InitGame(){
		levelScript.SetupScene (level);
	}

	public void GameOver(){
		enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
