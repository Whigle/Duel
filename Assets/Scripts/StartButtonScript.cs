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
		UnityEngine.SceneManagement.SceneManager.LoadScene("Duel");
		//UnityEngine.SceneManagement.SceneManager.UnloadScene(1);
	}

	public void nastepnaTura(){
		GameLogicDataScript.nastepnaTura=true;
		Vector3 tmp = new Vector3(0.0f,-300.0f,0.0f);
		gameObject.GetComponent<Transform>().position=tmp;
	}
}
