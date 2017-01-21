using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Text;
using System;

public class NetworkMenu : MonoBehaviour {

	public string connectionIP = "127.0.0.1";
	public int portNumber = 8632;
	string plikOstatniegoPolaczenia="last.con";
	public Texture texture;
	Rect rectangle, rectangle2;
	//static bool server=false;
	bool polaczenie;
	bool blad=false;
	bool klik=false;
	bool odlaczonoOdSerwera=false;

	void Start(){
		rectangle=Rect.MinMaxRect(Screen.width/2-100,Screen.height/2-100,Screen.width/2+100,Screen.height/2+100);
		rectangle2=Rect.MinMaxRect(Screen.width/2-120,Screen.height/2-120,Screen.width/2+120,Screen.height/2+120);
		Network.maxConnections=1;
		if(System.IO.File.Exists(plikOstatniegoPolaczenia)){
			System.IO.StreamReader sr = new System.IO.StreamReader(plikOstatniegoPolaczenia, Encoding.UTF8);   //otworzenie pliku, ustawienie kodowania
			while (!sr.EndOfStream) //do konca strumienia
			{
				connectionIP = sr.ReadLine();
				portNumber = System.Convert.ToInt32(sr.ReadLine());
			}
			sr.Close();
		}
		if (GameLogicDataScript.multiplayer==false) gameObject.GetComponent<NetworkMenu>().enabled=false;
		polaczenie = false;
	}

	void Update(){
		if (!polaczenie) GameLogicDataScript.polaczono=false;
		if (polaczenie) {
			System.IO.StreamWriter sr = new System.IO.StreamWriter(plikOstatniegoPolaczenia); //otworzenie pliku do zapisu
			sr.WriteLine(connectionIP);
			sr.WriteLine(portNumber);
			sr.Close();
			if (Network.connections.Length!=0) GameLogicDataScript.polaczono=true;
		}
		if (GameLogicDataScript.polaczono==true&&Network.connections.Length==0) {
			odlaczonoOdSerwera=true;
			polaczenie=false;
		}

	}


	// Use this for initialization
	private void OnConnectedToServer(){
		polaczenie=true;
	}

	private void OnServerInitialized(){
		polaczenie=true;
		//server=true;
	}

	private void OnDisconnectedFromServer(){
		odlaczonoOdSerwera=true;
		polaczenie=false;
	}

	private void PlayerDisconnected(){
		odlaczonoOdSerwera=true;
		polaczenie=false;
	}

	private void OnFailedToConnect(NetworkConnectionError error){
		if (error==NetworkConnectionError.NoError){
			print("Udało się");
			blad=false;
		}
		else{
			print("BŁOND");
			blad=true;
		}
		print(error.ToString());
	}

	private void OnGUI(){
		
		if (!polaczenie){
			if (odlaczonoOdSerwera){
				GUI.DrawTexture(rectangle2,texture);
				GUILayout.BeginArea(rectangle);
				GUILayout.Label("Stracono połączenie.");
				if(GUILayout.Button("Wróć do menu")){
					odlaczonoOdSerwera=true;
					GameLogicDataScript.resetVariables();
					UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
				}
				GUILayout.EndArea();
			}
			else{
				GUI.DrawTexture(rectangle2,texture);
				GUILayout.BeginArea(rectangle);
				connectionIP = GUILayout.TextField(connectionIP);
				int.TryParse(GUILayout.TextField(portNumber.ToString()), out portNumber);

				if(GUILayout.Button("Dołącz")) {
					blad=false;
					klik=true;
					Network.Connect(connectionIP,portNumber);
				}

				if(GUILayout.Button("Załóż grę")) Network.InitializeServer(4,portNumber,false);
				if (klik&&!blad) GUILayout.Label("Łączenie...");
				if (blad) {
					GUILayout.Label("Błąd połączenia.\nSprawdź adres i numer portu.");
					klik=false;
				}
				GUILayout.EndArea();
			}
		}
		else {
			blad=false; klik=false; odlaczonoOdSerwera=false;
			//GUILayout.Label("Połączeń: " + Network.connections.Length.ToString());
		}
	}

}
