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
}
