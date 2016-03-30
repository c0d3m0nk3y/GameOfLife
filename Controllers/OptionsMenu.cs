using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsMenu : MonoBehaviour {

	bool fullscreen = false;
	int width = 1024;
	int height = 768;

	public InputField widthInputField;
	public InputField heightInputField;

	EventSystem system;

	// Use this for initialization
	void Start () {
		widthInputField.text = width.ToString();
		heightInputField.text = height.ToString();
		system = EventSystem.current;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Tab)) {
			Selectable next = null;// system.currentSelectedGameObject.GetComponent<Selectable> ().FindSelectableOnDown ();

			if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
				next = system.currentSelectedGameObject.GetComponent<Selectable> ().FindSelectableOnUp ();
				if(next == null) {
					next = system.lastSelectedGameObject.GetComponent<Selectable> ();
				}
			} else {
				next = system.currentSelectedGameObject.GetComponent<Selectable> ().FindSelectableOnDown ();
				if(next == null) {
					next = system.lastSelectedGameObject.GetComponent<Selectable> ();
				}
			}

			if(next != null) {
				InputField inputField = next.GetComponent<InputField> ();
				if(inputField != null) {
					inputField.OnPointerClick (new PointerEventData (system));
				}

				system.SetSelectedGameObject (next.gameObject, new BaseEventData (system));
			}
		}
	}

	public void OnFullscreenToggled() {
		fullscreen = !fullscreen;
	}

	public void OnWidthChanged() {
		width = int.Parse(widthInputField.text);
	}

	public void OnHeightChanged() {
		
		height = int.Parse(heightInputField.text);
	}

	public void OnBackClicked() {
		SceneManager.LoadScene ("MainMenu");
	}

	public void OnApplyClicked() {
		Screen.SetResolution (width, height, fullscreen);
	}
}
