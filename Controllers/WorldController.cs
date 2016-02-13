using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class WorldController : MonoBehaviour {

	bool running;
	public Sprite floorSprite;
	public int worldWidth, worldHeight;
	public bool startRandomised;
	public Color cellColours;

	public static WorldController Instance { get; protected set; }
	public World World { get; protected set; }

	Dictionary<Cell, GameObject> cellMap;

	float elapsedTime = 0f;

	public float secondsPerGeneration = 1f;

	public float SecondsPerGeneration {
		get {
			return secondsPerGeneration;
		}
		set {
			secondsPerGeneration = value;
		}
	}

	// Use this for initialization
	void Start () {
		Instance = this;

		running = false;

		World = new World (worldWidth, worldHeight);

		cellMap = new Dictionary<Cell, GameObject> ();

		for (int x = 0; x < World.Width; x++) {
			for (int y = 0; y < World.Height; y++) {
				Cell cell = World.GetCellAt (x, y);

				GameObject go = new GameObject ();
				go.name = "Cell_" + x + "_" + y;
				go.transform.position = new Vector3 (cell.X, cell.Y, 0);
				go.transform.SetParent (this.transform, true);
				go.AddComponent<SpriteRenderer> ().color = cellColours;

				cellMap.Add (cell, go);


				cell.RegisterCellChangedCallback (  OnCellChanged  );
			}
		}
		
		if(startRandomised)
			World.RandomiseCells ();
	}

	public void setSecondsPerGen(float f) {
		secondsPerGeneration = f;
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

	void OnCellChanged(Cell cell) {
		if (!cellMap.ContainsKey (cell)) {
			Debug.LogError ("OnCellChanged cannot find " + cell + " in the Dictionary");
			return;
		}

		GameObject go = cellMap [cell];

		if(cell.Alive) {
			go.GetComponent<SpriteRenderer> ().sprite = floorSprite;
		} else {
			go.GetComponent<SpriteRenderer> ().sprite = null;
		}
	}

	void DestroyAllTileGameObjects() {

		while(cellMap.Count > 0) {
			Cell cell = cellMap.Keys.First();
			GameObject go = cellMap [cell];

			cellMap.Remove (cell);

			cell.UnregisterCellChangedCallback (OnCellChanged);

			Destroy (go);

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

	public void randomiseCells() {
		World.RandomiseCells ();
	}
}
