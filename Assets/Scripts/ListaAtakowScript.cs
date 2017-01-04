using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class ListaAtakowScript : MonoBehaviour {

	private bool gamePaused = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}


	public void wylacz() {
		if (gamePaused) {
			GameObject.Find ("CanvasLista").GetComponent<Canvas> ().enabled=false;
			Time.timeScale = 1;
			MousePointFields.setEnable(true);
			gamePaused = false;
		} else {
			GameObject.Find ("CanvasLista").GetComponent<Canvas> ().enabled=true;
			Time.timeScale = 0;
			MousePointFields.setEnable(false);
			gamePaused = true;
		}
	}

}
