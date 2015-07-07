/* Author: Guangpeng Li
 * University of Liverpool
 * Date: 01/05/2015
 * 
 * The purpose of this class is to generate enemies
 * into the maze
 */
using UnityEngine;
using System.Collections.Generic;

public class EnemyGenerator : MonoBehaviour {
	// Minimum number of bots
	const int MIN_NUMBER_BOTS = 0;

	/* 
	 * Game object prefabs
	 */
	public Transform player;
	public GameObject attackbotPrefab;
	public GameObject defencebotPrefab;
	public GameObject speedbotPrefab;
	/*
	 * Minimum and maximum distances to generate the bots
	 */
	public int maxDisPlayer;
	public int minDisPlayer;

	// Use this for initialization
	void Start () {
		// Adding extra bots over time
		InvokeRepeating("AddNewAtkBots", GameMaster.newABotPerSecs, GameMaster.newABotPerSecs);
		//InvokeRepeating("AddNewSpdBots", newABotPerSecs, newABotPerSecs);
		InvokeRepeating("AddNewDefBots", GameMaster.newDBotPerSecs, GameMaster.newDBotPerSecs);
	}
	
	// Update is called once per frame
	void Update () {

	}

	/*
	 * Increase the number of extra Attack bots
	 */
	void AddNewAtkBots(){
		bool free = false;
		Vector3 posision;
		do{
			posision = GridGenerator.gridMapWorldPosition[Random.Range(0,GridGenerator.gridMapWorldPosition.Count-1)];
			float distance = Vector3.Distance(posision,player.position);
			if(distance > 10f && distance < 25f)
				free = true;
		}while(!free);

		Instantiate(attackbotPrefab, posision, Quaternion.identity);

		if(!PlayerHealth.isAlive)
			CancelInvoke("AddNewSpdBots");
	}
	/*
	 * Increase the number of extra speed bots
	 */
	void AddNewSpdBots(){
	}
	/*
	 * Increase the number of extra defence bots
	 */
	void AddNewDefBots(){
		bool free = false;
		Vector3 posision;
		do{
			posision = GridGenerator.gridMapWorldPosition[Random.Range(0,GridGenerator.gridMapWorldPosition.Count-1)];
			float distance = Vector3.Distance(posision,player.position);
			if(distance > 10f && distance < 25f)
				free = true;
		}while(!free);
		
		Instantiate(defencebotPrefab, posision, Quaternion.identity);
		
		if(!PlayerHealth.isAlive)
			CancelInvoke("AddNewSpdBots");
	}
}
