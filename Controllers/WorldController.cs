using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {
	public Sprite floorSprite;
	public int worldWidth, worldHeight;
	public bool startRandomised;

	public static WorldController Instance { get; protected set; }
	public World World { get; protected set; }

	float elapsedTime = 0f;

	public float secondsPerGeneration = 1f;

	// Use this for initialization
	void Start () {
		Instance = this;

		World = new World (worldWidth, worldHeight);

		for (int x = 0; x < World.Width; x++) {
			for (int y = 0; y < World.Height; y++) {
				Tile tile = World.GetTileAt (x, y);

				GameObject tile_go = new GameObject ();
				tile_go.name = "Tile_" + x + "_" + y;
				tile_go.transform.position = new Vector3 (tile.X, tile.Y, 0);
				tile_go.transform.SetParent (this.transform, true);
				tile_go.AddComponent<SpriteRenderer> ();

				tile.RegisterTileChangedCallback ( t => { OnTileChanged(tile, tile_go); } );
			}
		}
		
		if(startRandomised)
			World.RandomizeTiles ();
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;

		if (elapsedTime > secondsPerGeneration) {
			elapsedTime = 0f;

			foreach (Tile t in World.GetTiles()) {
				t.calculateNumNeighbours ();
			}

			foreach (Tile t in World.GetTiles()) {
				t.calculateLivingState ();
			}
		}
	}

	void OnTileChanged(Tile tile, GameObject tile_go) {
		if(tile.Alive) {
			tile_go.GetComponent<SpriteRenderer> ().sprite = floorSprite;
		} else {
			tile_go.GetComponent<SpriteRenderer> ().sprite = null;
		}
	}

	public Tile GetTileAt(Vector3 coord) {
		return WorldController.Instance.World.GetTileAt (
			Mathf.FloorToInt (coord.x),
			Mathf.FloorToInt (coord.y));
	}

	public Tile GetTileAt(int x, int y) {
		return WorldController.Instance.World.GetTileAt (x, y);
	}
}
