using UnityEngine;
using System.Collections;

public class WorldController : MonoBehaviour {

	bool running;
	public Sprite floorSprite;
	public int worldWidth, worldHeight;
	public bool startRandomised;
	public Color cellColours;

	public static WorldController Instance { get; protected set; }
	public World World { get; protected set; }

	float elapsedTime = 0f;

	public float secondsPerGeneration = 1f;

	// Use this for initialization
	void Start () {
		Instance = this;

		running = false;

		World = new World (worldWidth, worldHeight);

		for (int x = 0; x < World.Width; x++) {
			for (int y = 0; y < World.Height; y++) {
				Cell cell = World.GetCellAt (x, y);

				GameObject go = new GameObject ();
				go.name = "Cell_" + x + "_" + y;
				go.transform.position = new Vector3 (cell.X, cell.Y, 0);
				go.transform.SetParent (this.transform, true);
				go.AddComponent<SpriteRenderer> ().color = cellColours;

				cell.RegisterCellChangedCallback ( t => { OnCellChanged(cell, go); } );
			}
		}
		
		if(startRandomised)
			World.RandomiseCells ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!running)
			return;
		
		elapsedTime += Time.deltaTime;

		if (elapsedTime > secondsPerGeneration) {
			elapsedTime = 0f;

			foreach (Cell t in World.GetCells()) {
				t.calculateNumNeighbours ();
			}

			foreach (Cell t in World.GetCells()) {
				t.calculateLivingState ();
			}
		}
	}

	void OnCellChanged(Cell cell, GameObject go) {
		if(cell.Alive) {
			go.GetComponent<SpriteRenderer> ().sprite = floorSprite;
		} else {
			go.GetComponent<SpriteRenderer> ().sprite = null;
		}
	}

	public Cell GetCellAt(Vector3 coord) {
		return WorldController.Instance.World.GetCellAt (
			Mathf.FloorToInt (coord.x),
			Mathf.FloorToInt (coord.y));
	}

	public Cell GetCellAt(int x, int y) {
		return WorldController.Instance.World.GetCellAt (x, y);
	}

	public void toggleRunning() {
		running = !running;
	}
}
