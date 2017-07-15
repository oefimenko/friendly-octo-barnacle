using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager {

	private static GameManager instance;
	public static GameManager Instance {
		get
		{
			if (instance != null)
			{
				return instance;
			}
			else
			{
				instance = new GameManager();
				return instance;
			}
		}
	}

	public static void Purge () {
		if (instance != null)
		{
			instance.Destroy();
			instance = null;
		}
	}

	private GameObject gui;
	private string username;
	private HTTPClient client;

	public string UserName { get { return username; } }

	private GameManager () {
		client = new HTTPClient(Endpoints.HTTPEndpoint);
		gui = GameObject.FindGameObjectWithTag ("MainMenuGUI");
		Button[] buttons = gui.GetComponentsInChildren<Button>();
		for (int i = 0; i < buttons.Length; i++) {
			string name = buttons[i].gameObject.name;
			if (name.Contains("Training")) {
				buttons [i].onClick.AddListener (OnTrainingStart);
			} else if (name.Contains("Fight")) {
				buttons [i].onClick.AddListener (OnFightStart);
			}
		}
		InputField input = gui.GetComponentInChildren<InputField>();
		input.onValueChanged.AddListener (OnUsernameProvided);
	}

	public void Destroy () {
		
	}

	private void OnFightStart () {
		if (username != null) {
			LoadLevelAsync ();
			client.Skirmish ();
		}
	}

	private void OnTrainingStart () {
		if (username != null) {
			LoadLevelAsync ();
			client.Training ();
		}
	}

	private void OnUsernameProvided (string data) {
		username = data;
	}

	private void LoadLevelAsync () {
		client.Login(username);
		Image[] images = gui.GetComponentsInChildren<Image>();
		for (int i = 0; i < images.Length; i++) {
			if (images[i].gameObject.name.Contains ("Panel")) {
				images[i].gameObject.SetActive(true);
			}
		}
		SceneManager.LoadScene("Test");
	}
}