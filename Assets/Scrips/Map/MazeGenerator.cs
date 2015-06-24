using UnityEngine;
using UnityEditor;

public class MazeGenerator : MonoBehaviour {
	public Transform ground;
	public GameObject vertWall;
	public GameObject horiWall;
	public GameObject smallWall;
	public GameObject g;
	public static int[,] gridMap;
	int offset;
	int gridSizeX;
	int gridSizeZ;

	// Use this for initialization
	void Awake () {
		gridSizeX = GameMaster.gridSizeX;
		gridSizeZ = GameMaster.gridSizeZ;
		offset = GameMaster.gridSizeOffset;
		ground.position = new Vector3(GameMaster.worldPosX/2,0,GameMaster.worldPosZ/2);
		ground.localScale = new Vector3(gridSizeX, 1, gridSizeZ);
		NavMeshBuilder.BuildNavMesh();
		GenerateMapData();
		GenerateMapVisual();
	}

	void GenerateMapData() {
		gridMap = new int[gridSizeX, gridSizeZ];
		Generate(0,0);
	}

	void GenerateMapVisual() {
		GameObject t;

        // draw Wall from north to south
        for (int z = gridSizeZ; 0 < z; z--) {
            for (int x = 0; x < gridSizeX; x++) {
                // draw the north wall
                if ((gridMap[x, z - 1] & 1) == 0)
                    t = (GameObject)Instantiate(horiWall, new Vector3(x * offset + 4.5f, -.5f, z * offset), Quaternion.identity);
                else
                    t = (GameObject)Instantiate(smallWall, new Vector3(x * offset, -.5f, z * offset), Quaternion.identity);
                t.gameObject.transform.parent = transform;
                // draw the west wall
                if ((gridMap[x, z - 1] & 8) == 0)
                    t = (GameObject)Instantiate(vertWall, new Vector3(x * offset, -.5f, z * offset - 5), Quaternion.identity);
                t.gameObject.transform.parent = transform;
                // draw the down ground
                if ((gridMap[x, z - 1] & 32) == 0)
                    t = (GameObject)Instantiate(g, new Vector3(x * offset + 5f, -.5f, z * offset - 5f), Quaternion.identity);
                t.gameObject.transform.parent = transform;
            }
            t = (GameObject)Instantiate(smallWall, new Vector3(gridSizeX * offset, -.5f, z * offset), Quaternion.identity);
            t.gameObject.transform.parent = transform;
            t = (GameObject)Instantiate(vertWall, new Vector3(gridSizeX * offset, -.5f, z * offset - 5), Quaternion.identity);
            t.gameObject.transform.parent = transform;
        }
        // draw the south wall
        for (int x = 0; x < gridSizeX; x++) {
            t = (GameObject)Instantiate(horiWall, new Vector3(x * offset + 4.5f, -.5f, 0), Quaternion.identity);
            t.gameObject.transform.parent = transform;
        }

        t = (GameObject)Instantiate(smallWall, new Vector3(gridSizeX * offset, -.5f, 0), Quaternion.identity);
        t.gameObject.transform.parent = transform;
	}

	void Generate (int cx, int cz) {
		Direction[] direction = new Direction[4]{new Direction("North"), new Direction("South"), 
			new Direction("West"), new Direction("East")};
		Shuffle(direction);
		for(int i = 0; i < direction.Length; i++) {
			int nx = cx + direction[i].x;
			int nz = cz + direction[i].z;
			if(Between(nx, gridSizeX) && Between(nz, gridSizeZ) && (gridMap[nx,nz] == 0)) {
				gridMap[cx,cz] |= direction[i].bit;
				gridMap[nx,nz] |= direction[i].oBit;
				Generate(nx,nz);
			}
		}
	}
	
	bool Between(int val, int max) {
		return (val >= 0) && (val < max);
	}
	
	void Shuffle(Direction[] array) {
		int m = array.Length;
		// While there remain elements to Shuffle…
		for(int j = 0; j <= m; j++) {
			// Pick a remaining element…
			int i = (int)Mathf.Floor(Random.value * m--);
			// And swap it with the current element.
			Direction t = array[m];
			array[m] = array[i];
			array[i] = t;
		}
	}
	
	class Direction {
		public int bit;
		public int oBit;
		public int x, z;
		public string s;
		
		public Direction(string s) {
			this.s = s;
			setDirection();
		}
		
		public void setDirection() {
			switch(s) {
			case"North": 
				bit = 1;
				oBit = 2;
				x = 0;
				z = 1;
				break;
			case"South":
				bit = 2;
				oBit = 1;
				x = 0;
				z = -1;
				break;
			case"East":
				bit = 4;
				oBit = 8;
				x = 1;
				z = 0;
				break;
			case"West":
				bit = 8;
				oBit = 4;
				x = -1;
				z = 0;
				break;
			}
		}
	}
}
