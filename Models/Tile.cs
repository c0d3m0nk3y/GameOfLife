﻿using UnityEngine;
using System.Collections;
using System;

public class Tile {

	bool alive;

	public bool Alive {
		get {
			return alive;
		}
		set {
			alive = value;
			if(cbTileChanged != null)
				cbTileChanged (this);
		}
	}

	public void Toggle() {
		Alive = !Alive;
	}

	int numNeighbours = 0;

	Action<Tile> cbTileChanged;

	World world;

	public int X { get; protected set; }
	public int Y { get; protected set; }

	public Tile( World world, int x, int y) {
		this.world = world;
		this.X = x;
		this.Y = y;

		Alive = false;
	}

	public void calculateNumNeighbours() {
		numNeighbours = 0;

		if (X > 0 && world.GetTileAt (X - 1, Y).Alive == true) { // left
			numNeighbours++;
		}
		if (X < world.Width - 1 && world.GetTileAt (X + 1, Y).Alive == true) { // right
			numNeighbours++;
		}
		if (Y > 0 && world.GetTileAt (X, Y - 1).Alive == true) { // below
			numNeighbours++;
		}
		if (Y < world.Height - 1 && world.GetTileAt (X, Y + 1).Alive == true) { // above
			numNeighbours++;
		}

		if ((X > 0 && Y > 0) && world.GetTileAt (X - 1, Y - 1).Alive == true) { // below left
			numNeighbours++;
		}
		if ((X > 0 && Y < world.Width - 1) && world.GetTileAt (X - 1, Y + 1).Alive == true) { // above left
			numNeighbours++;
		}
		if ((X < world.Height - 1 && Y > 0) && world.GetTileAt (X + 1, Y - 1).Alive == true) { // below right
			numNeighbours++;
		}
		if ((X < world.Height - 1 && Y < world.Width - 1) && world.GetTileAt (X + 1, Y + 1).Alive == true) { // above right
			numNeighbours++;
		}
	}

	public void calculateLivingState() {
		if (Alive) {
			if (numNeighbours < 2 || numNeighbours > 3) {
				Alive = false;
			}
		} else {
			if (numNeighbours == 3) {
				Alive = true;
			}
		}
	}

	public void RegisterTileChangedCallback(Action<Tile> callback) {
		cbTileChanged += callback;
	}

	public void UnregisterTileChangedCallback(Action<Tile> callback) {
		cbTileChanged -= callback;
	}
}
