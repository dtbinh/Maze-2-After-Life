using UnityEngine;
using System.Collections;

public class EnemyDrop : MonoBehaviour {
	public ItemDrop[] itemDrops;

	public void DropItem () {
		foreach(ItemDrop i in itemDrops){
			if(Random.value <= i.dropRate)
				Destroy(Instantiate(i.item,transform.position,Quaternion.identity),5);
		}
	}
}

[System.Serializable]
public class ItemDrop {
	public GameObject item;
	public float dropRate;
}
