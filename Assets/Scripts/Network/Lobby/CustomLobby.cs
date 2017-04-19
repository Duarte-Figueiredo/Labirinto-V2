using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CustomLobby : NetworkLobbyManager {
	public string scene_name { get; private set; }
	public bool isHost=false, isClient=false, isServer=false;

	void OnEnable()
	{
		//Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
		SceneManager.sceneLoaded+=OnLevelFinishedLoading;
	}

	void OnDisable()
	{
		//Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
		SceneManager.sceneLoaded-=OnLevelFinishedLoading;
	}

	void OnLevelFinishedLoading(Scene scene,LoadSceneMode mode)
	{
		scene_name=scene.name;
		Debug.Log("@ OnLevelFinishedLoading Level:"+scene.name);
		if(scene.name.Equals("Lobby"))
		{
			SetupMenuSceneButtons();
		}
	}

	void SetupMenuSceneButtons()
	{
		//Debug.Log("@ SetupMenuSceneButtons");
		GameObject.Find("ButtonHost").GetComponent<Button>().onClick.RemoveAllListeners();
		GameObject.Find("ButtonHost").GetComponent<Button>().onClick.AddListener(StartUpHost);

		GameObject.Find("ButtonJoin").GetComponent<Button>().onClick.RemoveAllListeners();
		GameObject.Find("ButtonJoin").GetComponent<Button>().onClick.AddListener(JoinGame);
	}

	public void StartUpHost()
	{
		Debug.Log("@ StartUpHost");
		isHost=true;
		SetPort();

		NetworkManager.singleton.StartHost();
	}

	public void JoinGame()
	{
		Debug.Log("@ JoinGame");
		isClient=true;
		SetIpAdress();
		SetPort();

		NetworkManager.singleton.StartClient();
	}
	void SetPort()
	{
		NetworkManager.singleton.networkPort=7777;
	}
	void SetIpAdress()
	{
		string ip = "localhost";// GameObject.Find("IpAdress").GetComponent<Text>().text;
		NetworkManager.singleton.networkAddress=ip;
	}

	public override void OnServerAddPlayer(NetworkConnection conn,short playerControllerId)
	{
		Debug.Log("@ OnServerAddPlayer");
		base.OnServerAddPlayer(conn,playerControllerId);
	}
	public override GameObject OnLobbyServerCreateLobbyPlayer(NetworkConnection conn,short playerControllerId)
	{
		Debug.Log("@ OLSCLobbyPlayer");
		return base.OnLobbyServerCreateLobbyPlayer(conn,playerControllerId);
	}

	public override GameObject OnLobbyServerCreateGamePlayer(NetworkConnection conn,short playerControllerId)//sets up player object whem game starts
	{
		Debug.Log("@ OLSCGamePlayer");
		return base.OnLobbyServerCreateGamePlayer(conn,playerControllerId);
	}

	public override void OnStopHost()
	{
		isHost=false;
		base.OnStopHost();
	}
	public override void OnStopClient()
	{
		isClient=false;
		base.OnStopClient();
	}
	public override void OnStopServer()
	{
		isServer=false;
		base.OnStopServer();
	}

	public override bool OnLobbyServerSceneLoadedForPlayer(GameObject lobbyPlayer,GameObject gamePlayer)
	{
		/* CHANGE LOBBY PLAYER SETTINGS HERE FOR ALL PLAYERS
		var cc = lobbyPlayer.GetComponent<ColorControl>();
		var player = gamePlayer.GetComponent<Player>();
		player.myColor=cc.myColor;
		return true;*/
		return base.OnLobbyServerSceneLoadedForPlayer(lobbyPlayer,gamePlayer);
	}
}
