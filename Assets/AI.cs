using UnityEngine;

public class AI : MonoBehaviour {
    NavMeshAgent ai;
    public Transform target;

	// Use this for initialization
	void Start () {
        ai = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
        ai.SetDestination(target.position);
	}
}
