using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkSpawnersHandler : NetworkBehaviour {

	[SyncVar]
	public int i=3;
	private Vector3[] Spawn_Pos;
	void Start () {
		this.name = "NetworkSpawners";
		Spawn_Pos = new Vector3[4];
	
		int j = 0;
		foreach (Transform child in transform) 
			Spawn_Pos [j++] = child.transform.position;	
	}
	
	public Vector3 Get_Spawn()
	{	
		if (i < 0)
			return Vector3.zero;
		return Spawn_Pos [i--];
	}
}
