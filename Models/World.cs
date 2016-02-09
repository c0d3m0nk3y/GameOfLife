using UnityEngine;
using System.Collections;

public class World {

	Tile[,] tiles;

	public int Width { get; protected set; }
	public int Height { get; protected set; }

	public World(int width, int height) {
		this.Width = width;
		this.Height = height;

		tiles = new Tile[width, height];

		for (int x = 0; x < width; x++) {
			for (int y = 0; y < height; y++) {
				tiles[x,y] = new Tile(this, x, y);
			}
		}

		Debug.Log ((width * height) + " Tiles");
	}

	public void RandomizeTiles() {
		for (int x = 0; x < Width; x++) {
			for (int y = 0; y < Height; y++) {
				if(Random.Range(0, 2) == 0) {
					tiles [x, y].Alive = true;
				} else {
					tiles[x,y].Alive = false;
				}
			}
		}
	}

	public Tile GetTileAt(int x, int y) {
		if (x >= Width || x < 0 || y >= Height || y < 0) {
//			Debug.LogError ("Tile (" + x + "," + y + ") is out of range.");
			return null;
		}

		return tiles[x,y];
	}

	public Tile[,] GetTiles() {
		return tiles;
	}
}
