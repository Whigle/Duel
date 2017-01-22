using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartButtonScript : MonoBehaviour {

	public InputField IF;
	private static bool credits=false;
	public Texture texture;
	Rect rectangle, rectangle2;

	// Use this for initialization
	void Start () {
		float wysokosc=85;
		rectangle=Rect.MinMaxRect(Screen.width/2-160,Screen.height/2-140+wysokosc,Screen.width/2+160,Screen.height/2+140+wysokosc);
		rectangle2=Rect.MinMaxRect(Screen.width/2-180,Screen.height/2-160+wysokosc,Screen.width/2+180,Screen.height/2+160+wysokosc);
	}
	
	// Update is called once per frame
	void Update () {
		if(UnityEngine.SceneManagement.SceneManager.GetActiveScene()==UnityEngine.SceneManagement.SceneManager.GetSceneByName("MainMenu")){
			if (!GameObject.Find("InputField").GetComponent<InputField>().interactable&&!credits&&!Input.GetMouseButton(0)&&!Input.GetMouseButton(1)&&!Input.GetMouseButton(2)) enable(true);
		}
	}

	public void startGame(){
		GameLogicDataScript.multiplayer=false;
		//(IF.text!="") ? GameLogicDataScript.nazwaGracza = IF.text : GameLogicDataScript.nazwaGracza = "";
		if (IF.text != "") {
			GameLogicDataScript.nazwaGracza = IF.text;
		} else {
			GameLogicDataScript.nazwaGracza = "Gracz 1";
		}
		UnityEngine.SceneManagement.SceneManager.LoadScene("Duel");

	}

	public void startMultiGame(){
		GameLogicDataScript.multiplayer=true;
		if (IF.text != "") {
			GameLogicDataScript.nazwaGracza = IF.text;
		} else {
			GameLogicDataScript.nazwaGracza = "Gracz 2";
		}
		UnityEngine.SceneManagement.SceneManager.LoadScene("Duel");
	}

	public void backToMenu(){
		GameLogicDataScript.resetVariables();
		UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
	}

	public void quitGame(){
		Application.Quit();
	}

	public void showCredits(){
		credits=true;
		enable(false);
	}

	private void OnGUI(){
		if (credits){
			credits=true;
			GUI.DrawTexture(rectangle2,texture);
			Color old=GUI.color;
			GUI.color=Color.red;
			GUILayout.BeginArea(rectangle);
			GUILayout.Label("CMYK Ferrets:\n-Norbert Błaszczyk\n-Ada Stachowiak\n-Natalia Wylezińska\n-Marcin Zieliński\n\n\nMuzyka:\nLord of the Land Kevin MacLeod (incometech.com)\nLicensed under Creative Commons: By Attribution 3.0 License\nhttp://creativecommons.org/licenses/by/3.0/");
			GUI.color=old;
			if(GUILayout.Button("Wróć")){
				credits=false;
				enable(false);
			}
			GUILayout.EndArea();
		}
	}

	private void enable(bool onoff){
		GameObject.Find("InputField").GetComponent<InputField>().interactable=onoff;
		GameObject.Find("StartSingleButton").GetComponent<Button>().interactable=onoff;
		GameObject.Find("ListaAtakowButton").GetComponent<Button>().interactable=onoff;
		GameObject.Find("CreditsButton").GetComponent<Button>().interactable=onoff;
		GameObject.Find("ExitButton").GetComponent<Button>().interactable=onoff;
		GameObject.Find("StartMultiButton").GetComponent<Button>().interactable=onoff;
	}
}
