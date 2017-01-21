using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ListaAtakowScript : MonoBehaviour {

	static bool gamePaused = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void wylacz() {
		if (gamePaused) {
			GameObject.Find ("CanvasLista").GetComponent<Canvas> ().enabled=false;
			GameObject.Find ("CanvasListaPod").GetComponent<Canvas> ().enabled=false;
			//Time.timeScale = 1;
			MousePointFields.setEnable(true);
			gamePaused = false;
		} else {
			GameObject.Find ("CanvasLista").GetComponent<Canvas> ().enabled=true;
			//Time.timeScale = 0;
			MousePointFields.setEnable(false);
			gamePaused = true;
		}
	}	
	public void Dalej() {
		//GameObject.Find ("CanvasLista").GetComponent<Canvas> ().enabled=false;
		GameObject.Find ("CanvasListaPod").GetComponent<Canvas> ().enabled=true;
	}
	public void Wstecz() {
		GameObject.Find ("CanvasListaPod").GetComponent<Canvas> ().enabled=false;
		GameObject.Find ("CanvasLista").GetComponent<Canvas> ().enabled=true;
	}

}
