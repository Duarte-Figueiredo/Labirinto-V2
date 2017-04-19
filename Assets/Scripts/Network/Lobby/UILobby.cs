using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class UILobby : MonoBehaviour {

	private CustomLobby nLM;
	private Button leave;

	void Start ()
	{
		nLM=GameObject.Find("NetworkManager").GetComponent<CustomLobby>();
		leave =transform.FindChild("ButtonLeaveLobby").GetComponent<Button>();

		SetAllButtons();
	}

	void SetAllButtons()
	{
		Debug.Log("@ UI SetAllButtons");

		leave.onClick.RemoveAllListeners();

		if(nLM.isHost)
			SetButtonsHost();
		else if(nLM.isClient)
			SetButtonsClient();
		else if(nLM.isServer)
			SetButtonsServer();

	}

	void SetButtonsHost()
	{
		leave.onClick.AddListener(nLM.StopHost);
	}

	void SetButtonsClient()
	{
		leave.onClick.AddListener(nLM.StopClient);
	}

	void SetButtonsServer()
	{ 
		leave.onClick.AddListener(nLM.StopServer);
	}
}
