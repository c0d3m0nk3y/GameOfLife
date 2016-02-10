using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {

	public float itemTargetHeight;

	void Start() {
		AdjustSize ();
	}

	void Update() {
		
	}

	public void AdjustSize() {
		Vector2 size = this.GetComponent<RectTransform> ().sizeDelta;
		size.y = this.transform.childCount * itemTargetHeight;
		this.GetComponent<RectTransform> ().sizeDelta = size;
	}
}
