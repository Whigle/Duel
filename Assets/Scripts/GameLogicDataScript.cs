using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Text;

/// <summary>
/// Klasa odpowiada za logikę i przetrzymywanie danych o rozgrywce.
/// </summary>
public class GameLogicDataScript : MonoBehaviour {

	string Textt="Wroc do gry";

	static public String nazwaGracza="Gracz";
	static public String nazwaPrzeciwnika="NPC";
	
	static public bool multiplayer=false;
	static public bool polaczono=false;
	bool pobralemAkcjePrzeciwnika=false;
	bool skonczonopotej=false;
	bool skonczonopoprzeciwnej=false;
	bool skonczylemprzesyl=false;


	GameObject tlg;
	GameObject tld;
	GameObject tpd;
	GameObject tpg;

	//POLA STATYSTYK
	/// <summary>
	/// Przechowuje informację o liczbie aktualnej tury.
	/// </summary>
	public int tura=1;
	static public int zablokowaneAtaki=0;
	static public int wykonaneAtaki=0;
	static public int wykonaneObrony=0;
	static public double pozostaleZycie=0;
	static public int wynik=0;
	//static string statystyki;

	static List<string> wyniki=new List<string>();
	static List<string> tury=new List<string>();
	static List<string> zablAtaki=new List<string>();
	static List<string> wykAtaki=new List<string>();
	static List<string> wykObrony=new List<string>();
	static List<string> pozZycie=new List<string>();
	static List<string> nicki=new List<string>();
	static List<string> nickip=new List<string>();

	static string plikStatystyk="stats.sts";


	//POLA DOTYCZĄCE LOGIKI ROZGRYWKI
	/// <summary>
	/// Długość jednej rundy.
	/// </summary>
	static double interwal=12.0;//6.0;
	/// <summary>
	/// Zmienna przechowująca czas jaki upłynął od początku rundy.
	/// </summary>
	double czas=0;
	/*/// <summary>
	/// Długość fazy wizualizacji walki.
	/// </summary>
	static double interwalWalka=4.0;*/
	/// <summary>
	/// Długość zmiany miedzy fazą turową a wizualizacji walki.
	/// </summary>
	static double interwalPrzejscie=1.0;
///////////////////////
	/// <summary>
	/// Zmienna przechowująca czas jaki upłynął od początku akcji.
	/// </summary>
	double czasAkcjiGracza=0;
	/// <summary>
	/// Zmienna przechowująca czas jaki upłynął od początku akcji.
	/// </summary>
	double czasAkcjiNPC=0;
	/// <summary>
	/// Długość akcji NPC.
	/// </summary>
	public static double interwalAkcjaNPC=0.0;
	/// <summary>
	/// Długość akcji Gracza.
	/// </summary>
	public static double interwalAkcjaGracza=0.0;



	/// <summary>
	/// Flaga, która przyjmuje fałsz po upływie czasu tury.
	/// </summary>
	bool turaTrwa=true;
	/// <summary>
	/// Flaga, która przyjmuje prawdę w sytuacji, gdy życie przynajmniej jednego z graczy będzie mniejsze lub równe 0.
	/// </summary>
	bool koniecRozgrywki=false;
	/// <summary>
	/// Flaga przyjmująca prawdę, gdy gracz wciśnie przycisk dalej.
	/// </summary>
	static public bool nastepnaTura=false;
	bool wykonane=false;
	GameObject player;
	GameObject opponent;

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
	/// Lista przechowująca historię akcji wykonanych przez gracza.
	/// </summary>
	List<Czynnosc> historiaAkcjiGracza = new List<Czynnosc>();
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
	/// Przechowuje informację o rodzaju ataku (do wyświetlania)
	/// </summary>
	string rodzajAtaku;
	/// <summary>
	/// Przechowuje informację o stratach gracza
	/// </summary>
	public static double stratyGracza;
	/// <summary>
	/// Przechowuje informację o stratach NPC
	/// </summary>
	public static double stratyNPC;
	
	bool akcjaGraczaSkonczona=false;
	bool akcjaNPCSkonczona=false;
	bool animacjaGraczaSkonczona=false;
	bool animacjaNPCSkonczona=false;
	bool jest=true;
	bool przejscieSkonczone=false;

	static string [] poziomyTrudnosci = { "Easy", "Normal", "Hard" };
	static string [] strategie = { "Agressive", "Balanced", "Deffensive" };
	static string poziomTrudnosci = poziomyTrudnosci[1];
	static string strategia = strategie[0];
	int [,] prawdopodobienstwo = new int[3,3];

	static public double mnoznikCzasu=0.5; //mnożnik czasu akcji czas akcji=PA*mnoznik

	static int pozostalePAGracza=10;
	static int pozostalePANPC=10;
	static int i=0, j=0, ii=0, jj=0, iAnim=0, jAnim=0;
	static int aktKosztPAGracza;
	static int aktKosztPANPC;
	static int pokryte;

	List<Czynnosc> runda1P = new List<Czynnosc>();
	List<Czynnosc> runda2P = new List<Czynnosc>();
	List<Czynnosc> runda3P = new List<Czynnosc>();
	List<Czynnosc> runda4P = new List<Czynnosc>();

	List<Czynnosc> runda1N = new List<Czynnosc>();
	List<Czynnosc> runda2N = new List<Czynnosc>();
	List<Czynnosc> runda3N = new List<Czynnosc>();
	List<Czynnosc> runda4N = new List<Czynnosc>();

	/// <summary>
	/// Metoda uruchamiana podczas inicjalizacji klasy w momencie startu skryptu.
	/// Inicjalizuje pola graczy, czas i teksty wyświetlane na HUD'zie
	/// </summary>
	void Start () {
		tlg=GameObject.Find("tabliczkalewagorna");
		tld=GameObject.Find("tabliczkalewadolna");
		tpd=GameObject.Find("tabliczkaprawadolna");
		tpg=GameObject.Find("tabliczkaprawagorna");
		player=GameObject.Find("Player");
		opponent=GameObject.Find("Opponent");
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

		GameObject.Find ("CanvasLista").GetComponent<Canvas> ().enabled = false;
		GameObject.Find ("CanvasNewGame").GetComponent<Canvas> ().enabled = false;
		GameObject.Find("RodzajAtakuHUDText").GetComponent<Text>().text=rodzajAtaku;
		GameObject.Find("RodzajAtakuHUDText").GetComponent<Text> ().fontSize = 15;
		GameObject.Find("RundaHUDText").GetComponent<Text>().text="Runda 1";
		aktualizujPA();
		aktualizujZycie();
		czas=Time.time;	//inicjalizacja czasu	
		aktualizujCzas();

		ComboGracza = 1;
		ComboNPC = 1;

		czasowy = Time.time;
	}

	int ComboGracza;
	public int ComboNPC;

	GameObject Pierwsze;
	GameObject Drugie;
	GameObject Trzecie;


	bool wlaczono=false;

	/// <summary>
	/// Metoda wykonywana w każdej klatce.
	/// Odpowiada za całą logikę rozgrywki. 
	/// </summary>
	void Update () {
		if (!polaczono && multiplayer){
			MousePointFields.setEnable(false);
			czas=Time.time;
		}
		else{
			if(turaTrwa){
				if(!wlaczono && multiplayer) {
					MousePointFields.setEnable(true); 
					wlaczono=true;
					if (multiplayer&&polaczono) {
						NetworkView nv=gameObject.GetComponent<NetworkView>();
						nv.stateSynchronization=NetworkStateSynchronization.ReliableDeltaCompressed;
						nv.RPC("nadajMojaNazwe",RPCMode.Others,nazwaGracza);
					}
				}
				GameObject.Find ("CanvasButton").GetComponent<Canvas> ().enabled=true;
				GameObject.Find ("RodzajAtakuHUDText").GetComponent<Text> ().text = rodzajAtaku; //wyświetl informacje o rodzaju ataku			

				//przetworzenie jednej akcji gracza i dodanie jej do kolejki
				if (!MousePointFields.getCheck()){
					//hitboxGracza=MousePointFields.getHitbox(); 	//pobieram hitbox z HUD'a
					bool [,] temp = new bool[3,3];
					for (int i = 0; i < 3; i++) {
						for (int j = 0; j < 3; j++) {
							hitboxGracza [i, j] = MousePointFields.getHitbox () [i, j];
							temp[i,j] = MousePointFields.getHitbox () [i, j];
						}
					}
					modeGracza=MousePointFields.getMode();	//pobieram tryb walki
					iloscPolGracza=countMarkedFields();	//wyliczam ilość zaznaczonych w hitboxie punktów
					GameObject pierwsze=MousePointFields.pierwszezaznaczone;
					if (Trzecie != null) {
						if (pierwsze == Trzecie) {
							ComboGracza++;
						} else
							ComboGracza = 1;
					} else if (Drugie != null) {
						if (pierwsze == Drugie) {
							ComboGracza++;
						}	else
							ComboGracza = 1;
					} else {
						if (pierwsze == Pierwsze) {
							ComboGracza++;
						}	else
							ComboGracza = 1;
					}
					GameObject ostatnie=MousePointFields.ostatniezaznaczone;

					Pierwsze = pierwsze;
					Drugie=MousePointFields.drugiezaznaczone;
					Trzecie = ostatnie;
					//wyliczanie kosztu akcji
					if(modeGracza){
						if(iloscPolGracza==1) { kosztAkcjiGracza=4; }
						else if(iloscPolGracza==2) { kosztAkcjiGracza=2; }
						else if(iloscPolGracza==3) { kosztAkcjiGracza=3; }
					}
					else kosztAkcjiGracza=iloscPolGracza;	//ile bronisz tyle placisz

					//jesli gracza stac na akcje to dodaj do kolejki
					if (kosztAkcjiGracza<=punktyAkcjiGracza){
						punktyAkcjiGracza-=kosztAkcjiGracza;	//odejmuje punkty akcji
						if (modeGracza) {
							akcjeGracza.Add(new Atak(true, temp, kosztAkcjiGracza, iloscPolGracza, mocGracza*ComboGracza));
							//historiaAkcjiGracza.Add(new Atak(true, temp, kosztAkcjiGracza, iloscPolGracza, mocGracza));
							if (kosztAkcjiGracza == 4) {
								rodzajAtaku += "\nPchnięcie!";
							} else if (kosztAkcjiGracza == 2) {
								rodzajAtaku += "\nSzybki atak!";
							} else {
								rodzajAtaku += "\nMocny atak!";
							}
							GameObject.Find ("RodzajAtakuHUDText").GetComponent<Text> ().text = rodzajAtaku; //wyświetl informacje o rodzaju ataku			
							if (pierwsze!=null){
								((Atak)akcjeGracza[akcjeGracza.Count-1]).setFirst(pierwsze.GetComponent<MousePointFields>().getIndX(),pierwsze.GetComponent<MousePointFields>().getIndY());
								//((Atak)historiaAkcjiGracza[historiaAkcjiGracza.Count-1]).setFirst(pierwsze.GetComponent<MousePointFields>().getIndX(),pierwsze.GetComponent<MousePointFields>().getIndY());
							}
							if (ostatnie!=null){
								((Atak)akcjeGracza[akcjeGracza.Count-1]).setLast(ostatnie.GetComponent<MousePointFields>().getIndX(),ostatnie.GetComponent<MousePointFields>().getIndY());
								//((Atak)historiaAkcjiGracza[historiaAkcjiGracza.Count-1]).setLast(ostatnie.GetComponent<MousePointFields>().getIndX(),ostatnie.GetComponent<MousePointFields>().getIndY());
							}
						}
						else {
							rodzajAtaku += "\nObrona!";
							GameObject.Find ("RodzajAtakuHUDText").GetComponent<Text> ().text = rodzajAtaku; //wyświetl informacje o rodzaju ataku			
							akcjeGracza.Add(new Obrona(true, temp, kosztAkcjiGracza, iloscPolGracza));
							//historiaAkcjiGracza.Add(new Obrona(true, temp, kosztAkcjiGracza, iloscPolGracza));
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
				if(!przejscieSkonczone) {
					player.transform.position=new Vector3(0.0f,0.0f,-0.7f);
					opponent.transform.position=new Vector3(0.0f,0.0f,0.7f);
					GameObject.Find("Main Camera").GetComponent<Animation>().Play("Kamera");
				}
				else{
					if (!koniecRozgrywki) {
						if (!wykonane) {
							//print("AkcjiGracza: "+(akcjeGracza.Count-i).ToString()+" AkcjeNPC: "+(akcjeNPC.Count-j).ToString());
							if (((pozostalePAGracza >= 0) || (pozostalePANPC >= 0)) && (koniecRozgrywki == false)) {
								//print("------------------------------------------------------------i="+i+" j="+j);
								if ((akcjeGracza.Count <= 0) || (i >= akcjeGracza.Count))
									pozostalePAGracza = 0;
								if ((akcjeNPC.Count <= 0) || (j >= akcjeNPC.Count))
									pozostalePANPC = 0;
								aktKosztPAGracza = 0;
								aktKosztPANPC = 0;
								pokryte = 0;
								ii = 0;
								jj = 0;

								if (!animacjaGraczaSkonczona) {
									akcjeGracza [iAnim].animuj ();
									GameObject.Find ("strataNPCText").GetComponent<Text> ().text = "-" + GameLogicDataScript.stratyNPC.ToString () + " p";
								}

								if (!animacjaNPCSkonczona) {
									akcjeNPC [jAnim].animuj ();
									GameObject.Find ("strataGraczaText").GetComponent<Text> ().text = "-" + GameLogicDataScript.stratyGracza.ToString () + " p";
								}
								if ((pozostalePAGracza > 0) && (pozostalePANPC > 0)) {
									aktKosztPAGracza = akcjeGracza [i].getKoszt ();
									aktKosztPANPC = akcjeNPC [j].getKoszt ();



									pokryte = akcjeGracza [i].porownajHitbox (akcjeNPC [j]);
									//jeśli npc broni, a gracz atakuje
									if ((akcjeNPC [j].getTypAkcji () == false) && (akcjeGracza [i].getTypAkcji ())) {
										if ((akcjaNPCSkonczona) && (akcjaGraczaSkonczona)) {
											jAnim = j;
											iAnim = i;
											/*print("------------------------------------------------------------AKCJA! n:D g:A");
											print(akcjeGracza[i]);
											print(akcjeGracza[iAnim].getAnimacja());
											print(akcjeNPC[j]);
											print(akcjeNPC[jAnim].getAnimacja());*/
											akcjaNPCSkonczona = false;
											akcjaGraczaSkonczona = false;
											animacjaNPCSkonczona = false;
											animacjaGraczaSkonczona = false;
											czasAkcjiGracza = Time.time;
											czasAkcjiNPC = Time.time;
											interwalAkcjaGracza = System.Convert.ToDouble (aktKosztPAGracza) * mnoznikCzasu;
											interwalAkcjaNPC = System.Convert.ToDouble (aktKosztPANPC) * mnoznikCzasu;				

											pozostalePAGracza -= aktKosztPAGracza;
											akcjeGracza [i].setIloscPol (akcjeGracza [i].getIloscPol () - pokryte);
											((Atak)akcjeGracza [i]).aktualizujObrazenia ();

											decreaseZycieNPC (((Atak)akcjeGracza [i]).getObrazenia () * ((Atak)akcjeGracza [i]).getMoc ());
											//stratyNPC = ((Atak)akcjeGracza [i]).getObrazenia () * ((Atak)akcjeGracza [i]).getMoc ();

											ii = 1; 
											if (Math.Abs (pozostalePAGracza - pozostalePANPC) >= aktKosztPANPC) {
												pozostalePANPC -= aktKosztPANPC;
												jj = 1;
											}
										}

									}
									//gracz broni NPC atakuje
									else if ((akcjeNPC [j].getTypAkcji ()) && (akcjeGracza [i].getTypAkcji () == false)) {
										if ((akcjaGraczaSkonczona) && (akcjaNPCSkonczona)) {
											jAnim = j;
											iAnim = i;
											/*print("------------------------------------------------------------AKCJA! n:A g:D");
											print(akcjeGracza[i]);
											print(akcjeGracza[iAnim].getAnimacja());
											print(akcjeNPC[j]);
											print(akcjeNPC[jAnim].getAnimacja());*/
											akcjaNPCSkonczona = false;
											akcjaGraczaSkonczona = false;
											animacjaNPCSkonczona = false;
											animacjaGraczaSkonczona = false;
											czasAkcjiGracza = Time.time;
											czasAkcjiNPC = Time.time;
											interwalAkcjaGracza = System.Convert.ToDouble (aktKosztPAGracza) * mnoznikCzasu;
											interwalAkcjaNPC = System.Convert.ToDouble (aktKosztPANPC) * mnoznikCzasu;

											if (pokryte>0) zablokowaneAtaki+=1;
											pozostalePANPC -= aktKosztPANPC;
											akcjeNPC [j].setIloscPol (akcjeNPC [j].getIloscPol () - pokryte);
											((Atak)akcjeNPC [j]).aktualizujObrazenia ();

											decreaseZycieGracza (((Atak)akcjeNPC [j]).getObrazenia () * ((Atak)akcjeNPC [j]).getMoc ());
											jj = 1;	
											if (Math.Abs (pozostalePAGracza - pozostalePANPC) >= aktKosztPAGracza) {
												pozostalePAGracza -= aktKosztPAGracza;
												ii = 1;
											}						
										}
									}
									//obaj atakują
									else if ((akcjeNPC [j].getTypAkcji ()) && (akcjeGracza [i].getTypAkcji ())) {
										if ((pozostalePAGracza - aktKosztPAGracza > pozostalePANPC - aktKosztPANPC) && (pokryte == 0)) {
											if (akcjaGraczaSkonczona) {
												iAnim = i;
												/*print("------------------------------------------------------------AKCJA! ng:A g szybszy");
												print(akcjeGracza[i]);
												print(akcjeGracza[iAnim].getAnimacja());*/
												akcjaGraczaSkonczona = false;
												animacjaGraczaSkonczona = false;
												czasAkcjiGracza = Time.time;
												interwalAkcjaGracza = System.Convert.ToDouble (aktKosztPAGracza) * mnoznikCzasu;

												pozostalePAGracza -= aktKosztPAGracza;

												decreaseZycieNPC (((Atak)akcjeGracza [i]).getObrazenia () * ((Atak)akcjeGracza [i]).getMoc ());
												ii = 1;
											}
										} else if ((pozostalePAGracza - aktKosztPAGracza < pozostalePANPC - aktKosztPANPC) && (pokryte == 0)) {
											if (akcjaNPCSkonczona) {
												jAnim = j;
												/*print("------------------------------------------------------------AKCJA! ng:A n szybszy");
												print(akcjeNPC[j]);
												print(akcjeNPC[jAnim].getAnimacja());*/
												akcjaNPCSkonczona = false;
												animacjaNPCSkonczona = false;
												czasAkcjiNPC = Time.time;
												interwalAkcjaNPC = System.Convert.ToDouble (aktKosztPANPC) * mnoznikCzasu;

												pozostalePANPC -= aktKosztPANPC;

												decreaseZycieGracza (((Atak)akcjeNPC [j]).getObrazenia () * ((Atak)akcjeNPC [j]).getMoc ());
												jj = 1;
											}
										} else if ((pozostalePAGracza - aktKosztPAGracza == pozostalePANPC - aktKosztPANPC) && (pokryte == 0)) {
											if ((akcjaGraczaSkonczona) && (akcjaNPCSkonczona)) {
												jAnim = j;
												iAnim = i;
												/*print("------------------------------------------------------------AKCJA! ng:A n rowny g");
												print(akcjeGracza[i]);
												print(akcjeGracza[iAnim].getAnimacja());
												print(akcjeNPC[j]);
												print(akcjeNPC[jAnim].getAnimacja());*/
												akcjaNPCSkonczona = false;
												akcjaGraczaSkonczona = false;
												animacjaNPCSkonczona = false;
												animacjaGraczaSkonczona = false;
												czasAkcjiGracza = Time.time;
												czasAkcjiNPC = Time.time;
												interwalAkcjaGracza = System.Convert.ToDouble (aktKosztPAGracza) * mnoznikCzasu;
												interwalAkcjaNPC = System.Convert.ToDouble (aktKosztPANPC) * mnoznikCzasu;

												pozostalePAGracza -= aktKosztPAGracza;
												pozostalePANPC -= aktKosztPANPC; 

												decreaseZycieNPC (((Atak)akcjeGracza [i]).getObrazenia () * ((Atak)akcjeGracza [i]).getMoc ());
												decreaseZycieGracza (((Atak)akcjeNPC [j]).getObrazenia () * ((Atak)akcjeNPC [j]).getMoc ());
												ii = 1;
												jj = 1;	
											}
										} else if (pokryte > 0) {
											if ((akcjaGraczaSkonczona) && (akcjaNPCSkonczona)) {
												jAnim = j;
												iAnim = i;
												/*print("------------------------------------------------------------AKCJA! ng:A ataki sie nie pokrywaja");
												print(akcjeGracza[i]);
												print(akcjeGracza[iAnim].getAnimacja());
												print(akcjeNPC[j]);
												print(akcjeNPC[jAnim].getAnimacja());*/
												akcjaNPCSkonczona = false;
												akcjaGraczaSkonczona = false;
												animacjaNPCSkonczona = false;
												animacjaGraczaSkonczona = false;
												czasAkcjiGracza = Time.time;
												czasAkcjiNPC = Time.time;
												interwalAkcjaGracza = System.Convert.ToDouble (aktKosztPAGracza) * mnoznikCzasu;
												interwalAkcjaNPC = System.Convert.ToDouble (aktKosztPANPC) * mnoznikCzasu;

												pozostalePAGracza -= aktKosztPAGracza;
												pozostalePANPC -= aktKosztPANPC;
												int obrazeniaG = ((Atak)akcjeGracza [i]).getObrazenia () - pokryte;
												int obrazeniaN = ((Atak)akcjeNPC [j]).getObrazenia () - pokryte;

												decreaseZycieNPC (obrazeniaG * ((Atak)akcjeGracza [i]).getMoc ());
												decreaseZycieGracza (obrazeniaN * ((Atak)akcjeNPC [j]).getMoc ());
												ii = 1;
												jj = 1;	
											}
										}
									}
									//obaj bronią
									else {
										if ((pozostalePAGracza - aktKosztPAGracza) > (pozostalePANPC - aktKosztPANPC)) {
											if (akcjaGraczaSkonczona) {
												iAnim = i;
												/*print("------------------------------------------------------------AKCJA! ng:D g szybszy");
												print(akcjeGracza[i]);
												print(akcjeGracza[iAnim].getAnimacja());*/
												akcjaGraczaSkonczona = false;
												animacjaGraczaSkonczona = false;
												czasAkcjiGracza = Time.time;
												interwalAkcjaGracza = System.Convert.ToDouble (aktKosztPAGracza) * mnoznikCzasu;

												pozostalePAGracza -= aktKosztPAGracza;
												ii = 1;
											}
										} 
										else if ((pozostalePAGracza - aktKosztPAGracza) < (pozostalePANPC - aktKosztPANPC)) {
											if (akcjaNPCSkonczona) {
												jAnim = j;
												/*print("------------------------------------------------------------AKCJA! ng:D n szybszy");
												print(akcjeNPC[j]);
												print(akcjeNPC[jAnim].getAnimacja());*/
												akcjaNPCSkonczona = false;
												animacjaNPCSkonczona = false;
												czasAkcjiNPC = Time.time;
												interwalAkcjaNPC = System.Convert.ToDouble (aktKosztPANPC) * mnoznikCzasu;

												pozostalePANPC -= aktKosztPANPC;
												jj = 1;
											}
										} 
										else {
											if ((akcjaGraczaSkonczona) && (akcjaNPCSkonczona)) {
												jAnim = j;
												iAnim = i;
												/*print("------------------------------------------------------------AKCJA!  ng:D rownoczesnie");
												print(akcjeGracza[i]);
												print(akcjeGracza[iAnim].getAnimacja());
												print(akcjeNPC[j]);
												print(akcjeNPC[jAnim].getAnimacja());*/
												akcjaNPCSkonczona = false;
												akcjaGraczaSkonczona = false;
												animacjaNPCSkonczona = false;
												animacjaGraczaSkonczona = false;
												czasAkcjiGracza = Time.time;
												czasAkcjiNPC = Time.time;
												interwalAkcjaGracza = System.Convert.ToDouble (aktKosztPAGracza) * mnoznikCzasu;
												interwalAkcjaNPC = System.Convert.ToDouble (aktKosztPANPC) * mnoznikCzasu;

												pozostalePAGracza -= aktKosztPAGracza;
												pozostalePANPC -= aktKosztPANPC;
												ii = 1;
												jj = 1;
											}
										}
									}
								} 
								else if ((pozostalePAGracza > 0) && (pozostalePANPC <= 0)) {
									aktKosztPAGracza = akcjeGracza [i].getKoszt ();
									if (akcjaGraczaSkonczona) {
										iAnim = i;
										/*print("------------------------------------------------------------AKCJA! ostatnia akcja g");
										print(akcjeGracza[i]);
										print(akcjeGracza[iAnim].getAnimacja());*/
										akcjaGraczaSkonczona = false;
										animacjaGraczaSkonczona = false;
										czasAkcjiGracza = Time.time;
										interwalAkcjaGracza = System.Convert.ToDouble (aktKosztPAGracza) * mnoznikCzasu;

										if (akcjeGracza [i].getTypAkcji ()) {
											pozostalePAGracza -= aktKosztPAGracza;

											decreaseZycieNPC (((Atak)akcjeGracza [i]).getObrazenia () * ((Atak)akcjeGracza [i]).getMoc ());
										} else {
											pozostalePAGracza -= aktKosztPAGracza;
										}
										ii = 1;
									}
								} 
								else if ((pozostalePANPC > 0) && (pozostalePAGracza <= 0)) {
									aktKosztPANPC = akcjeNPC [j].getKoszt ();
									if (akcjaNPCSkonczona) {
										jAnim = j;
										/*print("------------------------------------------------------------AKCJA! ostatnia akcja n");
										print(akcjeNPC[j]);
										print(akcjeNPC[jAnim].getAnimacja());*/
										akcjaNPCSkonczona = false;
										animacjaNPCSkonczona = false;
										czasAkcjiNPC = Time.time;
										interwalAkcjaNPC = System.Convert.ToDouble (aktKosztPANPC) * mnoznikCzasu;

										if (akcjeNPC [j].getTypAkcji ()) {
											pozostalePANPC -= aktKosztPANPC;

											decreaseZycieGracza (((Atak)akcjeNPC [j]).getObrazenia () * ((Atak)akcjeNPC [j]).getMoc ());
										} else {
											pozostalePANPC -= aktKosztPANPC;
										}
										jj = 1;
									}
								} 
								else {
									//print("Koniec kolejki");
									//j--; i--; animacjaGraczaSkonczona=false; animacjaNPCSkonczona=false;
									j = akcjeNPC.Count - 1;
									i = akcjeGracza.Count - 1;
									przejscieSkonczone = false;
									wykonane = true;
									/*print(akcjeGracza[i]);
									print(akcjeGracza[iAnim].getAnimacja());
									print(akcjeNPC[j]);
									print(akcjeNPC[jAnim].getAnimacja());*/
								}

								//jeśli, któryś z graczy umarł, zakończ rozgrywkę
								if ((zycieNPC<=0) || (zycieGracza<=0)){
									if ((zycieNPC<=0)&&(zycieGracza>0)) {
										wynik=1;
										zycieNPC=0;
										zapiszStatystyke();
										koniecRozgrywki = true;
									}
									else if ((zycieNPC>0)&&(zycieGracza<=0)) {
										wynik=0;
										zycieGracza=0;
										zapiszStatystyke();
										koniecRozgrywki = true;
									}
									else if ((zycieNPC<=0)&&(zycieGracza<=0)) {
										wynik=2;
										zycieNPC=0;
										zycieGracza=0;
										zapiszStatystyke();
										koniecRozgrywki = true;
									}
								}
								aktualizujZycie ();
							}

							if (!wykonane) {
								i += ii; 
								j += jj;
							}
						} else {
							if (jest) {
								//print("JEEEEEEEEEEEEEEEEEEEEEEST!!!!!!!");
								jest = false;
							}
						}
					}
					//jeśli to już koniec rozgrywki
					else {
						if (!animacjaGraczaSkonczona) akcjeGracza [iAnim].animuj ();
						if (!animacjaNPCSkonczona) akcjeNPC [jAnim].animuj ();
						if (animacjaNPCSkonczona&&animacjaGraczaSkonczona&&akcjaNPCSkonczona&&akcjaGraczaSkonczona){
							tpd.active=false;
							tld.active=false;
							tpg.active=false;
							tlg.active=false;
							GameObject.Find("RundaHUDText").GetComponent<Text>().text="";	//ukrywam (czyszczę tekst) rundy wyświetlane na HUD'zie
							GameObject.Find("CzasHUDText").GetComponent<Text>().text="";	//ukrywam (czyszczę tekst) czas wyświetlany na HUD'zie
							GameObject.Find("ZycieGraczaHUDText").GetComponent<Text>().text="";	//ukrywam (czyszczę tekst) życie gracza wyświetlane na HUD'zie
							GameObject.Find("ZycieNPCHUDText").GetComponent<Text>().text="";	//ukrywam (czyszczę tekst) życie NPC wyświetlane na HUD'zie
							GameObject.Find("PAGraczaHUDText").GetComponent<Text>().text="";	//ukrywam (czyszczę tekst) punkty akcji wyświetlane na HUD'zie
							/*statystyki=*/wczytajStatystyke();
							//GameObject.Find("WynikHUDText").GetComponent<Text>().text=statystyki;
							/*//jeśli to gracz stracił życie pokaż tekst przegranej na HUD'zie
							if((zycieGracza<=0)&&(zycieNPC>0)) {
								GameObject.Find("WynikHUDText").GetComponent<Text>().text="Przegrana\n"+tura.ToString()+" rund";
							}
							//jeśli to NPC stracił życie pokaż tekst wygranej na HUD'zie
							else if((zycieGracza>0)&&(zycieNPC<=0)) {
								GameObject.Find("WynikHUDText").GetComponent<Text>().text="Zwycięstwo\n"+tura.ToString()+" rund";
							}
							//jeśli to obaj stracili życie pokaż tekst remisu na HUD'zie
							else GameObject.Find("WynikHUDText").GetComponent<Text>().text="Remis\n"+tura.ToString()+" rund";*/
							GameObject.Find ("CanvasButton").GetComponent<Canvas> ().enabled=false;
							GameObject.Find ("CanvasNewGame").GetComponent<Canvas> ().enabled = true;

						}
					}
				}
			}
		}
	}

	bool zrobione=false;

	/// <summary>
	/// Metoda wykonywana po każdym wykonaniu metody Update
	/// </summary>
	void LateUpdate () {
		if (polaczono || !multiplayer){
			//jeśli nie skończył się czas tury
			if(turaTrwa){
				if(Input.GetKeyDown(KeyCode.Plus)||Input.GetKeyDown(KeyCode.KeypadPlus)) {
					interwal+=2;
				}
				if(Input.GetKeyDown(KeyCode.Minus)||Input.GetKeyDown(KeyCode.KeypadMinus)) {
					interwal-=1;
				}
				aktualizujCzas();
				aktualizujPA();
				//jeśli czas, który upłynał od początku tury jest większy od założonej długości tury
				if(Time.time-czas>interwal) {
					czas=Time.time;	//resetuję czas
					MousePointFields.clearHitbox(); //czyszczę hitbox na HUD'zie
					MousePointFields.setEnable(false);	//dezaktywuję HUD
					turaTrwa=false;	//przestawiam flagę trwania tury na fałsz

					GameObject.Find("HUDCombatArea").GetComponent<Renderer>().enabled=false;
					tlg.active=false;
					tpg.active=false;
					GameObject.Find("RundaHUDText").GetComponent<Text>().text="";	//ukrywam (czyszczę tekst) rundy wyświetlane na HUD'zie
					GameObject.Find("CzasHUDText").GetComponent<Text>().text="";	//ukrywam (czyszczę tekst) czas wyświetlany na HUD'zie
					GameObject.Find("PAGraczaHUDText").GetComponent<Text>().text="";	//ukrywam (czyszczę tekst) punkty akcji wyświetlane na HUD'zie

				}
			}
			else {
				GameObject.Find ("CanvasButton").GetComponent<Canvas> ().enabled=false;
				GameObject.Find ("RodzajAtakuHUDText").GetComponent<Text> ().text = "";
				rodzajAtaku = "";
				//czas=Time.time;
				if((!przejscieSkonczone)&&(!wykonane)){
					if(!pobralemAkcjePrzeciwnika){
						if (!multiplayer) zaplanujAkcjeNPC();
						else pobierzAkcjePrzeciwnika();
						pobralemAkcjePrzeciwnika=true;
					}
					if(((Time.time-czas)>interwalPrzejscie)&&pobralemAkcjePrzeciwnika&&((!multiplayer)||(skonczylemprzesyl))) {
						//print("Zmieniam flage przejscia");
						przejscieSkonczone=true;
						czas=Time.time;
						animacjaGraczaSkonczona=true;
						animacjaNPCSkonczona=true;
						akcjaGraczaSkonczona=false;
						akcjaNPCSkonczona=false;
						//zaplanujAkcjeNPC();
						pozostalePANPC=10; pozostalePAGracza=10;
						aktKosztPAGracza=0; aktKosztPANPC=0;
						pokryte=0; ii=0; jj=0; i=0; j=0; 
						iAnim=0; jAnim=0; jest=true;
						interwalAkcjaGracza=1; interwalAkcjaNPC=1;
						czasAkcjiGracza=Time.time;
						czasAkcjiNPC=Time.time;

					}
				}
				else{
					if((i<(akcjeGracza.Count+1))&&(!animacjaGraczaSkonczona)){
						if((Time.time-czasAkcjiGracza)>akcjeGracza[iAnim].getCzasAnimacji()){
							animacjaGraczaSkonczona=true;
							//print("Animacja Gracza Skonczona");
							player.GetComponent<Animation>().Stop();
							GameObject.Find ("strataNPCText").GetComponent<Text> ().text = "";
							stratyGracza = 0;
						}
					}
					if((j<(akcjeNPC.Count+1))&&(!animacjaNPCSkonczona)){
						if((Time.time-czasAkcjiNPC)>akcjeNPC[jAnim].getCzasAnimacji()){
							animacjaNPCSkonczona=true;
							//print("Animacja Gracza Skonczona");
							opponent.GetComponent<Animation>().Stop();
							GameObject.Find ("strataGraczaText").GetComponent<Text> ().text = "";
							stratyNPC = 0;
						}
					}
					//if(!wykonane) print("InterGracz: "+interwalAkcjaGracza.ToString()+"/"+(Time.time-czasAkcjiGracza).ToString());
					if(!akcjaGraczaSkonczona){
						if((Time.time-czasAkcjiGracza)>interwalAkcjaGracza) {
							akcjaGraczaSkonczona=true;
							player.GetComponent<Animation>().Stop();
							animacjaGraczaSkonczona=true;
							czasAkcjiGracza=Time.time;
							//print("Akcja Gracza Skonczona");
						}
					}
					//if(!wykonane) print("InterNPC: "+interwalAkcjaNPC.ToString()+"/"+(Time.time-czasAkcjiNPC).ToString());
					if(!akcjaNPCSkonczona){
						if((Time.time-czasAkcjiNPC)>interwalAkcjaNPC) {
							akcjaNPCSkonczona=true;
							opponent.GetComponent<Animation>().Stop();
							animacjaNPCSkonczona=true;
							czasAkcjiNPC=Time.time;
							//print("Akcja NPC Skonczona");
						}
					}

					if(/*(Time.time-czas>interwalWalka)||*/(wykonane&&akcjaGraczaSkonczona&&akcjaNPCSkonczona&&animacjaGraczaSkonczona&&animacjaGraczaSkonczona)){
						if(Input.GetMouseButton(2)/*Input.GetKey(KeyCode.Space)*/) {
							skonczonopotej=true;
							if(multiplayer){
								NetworkView nv=gameObject.GetComponent<NetworkView>();
								nv.stateSynchronization=NetworkStateSynchronization.ReliableDeltaCompressed;
								nv.RPC("Skonczylem",RPCMode.Others);
							}
							else skonczonopoprzeciwnej=true;
						}
						if (skonczonopoprzeciwnej&&skonczonopotej){
							nastepnaTura=true;
							wykonane=false;
						}
						if (!koniecRozgrywki) {
							tpd.active=false;
							tld.active=false;
							GameObject.Find("ZycieGraczaHUDText").GetComponent<Text>().text="";	//ukrywam (czyszczę tekst) życie gracza wyświetlane na HUD'zie
							GameObject.Find("ZycieNPCHUDText").GetComponent<Text>().text="";	//ukrywam (czyszczę tekst) życie NPC wyświetlane na HUD'zie
							/*if (skonczonopotej&&multiplayer) GameObject.Find("NastepnaTuraHUDText").GetComponent<Text>().text="Poczekaj na drugiego gracza...";
							else GameObject.Find("NastepnaTuraHUDText").GetComponent<Text>().text="Wciśnij [ŚPM], aby przejść do kolejnej tury.";*/
							if (skonczonopotej&&multiplayer) Textt="Poczekaj...";
							else Textt="Wroc do gry";
							rysujHistore ();
							pokazTabelke();
							//wykonane=true;
							//jeśli jeszcze żyją przygotuj następną turę
							if (nastepnaTura) {
								//print("NASTEPNA TURA!!!!");
								dodajDoHistorii();
								zrobione = false;
								//rodzajAtaku += "\n\nRunda " + (tura+1);
								pobralemAkcjePrzeciwnika=false;
								skonczonopotej=false;
								skonczonopoprzeciwnej=false;
								skonczylemprzesyl=false;
								pozostalePANPC=10; pozostalePAGracza=10;
								aktKosztPAGracza=0; aktKosztPANPC=0;
								pokryte=0; ii=0; jj=0; i=0; j=0; 
								iAnim=0; jAnim=0; jest=true;
								interwalAkcjaGracza=1; interwalAkcjaNPC=1;
								czasAkcjiGracza=99; czasAkcjiNPC=99;
								ukryjTabelke();
								//runda1P.Clear();
								//runda1N.Clear();
								akcjeGracza.Clear ();
								akcjeNPC.Clear ();
								GameObject.Find("NastepnaTuraHUDText").GetComponent<Text>().text="";
								nastepnaTura=false;
								GameObject.Find("Main Camera").GetComponent<Animation>().Stop();
								GameObject.Find("Main Camera").GetComponent<Animation>().PlayQueued("KameraReturn");
								player.transform.position=new Vector3(0.0f,0.0f,-2.5f);
								opponent.transform.position=new Vector3(0.0f,0.0f,2.5f);
								punktyAkcjiGracza=10;	//przywracam punkty akcji graczowi
								punktyAkcjiNPC=10;	//przywracam punkty akcji NPC
								MousePointFields.reset();	//resetuje HUD
								MousePointFields.setEnable(true);	//ponownie aktywuje HUD
								turaTrwa=true;	//wznawiam ture
								tura++;	//kolejna tura
								GameObject.Find("RundaHUDText").GetComponent<Text>().text="Runda "+tura.ToString();	//aktualizuję rundę wyświetlaną na HUD'zie
								czas=Time.time; //resetuje czas
								aktualizujCzas();
								aktualizujPA();

								GameObject.Find("HUDCombatArea").GetComponent<Renderer>().enabled=true;
								tlg.active=true;
								tpg.active=true;
								tpd.active=true;
								tld.active=true;
								aktualizujZycie();
								wykonane=false;
								przejscieSkonczone=false;
							}
						}
					}
				}
			}
		}
	}

	void zapiszStatystyke(){
		string str;
		pozostaleZycie=zycieGracza;
		str=wynik+";"+nazwaGracza+";"+nazwaPrzeciwnika+";"+tura+";"+wykonaneAtaki+";"+zablokowaneAtaki+";"+wykonaneObrony+";"+pozostaleZycie;
		//string old=wczytajStatystyke();
		if(System.IO.File.Exists(plikStatystyk)) wczytajStatystyke();

		System.IO.StreamWriter sr = new System.IO.StreamWriter(plikStatystyk); //otworzenie pliku do zapisu
		sr.WriteLine(str);
		for(int i=0;i<wyniki.Count;i++){
			sr.WriteLine(wyniki[i]+";"+nicki[i]+";"+nickip[i]+";"+tury[i]+";"+wykAtaki[i]+";"+zablAtaki[i]+";"+wykObrony[i]+";"+pozZycie[i]);
		}
		//sr.WriteLine(old);
		sr.Close();
	}

	void wczytajStatystyke(){
		int max=20;
		wyniki.Clear();
		nicki.Clear();
		nickip.Clear();
		tury.Clear();
		wykAtaki.Clear();
		zablAtaki.Clear();
		wykObrony.Clear();
		pozZycie.Clear();
		//string str="";
		tura=0;
		zablokowaneAtaki=0;
		wykonaneAtaki=0;
		wykonaneObrony=0;
		pozostaleZycie=0;
		wynik=0;
		if(System.IO.File.Exists(plikStatystyk)){
			System.IO.StreamReader sr = new System.IO.StreamReader(plikStatystyk, Encoding.UTF8);   //otworzenie pliku, ustawienie kodowania
			while (!sr.EndOfStream) //do konca strumienia
			{
				string line = sr.ReadLine();
				if(line.Contains(";")){
					string [] pola = line.Split(';');
					//if (line.Length>0)str+=line+"\n";
					wyniki.Add(pola[0]);
					nicki.Add(pola[1]);
					nickip.Add(pola[2]);
					tury.Add(pola[3]);
					wykAtaki.Add(pola[4]);
					zablAtaki.Add(pola[5]);
					wykObrony.Add(pola[6]);
					pozZycie.Add(pola[7]);
				}
			}
			sr.Close();
		}
		GameObject.Find("WynikiHUDText").GetComponent<Text>().text="\tWynik\n";
		GameObject.Find("NickiHUDText").GetComponent<Text>().text="Gracz\n";
		GameObject.Find("NickipHUDText").GetComponent<Text>().text="Przeciwnik\n";
		GameObject.Find("RundyHUDText").GetComponent<Text>().text="Rund\n";
		GameObject.Find("AtakiHUDText").GetComponent<Text>().text="Ataków\n";
		GameObject.Find("ZAtakiHUDText").GetComponent<Text>().text="Zabl.\n";
		GameObject.Find("ObronyHUDText").GetComponent<Text>().text="Obron\n";
		GameObject.Find("PZycieHUDText").GetComponent<Text>().text="Życie\n";
		int w=wyniki.Count;
		if(w>max)w=max;
		for(int i=0;i<w;i++){
			GameObject.Find("WynikiHUDText").GetComponent<Text>().text+=(i+1)+". ";
			switch(wyniki[i]){
				case "0": GameObject.Find("WynikiHUDText").GetComponent<Text>().text+="Porażka"; break;
				case "1": GameObject.Find("WynikiHUDText").GetComponent<Text>().text+="Zwycięstwo"; break;
				case "2": GameObject.Find("WynikiHUDText").GetComponent<Text>().text+="Remis"; break;
				default: GameObject.Find("WynikiHUDText").GetComponent<Text>().text+="-"; break;
			}
			GameObject.Find("WynikiHUDText").GetComponent<Text>().text+="\n";
		}
		w=nicki.Count;
		if(w>max)w=max;
		for(int i=0;i<w;i++){
			GameObject.Find("NickiHUDText").GetComponent<Text>().text+=nicki[i]+"\n";
		}
		w=nickip.Count;
		if(w>max)w=max;
		for(int i=0;i<w;i++){
			GameObject.Find("NickipHUDText").GetComponent<Text>().text+=nickip[i]+"\n";
		}
		w=tury.Count;
		if(w>max)w=max;
		for(int i=0;i<w;i++){
			GameObject.Find("RundyHUDText").GetComponent<Text>().text+=tury[i]+"\n";
		}
		w=wykAtaki.Count;
		if(w>max)w=max;
		for(int i=0;i<w;i++){
			GameObject.Find("AtakiHUDText").GetComponent<Text>().text+=wykAtaki[i]+"\n";
		}
		w=zablAtaki.Count;
		if(w>max)w=max;
		for(int i=0;i<w;i++){
			GameObject.Find("ZAtakiHUDText").GetComponent<Text>().text+=zablAtaki[i]+"\n";
		}
		w=wykObrony.Count;
		if(w>max)w=max;
		for(int i=0;i<w;i++){
			GameObject.Find("ObronyHUDText").GetComponent<Text>().text+=wykObrony[i]+"\n";
		}
		w=pozZycie.Count;
		if(w>max)w=max;
		for(int i=0;i<w;i++){
			GameObject.Find("PZycieHUDText").GetComponent<Text>().text+=pozZycie[i]+"\n";
		}

	}
	
	void pobierzAkcjePrzeciwnika(){
		NetworkView nv=gameObject.GetComponent<NetworkView>();
		nv.stateSynchronization=NetworkStateSynchronization.ReliableDeltaCompressed;
		string doprzeslania = punktyAkcjiGracza.ToString()+";";
		foreach(Czynnosc c in akcjeGracza){
			doprzeslania+=c.serializuj();
		}
		print("Wysyłam: "+doprzeslania);
		nv.RPC("OdbierzDane",RPCMode.Others,doprzeslania);
	}

	[RPC]
	public void Skonczylem (){
		skonczonopoprzeciwnej=true;
	}

	[RPC]
	public void nadajMojaNazwe (String nazwaG){
		nazwaPrzeciwnika = nazwaG;
	}

	[RPC]
	public void OdbierzDane (String text)
	{
		Debug.Log(text);
		string [] dane = text.Split(';');
		punktyAkcjiNPC = System.Convert.ToInt32(dane[0]);
		bool koniec=false;
		int i=1;
		while (!koniec){
			if (i<dane.Length){
				if (dane[i]=="A"){
					bool[,] hitboxataku = new bool[3,3];
					i++;
					int ile=System.Convert.ToInt32(dane[i]);
					i++;
					for (int j=0;j<ile;j++){
						hitboxataku[System.Convert.ToInt32(dane[i]),System.Convert.ToInt32(dane[i+1])]=true;
						i+=2;
					}
					int kosztatk = System.Convert.ToInt32(dane[i]);
					i++;
					int iloscpolatk = System.Convert.ToInt32(dane[i]);
					i++;
					int mocatk = System.Convert.ToInt32(dane[i]);
					i++;
					int firstatk = System.Convert.ToInt32(dane[i]);
					i++;
					int lastatk = System.Convert.ToInt32(dane[i]);
					i++;
					Atak a = new Atak(false,hitboxataku,kosztatk,iloscpolatk,mocatk);
					a.setFirst(firstatk);
					a.setLast(lastatk);
					akcjeNPC.Add(a);
				}
				else if (dane[i]=="O"){
					bool[,] hitboxobrony = new bool[3,3];
					i++;
					int ile=System.Convert.ToInt32(dane[i]);
					i++;
					for (int j=0;j<ile;j++){
						hitboxobrony[System.Convert.ToInt32(dane[i]),System.Convert.ToInt32(dane[i+1])]=true;
						i+=2;
					}
					int kosztobr = System.Convert.ToInt32(dane[i]);
					i++;
					int iloscpolobr = System.Convert.ToInt32(dane[i]);
					i++;
					Obrona o = new Obrona(false,hitboxobrony,kosztobr,iloscpolobr);
					akcjeNPC.Add(o);
				}
				else {
					koniec=true;
				}
			}
			else koniec=true;
		}
		skonczylemprzesyl=true;
	}

	void dodajDoHistorii(){
		foreach(Czynnosc c in akcjeGracza){
			if(c.getTypAkcji()){
				historiaAkcjiGracza.Add(new Atak(true,c.getHitbox(),c.getKoszt(),c.getIloscPol(),((Atak)c).getMoc()));
			}
			else{
				historiaAkcjiGracza.Add(new Obrona(true,c.getHitbox(),c.getKoszt(),c.getIloscPol()));
			}
		}
	}

	bool czyZaznaczyc(int startx, int starty, int hor, int vert, int indx, int indy){
		bool czy=false;
		if (((startx!=hor)&&(starty!=vert))&&((startx!=indx)&&(starty!=indy))) czy=true;
		if (((startx==hor)&&(starty!=vert))||((startx!=hor)&&(starty==vert))){
			if (startx==1&&starty==1) czy=false;
			else if(hor==1&&vert==1){
				if (((Mathf.Abs(startx-indx)>1)||(Mathf.Abs(starty-indy)>1))) czy=true;
			}
			else if((startx==1||starty==1)&&(hor!=1&&vert!=1)){
				if (startx==hor||starty==vert) czy=true;
			}
			else if(((hor==startx)&&(hor==indx)&&(vert!=indy))||((vert==starty)&&(vert==indy)&&(hor!=indx))) czy=true;
			else if (((vert==indy)||(hor==indx))&&(indx!=1&&indy!=1)) czy=true;
		}
//		print (startx+" "+starty+" "+hor+" "+vert+" "+indx+" "+indy+" "+czy);

		return czy;
	}
	public int pierwszyNPCx = -1;
	public int pierwszyNPCy = -1;

	public int drugiNPCx = -1;
	public int drugiNPCy = -1;

	public int ostatniNPCx = -1;
	public int ostatniNPCy = -1;

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
			int vert=-1;	//pierwszy indeks drugiego pola do zaznaczenia w tablicy
			int hor=-1;	//drugi indeks drugiego pola do zaznaczenia w tablicy
			int vert2=-1;	//pierwszy indeks trzeciego pola do zaznaczenia w tablicy
			int hor2=-1;	//drugi indeks trzeciego pola do zaznaczenia w tablicy
			//losowanie indeksów punktu startowego, wylosowana zostanie liczba całkowita w przedziale 0-2
			startx=System.Convert.ToInt32(UnityEngine.Random.value*2);
			starty=System.Convert.ToInt32(UnityEngine.Random.value*2);

			double czyAtakuje;

			if (zycieNPC<20) {
				strategia=strategie[2];
				if (zycieGracza<15) strategia=strategie[0];
			}

			if (strategia==strategie[0]) czyAtakuje=0.1;
			else if (strategia==strategie[1]) czyAtakuje=0.3;
			else if (strategia==strategie[2]) czyAtakuje=0.6;
			else czyAtakuje=0.2;
			//jeśli NPC posiada przynajmniej 2 punkty akcji, to zostanie wybrany atak z prawdopodobieństwem 80%
			if((UnityEngine.Random.value>czyAtakuje)&&(punktyAkcjiNPC>1)) modeNPC=true;
			//jeśli NPC posiada tylko jeden punkt akcji lub mniej albo z prawdopodobieństwa nie wybrano ataku to zostanie wybrana obrona
			else modeNPC=false;

			//jeśli została wybrana obrona i NPC ma przynajmniej 1 pkt akcji
			if ((!modeNPC) && (punktyAkcjiNPC>=1)) {

				//zaznaczam pierwsze pole w najbardziej atakowanym przez gracza miejscu
				if (zwrocIloscAkcji(true)>0){
					print("Będę się bronił logicznie!");
					zwrocWybieranePola(true);
					startx=zwrocIndeks(false,true,true);
					starty=zwrocIndeks(true,true,true);
					print("Najwiecej1: "+prawdopodobienstwo[startx,starty]+" Indeksy: "+startx+" "+starty);
				}
				hitboxNPC[startx,starty]=true;	//zaznacza punkt startowy w hitboxie

				kosztAkcjiNPC=1; iloscPolNPC++;	//dodaję 1 do kosztu akcji i do ilości zaznaczonych pól
				//jeśli NPC ma więcej niż 1 punkt akcji
				if (punktyAkcjiNPC>1){
					int najwiecej=0, indx=0, indy=0;
					if (zwrocIloscAkcji(true)>0){
						najwiecej=0;
						if (startx-1>=0){
							if (prawdopodobienstwo[startx-1, starty]>=najwiecej){
								if ((prawdopodobienstwo[startx-1, starty]!=najwiecej)||(UnityEngine.Random.value<=0.5)){
									najwiecej=prawdopodobienstwo[startx-1, starty];
									hor=startx-1; vert=starty;
								}
							}
							if (((startx!=1)&&(starty!=1))||((startx==1)&&(starty==1))){
								if (starty-1>=0){
									if (prawdopodobienstwo[startx-1, starty-1]>=najwiecej){
										if ((prawdopodobienstwo[startx-1, starty-1]!=najwiecej)||(UnityEngine.Random.value<=0.5)){
											najwiecej=prawdopodobienstwo[startx-1, starty-1];
											hor=startx-1; vert=starty-1;
										}
									}
								}
								if (starty+1<=2){
									if (prawdopodobienstwo[startx-1, starty+1]>=najwiecej){
										if ((prawdopodobienstwo[startx-1, starty+1]!=najwiecej)||(UnityEngine.Random.value<=0.5)){
											najwiecej=prawdopodobienstwo[startx-1, starty+1];
											hor=startx-1; vert=starty+1;
										}
									}
								}
							}
						}
						if (startx+1<=2){
							if (prawdopodobienstwo[startx+1, starty]>=najwiecej){
								if ((prawdopodobienstwo[startx+1, starty]!=najwiecej)||(UnityEngine.Random.value<=0.5)){
									najwiecej=prawdopodobienstwo[startx+1, starty];
									hor=startx+1; vert=starty;
								}
							}
							if (((startx!=1)&&(starty!=1))||((startx==1)&&(starty==1))){
								if (starty-1>=0){
									if (prawdopodobienstwo[startx+1, starty-1]>=najwiecej){
										if ((prawdopodobienstwo[startx+1, starty-1]!=najwiecej)||(UnityEngine.Random.value<=0.5)){
											najwiecej=prawdopodobienstwo[startx+1, starty-1];
											hor=startx+1; vert=starty-1;
										}
									}
								}
								if (starty+1<=2){
									if (prawdopodobienstwo[startx+1, starty+1]>=najwiecej){
										if ((prawdopodobienstwo[startx+1, starty+1]!=najwiecej)||(UnityEngine.Random.value<=0.5)){
											najwiecej=prawdopodobienstwo[startx+1, starty+1];
											hor=startx+1; vert=starty+1;
										}
									}
								}
							}
						}
						if (starty-1>=0){
							if (prawdopodobienstwo[startx, starty-1]>=najwiecej){
								if ((prawdopodobienstwo[startx, starty-1]!=najwiecej)||(UnityEngine.Random.value<=0.5)){
									najwiecej=prawdopodobienstwo[startx, starty-1];
									hor=startx; vert=starty-1;
								}
							}
						}
						if (starty+1<=2){
							if (prawdopodobienstwo[startx, starty+1]>=najwiecej){
								if ((prawdopodobienstwo[startx, starty+1]!=najwiecej)||(UnityEngine.Random.value<=0.5)){
									najwiecej=prawdopodobienstwo[startx, starty+1];
									hor=startx; vert=starty+1;
								}
							}
						}
						print("Najwiecej2: "+najwiecej+" Indeksy: "+hor+" "+vert);
					}
					else{
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
						} while(((startx==hor)&&(starty==vert))||!(((startx!=1&&starty!=1)||(startx==1&&starty==1))||((!(startx!=1&&starty!=1)||!(startx==1&&starty==1))&&((startx==hor)||(starty==vert))))); //jeśli nowe indeksy pokryły się z poprzednim polem, to wylosuj od nowa
					}
					if (hor>0&&vert>0){
						hitboxNPC[hor,vert]=true;	//zaznacza drugie pole w hitboxie
						kosztAkcjiNPC=2; iloscPolNPC++;	//dodaję 1 do kosztu akcji i do ilości zaznaczonych pól
						//jeśli NPC ma więcej niż 2 punkt akcji
						if ((punktyAkcjiNPC>2)&&((startx!=1)&&(starty!=1))){
							if (zwrocIloscAkcji(true)>0){
								najwiecej=0; indx=hor; indy=vert;
								if (hor-1>=0){
									indx=hor-1;
									if ((indx!=startx)||(vert!=starty)){
										indy=vert;
										if (czyZaznaczyc(startx,starty,hor,vert,indx,indy)){
											if (prawdopodobienstwo[indx, indy]>=najwiecej){
												if ((prawdopodobienstwo[indx, indy]!=najwiecej)||(UnityEngine.Random.value<=0.5)){
													najwiecej=prawdopodobienstwo[indx, indy];
													hor2=indx; vert2=indy;
												}
											}
										}
									}
									if ((indx!=startx)||(vert-1!=starty)){
										if (vert-1>=0){
											indy=vert-1;
											if (czyZaznaczyc(startx,starty,hor,vert,indx,indy)){
												if (prawdopodobienstwo[indx, indy]>=najwiecej){
													if ((prawdopodobienstwo[indx, indy]!=najwiecej)||(UnityEngine.Random.value<=0.5)){
														najwiecej=prawdopodobienstwo[indx, indy];
														hor2=indx; vert2=indy;
													}
												}
											}
										}
									}
									if ((indx!=startx)||(vert+1!=starty)){
										if (vert+1<=2){
											indy=vert+1;
											if (czyZaznaczyc(startx,starty,hor,vert,indx,indy)){
												if (prawdopodobienstwo[indx, indy]>=najwiecej){
													if ((prawdopodobienstwo[indx, indy]!=najwiecej)||(UnityEngine.Random.value<=0.5)){
														najwiecej=prawdopodobienstwo[indx, indy];
														hor2=indx; vert2=indy;
													}
												}
											}
										}
									}
								}
								if (hor+1<=2){
									indx=hor+1;
									if ((indx!=startx)||(vert!=starty)){
										indy=vert;
										if (czyZaznaczyc(startx,starty,hor,vert,indx,indy)){
											if (prawdopodobienstwo[indx, indy]>=najwiecej){
												if ((prawdopodobienstwo[indx, indy]!=najwiecej)||(UnityEngine.Random.value<=0.5)){
													najwiecej=prawdopodobienstwo[indx, indy];
													hor2=indx; vert2=indy;
												}
											}
										}
									}
									if ((indx!=startx)||(vert-1!=starty)){
										if (vert-1>=0){
											indy=vert-1;
											if (czyZaznaczyc(startx,starty,hor,vert,indx,indy)){
												if (prawdopodobienstwo[indx, indy]>=najwiecej){
													if ((prawdopodobienstwo[indx, indy]!=najwiecej)||(UnityEngine.Random.value<=0.5)){
														najwiecej=prawdopodobienstwo[indx, indy];
														hor2=indx; vert2=indy;
													}
												}
											}
										}
									}
									if ((indx!=startx)||(vert+1!=starty)){
										if (vert+1<=2){
											indy=vert+1;
											if (czyZaznaczyc(startx,starty,hor,vert,indx,indy)){
												if (prawdopodobienstwo[indx, indy]>=najwiecej){
													if ((prawdopodobienstwo[indx, indy]!=najwiecej)||(UnityEngine.Random.value<=0.5)){
														najwiecej=prawdopodobienstwo[indx, indy];
														hor2=indx; vert2=indy;
													}
												}
											}
										}
									}
								}
								indx=hor;
								if ((hor!=startx)||(vert-1!=starty)){
									if (vert-1>=0){
										indy=vert-1;
										if (czyZaznaczyc(startx,starty,hor,vert,indx,indy)){
											if (prawdopodobienstwo[indx, indy]>=najwiecej){
												if ((prawdopodobienstwo[indx, indy]!=najwiecej)||(UnityEngine.Random.value<=0.5)){
													najwiecej=prawdopodobienstwo[indx, indy];
													hor2=indx; vert2=indy;
												}
											}
										}
									}
								}
								if ((hor!=startx)||(vert+1!=starty)){
									if (vert+1<=2){
										indy=vert+1;
										if (czyZaznaczyc(startx,starty,hor,vert,indx,indy)){
											if (prawdopodobienstwo[indx, indy]>=najwiecej){
												if ((prawdopodobienstwo[indx, indy]!=najwiecej)||(UnityEngine.Random.value<=0.5)){
													najwiecej=prawdopodobienstwo[indx, indy];
													hor2=indx; vert2=indy;
												}
											}
										}
									}
								}
								print("Najwiecej3: "+najwiecej+" Indeksy: "+hor2+" "+vert2);
							}
							else{
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
								} while((((hor==hor2)&&(vert==vert2))||((startx==hor2)&&(starty==vert2)))||!(czyZaznaczyc(startx,starty,hor,vert,hor2,vert2)));	//jeśli nowe indeksy pokryły się z poprzednimi polami, to wylosuj od nowa
							}
							if (hor2>0&&vert2>0){
								hitboxNPC[hor2,vert2]=true;	//zaznacza trzecie pole w hitboxie
								kosztAkcjiNPC=3; iloscPolNPC++;	//dodaję 1 do kosztu akcji i do ilości zaznaczonych pól
							}
						}
					}
				}
			}
			//jeśli został wybrany atak i NPC ma przynajmniej 1 pkt akcji
			if ((modeNPC)&&(punktyAkcjiNPC>=1)){
				zwrocWybieranePola(false);
				//zaznaczam pierwsze pole w najmniej bronionym przez gracza miejscu
				if (zwrocIloscAkcji(false)>0){
					//print("Będę atakował logicznie!");
					zwrocWybieranePola(false);
					startx=zwrocIndeks(false,false,true);
					starty=zwrocIndeks(true,false,true);
					//print("Najmniej1: "+prawdopodobienstwo[startx,starty]+" Indeksy: "+startx+" "+starty);
				}
				hitboxNPC[startx,starty]=true;	//zaznacza punkt startowy w hitboxie		*------------------------------------------PAMIETAJ ODKOMENTOWAC BO TO JEST POPRAWNE 


				if (ostatniNPCx == startx && ostatniNPCy == starty) {
					ComboNPC++;
				} else
					ComboNPC = 1;

				ostatniNPCx = startx;
				ostatniNPCy = starty;

				iloscPolNPC=1; //ustawiam ilość zaznaczonych pól na 1
				int rodzajAtaku=0;	//inicjalizuję zmienną rodzaj ataku
				rodzajAtaku=System.Convert.ToInt32(UnityEngine.Random.value*2);	//losuję wartość z przedziału 0-2 i przypisuję do rodzajAtaku

				if(strategia==strategie[0]) rodzajAtaku-=1; //tylko ciężkie lub pchnięcia

				if(rodzajAtaku==1&&(startx==1&&starty==1)) {
					if(strategia==strategie[0]) rodzajAtaku=0;
					else if (UnityEngine.Random.value<0.3){
						rodzajAtaku=2;
					}
					else rodzajAtaku=0;
				}
				//jeśli rodzaj ataku = 0, to zakładam, że wybrano pchnięcie
				if(rodzajAtaku<=0){
					//print("Pchnięcie");
					if(punktyAkcjiNPC>=4) kosztAkcjiNPC=4;	//ustawiam koszt akcji pchnięcia jeśli NPC ma potrzebną ilość punktów akcji
					else rodzajAtaku=1;	//jeśli nie ma potrzebnych punktów akcji, to zmieniam rodzaj na ciężki atak

				}
				//jesli rodzaj ataku = 1, to zakładam, że wybrano ciężki atak
				if(rodzajAtaku==1){
					//print("Ciężki");
					//jeśli NPC ma potrzebne punkty akcji
					if(punktyAkcjiNPC>=3&&(startx!=1&&starty!=1)){
						int najmniej=999, indx=0, indy=0;
						if (zwrocIloscAkcji(false)>0){
							najmniej=999;
							if (startx-1>=0){
								if (prawdopodobienstwo[startx-1, starty]<=najmniej){
									if ((prawdopodobienstwo[startx-1, starty]!=najmniej)||(UnityEngine.Random.value<=0.5)){
										najmniej=prawdopodobienstwo[startx-1, starty];
										hor=startx-1; vert=starty;
									}
								}
								if (((startx!=1)&&(starty!=1))||((startx==1)&&(starty==1))){
									if (starty-1>=0){
										if (prawdopodobienstwo[startx-1, starty-1]<=najmniej){
											if ((prawdopodobienstwo[startx-1, starty-1]!=najmniej)||(UnityEngine.Random.value<=0.5)){
												najmniej=prawdopodobienstwo[startx-1, starty-1];
												hor=startx-1; vert=starty-1;
											}
										}
									}
									if (starty+1<=2){
										if (prawdopodobienstwo[startx-1, starty+1]<=najmniej){
											if ((prawdopodobienstwo[startx-1, starty+1]!=najmniej)||(UnityEngine.Random.value<=0.5)){
												najmniej=prawdopodobienstwo[startx-1, starty+1];
												hor=startx-1; vert=starty+1;
											}
										}
									}
								}
							}
							if (startx+1<=2){
								if (prawdopodobienstwo[startx+1, starty]<=najmniej){
									if ((prawdopodobienstwo[startx+1, starty]!=najmniej)||(UnityEngine.Random.value<=0.5)){
										najmniej=prawdopodobienstwo[startx+1, starty];
										hor=startx+1; vert=starty;
									}
								}
								if (((startx!=1)&&(starty!=1))||((startx==1)&&(starty==1))){
									if (starty-1>=0){
										if (prawdopodobienstwo[startx+1, starty-1]<=najmniej){
											if ((prawdopodobienstwo[startx+1, starty-1]!=najmniej)||(UnityEngine.Random.value<=0.5)){
												najmniej=prawdopodobienstwo[startx+1, starty-1];
												hor=startx+1; vert=starty-1;
											}
										}
									}
									if (starty+1<=2){
										if (prawdopodobienstwo[startx+1, starty+1]<=najmniej){
											if ((prawdopodobienstwo[startx+1, starty+1]!=najmniej)||(UnityEngine.Random.value<=0.5)){
												najmniej=prawdopodobienstwo[startx+1, starty+1];
												hor=startx+1; vert=starty+1;
											}
										}
									}
								}
							}
							if (starty-1>=0){
								if (prawdopodobienstwo[startx, starty-1]<=najmniej){
									if ((prawdopodobienstwo[startx, starty-1]!=najmniej)||(UnityEngine.Random.value<=0.5)){
										najmniej=prawdopodobienstwo[startx, starty-1];
										hor=startx; vert=starty-1;
									}
								}
							}
							if (starty+1<=2){
								if (prawdopodobienstwo[startx, starty+1]<=najmniej){
									if ((prawdopodobienstwo[startx, starty+1]!=najmniej)||(UnityEngine.Random.value<=0.5)){
										najmniej=prawdopodobienstwo[startx, starty+1];
										hor=startx; vert=starty+1;
									}
								}
							}
							print("Najmniej2: "+najmniej+" Indeksy: "+hor+" "+vert);
						}
						else{
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
							} while(((startx==hor)&&(starty==vert))||!(((startx!=1&&starty!=1)||(startx==1&&starty==1))||((!(startx!=1&&starty!=1)||!(startx==1&&starty==1))&&((startx==hor)||(starty==vert))))); //jeśli nowe indeksy pokryły się z poprzednim polem, to wylosuj od nowa
						}
						hitboxNPC[hor,vert]=true; //zaznacza drugie pole w hitboxie
						ostatniNPCx = hor;
						ostatniNPCy = vert;

						if (zwrocIloscAkcji(false)>0){
							najmniej=999; indx=hor; indy=vert;
							//print (indx+" "+indy+" "+czyZaznaczyc(startx,starty,hor,vert,indx,indy));
							if (hor-1>=0){
								indx=hor-1;
								if ((indx!=startx)||(vert!=starty)){
									indy=vert;
									if (czyZaznaczyc(startx,starty,hor,vert,indx,indy)){
										if (prawdopodobienstwo[indx, indy]<=najmniej){
											if ((prawdopodobienstwo[indx, indy]!=najmniej)||(UnityEngine.Random.value<=0.5)){
												najmniej=prawdopodobienstwo[indx, indy];
												hor2=indx; vert2=indy;
											}
										}
									}
								}
								if ((indx!=startx)||(vert-1!=starty)){
									if (vert-1>=0){
										indy=vert-1;
										if (czyZaznaczyc(startx,starty,hor,vert,indx,indy)){
											if (prawdopodobienstwo[indx, indy]<=najmniej){
												if ((prawdopodobienstwo[indx, indy]!=najmniej)||(UnityEngine.Random.value<=0.5)){
													najmniej=prawdopodobienstwo[indx, indy];
													hor2=indx; vert2=indy;
												}
											}
										}
									}
								}
								if ((indx!=startx)||(vert+1!=starty)){
									if (vert+1<=2){
										indy=vert+1;
										if (czyZaznaczyc(startx,starty,hor,vert,indx,indy)){
											if (prawdopodobienstwo[indx, indy]<=najmniej){
												if ((prawdopodobienstwo[indx, indy]!=najmniej)||(UnityEngine.Random.value<=0.5)){
													najmniej=prawdopodobienstwo[indx, indy];
													hor2=indx; vert2=indy;
												}
											}
										}
									}
								}
							}
							if (hor+1<=2){
								indx=hor+1;
								if ((indx!=startx)||(vert!=starty)){
									indy=vert;
									if (czyZaznaczyc(startx,starty,hor,vert,indx,indy)){
										if (prawdopodobienstwo[indx, indy]<=najmniej){
											if ((prawdopodobienstwo[indx, indy]!=najmniej)||(UnityEngine.Random.value<=0.5)){
												najmniej=prawdopodobienstwo[indx, indy];
												hor2=indx; vert2=indy;
											}
										}
									}
								}
								if ((indx!=startx)||(vert-1!=starty)){
									if (vert-1>=0){
										indy=vert-1;
										if (czyZaznaczyc(startx,starty,hor,vert,indx,indy)){
											if (prawdopodobienstwo[indx, indy]<=najmniej){
												if ((prawdopodobienstwo[indx, indy]!=najmniej)||(UnityEngine.Random.value<=0.5)){
													najmniej=prawdopodobienstwo[indx, indy];
													hor2=indx; vert2=indy;
												}
											}
										}
									}
								}
								if ((indx!=startx)||(vert+1!=starty)){
									if (vert+1<=2){
										indy=vert+1;
										if (czyZaznaczyc(startx,starty,hor,vert,indx,indy)){
											if (prawdopodobienstwo[indx, indy]<=najmniej){
												if ((prawdopodobienstwo[indx, indy]!=najmniej)||(UnityEngine.Random.value<=0.5)){
													najmniej=prawdopodobienstwo[indx, indy];
													hor2=indx; vert2=indy;
												}
											}
										}
									}
								}
							}
							indx=hor;
							if ((hor!=startx)||(vert-1!=starty)){
								if (vert-1>=0){
									indy=vert-1;
									if (czyZaznaczyc(startx,starty,hor,vert,indx,indy)){
										if (prawdopodobienstwo[indx, indy]<=najmniej){
											if ((prawdopodobienstwo[indx, indy]!=najmniej)||(UnityEngine.Random.value<=0.5)){
												najmniej=prawdopodobienstwo[indx, indy];
												hor2=indx; vert2=indy;
											}
										}
									}
								}
							}
							if ((hor!=startx)||(vert+1!=starty)){
								if (vert+1<=2){
									indy=vert+1;
									if (czyZaznaczyc(startx,starty,hor,vert,indx,indy)){
										if (prawdopodobienstwo[indx, indy]<=najmniej){
											if ((prawdopodobienstwo[indx, indy]!=najmniej)||(UnityEngine.Random.value<=0.5)){
												najmniej=prawdopodobienstwo[indx, indy];
												hor2=indx; vert2=indy;
											}
										}
									}
								}
							}
							print("Najmniej3: "+najmniej+" Indeksy: "+hor2+" "+vert2);
						}
						else{
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
							} while((((hor==hor2)&&(vert==vert2))||((startx==hor2)&&(starty==vert2)))||!(czyZaznaczyc(startx,starty,hor,vert,hor2,vert2)));	//jeśli nowe indeksy pokryły się z poprzednimi polami, to wylosuj od nowa
						}
						hitboxNPC[hor2,vert2]=true;	//zaznacza trzecie pole w hitboxie
						ostatniNPCx = hor;
						ostatniNPCy = vert;
						kosztAkcjiNPC=3; iloscPolNPC=3;	//ustawiam kosztu akcji i ilości zaznaczonych pól
					}
					else rodzajAtaku=2;
				}
				//jesli rodzaj ataku = 2, to zakładam, że wybrano szybki atak
				if(rodzajAtaku==2){
					//print("Szybki");
					if(punktyAkcjiNPC>=2){
						int najmniej=999;
						if (zwrocIloscAkcji(false)>0){
							najmniej=999;
							if (startx-1>=0){
								if (prawdopodobienstwo[startx-1, starty]<=najmniej){
									if ((prawdopodobienstwo[startx-1, starty]!=najmniej)||(UnityEngine.Random.value<=0.5)){
										najmniej=prawdopodobienstwo[startx-1, starty];
										hor=startx-1; vert=starty;
									}
								}
								if (((startx!=1)&&(starty!=1))||((startx==1)&&(starty==1))){
									if (starty-1>=0){
										if (prawdopodobienstwo[startx-1, starty-1]<=najmniej){
											if ((prawdopodobienstwo[startx-1, starty-1]!=najmniej)||(UnityEngine.Random.value<=0.5)){
												najmniej=prawdopodobienstwo[startx-1, starty-1];
												hor=startx-1; vert=starty-1;
											}
										}
									}
									if (starty+1<=2){
										if (prawdopodobienstwo[startx-1, starty+1]<=najmniej){
											if ((prawdopodobienstwo[startx-1, starty+1]!=najmniej)||(UnityEngine.Random.value<=0.5)){
												najmniej=prawdopodobienstwo[startx-1, starty+1];
												hor=startx-1; vert=starty+1;
											}
										}
									}
								}
							}
							if (startx+1<=2){
								if (prawdopodobienstwo[startx+1, starty]<=najmniej){
									if ((prawdopodobienstwo[startx+1, starty]!=najmniej)||(UnityEngine.Random.value<=0.5)){
										najmniej=prawdopodobienstwo[startx+1, starty];
										hor=startx+1; vert=starty;
									}
								}
								if (((startx!=1)&&(starty!=1))||((startx==1)&&(starty==1))){
									if (starty-1>=0){
										if (prawdopodobienstwo[startx+1, starty-1]<=najmniej){
											if ((prawdopodobienstwo[startx+1, starty-1]!=najmniej)||(UnityEngine.Random.value<=0.5)){
												najmniej=prawdopodobienstwo[startx+1, starty-1];
												hor=startx+1; vert=starty-1;
											}
										}
									}
									if (starty+1<=2){
										if (prawdopodobienstwo[startx+1, starty+1]<=najmniej){
											if ((prawdopodobienstwo[startx+1, starty+1]!=najmniej)||(UnityEngine.Random.value<=0.5)){
												najmniej=prawdopodobienstwo[startx+1, starty+1];
												hor=startx+1; vert=starty+1;
											}
										}
									}
								}
							}
							if (starty-1>=0){
								if (prawdopodobienstwo[startx, starty-1]<=najmniej){
									if ((prawdopodobienstwo[startx, starty-1]!=najmniej)||(UnityEngine.Random.value<=0.5)){
										najmniej=prawdopodobienstwo[startx, starty-1];
										hor=startx; vert=starty-1;
									}
								}
							}
							if (starty+1<=2){
								if (prawdopodobienstwo[startx, starty+1]<=najmniej){
									if ((prawdopodobienstwo[startx, starty+1]!=najmniej)||(UnityEngine.Random.value<=0.5)){
										najmniej=prawdopodobienstwo[startx, starty+1];
										hor=startx; vert=starty+1;
									}
								}
							}
							//print("Najmniej2: "+najmniej+" Indeksy: "+hor+" "+vert);
						}
						else{
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
							} while(((startx==hor)&&(starty==vert))||!(((startx!=1&&starty!=1)||(startx==1&&starty==1))||((!(startx!=1&&starty!=1)||!(startx==1&&starty==1))&&((startx==hor)||(starty==vert))))); //jeśli nowe indeksy pokryły się z poprzednim polem, to wylosuj od nowa
						}
						hitboxNPC[hor,vert]=true; //zaznacza drugie pole w hitboxie
						ostatniNPCx = hor;
						ostatniNPCy = vert;
						kosztAkcjiNPC=2; iloscPolNPC=2;	//ustawiam kosztu akcji i ilości zaznaczonych pól
					}
				}
			}
			//jeśli to był atak, to dodaj nowy atak do kolejki
			//printHitboxNPC();
			if (modeNPC) {
				akcjeNPC.Add(new Atak(false,hitboxNPC,kosztAkcjiNPC,iloscPolNPC,mocNPC*ComboNPC));
			}
			//jeśli to była obrona, to dodaj nową obbronę do kolejki
			else {
				akcjeNPC.Add(new Obrona(false,hitboxNPC,kosztAkcjiNPC,iloscPolNPC));
			}
			punktyAkcjiNPC-=kosztAkcjiNPC;	//zabiera punkty wykonanej akcji z puli punktów akcji NPC
		}
	}
		

	void zwrocWybieranePola(bool tryb){

		for(int i=0;i<3;i++){
			for(int j=0;j<3;j++) prawdopodobienstwo[i, j]=0;
		}
		for(int i=0;i<historiaAkcjiGracza.Count;i++){
			if (historiaAkcjiGracza[i].getTypAkcji()==tryb){
				if (historiaAkcjiGracza[i].getHitbox()[0,0]) prawdopodobienstwo[0,0]+=1;
				if (historiaAkcjiGracza[i].getHitbox()[0,1]) prawdopodobienstwo[0,1]+=1;
				if (historiaAkcjiGracza[i].getHitbox()[0,2]) prawdopodobienstwo[0,2]+=1;
				if (historiaAkcjiGracza[i].getHitbox()[1,0]) prawdopodobienstwo[1,0]+=1;
				if (historiaAkcjiGracza[i].getHitbox()[1,1]) prawdopodobienstwo[1,1]+=1;
				if (historiaAkcjiGracza[i].getHitbox()[1,2]) prawdopodobienstwo[1,2]+=1;
				if (historiaAkcjiGracza[i].getHitbox()[2,0]) prawdopodobienstwo[2,0]+=1;
				if (historiaAkcjiGracza[i].getHitbox()[2,1]) prawdopodobienstwo[2,1]+=1;
				if (historiaAkcjiGracza[i].getHitbox()[2,2]) prawdopodobienstwo[2,2]+=1;
			}
		}
		string str="Akcje:\n";
		for(int i=0;i<3;i++){
			for(int j=0;j<3;j++){
				str+=prawdopodobienstwo[i,j];
				str+=" ";
			}
			str+="\n";
		}
		//print(str);
	}
		
	int zwrocIndeks(bool xory, bool najczesciej, bool losowo){
		int indx=-1, indy=-1;
		int ile;
		if (najczesciej) ile=-1;
		else ile=9999;
		for(int i=0;i<3;i++){
			for(int j=0;j<3;j++){
				if(najczesciej){
					if(ile<=prawdopodobienstwo[i,j]) {
						if (!losowo){
							indx=i; indy=j;
							ile=prawdopodobienstwo[i,j];
						}
						else {
							if (ile==prawdopodobienstwo[i,j]){
								if (UnityEngine.Random.value>=0.5){
									indx=i; indy=j;
									ile=prawdopodobienstwo[i,j];
								}
							}
							else{
								indx=i; indy=j;
								ile=prawdopodobienstwo[i,j];
							}
						}
					}
				}
				else{
					if(ile>=prawdopodobienstwo[i,j]) {
						if (!losowo){
							indx=i; indy=j;
							ile=prawdopodobienstwo[i,j];
						}
						else {
							if (ile==prawdopodobienstwo[i,j]){
								if (UnityEngine.Random.value>=0.5){
									indx=i; indy=j;
									ile=prawdopodobienstwo[i,j];
								}
							}
							else{
								indx=i; indy=j;
								ile=prawdopodobienstwo[i,j];
							}
						}
					}
				}
			}
		}
		//print(indx+" "+indy+" ile: "+ile);
		if(indx<0||indy<0){
			return -1;
		}
		else{
			if(!xory){
				return indx;
			}
			else{
				return indy;
			}
		}
	}

	int zwrocIloscAkcji(bool tryb){
		int ile=0;
		if (historiaAkcjiGracza.Count>0){
			foreach (Czynnosc c in historiaAkcjiGracza){
				if(c.getTypAkcji()==tryb) ile++;
			}
			return ile;
		}
		else return 0;
	}

	float drawpositionx=Screen.width*0.01f;
	float drawpositiony=Screen.height*0.1f;

	bool GUI1 = false;
	public Texture2D miecz;
	public Texture2D tarcza;

	public Texture2D mysz;
	public Texture2D myszl;
	public Texture2D myszr;
	public Texture2D myszs;



	List<List<Czynnosc>> listaListGracza = new List<List<Czynnosc>>();
	List<List<Czynnosc>> listaListNPC = new List<List<Czynnosc>>();

	void rysujHistore() {
		if (zrobione == false) {
			if (tura == 1) {
				listaListGracza.Add(new List<Czynnosc>());
				foreach (Czynnosc c in akcjeGracza) {
					runda1P.Add (c);

					listaListGracza [0].Add (c);
				}
				//listaListGracza.Add (runda1P);
				foreach (Czynnosc c in akcjeNPC) {
					runda1N.Add (c);
					listaListNPC.Add(new List<Czynnosc>());
					listaListNPC [0].Add (c);
				}
				//listaListNPC.Add (runda1N);
			}
			if (tura == 2) {
				foreach (Czynnosc c in akcjeGracza) {
					runda2P.Add (c);
					listaListGracza.Add(new List<Czynnosc>());
					listaListGracza [1].Add (c);
				}
				//listaListGracza.Add (runda2P);
				foreach (Czynnosc c in akcjeNPC) {
					runda2N.Add (c);
					listaListNPC.Add(new List<Czynnosc>());
					listaListNPC [1].Add (c);
				}
				//listaListNPC.Add (runda2N);
			}
			if (tura == 3) {
				foreach (Czynnosc c in akcjeGracza) {
					runda3P.Add (c);
					listaListGracza.Add(new List<Czynnosc>());
					listaListGracza [2].Add (c);
				}
				//listaListGracza.Add (runda3P);
				foreach (Czynnosc c in akcjeNPC) {
					runda3N.Add (c);
					listaListNPC.Add(new List<Czynnosc>());
					listaListNPC [2].Add (c);
				}
				//listaListNPC.Add (runda3N);
			}
			if (tura == 4) {
				foreach (Czynnosc c in akcjeGracza) {
					runda4P.Add (c);
					listaListGracza.Add(new List<Czynnosc>());
					listaListGracza [3].Add (c);
				}
				//listaListGracza.Add (runda4P);
				foreach (Czynnosc c in akcjeNPC) {
					runda4N.Add (c);
					listaListNPC.Add(new List<Czynnosc>());
					listaListNPC [3].Add (c);
				}
				//listaListNPC.Add (runda4N);
			} else if (tura > 4) {
				runda1P.Clear ();
				runda1N.Clear ();

				foreach (Czynnosc c in runda2P) {
					runda1P.Add (c);
				}
				foreach (Czynnosc c in runda2N) {
					runda1N.Add (c);
				}

				runda2P.Clear ();
				runda2N.Clear ();

				foreach (Czynnosc c in runda3P) {
					runda2P.Add (c);
				}
				foreach (Czynnosc c in runda3N) {
					runda2N.Add (c);
				}

				runda3P.Clear ();
				runda3N.Clear ();

				foreach (Czynnosc c in runda4P) {
					runda3P.Add (c);
				}
				foreach (Czynnosc c in runda4N) {
					runda3N.Add (c);
				}

				runda4P.Clear ();
				runda4N.Clear ();

				foreach (Czynnosc c in akcjeGracza) {
					runda4P.Add (c);
					listaListGracza.Add(new List<Czynnosc>());
					listaListGracza [tura-1].Add (c);
				}
				//listaListGracza.Add (runda4P);
				foreach (Czynnosc c in akcjeNPC) {
					runda4N.Add (c);
					listaListNPC.Add(new List<Czynnosc>());
					listaListNPC [tura-1].Add (c);
				}
				//listaListNPC.Add (runda4N);
			}
			zrobione = true;
		}
	}

	public Texture2D[] draws;
	GUIStyle style = new GUIStyle();

	float wys1AP = 0.09f;

	void jakRysowac(List<Czynnosc> akcje) {
		foreach (Czynnosc i in akcje) {
			int temp = i.getKoszt ();
			Texture2D tempM;
			if (i.getTypAkcji ())
				tempM = miecz;
			else
				tempM = tarcza;

			style.imagePosition = ImagePosition.ImageOnly;

			//Screen.width * 0.115f;

			GUI.DrawTexture (new Rect (drawpositionx + Screen.width * 0.115f * 0.02f, drawpositiony+ Screen.height * 0.005f, Screen.width * 0.115f - 2.0f * Screen.width * 0.115f * 0.02f, (Screen.height * wys1AP * temp)-(Screen.height*0.005f*2.0f)), border, ScaleMode.ScaleAndCrop);
			//GUI.Box (new Rect (drawpositionx + Screen.width * 0.115f * 0.02f, drawpositiony+ Screen.height * 0.005f, Screen.width * 0.115f/2f - Screen.width * 0.115f * 0.02f, Screen.height * wys1AP * temp - (Screen.height*0.005f*2.0f)), tempM,style);
			GUI.DrawTexture (new Rect (drawpositionx + Screen.width * 0.115f * 0.02f, drawpositiony+ Screen.height * 0.005f, Screen.width * 0.115f/2f - Screen.width * 0.115f * 0.02f, Screen.height * wys1AP * temp - (Screen.height*0.005f*2.0f)), tempM,ScaleMode.ScaleToFit);
			//GUI.Box (new Rect (drawpositionx, drawpositiony, Screen.width * 0.115f/2f, Screen.height * wys1AP * temp), tempM,style);

			GUI.DrawTexture (new Rect (drawpositionx+Screen.width * 0.115f/2f, drawpositiony, Screen.width * 0.115f/2f, Screen.height * wys1AP * temp), draws[0],ScaleMode.ScaleToFit);
			//GUI.Box (new Rect (drawpositionx+Screen.width * 0.115f/2f, drawpositiony, Screen.width * 0.115f/2f, Screen.height * wys1AP * temp), draws[0],style);
			for (int i2 = 0; i2 < 3; i2++) {
				for (int j2 = 0; j2 < 3; j2++) {
					if (i.getHitbox() [i2,j2] == true) {
						GUI.DrawTexture (new Rect (drawpositionx+Screen.width * 0.115f/2f, drawpositiony, Screen.width * 0.115f/2f, Screen.height * wys1AP * temp), draws[i2*3+j2+1],ScaleMode.ScaleToFit);
						//GUI.Box (new Rect (drawpositionx+Screen.width * 0.115f/2f, drawpositiony, Screen.width * 0.115f/2f, Screen.height * wys1AP * temp), draws[i2*3+j2+1],style);
					}
				}
			}

			drawpositiony += Screen.height * wys1AP * temp;



		}
		drawpositiony = Screen.height * 0.1f;
	}

	public Texture2D border;
	public Texture2D black;

	void drawBoxAndTextures (float drawposition, int odTury) {

		GUIStyle dlaHistorii = new GUIStyle ();
		dlaHistorii.alignment = TextAnchor.MiddleCenter;

		GUI.DrawTexture (new Rect (drawpositionx, Screen.height * 0.0f, Screen.width * 0.115f * 2.0f, Screen.height * 0.05f), border, ScaleMode.ScaleAndCrop);
		GUI.DrawTexture (new Rect (drawpositionx, Screen.height * 0.05f, Screen.width * 0.115f, Screen.height * 0.05f), border, ScaleMode.ScaleAndCrop);
		GUI.DrawTexture (new Rect (drawpositionx + Screen.width * 0.115f, Screen.height * 0.05f, Screen.width * 0.115f, Screen.height * 0.05f), border, ScaleMode.ScaleAndCrop);
		GUI.Box (new Rect (drawpositionx, Screen.height * 0.0f, Screen.width * 0.115f * 2.0f, Screen.height * 0.05f), "Runda " + odTury.ToString(), dlaHistorii);
		GUI.Box (new Rect (drawpositionx, Screen.height * 0.05f, Screen.width * 0.115f, Screen.height * 0.05f), (nazwaGracza!="") ? nazwaGracza : "Gracz 1", dlaHistorii);
		GUI.Box (new Rect (drawpositionx + Screen.width * 0.115f, Screen.height * 0.05f, Screen.width * 0.115f, Screen.height * 0.05f), (nazwaPrzeciwnika!="") ? nazwaPrzeciwnika : "Gracz 2", dlaHistorii);
		
	}

	int wyswietlanaRunda;
	bool czyResetowac = true;
	bool lewytak = true;
	bool lewynie = true;

	float czasowy;
	bool myszsz = true;

	bool lewo;
	bool prawo;

	void OnGUI() {
		if (GUI1 == true) {

			if (wyswietlanaRunda > 3 && lewytak)
				lewo = true;
			else
				lewo = false;

			if (wyswietlanaRunda < tura - 1 && lewynie)
				prawo = true;
			else
				prawo = false;

			if(Input.GetMouseButtonDown(0)&&wyswietlanaRunda>3&&lewytak) {
				wyswietlanaRunda--;
				lewytak = false;
			}
			if(Input.GetMouseButtonDown(1)&&wyswietlanaRunda<tura-1&&lewynie) {
				wyswietlanaRunda++;
				lewynie = false;
			}
			if (Input.GetMouseButtonUp(0)) {
				lewytak = true;
			}
			if (Input.GetMouseButtonUp(1)) {
				lewynie = true;
			}

			if (tura >= 1) {
				GUI.DrawTexture (new Rect (Screen.width*0.0f, Screen.height*0.0f, Screen.width*0.25f, Screen.height), black, ScaleMode.ScaleAndCrop);
				drawBoxAndTextures (drawpositionx, (wyswietlanaRunda > 3) ? wyswietlanaRunda-2 : 1);
			}if (tura >= 2) {
				GUI.DrawTexture (new Rect (Screen.width*0.25f, Screen.height*0.0f, Screen.width*0.25f, Screen.height), black, ScaleMode.ScaleAndCrop);
				drawpositionx = Screen.width * 0.26f;
				drawBoxAndTextures (drawpositionx, (wyswietlanaRunda > 3) ? wyswietlanaRunda-1 : 2);
			} if (tura >= 3) {
				GUI.DrawTexture (new Rect (Screen.width*0.50f, Screen.height*0.0f, Screen.width*0.25f, Screen.height), black, ScaleMode.ScaleAndCrop);
				drawpositionx = Screen.width * 0.51f;
				drawBoxAndTextures (drawpositionx, (wyswietlanaRunda > 3) ? wyswietlanaRunda : 3);
			} if (tura >= 4) {
				GUI.DrawTexture (new Rect (Screen.width*0.75f, Screen.height*0.0f, Screen.width*0.25f, Screen.height), black, ScaleMode.ScaleAndCrop);
				drawpositionx = Screen.width * 0.76f;
				drawBoxAndTextures (drawpositionx, (wyswietlanaRunda > 3) ? wyswietlanaRunda+1 : 4);
			}
			drawpositionx = Screen.width * 0.01f;
			if (tura >= 1) {
				//jakRysowac (runda1P);
				jakRysowac (listaListGracza [(wyswietlanaRunda > 3) ? wyswietlanaRunda - 3 : 0]);
				drawpositionx = drawpositionx + Screen.width * 0.115f;
				//jakRysowac (runda1N);
				jakRysowac (listaListNPC [(wyswietlanaRunda > 3) ? wyswietlanaRunda - 3 : 0]);
				drawpositionx = drawpositionx + Screen.width * 0.135f;
			}
			if (tura >= 2) {
				//jakRysowac (runda2P);
				jakRysowac (listaListGracza [(wyswietlanaRunda > 3) ? wyswietlanaRunda - 2 : 1]);
				drawpositionx = drawpositionx + Screen.width * 0.115f;
				//jakRysowac (runda2N);
				jakRysowac (listaListNPC [(wyswietlanaRunda > 3) ? wyswietlanaRunda - 2 : 1]);
				drawpositionx = drawpositionx + Screen.width * 0.135f;
			}
			if (tura >= 3) {
				//jakRysowac (runda3P);
				jakRysowac (listaListGracza [(wyswietlanaRunda > 3) ? wyswietlanaRunda - 1 : 2]);
				drawpositionx = drawpositionx + Screen.width * 0.115f;
				//jakRysowac (runda3N);
				jakRysowac (listaListNPC [(wyswietlanaRunda > 3) ? wyswietlanaRunda - 1 : 2]);
				drawpositionx = drawpositionx + Screen.width * 0.135f;
			}
			if (tura >= 4) {
				//jakRysowac (runda4P);
				jakRysowac (listaListGracza [(wyswietlanaRunda > 3) ? wyswietlanaRunda : 3]);
				drawpositionx = drawpositionx + Screen.width * 0.115f;
				//jakRysowac (runda4N);
				jakRysowac (listaListNPC [(wyswietlanaRunda > 3) ? wyswietlanaRunda : 3]);
			}
			drawpositionx = Screen.width * 0.01f;
			GUI.DrawTexture (new Rect (Screen.width * -0.01f, Screen.height * 0f, Screen.width * 0.02f, Screen.height), border, ScaleMode.ScaleAndCrop);
			GUI.DrawTexture (new Rect (Screen.width * 0.24f, Screen.height * 0f, Screen.width * 0.02f, Screen.height), border, ScaleMode.ScaleAndCrop);
			GUI.DrawTexture (new Rect (Screen.width * 0.49f, Screen.height * 0f, Screen.width * 0.02f, Screen.height), border, ScaleMode.ScaleAndCrop);
			GUI.DrawTexture (new Rect (Screen.width * 0.74f, Screen.height * 0f, Screen.width * 0.02f, Screen.height), border, ScaleMode.ScaleAndCrop);
			GUI.DrawTexture (new Rect (Screen.width * 0.99f, Screen.height * 0f, Screen.width * 0.02f, Screen.height), border, ScaleMode.ScaleAndCrop);

			if (0.25f < Time.time-czasowy) {
				myszsz = !myszsz;
				czasowy = Time.time;
			}

			if (lewo) {
				GUI.Box (new Rect (Screen.width * 0f, Screen.height * 0.00f, Screen.width * 0.075f, Screen.height * 0.025f), "Poprzednie Rundy");
				GUI.DrawTexture (new Rect (Screen.width * 0f, Screen.height * 0.025f, Screen.width * 0.05f, Screen.height * 0.075f), (myszsz) ? mysz : myszl, ScaleMode.ScaleToFit);
			}

			GUI.Box (new Rect (Screen.width * 0.4625f, Screen.height * 0.00f, Screen.width * 0.075f, Screen.height * 0.025f),Textt);
			GUI.DrawTexture (new Rect (Screen.width * 0.475f, Screen.height * 0.025f, Screen.width * 0.05f, Screen.height * 0.075f), (myszsz) ? mysz: myszs, ScaleMode.ScaleToFit);

			if (prawo) {
				GUI.Box (new Rect (Screen.width * 0.925f, Screen.height * 0.00f, Screen.width * 0.075f, Screen.height * 0.025f), "Nastepne Rundy");
				GUI.DrawTexture (new Rect (Screen.width * 0.95f, Screen.height * 0.025f, Screen.width * 0.05f, Screen.height * 0.075f), (myszsz) ? mysz : myszr, ScaleMode.ScaleToFit);
			}
		}
	}

	void pokazTabelke() {
		if (czyResetowac) {
			wyswietlanaRunda=tura-1;
			czyResetowac=false;
		}
		GUI1 = true;
	}

	void ukryjTabelke() {
		GUI1 = false;
		czyResetowac = true;
	}


	// GETTERY SETTERY I TYM PODOBNE

	/// <summary>
	/// Zmniejsza życie gracza o i.
	/// </summary>
	/// <param name="i">ilość punktów życia do zmniejszenia</param>
	public static void decreaseZycieGracza(double i){
		zycieGracza-=i;
		stratyGracza = i;
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
		stratyNPC = i;
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


	public static void resetVariables(){
		MousePointFields.setEnable(true);
		MousePointFields.setCheck(true);
		nazwaGracza="Gracz";
		nazwaPrzeciwnika="NPC";

		multiplayer=false;
		polaczono=false;

		zablokowaneAtaki=0;
		wykonaneAtaki=0;
		wykonaneObrony=0;
		pozostaleZycie=0;
		wynik=0;

		wyniki.Clear();
		tury.Clear();
		zablAtaki.Clear();
		wykAtaki.Clear();
		wykObrony.Clear();
		pozZycie.Clear();
		nicki.Clear();

		interwalAkcjaNPC=0.0;
		interwalAkcjaGracza=0.0;

		nastepnaTura=false;



		poziomTrudnosci = poziomyTrudnosci[1];
		strategia = strategie[0];

		pozostalePAGracza=10;
		pozostalePANPC=10;
		i=0; j=0; ii=0; jj=0; iAnim=0; jAnim=0;

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
		double g=zycieGracza, n=zycieNPC;
		if (g<0) g=0; 
		if (n<0) n=0;
		GameObject.Find("ZycieGraczaHUDText").GetComponent<Text>().text="PŻ Gracza: "+g.ToString();
		GameObject.Find("ZycieNPCHUDText").GetComponent<Text>().text="PŻ Przeciwnika: "+n.ToString();
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

	public bool getTuraTrwa() {
		return this.turaTrwa;
	}
}	

// KLASY ODPOWIEDZIALNE ZA REPREZENTACJE AKCJI

/// <summary>
/// Klasa abstrakcyjna reprezentująca akcję wykonaną przez jednego z graczy.
/// </summary>
public abstract class Czynnosc {
	protected bool typAkcji;
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

	protected GameObject character;
	protected Animator canimator;

	protected string animacja;
	protected string dzwiek;
	protected double szybkosc;
	protected bool odegrane=false;
	/// <summary>
	/// Inicjalizuje instancję klasy <see cref="Czynnosc"/>.
	/// </summary>
	/// <param name="gracz">Jeśli ustawiona na <c>true</c> akcja jest wykonywana przez gracza.</param>
	/// <param name="hitbox">Hitbox dla tej akcji.</param>
	/// <param name="koszt">Koszt tej akcji.</param>
	/// <param name="iloscPol">Ilosc zaznaczonych pól w tej akcji.</param>
	public Czynnosc(bool gracz, bool [,] hitbox, int koszt, int iloscPol){
		this.gracz=gracz;
		if (gracz) character=GameObject.Find("Player");
		else character=GameObject.Find("Opponent");
		canimator=this.character.GetComponent<Animator>();
		//this.hitbox=hitbox;
		this.hitbox=new bool[3,3];
		for (int i = 0; i < 3; i++) {
			for(int j=0;j<3;j++) {
				this.hitbox [i, j] = hitbox [i, j];
			}
		}
		this.koszt=koszt;
		this.iloscPol=iloscPol;
	}
		
	public int porownajHitbox(Czynnosc c){
		int ile=0;
		for (int i=0;i<3;i++){
			for (int j=0;j<3;j++){
				if ((c.getHitbox()[i,j])&&(this.hitbox[i,j])) ile++;
			}
		}
		return ile;
	}

	public void animuj(){
		canimator.SetFloat("szybkosc",System.Convert.ToSingle(this.szybkosc));
		canimator.Play(this.animacja);//,canimator.GetLayerIndex("Base Layer"), System.Convert.ToSingle(this.koszt*GameLogicDataScript.mnoznikCzasu));

		/*this.character.GetComponent<Animation>()[this.animacja].speed=System.Convert.ToSingle(szybkosc);
		this.character.GetComponent<Animation>().Play(this.animacja);*/

	}
	
	public double getCzasAnimacji(){
		/*if (gracz){
			return System.Convert.ToDouble(GameObject.Find("Player").GetComponent<Animation>().GetClip(this.animacja).length);
		}
		else {
			return System.Convert.ToDouble(GameObject.Find("Opponent").GetComponent<Animation>().GetClip(this.animacja).length);
		}*/
		return this.koszt*GameLogicDataScript.mnoznikCzasu;
	}

	public bool getGracz(){
		return gracz;
	}

	public bool [,] getHitbox(){
		return hitbox;
	}

	public int getKoszt(){
		return koszt;
	}

	public int getIloscPol(){
		return iloscPol;
	}

	public void setIloscPol(int ilosc){
		iloscPol=ilosc;
	}

	public string getAnimacja(){
		return animacja;
	}

	public string getDzwiek(){
		return dzwiek;
	}

	public bool getTypAkcji(){
		return typAkcji;
	}

	public virtual string serializuj(){
		return "czynnosc";
	}
	
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
	//first index x / last index y - indeksy pierwszego i ostatniego pola zaznaczonego w hitboxie - do kombosów
	int fIndX;
	int fIndY;
	int lIndX;
	int lIndY;
	/// <summary>
	/// Inicjalizuje instancję klasy <see cref="Atak"/>.
	/// </summary>
	/// <param name="gracz">Jeśli ustawiona na <c>true</c> akcja jest wykonywana przez gracza.</param>
	/// <param name="hitbox">Hitbox dla tej akcji.</param>
	/// <param name="koszt">Koszt tej akcji.</param>
	/// <param name="iloscPol">Ilosc zaznaczonych pól w tej akcji.</param>
	/// <param name="moc">Moc ataku.</param>
	public Atak(bool gracz, bool [,] hitbox, int koszt, int iloscPol, int moc) : base(gracz, hitbox, koszt, iloscPol) {
		if (gracz) GameLogicDataScript.wykonaneAtaki+=1;
		this.typAkcji=true;
		this.moc=moc;
		//w zależności od ilości pól stwierdzamy jaki to atak i przypisujemy mu obrażenia
		switch(iloscPol){
			case 1: this.obrazenia=4; this.animacja="ThrustAttack"; this.dzwiek="ThrustAttack"; break;
			case 2: this.obrazenia=2; this.animacja="FastAttack"; this.dzwiek="SlashAttack"; break;
			case 3: this.obrazenia=3; this.animacja="Attack"; this.dzwiek="SlashAttack"; break;
		default: this.animacja="Defense"; break;
		}
		float len=1;
		RuntimeAnimatorController ac = this.canimator.runtimeAnimatorController;
		foreach(AnimationClip animat in ac.animationClips){
			if(animat.name==this.animacja) {
				len=animat.length;
				szybkosc=System.Convert.ToDouble(len)/(this.koszt*GameLogicDataScript.mnoznikCzasu);
				//animat.frameRate=animat.frameRate*System.Convert.ToSingle(szybkosc);
			}
		}
		//szybkosc=System.Convert.ToDouble(this.character.GetComponent<Animation>().GetClip(this.animacja).length)/(this.koszt*GameLogicDataScript.mnoznikCzasu);
	}

	public int getMoc(){
		return moc;
	}

	public int getObrazenia(){
		return obrazenia;
	}

	public void aktualizujObrazenia(){
		if(this.obrazenia==4){
			if(iloscPol==0) obrazenia=0;
		}
		else if(this.obrazenia==3){
			obrazenia=iloscPol;
		}
		else if(this.obrazenia==2){
			obrazenia=iloscPol;
		}
		if (obrazenia<0) obrazenia=0;
	}

	public void setFirst(int x, int y){
		this.fIndX=x;
		this.fIndY=y;
	}

	public void setLast(int x, int y){
		this.lIndX=x;
		this.lIndY=y;
	}

	public void setFirst(int p){
		switch (p){
			case 1: this.fIndX=0; this.fIndY=0; break;
			case 4: this.fIndX=1; this.fIndY=0; break;
			case 7: this.fIndX=2; this.fIndY=0; break;
			case 2: this.fIndX=0; this.fIndY=1; break;
			case 5: this.fIndX=1; this.fIndY=1; break;
			case 8: this.fIndX=2; this.fIndY=1; break;
			case 3: this.fIndX=0; this.fIndY=2; break;
			case 6: this.fIndX=1; this.fIndY=2; break;
			case 9: this.fIndX=2; this.fIndY=2; break;
		}
	}

	public void setLast(int p){
		switch (p){
			case 1: this.lIndX=0; this.lIndY=0; break;
			case 4: this.lIndX=1; this.lIndY=0; break;
			case 7: this.lIndX=2; this.lIndY=0; break;
			case 2: this.lIndX=0; this.lIndY=1; break;
			case 5: this.lIndX=1; this.lIndY=1; break;
			case 8: this.lIndX=2; this.lIndY=1; break;
			case 3: this.lIndX=0; this.lIndY=2; break;
			case 6: this.lIndX=1; this.lIndY=2; break;
			case 9: this.lIndX=2; this.lIndY=2; break;
		}
	}

	//zwraca inta 1-9 oznaczającego pole, 1-[0,0] 5-[1,1] 9-[2,2]
	public int getFirst(){
		if (this.fIndX==0){
			if (this.fIndY==0) return 1;
			else if (this.fIndY==1) return 2;
			else return 3;
		}
		else if (this.fIndX==1){
			if (this.fIndY==0) return 4;
			else if (this.fIndY==1) return 5;
			else return 6;
		}
		else {
			if (this.fIndY==0) return 7;
			else if (this.fIndY==1) return 8;
			else return 9;
		}
	}

	public int getLast(){
		if (this.lIndX==0){
			if (this.lIndY==0) return 1;
			else if (this.lIndY==1) return 2;
			else return 3;
		}
		else if (this.lIndX==1){
			if (this.lIndY==0) return 4;
			else if (this.lIndY==1) return 5;
			else return 6;
		}
		else {
			if (this.lIndY==0) return 7;
			else if (this.lIndY==1) return 8;
			else return 9;
		}
	}

	//A;ileparwspolrzednych;x;y;x2;y2;x3;y3;koszt;iloscpol;moc;pierwszezaznaczenie;ostatniezaznaczenie;
	public override string serializuj(){
		string s="A;";
		int ilepol=0;
		for (int i=0;i<3;i++){
			for (int j=0;j<3;j++){
				if (hitbox[i,j]==true) ilepol++;
			}
		}
		s+=ilepol.ToString()+";";
		for (int i=0;i<3;i++){
			for (int j=0;j<3;j++){
				if (hitbox[i,j]==true) {
					s+=i+";"+j+";";
				}
			}
		}
		s+=koszt.ToString()+";";
		s+=iloscPol.ToString()+";";
		s+=moc.ToString()+";";
		s+=this.getFirst().ToString()+";";
		s+=this.getLast().ToString()+";";
		return s;
	}
	
	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="Atak"/>.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="Atak"/>.</returns>
	public override string ToString(){
		return "Jestem "+this.dzwiek+" obr:"+obrazenia+" Gracz: "+gracz;
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
		if (gracz) GameLogicDataScript.wykonaneObrony+=1;
		this.typAkcji=false;
		this.animacja="Defense";
		this.dzwiek="ShieldHit";
		float len=1;
		RuntimeAnimatorController ac = this.canimator.runtimeAnimatorController;
		foreach(AnimationClip animat in ac.animationClips){
			if(animat.name==this.animacja) {
				len=animat.length;
				szybkosc=System.Convert.ToDouble(len)/(this.koszt*GameLogicDataScript.mnoznikCzasu);
			}
		}
		//szybkosc=System.Convert.ToDouble(this.character.GetComponent<Animation>().GetClip(this.animacja).length)/(this.koszt*GameLogicDataScript.mnoznikCzasu);
	}
	
	//O;ileparwspolrzednych;x;y;x2;y2;x3;y3;koszt;iloscpol;
	public override string serializuj(){
		string s="O;";
		int ilepol=0;
		for (int i=0;i<3;i++){
			for (int j=0;j<3;j++){
				if (hitbox[i,j]==true) ilepol++;
			}
		}
		s+=ilepol.ToString()+";";
		for (int i=0;i<3;i++){
			for (int j=0;j<3;j++){
				if (hitbox[i,j]==true) {
					s+=i+";"+j+";";
				}
			}
		}
		s+=koszt.ToString()+";";
		s+=iloscPol.ToString()+";";
		return s;
	}
	
	/// <summary>
	/// Returns a <see cref="System.String"/> that represents the current <see cref="Obrona"/>.
	/// </summary>
	/// <returns>A <see cref="System.String"/> that represents the current <see cref="Obrona"/>.</returns>
	public override string ToString(){
		return "Jestem obrona ilosc pol"+iloscPol+" Gracz: "+gracz;
	}


}