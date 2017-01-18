using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartButtonScript : MonoBehaviour {

	public InputField IF;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
		}
		UnityEngine.SceneManagement.SceneManager.LoadScene("Duel");
	}

}
