using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class LevelBuilder : MonoBehaviour {

	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count (int min, int max){
			maximum = max;
			minimum = min;
		}
	}


	public int columns = 20;
	public int rows = 18;
	public Count wallcount = new Count(5, 9);

	//public GameObject exit;
	public GameObject[] bgTiles;
	public GameObject[] wallTiles;

	private Transform levelHolder;
	private List <Vector3> gridPositions = new List<Vector3>();

	void initializeList(){
		gridPositions.Clear ();

		for (int x = 1; x < columns - 1; ++x) {
			for (int y = 1; y < rows - 1; ++y) {
				gridPositions.Add(new Vector3(x,y,0f));
			}
		}
	}

	void LevelSetup(){
		levelHolder = new GameObject ("Level").transform;
		levelHolder.position = new Vector3 (0f, 0f, 0f);

		for (int x = 0; x < columns; ++x) {
			for (int y = 0; y < rows; ++y) {
				GameObject toInstantiate = bgTiles[0];
				if (x == 0 || x == columns-1 || y == 0 || y == rows-1 || ((x >= 12 && x <= 16) && y == 9))
					toInstantiate = wallTiles[0];

				GameObject instance = Instantiate(toInstantiate, new Vector3(x/9f - columns/18f + 0.06f, y/9f - rows/18f+0.05f, 0f), Quaternion.identity) as GameObject;

				instance.transform.SetParent(levelHolder);
			}
		}
	}


	Vector3 RandomPosition(){
		int randomIndex = Random.Range (0, gridPositions.Count);
		Vector3 randomPosition = gridPositions [randomIndex];
		gridPositions.RemoveAt (randomIndex);
		return randomPosition;
	}

	void LayoutObjectAtRandom(GameObject[] tileArray, int minimum, int maximum){
		int objectCount = Random.Range (minimum, maximum + 1);

		for (int i = 0; i < objectCount; ++i) {
			Vector3 randomPosition = RandomPosition ();
			GameObject tileChoice = tileArray [Random.Range (0, tileArray.Length)];
			Instantiate (tileChoice, randomPosition, Quaternion.identity);
		}
	}

	public void SetupScene(int level)
	{
		LevelSetup ();
		initializeList ();

	}


}
