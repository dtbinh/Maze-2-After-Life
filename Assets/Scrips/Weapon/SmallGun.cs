using UnityEngine;
using System.Collections;

public class SmallGun : Gun {
	public float fireRate;
	public float subFireRate;
	public GameObject small_bullet_prefab;
	public GameObject big_bullet_prefab;
	public WeaponManager weaponManager;
	public LineRenderer gunLine;
	public Light gunLight;

	float effectTime = 0.2f;

	void Start(){
		weaponManager.cooldownBar.maxValue = subFireRate;
	}

	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		if(PlayerHealth.isAlive){
			if(Input.GetMouseButton(0) && timer >= fireRate){
				Shoot();
			}

			if(timer >= fireRate * effectTime){
				DissableEffects();
			}


			if(Input.GetMouseButton(1) && weaponManager.GetBigBulletTimer >= subFireRate){
				weaponManager.GetBigBulletTimer = 0;
				subShootingSound.Play ();
				Destroy(Instantiate(big_bullet_prefab, transform.position, transform.rotation), 5);
			}
		}
	}

	void Shoot(){
		timer = 0;

		shootingSound.Play ();

		gunLight.enabled = true;
		gunLine.enabled = true;
		gunLine.SetPosition(0, transform.position);

		Ray ray = new Ray(transform.position - transform.forward, transform.forward);
		RaycastHit hitInfo;
		
		if(Physics.Raycast(ray, out hitInfo, Mathf.Infinity)){
			hitPoint = hitInfo.point;
			gunLine.SetPosition(1, hitInfo.point);
			
			GameObject go = hitInfo.collider.gameObject;
			
			EnemyHealth h = go.GetComponent<EnemyHealth>();
			if(h != null){
				h.TakeDamage((damage + PlayerStatus.damageIncreased) * DistanceDamageRatio(go.transform.position));
			}
			
			if(spark_effect != null)
				Destroy(Instantiate(spark_effect, hitInfo.point, transform.rotation),0.3f);
		}
	}

	void DissableEffects(){
		gunLine.enabled = false;
		gunLight.enabled = false;
	}
}
