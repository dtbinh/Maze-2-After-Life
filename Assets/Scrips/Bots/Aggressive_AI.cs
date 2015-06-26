using UnityEngine;
using System.Collections;

public class Aggressive_AI : Enemy_AI {
	public float attackRange;
	public int damage;
	// Use this for initialization
	void Start () {
		//nav.stoppingDistance = attackRange - 1;
	}
}
