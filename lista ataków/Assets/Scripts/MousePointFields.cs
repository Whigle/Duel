using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Klasa odpowiedzialna za przetwarzanie interakcji gracza z HUD'em.
/// </summary>
public class MousePointFields : MonoBehaviour {


	public static GameObject pierwszezaznaczone;
	public static GameObject drugiezaznaczone;
	public static GameObject ostatniezaznaczone;

	/// <summary>
	/// Przechowuje kolor danego pola.
	/// </summary>
	public Color target = Color.red;
	/// <summary>
	/// Przechowuje kolor tła.
	/// </summary>
	public Color areacolor = new Color(1,1,1,0.5f);
	/// <summary>
	/// Hitbox - tablica (3x3) wartości logicznych odzwierciedlająca zaznaczone pola.
	/// </summary>
	public static bool[,] hitbox = new bool[3,3];
	/// <summary>
	/// Obiekt klasy GameObject reprezentujący tło
	/// </summary>
	public GameObject HUDCombatField; /*tekst*/
	/// <summary>
	/// Indeksy danego pola w hitboxie.
	/// </summary>
	int idx, idy;
	/// <summary>
	/// Flaga reprezentująca widzialność tego pola.
	/// </summary>
	bool visibility=false;
	/// <summary>
	/// Flaga reprezentująca możliwość zaznaczania pól.
	/// </summary>
	static bool check=true;
	/// <summary>
	/// Flaga reprezentująca tryb walki. 1-atak, 0-obrona.
	/// </summary>
	static bool mode=false;
	/// <summary>
	/// Flaga reprezentująca aktywność HUD'a.
	/// </summary>
	static bool enable=true;

	/// <summary>
	/// Metoda uruchamiana podczas inicjalizacji klasy w momencie startu skryptu.
	/// Na podstawie nazwy obiektu przypisuje mu jego indeksy w hitboxie. Inicjalizuje obiekt tła.
	/// </summary>
	void Start()
	{
		HUDCombatField=GameObject.Find("HUDCombatArea");
		clearHitbox();
		switch (gameObject.name){
			case "TopLeftCombatField":
				idx=0; idy=0; break;
			case "TopCombatField":
				idx=0; idy=1; break;
			case "TopRightCombatField":
				idx=0; idy=2; break;
			case "LeftCombatField":
				idx=1; idy=0; break;
			case "MiddleCombatField":
				idx=1; idy=1; break;
			case "RightCombatField":
				idx=1; idy=2; break;
			case "BottomLeftCombatField":
				idx=2; idy=0; break;
			case "BottomCombatField":
				idx=2; idy=1; break;
			case "BottomRightCombatField":
				idx=2; idy=2; break;
		}
	}
	
	/// <summary>
	/// Metoda wykonywana w każdej klatce.
	/// Odpowiada za aktualizację flag i wartości kolorów. 
	/// </summary>
	void Update () 
	{
		//jeśli HUD jest aktywny
		if(enable){
			check=true; //włącz możliwość zaznaczania pól
			//jeśli gracz puścił któryś z przycisków myszy, to wyłącz możliwość zaznaczania pól
			if(Input.GetMouseButtonUp(0)||Input.GetMouseButtonUp(1)) {
				check=false;	
				pierwszezaznaczone = null;
				drugiezaznaczone = null;
				visibility=false;	//zrób niewidocznym
				target=Color.white;
			}
			//jeśli nie zostaly zaznaczone żadne pola, to przywróć tło do normalnego koloru
			else if (countMarkedFields()==0){
				areacolor=new Color(1,1,1,0.5f);
			}
			//jeśli może zaznaczać
			if(check){
				//jeśli gracz przytrzymuje LPM - atak
				if (Input.GetMouseButton(0)){
					areacolor=new Color(1,0,0,0.5f);	//zmień kolor tła na czerwony
					mode=true;	//ustaw tryb na atak
				}
				//jeśli gracz przytrzymuje PPM - obrona
				else if (Input.GetMouseButton(1)){
					areacolor=new Color(0,0,1,0.5f);	//zmień kolor tła na niebieski
					mode=false;	//ustaw tryb na obronę
				}
			}
			//przypisanie koloru tła do renderera obiektu tła
			HUDCombatField.GetComponent<SpriteRenderer>().color=areacolor;
			//przypisanie widoczności pola na podstawie flagi zaznaczenia z hitboxa i flagi widoczności, poprzez włączenie lub wyłączenie renderera
			if (!visibility){
				gameObject.GetComponent<Renderer>().enabled=hitbox[idx,idy];
			}
			else {
				gameObject.GetComponent<Renderer>().enabled=visibility;
			}
			//przypisanie koloru pola do renderera obiektu pola
			czyPokolorowac();
		}
		//jeśli HUD jest nieaktywny
		else{
			visibility = false;
			gameObject.GetComponent<Renderer>().enabled=false; //wyłącz wyświetlanie pola
			HUDCombatField.GetComponent<SpriteRenderer>().color=areacolor=new Color(1,1,1,0.5f); //ustaw kolor tła na normalny
		}
	}
	
	/// <summary>
	/// Metoda wykonuje się, jeśli najedziemy kursorem myszy na obiekt.
	/// Ustawia kolory i flagi pól.
	/// </summary>
	public void OnMouseOver()
	{
		//jeśli można zaznaczać
		if(check){
			//jeśli jest wciśnięty LPM
			if (Input.GetMouseButton(0)) czyMoznaZaznaczyc ();
			//jeśli jest wciśnięty PPM
			else if (Input.GetMouseButton(1)) czyMoznaZaznaczyc ();
			//jeśli nie jest wciśnięty żaden przycisk
			else {
				visibility=true; //zrób pole widocznym
				target = Color.green;	//zmień kolor pola na zielony
			}
		}
	}

	/// <summary>
	/// Metoda wykonująca się, gdy kursor zjedzie z obiektu.
	/// Zmienia widoczność i kolor pola.
	/// </summary>
	public void OnMouseExit()
	{
		visibility=false;	//zrób niewidocznym
		target=Color.white;	//zmień kolor na biały
	}

	/// <summary>
	/// Ustawia wszystkie flagi w hitboxie na fałsz.
	/// </summary>
	public static void clearHitbox(){
		for(int i=0;i<3;i++){
			for(int j=0;j<3;j++){
				hitbox[i,j]=false;
			}
		}
	}
		
	/// <summary>
	/// Metoda zwraca ilość zaznaczonych pól w hitboxie
	/// </summary>
	/// <returns>ilość zaznaczonych pól w hitboxie</returns>
	public int countMarkedFields(){
		int marked=0;
		for(int i=0;i<3;i++){
			for(int j=0;j<3;j++){
				if(hitbox[i,j]==true){
					marked++;
				}
			}
		}
		return marked;
	}
	/// <summary>
	/// Resetuje hitbox i flagi HUD'a.
	/// </summary>
	public static void reset(){
		check=true;
		clearHitbox();
		mode=false;

		pierwszezaznaczone = null;
		drugiezaznaczone = null;
		ostatniezaznaczone = null;

		GameObject.Find ("TopLeftCombatField").GetComponent<MousePointFields> ().visibility = false;
		GameObject.Find ("TopCombatField").GetComponent<MousePointFields> ().visibility = false;
		GameObject.Find ("TopRightCombatField").GetComponent<MousePointFields> ().visibility = false;
		GameObject.Find ("LeftCombatField").GetComponent<MousePointFields> ().visibility = false;
		GameObject.Find ("MiddleCombatField").GetComponent<MousePointFields> ().visibility = false;
		GameObject.Find ("RightCombatField").GetComponent<MousePointFields> ().visibility = false;
		GameObject.Find ("BottomLeftCombatField").GetComponent<MousePointFields> ().visibility = false;
		GameObject.Find ("BottomCombatField").GetComponent<MousePointFields> ().visibility = false;
		GameObject.Find ("BottomRightCombatField").GetComponent<MousePointFields> ().visibility = false;

	}

	/// <summary>
	/// Zwraca hitbox.
	/// </summary>
	/// <returns>hitbox</returns>
	public static bool[,] getHitbox(){
		return hitbox;
	}
	/// <summary>
	/// Zwraca tryb walki
	/// </summary>
	/// <returns><c>true</c>, jeśli atak, <c>false</c> obrona.</returns>
	public static bool getMode(){
		return mode;
	}
	/// <summary>
	/// Zwraca flagę zaznaczania pól
	/// </summary>
	/// <returns><c>true</c>, jeśli można zaznaczać, <c>false</c> jeśli nie można.</returns>
	public static bool getCheck(){
		return check;
	}
	/// <summary>
	/// Ustawia flagę zaznaczania
	/// </summary>
	/// <param name="c">Jeśli <c>true</c>, to można zaznaczać.</param>
	public static void setCheck(bool c){
		check=c;
	}
	/// <summary>
	/// Zwraca flagę aktywności HUD'a.
	/// </summary>
	/// <returns><c>true</c>, aktywny, <c>false</c> nieaktywny.</returns>
	public static bool getEnable(){
		return enable;
	}
	/// <summary>
	/// Ustawia flagę aktywności HUD'a.
	/// </summary>
	/// <param name="e"><c>true</c>, aktywny, <c>false</c> nieaktywny.</param>
	public static void setEnable(bool e){
		enable=e;
	}

	public int getIndX(){
		return this.idx;
	}

	public int getIndY(){
		return this.idy;
	}

	public bool getVisibility() {
		return this.visibility;
	}

	/// <summary>
	/// DEBUG
	/// Wyświetla hitbox i ilość pól na konsolę
	/// </summary>
	public void printHitbox(){
		string str="HITBOX:\n";
		for(int i=0;i<3;i++){
			for(int j=0;j<3;j++){
				if(hitbox[i,j]==true) str+="1";
				else str+="0";
				str+=" ";
			}
			str+="\n";
		}
		str+="Marked: ";
		str+=countMarkedFields().ToString();
		print(str);
	}

	public void czyMoznaZaznaczyc() {
		target = Color.white;
		if (countMarkedFields () < 3) {
			if (pierwszezaznaczone == null) {
				pierwszezaznaczone = this.gameObject;
				hitbox [idx, idy] = true;
			} 
			if (pierwszezaznaczone != null && pierwszezaznaczone != this.gameObject && drugiezaznaczone == null) {
				if ((pierwszezaznaczone.GetComponent<MousePointFields> ().idx != 1 && pierwszezaznaczone.GetComponent<MousePointFields> ().idy != 1) || (pierwszezaznaczone.GetComponent<MousePointFields> ().idx == 1 && pierwszezaznaczone.GetComponent<MousePointFields> ().idy == 1)) {
					if (Mathf.Abs (pierwszezaznaczone.GetComponent<MousePointFields> ().idx - idx) <= 1 && Mathf.Abs (pierwszezaznaczone.GetComponent<MousePointFields> ().idy - idy) <= 1) {
						drugiezaznaczone = this.gameObject;
						hitbox [idx, idy] = true;
					}
				} 
				if (!(pierwszezaznaczone.GetComponent<MousePointFields> ().idx != 1 && pierwszezaznaczone.GetComponent<MousePointFields> ().idy != 1) || !(pierwszezaznaczone.GetComponent<MousePointFields> ().idx == 1 && pierwszezaznaczone.GetComponent<MousePointFields> ().idy == 1)) {
					if ((Mathf.Abs (pierwszezaznaczone.GetComponent<MousePointFields> ().idx - idx) <= 1 && pierwszezaznaczone.GetComponent<MousePointFields> ().idy == idy) || (Mathf.Abs (pierwszezaznaczone.GetComponent<MousePointFields> ().idy - idy) <= 1 && pierwszezaznaczone.GetComponent<MousePointFields> ().idx == idx)) {
						drugiezaznaczone = this.gameObject;
						hitbox [idx, idy] = true;
					}
				}
			} else if (pierwszezaznaczone != null && pierwszezaznaczone != this.gameObject && drugiezaznaczone != null && drugiezaznaczone != this.gameObject) {
				int p1x = pierwszezaznaczone.GetComponent<MousePointFields> ().idx;
				int p1y = pierwszezaznaczone.GetComponent<MousePointFields> ().idy;
				int p2x = drugiezaznaczone.GetComponent<MousePointFields> ().idx;
				int p2y = drugiezaznaczone.GetComponent<MousePointFields> ().idy;
				print (p1x + " " + p1y + " " + p2x + " " + p2y);
				if (p1x != p2x && p1y != p2y) {
					if (Mathf.Abs (p2x - idx) <= 1 && Mathf.Abs (p2y - idy) <= 1 && p1x != idx && p1y != idy) {
						hitbox [idx, idy] = true;
						ostatniezaznaczone = this.gameObject;
					} 
				} 
				if ((p1x == p2x && p1y != p2y) || (p1x != p2x && p1y == p2y)) {
					if (p1x == 1 && p1y == 1) {
					} else if (p2x == 1 && p2y == 1) {
						if ((Mathf.Abs (p2x - idx) <= 1 && Mathf.Abs (p2y - idy) <= 1) && (Mathf.Abs (p1x - idx) > 1 || Mathf.Abs (p1y - idy) > 1)) {
							hitbox [idx, idy] = true;
							ostatniezaznaczone = this.gameObject;
						}
					} else if ((p1x == 1 || p1y == 1) && (p2x != 1 && p2y != 1)) {
						if((Mathf.Abs (p2x - idx) <= 1 && p2y == idy) || (Mathf.Abs (p2y - idy) <= 1 && p2x == idx)) {
							hitbox [idx, idy] = true;
							ostatniezaznaczone = this.gameObject;
						}
					} else {
						if (((Mathf.Abs (p2x - idx) <= 1 && p2y == idy) || (Mathf.Abs (p2y - idy) <= 1 && p2x == idx)) && this.gameObject != pierwszezaznaczone && (idx!=1&&idy!=1)) {
							hitbox [idx, idy] = true;
							ostatniezaznaczone = this.gameObject;
						}
					}
				}
			}
		}
	}

	public void czyPokolorowac() {
		if (pierwszezaznaczone != null && pierwszezaznaczone != this.gameObject && drugiezaznaczone == null) {
			if ((pierwszezaznaczone.GetComponent<MousePointFields> ().idx != 1 && pierwszezaznaczone.GetComponent<MousePointFields> ().idy != 1) || (pierwszezaznaczone.GetComponent<MousePointFields> ().idx == 1 && pierwszezaznaczone.GetComponent<MousePointFields> ().idy == 1)) {
				if (Mathf.Abs (pierwszezaznaczone.GetComponent<MousePointFields> ().idx - idx) <= 1 && Mathf.Abs (pierwszezaznaczone.GetComponent<MousePointFields> ().idy - idy) <= 1) {
					visibility = true;
					gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
					target=Color.yellow;
				}
			} 
			if (!(pierwszezaznaczone.GetComponent<MousePointFields> ().idx != 1 && pierwszezaznaczone.GetComponent<MousePointFields> ().idy != 1) || !(pierwszezaznaczone.GetComponent<MousePointFields> ().idx == 1 && pierwszezaznaczone.GetComponent<MousePointFields> ().idy == 1)) {
				if ((Mathf.Abs (pierwszezaznaczone.GetComponent<MousePointFields> ().idx - idx) <= 1 && pierwszezaznaczone.GetComponent<MousePointFields> ().idy == idy) || (Mathf.Abs (pierwszezaznaczone.GetComponent<MousePointFields> ().idy - idy) <= 1 && pierwszezaznaczone.GetComponent<MousePointFields> ().idx == idx)) {
					visibility = true;
					gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
					target=Color.yellow;
				}
			}
		} else if (pierwszezaznaczone != null && pierwszezaznaczone != this.gameObject && drugiezaznaczone != null && drugiezaznaczone != this.gameObject && ostatniezaznaczone==null) {
			visibility = false;	//zrób niewidocznym
			target = Color.white;
			int p1x = pierwszezaznaczone.GetComponent<MousePointFields> ().idx;
			int p1y = pierwszezaznaczone.GetComponent<MousePointFields> ().idy;
			int p2x = drugiezaznaczone.GetComponent<MousePointFields> ().idx;
			int p2y = drugiezaznaczone.GetComponent<MousePointFields> ().idy;
			print (p1x + " " + p1y + " " + p2x + " " + p2y);
			if (p1x != p2x && p1y != p2y) {
				if (Mathf.Abs (p2x - idx) <= 1 && Mathf.Abs (p2y - idy) <= 1 && p1x != idx && p1y != idy) {
					visibility = true;
					gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
					target=Color.yellow;
				} 
			} 
			if ((p1x == p2x && p1y != p2y) || (p1x != p2x && p1y == p2y)) {
				if (p1x == 1 && p1y == 1) {
				} else if (p2x == 1 && p2y == 1) {
					if ((Mathf.Abs (p2x - idx) <= 1 && Mathf.Abs (p2y - idy) <= 1) && (Mathf.Abs (p1x - idx) > 1 || Mathf.Abs (p1y - idy) > 1)) {
						visibility = true;
						gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
						target=Color.yellow;
					}
				} else if ((p1x == 1 || p1y == 1) && (p2x != 1 && p2y != 1)) {
					if ((Mathf.Abs (p2x - idx) <= 1 && p2y == idy) || (Mathf.Abs (p2y - idy) <= 1 && p2x == idx)) {
						visibility = true;
						gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
						target=Color.yellow;
					}
				} else {
					if (((Mathf.Abs (p2x - idx) <= 1 && p2y == idy) || (Mathf.Abs (p2y - idy) <= 1 && p2x == idx)) && this.gameObject != pierwszezaznaczone && (idx != 1 && idy != 1)) {
						visibility = true;
						gameObject.GetComponent<Renderer> ().material.color = Color.yellow;
						target=Color.yellow;
					}
				}
			}
		} else if (pierwszezaznaczone!=null && drugiezaznaczone !=null && ostatniezaznaczone !=null) {
			if (this.gameObject == pierwszezaznaczone || this.gameObject == drugiezaznaczone || this.gameObject == ostatniezaznaczone) {
				visibility = true;
				//this.gameObject.GetComponent<Renderer> ().material.color = Color.white;
				target = Color.white;
			} else {
				visibility = false;
			}
		} else gameObject.GetComponent<Renderer>().material.color=target;
	}
}