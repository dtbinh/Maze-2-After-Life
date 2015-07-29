using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridGenerator : MonoBehaviour {
	public Node[,] grid;
	public List<Vector3> gridMapWorldPosition;
	public GameObject enemyGenerator;
	public GameObject pickupGenerator;
	int gridSizeX;
	int gridSizeZ;
	int offset;
	// Use this for initialization
	void Start () {
		gridSizeX = GameMaster.gridSizeX;
		gridSizeZ = GameMaster.gridSizeZ;
		offset = GameMaster.gridSizeOffset;
		GenerateBlockData();
		enemyGenerator.SetActive(true);
		pickupGenerator.SetActive(true);
	}

	void GenerateBlockData(){
		grid = new Node[gridSizeX,gridSizeZ];
		gridMapWorldPosition = new List<Vector3>();
        for (int z = 0; z < gridSizeZ; z++) {
            for (int x = 0; x < gridSizeX; x++) {
                var v = new Vector3(x * offset + 5f, .5f, (gridSizeZ - z) * offset - 5f);
                grid[x, z] = new Node(MazeGenerator.gridMap[x, gridSizeX - 1 - z], v, x, z);
                gridMapWorldPosition.Add(v);
            }
        }
    }

	public List<Node> GetNeighbours(Vector3 pos) {
		Node node = GetBlock(pos);
		var neighbours = new List<Node>();
		// North wall
		if(((node.code & 2) != 0) && (node.z+1 < gridSizeZ))
			neighbours.Add(grid[node.x,node.z+1]);
		// South wall
		if(((node.code & 1) != 0) && (node.z-1 >= 0))
			neighbours.Add(grid[node.x,node.z-1]);
		// East wall
		if(((node.code & 4) != 0) && (node.x+1 < gridSizeX))
			neighbours.Add(grid[node.x+1,node.z]);
		// West wall
		if(((node.code & 8) != 0) && (node.x-1 >= 0))
			neighbours.Add(grid[node.x-1,node.z]);
		return neighbours;
	}

	public List<Node> GetDistanceNeighbours(Node start, int depth){
		int currentDepth = 0;
		var visited = new List<Node>();
		var distanceNeighbours = new List<Node>();
		var open = new Stack<Node>();
		open.Push(start);
		while(open.Count > 0){
			Node currentNode = open.Pop();

			if(currentDepth == depth){
				if(!distanceNeighbours.Contains(currentNode))
					distanceNeighbours.Add(currentNode);
				visited.Add(currentNode);
				currentDepth--;
			}else{
				visited.Add(currentNode);
				foreach(Node neighbour in GetNeighbours(currentNode.worldPosition)){
					if(!visited.Contains(neighbour))
						open.Push(neighbour);
				}
			}

			currentDepth++;
		}
		return distanceNeighbours;
	}

	public Node GetBlock(Vector3 pos){
		int x = (int)pos.x/offset;
		int z = (int)pos.z/offset;

		x = (x<0)?0:x;
		z = (z<0)?0:z;
		x = (x>=gridSizeX)?gridSizeX-1:x;
		z =	(z>=gridSizeZ)?gridSizeZ-1:z;

		return grid[Mathf.FloorToInt(x),Mathf.FloorToInt((gridSizeZ-1)-z)];
	}

	public bool CompareBlocks(Vector3 a, Vector3 b){
		return object.Equals (GetBlock (a).worldPosition, GetBlock (b).worldPosition);
	}
}
