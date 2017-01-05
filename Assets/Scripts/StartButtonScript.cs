using UnityEngine;
using System.Collections;

public class StartButtonScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void startGame(){
		GameLogicDataScript.multiplayer=false;
		UnityEngine.SceneManagement.SceneManager.LoadScene("Duel");
	}

	public void startMultiGame(){
		GameLogicDataScript.multiplayer=true;
		UnityEngine.SceneManagement.SceneManager.LoadScene("Duel");
	}

}
