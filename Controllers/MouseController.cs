using UnityEngine;
using System.Collections;

public class MouseController : MonoBehaviour {

	Vector3 previousPos, currentPos, dragStartPos;
	public float scrollSpeed, maxSize, minSize, panSpeed;

	GameObject cursor;
	public Sprite cursorSprite;

	void Start() {
		cursor = new GameObject ();
		cursor.AddComponent<SpriteRenderer> ().sprite = cursorSprite;
		cursor.GetComponent<SpriteRenderer> ().sortingLayerName = "UI";
	}

	// Update is called once per frame
	void Update () {
		currentPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);

		MoveCursor ();

		DragPlace ();

		PanCamera ();

		ZoomCamera ();

		previousPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
	}

	void DragPlace() {
		StartDrag ();

		EndDrag ();
	}

	void EndDrag() {
		if (Input.GetMouseButtonUp (0)) {

			// we want to make sure that start is smaller than end so we can
			// drag both directions on an axis, otherwise the loop condition
			// would instantly be met.
			int startX = Mathf.FloorToInt (Mathf.Min (dragStartPos.x, currentPos.x));
			int endX = Mathf.FloorToInt (Mathf.Max (dragStartPos.x, currentPos.x));

			int startY = Mathf.FloorToInt (Mathf.Min (dragStartPos.y, currentPos.y));
			int endY = Mathf.FloorToInt (Mathf.Max (dragStartPos.y, currentPos.y));

			for (int x = startX; x <= endX; x++) {
				for (int y = startY; y <= endY; y++) {
					Tile t = GetTileAt (x, y);
					if (t != null) {
						t.Toggle ();
					}
				}
			}
		}
	}

	void StartDrag() {
		if (Input.GetMouseButtonDown (0)) {
			dragStartPos = currentPos;
		}
	}

	void MoveCursor() {
		cursor.transform.position = new Vector3 (Mathf.FloorToInt(currentPos.x), Mathf.FloorToInt(currentPos.y), 1);
	}

	void PanCamera() {
		if (Input.GetMouseButton (1)) {
			Camera.main.transform.Translate ((previousPos - currentPos) * panSpeed);
		}
	}

	void ZoomCamera() {
		Camera.main.orthographicSize -= Camera.main.orthographicSize * Input.GetAxis ("Mouse ScrollWheel") * scrollSpeed;
		Camera.main.orthographicSize = Mathf.Clamp (Camera.main.orthographicSize, minSize, maxSize);
	}
		

	Tile GetTileAt(Vector3 coord) {
		return WorldController.Instance.GetTileAt (
			Mathf.FloorToInt (coord.x),
			Mathf.FloorToInt (coord.y));
	}

	Tile GetTileAt(int x, int y) {
		return WorldController.Instance.GetTileAt (x, y);
	}
}
