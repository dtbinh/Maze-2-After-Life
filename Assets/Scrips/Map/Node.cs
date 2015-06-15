using UnityEngine;

public class Node{
	public int code;
	public Vector3 worldPosition;
	public int x;
	public int z;
	
	public Node(int code, Vector3 worldPosition, int x, int z){
		this.code = code;
		this.worldPosition = worldPosition;
		this.x = x;
		this.z = z;
	}
}
