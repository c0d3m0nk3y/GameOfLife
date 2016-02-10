using UnityEngine;
using System.Collections.Generic;

public class MouseController : MonoBehaviour {

	Vector3 previousPos, currentPos, dragStartPos;
	public float scrollSpeed, maxSize, minSize, panSpeed;

	public Sprite cursorSprite;
	GameObject cursor;
	public Color highlightColour;
	public GameObject placeHighlight;
	List<GameObject> highlightList;

	void Start() {
		cursor = new GameObject ();
		cursor.AddComponent<SpriteRenderer> ().sprite = cursorSprite;
		cursor.GetComponent<SpriteRenderer> ().sortingLayerName = "UI";

		highlightList = new List<GameObject> ();
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
		RemoveHighlights ();

		StartDrag ();

		// we want to make sure that start is smaller than end so we can
		// drag both directions on an axis, otherwise the loop condition
		// would instantly be met.
		int startX = Mathf.FloorToInt (Mathf.Min (dragStartPos.x, currentPos.x));
		int endX = Mathf.FloorToInt (Mathf.Max (dragStartPos.x, currentPos.x));

		int startY = Mathf.FloorToInt (Mathf.Min (dragStartPos.y, currentPos.y));
		int endY = Mathf.FloorToInt (Mathf.Max (dragStartPos.y, currentPos.y));
		
		PreviewDrag (startX, endX, startY, endY);

		EndDrag (startX, endX, startY, endY);
	}

	void RemoveHighlights() {
		while (highlightList.Count > 0) {
			GameObject go = highlightList [highlightList.Count - 1];
			highlightList.RemoveAt (highlightList.Count - 1);
			SimplePool.Despawn (go);
		}
	}

	void PreviewDrag(int startX, int endX, int startY, int endY) {
		if (Input.GetMouseButton (0)) {

			for (int x = startX; x <= endX; x++) {
				for (int y = startY; y <= endY; y++) {
					Cell t = GetCellAt (x, y);
					if (t != null) {
						GameObject go = SimplePool.Spawn (placeHighlight, new Vector3 (x, y, 0), Quaternion.identity);
						go.transform.SetParent (this.transform, true);
						go.GetComponent<SpriteRenderer> ().color = highlightColour;
						highlightList.Add (go);
					}
				}
			}
		}
	}

	void EndDrag(int startX, int endX, int startY, int endY) {
		if (Input.GetMouseButtonUp (0)) {

			for (int x = startX; x <= endX; x++) {
				for (int y = startY; y <= endY; y++) {
					Cell t = GetCellAt (x, y);
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
		

	Cell GetCellAt(Vector3 coord) {
		return WorldController.Instance.GetCellAt (
			Mathf.FloorToInt (coord.x),
			Mathf.FloorToInt (coord.y));
	}

	Cell GetCellAt(int x, int y) {
		return WorldController.Instance.GetCellAt (x, y);
	}
}
