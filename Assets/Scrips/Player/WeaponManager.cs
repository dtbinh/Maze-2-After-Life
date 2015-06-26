using UnityEngine;
using UnityEngine.UI;

public class WeaponManager : MonoBehaviour {
	public Image[] weaponImage;
	public Slider cooldownBar;
	public LasserGun lasserGun;
	public CanvasGroup smallGunCD;
	float bigBulletTimer;
	int weaponID;
	Color[] weaponActivated;
	readonly Color DISACTIVATED = new Color32(0,0,0,250);

	void Awake(){
		weaponActivated = new Color[weaponImage.Length];
		for (int i = 0; i < weaponImage.Length; i++) {
			weaponActivated [i] = weaponImage [i].color;
			weaponImage [i].color = DISACTIVATED;
		}
		weaponImage [0].color = weaponActivated [0];
	}
	
	// Update is called once per frame
	void Update () {
		if(!lasserGun.isChargedFiring){
			if ((Input.GetKeyDown (KeyCode.Alpha1) || Input.GetKeyDown (KeyCode.Keypad1)) && weaponID != 0) {
				SwitchWeapon(0);
				smallGunCD.alpha = 1;
			} else if ((Input.GetKeyDown (KeyCode.Alpha2) || Input.GetKeyDown (KeyCode.Keypad2)) && weaponID != 1) {
				SwitchWeapon(1);
				smallGunCD.alpha = 0;
			}
		}
		bigBulletTimer += Time.deltaTime;
		cooldownBar.value = bigBulletTimer;
	}

	public float GetBigBulletTimer{
		get{return bigBulletTimer;}
		set{bigBulletTimer = value;}
	}

	void SwitchWeapon(int id){
		weaponImage [weaponID].color = DISACTIVATED;
		weaponID = id;
		foreach(Transform t in transform){
			t.gameObject.SetActive(false);
		}
		transform.GetChild(weaponID).gameObject.SetActive(true);
		weaponImage [weaponID].color = weaponActivated[weaponID];
	}
}
