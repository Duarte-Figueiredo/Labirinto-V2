using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LobbyPlayerCustom : NetworkLobbyPlayer {
	[SyncVar(hook = "OnMyName")]
	public string playerName = "";

	private NetworkInstanceId NetID;
	private bool ready = false;

	private GameObject uiPanel;

	void Start()
	{
		DontDestroyOnLoad(this.transform);
		uiPanel=transform.FindChild("LobbyUI").gameObject;

		NetID = this.netId;
		if(!isLocalPlayer)
		{
			uiPanel.transform.FindChild("LocalPlayer").gameObject.SetActive(false);
			uiPanel.transform.FindChild("NotLocalPlayer").gameObject.SetActive(true);
		}
	}
	public override void OnClientEnterLobby()
	{
		Debug.Log("@ OnClientEnterLobby");
		base.OnClientEnterLobby();

		this.transform.position=GameObject.Find("Floor").GetComponent<FloorLobbyManage>().getLobbyPosition(this.netId);
	}

	public override void OnClientExitLobby()
	{
		Debug.Log("@ OnClientExitLobby");
		base.OnClientExitLobby();
	}

	public void OnMyName(string newName)
	{
		playerName=newName;
		//nameInput.text=playerName;//TODO
	}

	public override void OnClientReady(bool readyState)
	{
		ready=readyState;
		if(readyState)
			uiPanel.transform.FindChild("NotLocalPlayer").transform.FindChild("Status").GetComponent<Text>().text="Ready";
		else
			uiPanel.transform.FindChild("NotLocalPlayer").transform.FindChild("Status").GetComponent<Text>().text="Not Ready";
		base.OnClientReady(readyState);
	}
	public void OnDestroy()
	{
		try
		{
			GameObject.Find("Floor").GetComponent<FloorLobbyManage>().releaseSpawnPoint(NetID);
		}
		catch(System.Exception)
		{

			Debug.LogError("Error @OnDestroy");
		}
	}
}
