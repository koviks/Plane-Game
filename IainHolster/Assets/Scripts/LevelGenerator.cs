using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelGenerator : MonoBehaviour {

	public int levelsize;
	public int seed;
	public List<GameObject> tilelist = new List<GameObject>();
	public int[,] inttilemap;

	//Tiles
	public GameObject groundtile1;
	//Ground Scatter
	public GameObject Cactus1;
	public float cactusscatterodds = 0.1f; //Odds for spawning cactus every square
	public GameObject Rock1;
	public GameObject Rock2;
	public float rockscatterodds = 0.1f; //Odds for spawning rock every square
	public GameObject BigRock1;
	public float bigrockscatterodds = 0.1f; //Odds for spawning rock every square
	//Grass
	public GameObject Grass1;
	public float grassscatterodds = 0.1f; //Odds for spawning grass every square
	public List<Vector2> grasslist = new List<Vector2>();

	//Grass Generation
	int GetSurroundingWallCount(int gridX, int gridY) {
		int wallCount = 0;
		for (int neighbourX = gridX - 1; neighbourX <= gridX + 1; neighbourX ++) {
			for (int neighbourY = gridY - 1; neighbourY <= gridY + 1; neighbourY ++) {
				if (neighbourX >= 0 && neighbourX < levelsize && neighbourY >= 0 && neighbourY < levelsize) {
					if (neighbourX != gridX || neighbourY != gridY) {
						if (grasslist.Contains (new Vector2 (neighbourX, neighbourY))) {
							wallCount += 1;
						}
					}
				}
			}
		}
		return wallCount;
	}

	void Start () {
		Random.seed = seed;
		inttilemap = new int[Mathf.RoundToInt(levelsize), Mathf.RoundToInt(levelsize)];
		//Grass Generation
		for (int x = 0; x < levelsize; x++) {
			for (int y = 0; y < levelsize; y++) {
				float grasschance = Random.Range(1, 1 / grassscatterodds);
				if (Mathf.RoundToInt (grasschance) == 1) {
					grasslist.Add (new Vector2 (x, y));
				}
			}
		}
		//Smoothing Out
		for (int x = 0; x < levelsize; x++) {
			for (int y = 0; y < levelsize; y++) {
				int neighbourWallTiles = GetSurroundingWallCount (x, y);
				if (neighbourWallTiles > 3) {
					grasslist.Add (new Vector2 (x, y));
				}
			}
		}
		//Tile Generation
		for (int x = 0; x < levelsize; x++) {
			for (int y = 0; y < levelsize; y++) {
				float offset = Random.Range (-0.50f, 0.0f);
				GameObject tile = Instantiate(groundtile1, new Vector3(x * 2, offset, y * 2), transform.rotation) as GameObject;
				tile.GetComponent<BoxCollider> ().center = new Vector3(0, -offset, 0);
				float colordifference = Random.Range (-0.05f, 0.05f);
				tile.GetComponent<MeshRenderer> ().material.color = tile.GetComponent<MeshRenderer> ().material.color + new Color(colordifference, colordifference, colordifference);
				//Adding to List
				tilelist.Add(tile);
				//Spawning Ground Scatter
				float cactuschance = Random.Range(1, 1 / cactusscatterodds);
				float rockchance = Random.Range(1, 1 / rockscatterodds);
				float bigrockchance = Random.Range(1, 1 / bigrockscatterodds);
				//Grass
				if (grasslist.Contains(new Vector2(x, y)) && Mathf.RoundToInt (bigrockchance) != 1) {
					GameObject grass = Instantiate (Grass1, tile.transform.Find ("ObjectSpawn").transform.position, transform.rotation) as GameObject;
					//Random Rotation
					int randomrot = Random.Range (1, 5);
					if (randomrot == 1) {
						grass.transform.eulerAngles = new Vector3 (0, 0, 0);
					} else if (randomrot == 2) {
						grass.transform.eulerAngles = new Vector3 (0, 90, 0);
					} else if (randomrot == 3) {
						grass.transform.eulerAngles = new Vector3 (0, 180, 0);
					} else if (randomrot == 4) {
						grass.transform.eulerAngles = new Vector3 (0, 270, 0);
					}
					//Random Size
					grass.transform.localScale += new Vector3 (0, Random.Range (-0.5f, 0.5f));
					//Change In Color
					float randomdark = Random.Range (0.0f, 0.35f);
					grass.transform.GetChild (0).GetComponent<MeshRenderer> ().material.color -= new Color (randomdark, randomdark, randomdark);
				}
				//Cactus
				if (Mathf.RoundToInt (cactuschance) == 1) {
					GameObject cactus = Instantiate (Cactus1, tile.transform.Find ("ObjectSpawn").transform.position, transform.rotation) as GameObject;
					//Random Rotation
					int randomrot = Random.Range (1, 5);
					if (randomrot == 1) {
						cactus.transform.eulerAngles = new Vector3 (0, 0, 0);
					} else if (randomrot == 2) {
						cactus.transform.eulerAngles = new Vector3 (0, 90, 0);
					} else if (randomrot == 3) {
						cactus.transform.eulerAngles = new Vector3 (0, 180, 0);
					} else if (randomrot == 4) {
						cactus.transform.eulerAngles = new Vector3 (0, 270, 0);
					}
					//Random Size
					cactus.transform.localScale += new Vector3 (0, Random.Range (-0.25f, 0.25f));
					//Change In Color
					float randomdark = Random.Range (0.0f, 0.35f);
					cactus.transform.GetChild (0).GetComponent<MeshRenderer> ().material.color -= new Color (randomdark, randomdark, randomdark);
				}
				//Rocks
				else if (Mathf.RoundToInt (rockchance) == 1) {
					GameObject selectedrockprefab = Rock1;
					int randomrock = Random.Range (1, 3);
					if (randomrock == 1) {
						selectedrockprefab = Rock1;
					}
					if (randomrock == 2) {
						selectedrockprefab = Rock2;
					}
					GameObject rock = Instantiate (selectedrockprefab, tile.transform.Find ("ObjectSpawn").transform.position, transform.rotation) as GameObject;
					//Random Rotation
					int randomrot = Random.Range (1, 5);
					if (randomrot == 1) {
						rock.transform.eulerAngles = new Vector3 (0, 0, 0);
					} else if (randomrot == 2) {
						rock.transform.eulerAngles = new Vector3 (0, 90, 0);
					} else if (randomrot == 3) {
						rock.transform.eulerAngles = new Vector3 (0, 180, 0);
					} else if (randomrot == 4) {
						rock.transform.eulerAngles = new Vector3 (0, 270, 0);
					}
				} 
				//Big Rocks
				else if (Mathf.RoundToInt (bigrockchance) == 1) {
					GameObject bigrock = Instantiate (BigRock1, tile.transform.Find ("ObjectSpawn").transform.position, transform.rotation) as GameObject;
					//Random Rotation
					int randomrot = Random.Range (1, 5);
					if (randomrot == 1) {
						bigrock.transform.eulerAngles = new Vector3 (0, 0, 0);
					} else if (randomrot == 2) {
						bigrock.transform.eulerAngles = new Vector3 (0, 90, 0);
					} else if (randomrot == 3) {
						bigrock.transform.eulerAngles = new Vector3 (0, 180, 0);
					} else if (randomrot == 4) {
						bigrock.transform.eulerAngles = new Vector3 (0, 270, 0);
					}
					//Change In Color
					float randomdark = Random.Range (0.0f, 0.3f);
					bigrock.transform.GetChild (0).GetComponent<MeshRenderer> ().material.color -= new Color (randomdark, randomdark, randomdark);
				}
			}
		}
		UpdateTileMap();
	}

	void UpdateTileMap(){
		foreach (GameObject tile in tilelist) {
			//Checks if there is an object ontop of the tile
			RaycastHit[] objectcheckraycast = Physics.RaycastAll (tile.transform.position, Vector3.up * 100);
			foreach (RaycastHit rayhit in objectcheckraycast) {
				if (rayhit.transform.tag == "Object") {
					//1 for int means there is an object there
					inttilemap [Mathf.RoundToInt(tile.transform.position.x / 2), Mathf.RoundToInt(tile.transform.position.z / 2)] = 1;
				}
			}
		}
	}
}
