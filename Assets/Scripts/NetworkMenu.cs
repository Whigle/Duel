using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkMenu : MonoBehaviour {

	public string connectionIP = "127.0.0.1";
	public int portNumber = 8632;

	bool polaczenie;

	void Start(){
		if (GameLogicDataScript.multiplayer==false) gameObject.GetComponent<NetworkMenu>().enabled=false;
		polaczenie = false;
	}

	void Update(){
		if (polaczenie&&(Network.connections.Length>0)) GameLogicDataScript.polaczono=true;
	}

	// Use this for initialization
	private void OnConnectedToServer(){
		polaczenie=true;
	}

	private void OnServerInitialized(){
		polaczenie=true;
	}

	private void OnDisconnectedFromServer(){
		polaczenie=false;
	}

	private void OnGUI(){
		if (!polaczenie){
			connectionIP = GUILayout.TextField(connectionIP);
			int.TryParse(GUILayout.TextField(portNumber.ToString()), out portNumber);

			if(GUILayout.Button("Dołącz")) Network.Connect(connectionIP,portNumber);

			if(GUILayout.Button("Załóż grę")) Network.InitializeServer(4,portNumber,true);
		}
		else GUILayout.Label("Połączeń: " + Network.connections.Length.ToString());
	}

}
