﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

/// <summary>
/// Klasa odpowiedzialna za przetwarzanie interakcji gracza z HUD'em.
/// </summary>
public class MousePointFields : MonoBehaviour {
	
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
	public GameObject HUDCombatField;
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
			gameObject.GetComponent<Renderer>().material.color=target;
		}
		//jeśli HUD jest nieaktywny
		else{
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
			if (Input.GetMouseButton(0)){
				target = Color.white;	//ustaw kolor pola na biały
				if (countMarkedFields()<3) hitbox[idx,idy] = true;	//przestaw flagę pola na zaznaczone (prawda)
			}
			//jeśli jest wciśnięty PPM
			else if (Input.GetMouseButton(1)){
				target = Color.white;	//ustaw kolor pola na biały
				if (countMarkedFields()<3) hitbox[idx,idy] = true;	//przestaw flagę pola na zaznaczone (prawda)
			}
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
}