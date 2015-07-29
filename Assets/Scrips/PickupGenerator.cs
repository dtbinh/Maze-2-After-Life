/* Author: Guangpeng Li
 * University of Liverpool
 * Date: 01/05/2015
 * 
 * The purpose of this class is to generate the pick ups
 */
using UnityEngine;
using System.Collections.Generic;

public class PickupGenerator : MonoBehaviour {
	public GameObject player;
	public GameObject treasure;

	GridGenerator gridGenerator;

	// Use this for initialization
	void Start () {
		gridGenerator = GameObject.Find("_GridMapGenerator").GetComponent<GridGenerator>();

		int index = Random.Range (0, gridGenerator.gridMapWorldPosition.Count - 1);
		player.transform.position = gridGenerator.gridMapWorldPosition[index];

		InstantiateTreasure (index);
	}
	
	// Update is called once per frame
	void Update () {
	}
	/*
	 * Generate the treasures in random locations
	 */
	void InstantiateTreasure(int playerIndex){
		/*// Number of treasures
		int[] treasurePosition = new int[GameMaster.targetTreasureNumber];
		// First treasure Z position
		treasurePosition [0] = 5;
		// Last treasure Z position
		treasurePosition [GameMaster.targetTreasureNumber-1] = GameMaster.worldPosZ - 5;
		// All treasures in the middle Z position
		for(int i = 1; i < treasurePosition.Length-1; i++){
			treasurePosition [i] = GameMaster.worldPosZ / (i + 1);
		}

		for(int i = 0; i < treasurePosition.Length; i++){
			bool instantiated = false;
			do{
				int index = Random.Range (0, freePos.Count - 1);
				if(freePos[index] != freePos[playerIndex] && Mathf.Approximately (freePos[index].z, treasurePosition[i])){
					Instantiate (treasure, freePos[index], Quaternion.identity);
					instantiated = true;
				}
			}while(!instantiated);
		}*/
		
		for (int z = 0; z < GameMaster.gridSizeZ; z++) {
			Instantiate (treasure, gridGenerator.grid[Random.Range(0,GameMaster.gridSizeX),z].worldPosition, Quaternion.identity);
		}
	}
}