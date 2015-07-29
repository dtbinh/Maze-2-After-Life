using UnityEngine;
using System.Collections;

public class Gun : MonoBehaviour {
	
	public float damage;
	public LayerMask enemyLayer;
	public LayerMask wallLayer;
	public GameObject spark_effect;
	public static Vector3 hitPoint;

	public AudioSource shootingSound;
	public AudioSource subShootingSound;

	protected float timer;

	protected float DistanceDamageRatio(Vector3 target){
		float distance = Vector3.Distance(transform.position, target);
		if(distance > 5)
			return 5 / distance;
		else return 1;
	}
}
