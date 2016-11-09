﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// Klasa odpowiada za logikę i przetrzymywanie danych o rozgrywce.
/// </summary>
public class GameLogicDataScript : MonoBehaviour {
	
//POLA DOTYCZĄCE LOGIKI ROZGRYWKI
	/// <summary>
	/// Długość jednej rundy.
	/// </summary>
	static double interwal=6.0;
	/// <summary>
	/// Zmienna przechowująca czas jaki upłynął od początku rundy.
	/// </summary>
	double czas=0;
	/// <summary>
	/// Przechowuje informację o liczbie aktualnej tury.
	/// </summary>
	public int tura=1;
	/// <summary>
	/// Flaga, która przyjmuje fałsz po upływie czasu tury.
	/// </summary>
	bool turaTrwa=true;
	/// <summary>
	/// Flaga, która przyjmuje prawdę w sytuacji, gdy życie przynajmniej jednego z graczy będzie mniejsze lub równe 0.
	/// </summary>
	bool koniecRozgrywki=false;

//POLA DOTYCZĄCE POSZCZEGÓLNYCH GRACZY
	/// <summary>
	/// Przechowuje punkty akcji gracza.
	/// </summary>
	int punktyAkcjiGracza;
	/// <summary>
	/// Przechowuje koszt aktualnie wybranej akcji gracza.
	/// </summary>
	int kosztAkcjiGracza;
	/// <summary>
	/// Przechowuje ilość pól aktualnie zaznaczonych przez gracza.
	/// </summary>
	int iloscPolGracza;
	/// <summary>
	/// Przechowuje informację o mocy gracza. Moc wpływa na obrażenia zadane w walce.
	/// </summary>
	int mocGracza;
	/// <summary>
	/// Przechowuje punkty życia gracza.
	/// </summary>
	static double zycieGracza;
	/// <summary>
	/// Przechowuje hitbox gracza, czyli tablicę (3x3) wartości logicznych odzwierciedlających zaznaczone pola.
	/// </summary>
	bool[,] hitboxGracza = new bool[3,3];
	/// <summary>
	/// Lista przechowująca akcje wykonane przez gracza w danej turze.
	/// </summary>
	List<Czynnosc> akcjeGracza = new List<Czynnosc>();
	/// <summary>
	/// Przechowuje informację o trybie walki gracza. Prawda - atak, fałsz - obrona.
	/// </summary>
	bool modeGracza;
	/// <summary>
	/// Przechowuje punkty akcji gracza.
	/// </summary>
	int punktyAkcjiNPC;
	/// <summary>
	/// Przechowuje koszt aktualnie wybranej akcji NPC.
	/// </summary>
	int kosztAkcjiNPC;
	/// <summary>
	/// Przechowuje ilość pól aktualnie zaznaczonych przez NPC.
	/// </summary>
	int iloscPolNPC;
	/// <summary>
	/// Przechowuje informację o mocy NPC. Moc wpływa na obrażenia zadane w walce.
	/// </summary>
	int mocNPC;
	/// <summary>
	/// Przechowuje punkty życia NPC.
	/// </summary>
	static double zycieNPC;
	/// <summary>
	/// Przechowuje hitbox NPC, czyli tablicę (3x3) wartości logicznych odzwierciedlających zaznaczone pola.
	/// </summary>
	static bool[,] hitboxNPC = new bool[3,3];
	/// <summary>
	/// Lista przechowująca akcje wykonane przez NPC w danej turze.
	/// </summary>
	List<Czynnosc> akcjeNPC = new List<Czynnosc>();
	/// <summary>
	/// Przechowuje informację o trybie walki NPC. Prawda - atak, fałsz - obrona.
	/// </summary>
	bool modeNPC;

	/// <summary>
	/// Metoda uruchamiana podczas inicjalizacji klasy w momencie startu skryptu.
	/// Inicjalizuje pola graczy, czas i teksty wyświetlane na HUD'zie
	/// </summary>
	void Start () {
		punktyAkcjiGracza=10;
		kosztAkcjiGracza=99;
		iloscPolGracza=0;
		mocGracza=1;
		zycieGracza=100;

		punktyAkcjiNPC=10;
		kosztAkcjiNPC=99;
		iloscPolNPC=0;
		mocNPC=1;
		zycieNPC=100;

		GameObject.Find("RundaHUDText").GetComponent<Text>().text="Runda 1";
		aktualizujPA();
		aktualizujZycie();
		czas=Time.time;	//inicjalizacja czasu	
		aktualizujCzas();
	}

	/// <summary>
	/// Metoda wykonywana w każdej klatce.
	/// Odpowiada za całą logikę rozgrywki. 
	/// </summary>
	void Update () {
		if(turaTrwa){
			aktualizujPA();
			//przetworzenie jednej akcji gracza i dodanie jej do kolejki
			if (!MousePointFields.getCheck()){
				hitboxGracza=MousePointFields.getHitbox(); 	//pobieram hitbox z HUD'a
				modeGracza=MousePointFields.getMode();	//pobieram tryb walki
				iloscPolGracza=countMarkedFields();	//wyliczam ilość zaznaczonych w hitboxie punktów

				//wyliczanie kosztu akcji
				if(modeGracza){
					if(iloscPolGracza==1) kosztAkcjiGracza=4;		//pchniecie
					else if(iloscPolGracza==2) kosztAkcjiGracza=2;	//szybki
					else if(iloscPolGracza==3) kosztAkcjiGracza=3;	//mocny
				}
				else kosztAkcjiGracza=iloscPolGracza;	//ile bronisz tyle placisz

				//jesli gracza stac na akcje to dodaj do kolejki
				if (kosztAkcjiGracza<=punktyAkcjiGracza){
					punktyAkcjiGracza-=kosztAkcjiGracza;	//odejmuje punkty akcji
					if (modeGracza) {
						akcjeGracza.Add(new Atak(true, hitboxGracza, kosztAkcjiGracza, iloscPolGracza, mocGracza));
					}
					else {
						akcjeGracza.Add(new Obrona(true, hitboxGracza, kosztAkcjiGracza, iloscPolGracza));
					}
					//jeśli graczowi po tej akcji nie zostaną już punkty
					if (punktyAkcjiGracza<=0) {
						MousePointFields.clearHitbox();	//czyści hitbox w HUD'zie
						MousePointFields.setEnable(false);	//blokuje HUD
					}
				}

				MousePointFields.setCheck(true);	//przywracam zaznaczalnosc pol HUD'a
				MousePointFields.reset();	//resetuje HUD
			}
		}
		//jeśli czas tury się skończył przechodzimy do wykonywania czynności z kolejki
		else {
			if(!koniecRozgrywki){
				zaplanujAkcjeNPC();
				aktualizujPA();
				//wykonywanie akcji gracza
				foreach(Czynnosc c in akcjeGracza) {
					c.wykonaj();	//wykonuje akcje
				}
				//wykonywanie akcji NPC
				foreach(Czynnosc c in akcjeNPC) {
					c.wykonaj();	//wykonuje akcje
				}
				akcjeGracza.Clear(); //czyszczenie kolejki akcji gracza
				akcjeNPC.Clear();	//czyszczenie kolejki akcji NPC

				//jeśli, któryś z graczy umarł, zakończ rozgrywkę
				if ((zycieNPC<=0)||(zycieGracza<=0)) koniecRozgrywki=true;
				//jeśli jeszcze żyją przygotuj następną turę
				else{
					punktyAkcjiGracza=10;	//przywracam punkty akcji graczowi
					punktyAkcjiNPC=10;	//przywracam punkty akcji NPC
					MousePointFields.reset();	//resetuje HUD
					MousePointFields.setEnable(true);	//ponownie aktywuje HUD
					turaTrwa=true;	//wznawiam ture
					tura++;	//kolejna tura
					GameObject.Find("RundaHUDText").GetComponent<Text>().text="Runda "+tura.ToString();	//aktualizuję rundę wyświetlaną na HUD'zie
					czas=Time.time; //resetuje czas
				}
			}
			//jeśli to już koniec rozgrywki
			else{
				GameObject.Find("RundaHUDText").GetComponent<Text>().text="";	//ukrywam (czyszczę tekst) rundy wyświetlane na HUD'zie
				GameObject.Find("CzasHUDText").GetComponent<Text>().text="";	//ukrywam (czyszczę tekst) czas wyświetlany na HUD'zie
				GameObject.Find("ZycieGraczaHUDText").GetComponent<Text>().text="";	//ukrywam (czyszczę tekst) życie gracza wyświetlane na HUD'zie
				GameObject.Find("ZycieNPCHUDText").GetComponent<Text>().text="";	//ukrywam (czyszczę tekst) życie NPC wyświetlane na HUD'zie
				GameObject.Find("PAGraczaHUDText").GetComponent<Text>().text="";	//ukrywam (czyszczę tekst) punkty akcji wyświetlane na HUD'zie
				//jeśli to gracz stracił życie pokaż tekst przegranej na HUD'zie
				if((zycieGracza<=0)&&(zycieNPC>0)) GameObject.Find("WynikHUDText").GetComponent<Text>().text="Przegrana\n"+tura.ToString()+" rund";
				//jeśli to NPC stracił życie pokaż tekst wygranej na HUD'zie
				else if((zycieGracza>0)&&(zycieNPC<=0)) GameObject.Find("WynikHUDText").GetComponent<Text>().text="Zwycięstwo\n"+tura.ToString()+" rund";
				//jeśli to obaj stracili życie pokaż tekst remisu na HUD'zie
				else GameObject.Find("WynikHUDText").GetComponent<Text>().text="Remis\n"+tura.ToString()+" rund";
			}
		}
		aktualizujCzas();
	}

	/// <summary>
	/// Metoda wykonywana po każdym wykonaniu metody Update
	/// </summary>
	void LateUpdate () {
		//jeśli nie skończył się czas tury
		if(turaTrwa){
			//jeśli czas, który upłynał od początku tury jest większy od założonej długości tury
			if(Time.time-czas>interwal) {
				czas=Time.time;	//resetuję czas
				MousePointFields.clearHitbox(); //czyszczę hitbox na HUD'zie
				MousePointFields.setEnable(false);	//dezaktywuję HUD
				turaTrwa=false;	//przestawiam flagę trwania tury na fałsz
			}
		}
	}

	/// <summary>
	/// Metoda planująca kolejkę akcji NPC w danej turze.
	/// </summary>
	void zaplanujAkcjeNPC(){
		//dopóki NPC ma dostępne punkty akcji
		while (punktyAkcjiNPC>0){
			clearHitboxNPC();	//czyści hitbox NPC
			kosztAkcjiNPC=0; iloscPolNPC=0; //zeruje koszt aktualnej akcji NPC i ilość aktualnie zaznaczonych przez NPC pól
			int startx=0;	//pierwszy indeks w tablicy dla punktu startowego (od, którego zacznie się zaznaczanie pól)
			int starty=0;	//drugi indeks w tablicy dla punktu startowego
			int vert=0;	//pierwszy indeks drugiego pola do zaznaczenia w tablicy
			int hor=0;	//drugi indeks drugiego pola do zaznaczenia w tablicy
			int vert2=0;	//pierwszy indeks trzeciego pola do zaznaczenia w tablicy
			int hor2=0;	//drugi indeks trzeciego pola do zaznaczenia w tablicy
			//losowanie indeksów punktu startowego, wylosowana zostanie liczba całkowita w przedziale 0-2
			startx=System.Convert.ToInt32(UnityEngine.Random.value*2);
			starty=System.Convert.ToInt32(UnityEngine.Random.value*2);

			//jeśli NPC posiada przynajmniej 2 punkty akcji, to zostanie wybrany atak z prawdopodobieństwem 80%
			if((UnityEngine.Random.value>0.2)&&(punktyAkcjiNPC>1)) modeNPC=true;
			//jeśli NPC posiada tylko jeden punkt akcji lub mniej albo z prawdopodobieństwa nie wybrano ataku to zostanie wybrana obrona
			else modeNPC=false;

			//jeśli została wybrana obrona i NPC ma przynajmniej 1 pkt akcji
			if ((!modeNPC) && (punktyAkcjiNPC>=1)) {
				hitboxNPC[startx,starty]=true;	//zaznacza punkt startowy w hitboxie
				kosztAkcjiNPC=1; iloscPolNPC++;	//dodaję 1 do kosztu akcji i do ilości zaznaczonych pól
				//jeśli NPC ma więcej niż 1 punkt akcji
				if (punktyAkcjiNPC>1){
					do {
						//losuję wartości całkowite od -1 do 1
						vert=System.Convert.ToInt32(UnityEngine.Random.value*2)-1;
						hor=System.Convert.ToInt32(UnityEngine.Random.value*2)-1;
						//jeśli po dodaniu tych wartości do odpowiednich indeksów, wyjdzie indeks z poza tablicy, to wyzeruj wartość
						if (((startx==0)&&(hor==-1))||((startx==2)&&(hor==1))) hor=0;
						if (((starty==0)&&(vert==-1))||((starty==2)&&(vert==1))) vert=0;
						//przypisz indeksy kolejnych pól poprzez dodanie wartości do indeksu poprzedniego pola
						hor=startx+hor; 
						vert=starty+vert;
					} while((startx-hor!=0)&&(starty-vert!=0)); //jeśli nowe indeksy pokryły się z poprzednim polem, to wylosuj od nowa
					hitboxNPC[hor,vert]=true;	//zaznacza drugie pole w hitboxie
					kosztAkcjiNPC=2; iloscPolNPC++;	//dodaję 1 do kosztu akcji i do ilości zaznaczonych pól
					//jeśli NPC ma więcej niż 2 punkt akcji
					if (punktyAkcjiNPC>2){
						do {
							//losuję wartości całkowite od -1 do 1
							vert2=System.Convert.ToInt32(UnityEngine.Random.value*2)-1;
							hor2=System.Convert.ToInt32(UnityEngine.Random.value*2)-1;
							//jeśli po dodaniu tych wartości do odpowiednich indeksów, wyjdzie indeks z poza tablicy, to wyzeruj wartość
							if (((hor==0)&&(hor2==-1))||((hor==2)&&(hor2==1))) hor2=0;
							if (((vert==0)&&(vert2==-1))||((vert==2)&&(vert2==1))) vert2=0;
							//przypisz indeksy kolejnych pól poprzez dodanie wartości do indeksu poprzedniego pola
							hor2=hor+hor2;
							vert2=vert+vert2;
						} while((hor-hor2!=0)&&(vert-vert2!=0)&&(startx-hor2!=0)&&(starty-vert2!=0));	//jeśli nowe indeksy pokryły się z poprzednimi polami, to wylosuj od nowa
						hitboxNPC[hor2,vert2]=true;	//zaznacza trzecie pole w hitboxie
						kosztAkcjiNPC=3; iloscPolNPC++;	//dodaję 1 do kosztu akcji i do ilości zaznaczonych pól
					}
				}
			}
			//jeśli został wybrany atak i NPC ma przynajmniej 1 pkt akcji
			if ((modeNPC)&&(punktyAkcjiNPC>=1)){
				hitboxNPC[startx,starty]=true;	//zaznacza punkt startowy w hitboxie
				iloscPolNPC=1; //ustawiam ilość zaznaczonych pól na 1
				int rodzajAtaku=0;	//inicjalizuję zmienną rodzaj ataku
				rodzajAtaku=System.Convert.ToInt32(UnityEngine.Random.value*2);	//losuję wartość z przedziału 0-2 i przypisuję do rodzajAtaku
				//jeśli rodzaj ataku = 0, to zakładam, że wybrano pchnięcie
				if(rodzajAtaku<=0){
					if(punktyAkcjiNPC>=4) kosztAkcjiNPC=4;	//ustawiam koszt akcji pchnięcia jeśli NPC ma potrzebną ilość punktów akcji
					else rodzajAtaku=1;	//jeśli nie ma potrzebnych punktów akcji, to zmieniam rodzaj na ciężki atak
				}
				//jesli rodzaj ataku = 1, to zakładam, że wybrano ciężki atak
				if(rodzajAtaku==1){
					//jeśli NPC ma potrzebne punkty akcj
					if(punktyAkcjiNPC>=3){
						do {
							//losuję wartości całkowite od -1 do 1
							vert=System.Convert.ToInt32(UnityEngine.Random.value*2)-1;
							hor=System.Convert.ToInt32(UnityEngine.Random.value*2)-1;
							//jeśli po dodaniu tych wartości do odpowiednich indeksów, wyjdzie indeks z poza tablicy, to wyzeruj wartość
							if (((startx==0)&&(hor==-1))||((startx==2)&&(hor==1))) hor=0;
							if (((starty==0)&&(vert==-1))||((starty==2)&&(vert==1))) vert=0;
							//przypisz indeksy kolejnych pól poprzez dodanie wartości do indeksu poprzedniego pola
							hor=startx+hor; 
							vert=starty+vert;
						} while((startx-hor!=0)&&(starty-vert!=0)); //jeśli nowe indeksy pokryły się z poprzednim polem, to wylosuj od nowa
						hitboxNPC[hor,vert]=true; //zaznacza drugie pole w hitboxie
						do {
							//losuję wartości całkowite od -1 do 1
							vert2=System.Convert.ToInt32(UnityEngine.Random.value*2)-1;
							hor2=System.Convert.ToInt32(UnityEngine.Random.value*2)-1;
							//jeśli po dodaniu tych wartości do odpowiednich indeksów, wyjdzie indeks z poza tablicy, to wyzeruj wartość
							if (((hor==0)&&(hor2==-1))||((hor==2)&&(hor2==1))) hor2=0;
							if (((vert==0)&&(vert2==-1))||((vert==2)&&(vert2==1))) vert2=0;
							//przypisz indeksy kolejnych pól poprzez dodanie wartości do indeksu poprzedniego pola
							hor2=hor+hor2;
							vert2=vert+vert2;
						} while((hor-hor2!=0)&&(vert-vert2!=0)&&(startx-hor2!=0)&&(starty-vert2!=0));	//jeśli nowe indeksy pokryły się z poprzednimi polami, to wylosuj od nowa
						hitboxNPC[hor2,vert2]=true;	//zaznacza trzecie pole w hitboxie
						kosztAkcjiNPC=3; iloscPolNPC=3;	//ustawiam kosztu akcji i ilości zaznaczonych pól
					}
					else rodzajAtaku=2;
				}
				//jesli rodzaj ataku = 2, to zakładam, że wybrano szybki atak
				if(rodzajAtaku==2){
					if(punktyAkcjiNPC>=2){
						do {
							//losuję wartości całkowite od -1 do 1
							vert=System.Convert.ToInt32(UnityEngine.Random.value*2)-1;
							hor=System.Convert.ToInt32(UnityEngine.Random.value*2)-1;
							//jeśli po dodaniu tych wartości do odpowiednich indeksów, wyjdzie indeks z poza tablicy, to wyzeruj wartość
							if (((startx==0)&&(hor==-1))||((startx==2)&&(hor==1))) hor=0;
							if (((starty==0)&&(vert==-1))||((starty==2)&&(vert==1))) vert=0;
							//przypisz indeksy kolejnych pól poprzez dodanie wartości do indeksu poprzedniego pola
							hor=startx+hor; 
							vert=starty+vert;
						} while((startx-hor!=0)&&(starty-vert!=0));	//jeśli nowe indeksy pokryły się z poprzednim polem, to wylosuj od nowa
						hitboxNPC[hor,vert]=true; //zaznacza drugie pole w hitboxie
						kosztAkcjiNPC=2; iloscPolNPC=2;	//ustawiam kosztu akcji i ilości zaznaczonych pól
					}
				}
			}
			//jeśli to był atak, to dodaj nowy atak do kolejki
			if (modeNPC) {
				akcjeNPC.Add(new Atak(false,hitboxNPC,kosztAkcjiNPC,iloscPolNPC,mocNPC));
			}
			//jeśli to była obrona, to dodaj nową obbronę do kolejki
			else {
				akcjeNPC.Add(new Obrona(false,hitboxNPC,kosztAkcjiNPC,iloscPolNPC));
			}
			punktyAkcjiNPC-=kosztAkcjiNPC;	//zabiera punkty wykonanej akcji z puli punktów akcji NPC
		}
	}


// GETTERY SETTERY I TYM PODOBNE

	/// <summary>
	/// Zmniejsza życie gracza o i.
	/// </summary>
	/// <param name="i">ilość punktów życia do zmniejszenia</param>
	public static void decreaseZycieGracza(double i){
		zycieGracza-=i;
	}
	/// <summary>
	/// Zwiększa życie gracza o i.
	/// </summary>
	/// <param name="i">ilość punktów życia do zwiększenia</param>
	public static void increaseZycieGracza(double i){
		zycieGracza+=i;
	}
	/// <summary>
	/// Zwraca ilość punktów życia gracza.
	/// </summary>
	/// <returns>punkty życia gracza</returns>
	public static double getZycieGracza(){
		return zycieGracza;
	}
	/// <summary>
	/// Zmniejsza życie NPC o i.
	/// </summary>
	/// <param name="i">ilość punktów życia do zmniejszenia</param>
	public static void decreaseZycieNPC(double i){
		zycieNPC-=i;
	}
	/// <summary>
	/// Zwiększa życie NPC o i.
	/// </summary>
	/// <param name="i">ilość punktów życia do zwiększenia</param>
	public static void increaseZycieNPC(double i){
		zycieNPC+=i;
	}
	/// <summary>
	/// Zwraca ilość punktów życia NPC.
	/// </summary>
	/// <returns>punkty życia NPC</returns>
	public static double getZycieNPC(){
		return zycieNPC;
	}
	/// <summary>
	/// Ustawia wszystkie flagi w hitboxie NPC na fałsz.
	/// </summary>
	public static void clearHitboxNPC(){
		for(int i=0;i<3;i++){
			for(int j=0;j<3;j++){
				hitboxNPC[i,j]=false;
			}
		}
	}


// METODY SLUZACE DO AKTUALIZACJI INFORMACJI NA HUD'ZIE

	/// <summary>
	/// Aktualizuje czas wyświetlany na HUD'zie. Metoda przelicza czas używany w kodzie na czas jaki pozostał w danej turze.
	/// </summary>
	public void aktualizujCzas(){
		int pozostalo;
		pozostalo=Convert.ToInt32(interwal-(Time.time-czas));
		if(!koniecRozgrywki) GameObject.Find("CzasHUDText").GetComponent<Text>().text="Czas: "+pozostalo.ToString();
		else GameObject.Find("CzasHUDText").GetComponent<Text>().text="";
	}
	/// <summary>
	/// Aktualizuje ilość punktów akcji wyświetlanych na HUD'zie
	/// </summary>
	public void aktualizujPA(){
		GameObject.Find("PAGraczaHUDText").GetComponent<Text>().text="PA Gracza: "+punktyAkcjiGracza.ToString();
	}
	/// <summary>
	/// Aktualizuje zycie gracza i przeciwnika wyświetlane na HUD'zie
	/// </summary>
	public static void aktualizujZycie(){
		GameObject.Find("ZycieGraczaHUDText").GetComponent<Text>().text="HP Gracza: "+zycieGracza.ToString();
		GameObject.Find("ZycieNPCHUDText").GetComponent<Text>().text="HP Przeciwnika: "+zycieNPC.ToString();
	}


// METODY SLUZACE DO SPRAWDZANIA

	/// <summary>
	/// Metoda zwraca ilość zaznaczonych pól w hitboxie gracza
	/// </summary>
	/// <returns>ilość zaznaczonych pól w hitboxie</returns>
	public int countMarkedFields(){
		int marked=0;
		for(int i=0;i<3;i++){
			for(int j=0;j<3;j++){
				if(hitboxGracza[i,j]==true){
					marked++;
				}
			}
		}
		return marked;
	}
//DEBUG
	/// <summary>
	/// DEBUG
	/// Wyświetla hitbox gracza na konsolę
	/// </summary>
	public void printHitbox(){
		string str="HITBOX:\n";
		for(int i=0;i<3;i++){
			for(int j=0;j<3;j++){
				if(hitboxGracza[i,j]==true) str+="1";
				else str+="0";
				str+=" ";
			}
			str+="\n";
		}
		print(str);
	}
	/// <summary>
	/// DEBUG
	/// Wyświetla hitbox NPC na konsolę
	/// </summary>
	public void printHitboxNPC(){
		string str="HITBOX NPC:\n";
		for(int i=0;i<3;i++){
			for(int j=0;j<3;j++){
				if(hitboxNPC[i,j]==true) str+="1";
				else str+="0";
				str+=" ";
			}
			str+="\n";
		}
		print(str);
	}
}	

// KLASY ODPOWIEDZIALNE ZA REPREZENTACJE AKCJI

/// <summary>
/// Klasa abstrakcyjna reprezentująca akcję wykonaną przez jednego z graczy.
/// </summary>
public abstract class Czynnosc {
	/// <summary>
	/// Flaga, która przyjmuje prawdę, gdy akcja jest wykonana przez gracza.
	/// </summary>
	protected bool gracz;
	/// <summary>
	/// Kopia hitbox'a danej akcji.
	/// </summary>
	protected bool [,] hitbox;
	/// <summary>
	/// Koszt akcji w punktach akcji.
	/// </summary>
	protected int koszt;
	/// <summary>
	/// Ilość zaznaczonych pól.
	/// </summary>
	protected int iloscPol;

	/// <summary>
	/// Inicjalizuje instancję klasy <see cref="Czynnosc"/>.
	/// </summary>
	/// <param name="gracz">Jeśli ustawiona na <c>true</c> akcja jest wykonywana przez gracza.</param>
	/// <param name="hitbox">Hitbox dla tej akcji.</param>
	/// <param name="koszt">Koszt tej akcji.</param>
	/// <param name="iloscPol">Ilosc zaznaczonych pól w tej akcji.</param>
	public Czynnosc(bool gracz, bool [,] hitbox, int koszt, int iloscPol){
		this.gracz=gracz;
		this.hitbox=hitbox;
		this.koszt=koszt;
		this.iloscPol=iloscPol;
	}
	/// <summary>
	/// Metoda abstrakcyjna odpowiadająca za wykonanie akcji.
	/// </summary>
	public abstract void wykonaj();
	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="Czynnosc"/>.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="Czynnosc"/>.</returns>
	public override string ToString(){
		return "czynnosc";
	}
}

/// <summary>
/// Klasa reprezentująca Atak.
/// </summary>
public class Atak : Czynnosc {
	/// <summary>
	/// Moc ataku.
	/// </summary>
	int moc=1;
	/// <summary>
	/// Zadane obrażenia.
	/// </summary>
	int obrazenia;
	/// <summary>
	/// Inicjalizuje instancję klasy <see cref="Atak"/>.
	/// </summary>
	/// <param name="gracz">Jeśli ustawiona na <c>true</c> akcja jest wykonywana przez gracza.</param>
	/// <param name="hitbox">Hitbox dla tej akcji.</param>
	/// <param name="koszt">Koszt tej akcji.</param>
	/// <param name="iloscPol">Ilosc zaznaczonych pól w tej akcji.</param>
	/// <param name="moc">Moc ataku.</param>
	public Atak(bool gracz, bool [,] hitbox, int koszt, int iloscPol, int moc) : base(gracz, hitbox, koszt, iloscPol) {
		this.moc=moc;
		//w zależności od ilości pól stwierdzamy jaki to atak i przypisujemy mu obrażenia
		switch(iloscPol){
			case 1: this.obrazenia=4; break;
			case 2: this.obrazenia=2; break;
			case 3: this.obrazenia=3; break;
		}
	}
	/// <summary>
	/// Metoda odpowiadająca za wykonanie ataku.
	/// </summary>
	public override void wykonaj(){
		if (gracz) GameLogicDataScript.decreaseZycieNPC(obrazenia*moc);	//jeśli to gracz wykonuje akcję, to zmniejsz życie NPC
		else GameLogicDataScript.decreaseZycieGracza(obrazenia*moc);	//jeśli to NPC wykonuje akcję, to zmniejsz życie gracza
		GameLogicDataScript.aktualizujZycie();	//aktualizuj wyświetlane życie
	}
	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="Atak"/>.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="Atak"/>.</returns>
	public override string ToString(){
		return "Jestem atakiem "+obrazenia;
	}
}

/// <summary>
/// Klasa reprezentująca obronę.
/// </summary>
public class Obrona : Czynnosc {
	/// <summary>
	/// Initializes a new instance of the <see cref="Obrona"/> class.
	/// </summary>
	/// <param name="gracz">Jeśli ustawiona na <c>true</c> akcja jest wykonywana przez gracza.</param>
	/// <param name="hitbox">Hitbox dla tej akcji.</param>
	/// <param name="koszt">Koszt tej akcji.</param>
	/// <param name="iloscPol">Ilosc zaznaczonych pól w tej akcji.</param>
	public Obrona(bool gracz, bool [,] hitbox, int koszt, int iloscPol) : base(gracz, hitbox, koszt, iloscPol) {
	}
	/// <summary>
	/// Metoda odpowiadająca za wykonanie obrony.
	/// </summary>
	public override void wykonaj(){
		//jeśli to gracz się broni
		if (gracz) {
			//BRONIĘ SIĘ!
		}
		//jeśli to NPC się broni
		else {
			//BRONI SIĘ!
		}
	}
	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="Obrona"/>.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="Obrona"/>.</returns>
	public override string ToString(){
		return "Jestem obrona "+iloscPol;
	}
}