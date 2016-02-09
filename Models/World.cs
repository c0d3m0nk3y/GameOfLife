using UnityEngine;
using System.Collections;

public class World {

	Cell[,] cells;

	public int Width { get; protected set; }
	public int Height { get; protected set; }

	public World(int width, int height) {
		this.Width = width;
		this.Height = height;

		cells = new Cell[width, height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				cells[x,y] = new Cell(this, x, y);
			}
		}

		Debug.Log ((width * height) + " Cells");
	}

	public void RandomiseCells() {
		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				if(Random.Range(0, 2) == 0) {
					cells [x, y].Alive = true;
				} else {
					cells[x,y].Alive = false;
				}
			}
		}
	}

	public Cell GetCellAt(int x, int y) {
		if (x >= Width || x < 0 || y >= Height || y < 0) {
//			Debug.LogError ("Cell (" + x + "," + y + ") is out of range.");
			return null;
		}

		return cells[x,y];
	}

	public Cell[,] GetCells() {
		return cells;
	}
}
