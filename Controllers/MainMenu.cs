using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnCampaignClicked() {
		SceneManager.LoadScene ("Game");
	}

	public void OnExitClicked() {
		Debug.Log ("Exit Clicked");
		Application.Quit ();
	}

	public void OnOptionsClicked() {
		SceneManager.LoadScene("OptionsMenu");
	}
}
