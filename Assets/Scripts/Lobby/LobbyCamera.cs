using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LobbyCamera : MonoBehaviour {

	private NetworkLobbyManager nLM;
	[Range(0,1)]
	public float speed;
	private bool networkActive= false,translating;
	private Vector3 homePos,lobbyPos;
	private Vector3 startPos, finalPos;
	private float t = 0;
	void Start()
	{
		nLM=GameObject.Find("NetworkManager").GetComponent<CustomLobby>();
		homePos=new Vector3(20,180,0);
		lobbyPos=new Vector3(20,0,0);
	}
	void FixedUpdate()
	{
		if(nLM.isNetworkActive&&!networkActive)
			setToRotate(homePos,lobbyPos);
		else if(!nLM.isNetworkActive&&networkActive)
			setToRotate(lobbyPos,homePos);


		if(translating)
			RotateTo();
	}

	void RotateTo()
	{
		if(t>=1)
		{
			t=0;
			translating=false;
			this.transform.rotation=Quaternion.Euler(finalPos);
			return;
		}
		this.transform.rotation=Quaternion.Lerp(
			Quaternion.Euler(startPos),
			Quaternion.Euler(finalPos),
			t);
		t+=speed;
	}
	void setToRotate(Vector3 pStart,Vector3 pFinal)
	{
		networkActive=nLM.isNetworkActive;
		startPos=pStart;
		finalPos=pFinal;
		translating=true;
	}
}
