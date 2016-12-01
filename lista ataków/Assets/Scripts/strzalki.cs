using UnityEngine;
using System.Collections;

public class strzalki : MonoBehaviour {

	public static GameObject pierwszezaznaczone;
	public static GameObject drugiezaznaczone;
	public static GameObject ostatniezaznaczone;

	public GameObject poczatek;
	public GameObject koniec;

	bool visibility;

	// Use this for initialization
	void Start () {
		visibility = false;
	}
	
	// Update is called once per frame
	void Update () {

		if (GameObject.Find ("GameLogicData").GetComponent<GameLogicDataScript> ().getTuraTrwa ()) {
			if (poczatek.GetComponent<MousePointFields> ().target == Color.green) {
				visibility = true;
			} else if (poczatek.GetComponent<MousePointFields> ().target == Color.white && koniec.GetComponent<MousePointFields> ().target == Color.yellow && poczatek.GetComponent<MousePointFields> ().getVisibility () == true && koniec.GetComponent<MousePointFields> ().getVisibility () == true) {
				visibility = true;
			} else
				visibility = false;

			if (visibility == true) {
				gameObject.GetComponent<Renderer> ().enabled = true;
			} else {
				gameObject.GetComponent<Renderer> ().enabled = false;
			}
		} else gameObject.GetComponent<Renderer> ().enabled = false;
	}
}
