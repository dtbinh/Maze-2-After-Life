using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour {
	public int buffTime;
	public static int damageIncreased;
	public static int speedIncreased = 3;
	public Image buffIcon;
	public Text treasureDisplay;
	public AudioSource treasureOpen;
	public AudioClip winClip;

	int treasureCounter;

	// Use this for initialization
	void Start () {
		damageIncreased = 0;
	}
	
	// Update is called once per frame
	void Update () {
		treasureDisplay.text = treasureCounter+"/"+GameMaster.targetTreasureNumber;
	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "Chest"){
			treasureCounter++;
			c.GetComponent<BoxCollider> ().enabled = false;
			c.GetComponent<MeshCollider>().enabled = false;
			c.GetComponent<MeshRenderer>().enabled = false;
			treasureOpen.Play ();
			Destroy (c.gameObject, 1);
			if(treasureCounter == GameMaster.targetTreasureNumber){
				PlayerHealth.isAlive = false;
				treasureOpen.clip = winClip;
				treasureOpen.Play();
				Invoke ("Load", 2);
			}
		}
	}

	void Load(){
		GameMaster.win = true;
		Application.LoadLevel("Level_Result");
	}

	public void Buff(string name){
		ResetStats();

		switch(name){
		case "Power(Clone)":
			damageIncreased = 10;
			buffIcon.color = Color.red;
			break;
		case "Speed(Clone)":
			speedIncreased = 3;
			buffIcon.color = Color.blue;
			break;
		}
		CancelInvoke("ResetStats");
		Invoke("ResetStats", buffTime);
	}

	public void ResetStats(){
		damageIncreased = 0;
		speedIncreased = 0;
		buffIcon.color = Color.clear;
	}
}
