using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(MenuController))]
public class MenuControllerEditor : Editor {

	public override void OnInspectorGUI() {
		DrawDefaultInspector ();

		if(GUILayout.Button ("Refresh")) {
			((MenuController)target).AdjustSize();
		}
	}
}
