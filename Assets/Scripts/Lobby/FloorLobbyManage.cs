using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FloorLobbyManage : MonoBehaviour {

	class LobbySpawns
	{
		bool avaible = true;
		NetworkInstanceId netID;
		Vector3 position;

		public void LobbySpawn(bool avaible,Vector3 position)
		{
			this.avaible=avaible;
			this.position=position;
		}
		public void set_ava(bool ava,NetworkInstanceId netID)
		{
			if(!ava)
				this.netID=netID;
			avaible=ava;
		}
		public void set_position(Vector3 position) { this.position=position; }

		public bool get_ava() { return avaible; }
		public Vector3 get_position() { return position; }
		public NetworkInstanceId get_NetID() { return netID; }


	}
	List<LobbySpawns> LobbySpawnList = new List<LobbySpawns>();
	void Start () {
		foreach(Transform child in transform.FindChild("LobbySpawnPoints"))
		{
			LobbySpawns newLS = new LobbySpawns();
			newLS.LobbySpawn(true,child.transform.position);
			LobbySpawnList.Add(newLS);
		}
	}

	public Vector3 getLobbyPosition(NetworkInstanceId netID)
	{
		for(int i = 0;(i<LobbySpawnList.Count);i++)
		{
			if(LobbySpawnList[i].get_ava())
			{
				LobbySpawnList[i].set_ava(false,netID);
				return LobbySpawnList[i].get_position();
			}
		}
		return Vector3.zero;
	}

	public void releaseSpawnPoint(NetworkInstanceId netID)
	{
		for(int i = 0;i<LobbySpawnList.Count;i++)
		{
			if(netID.Value==LobbySpawnList[i].get_NetID().Value)
			{
				LobbySpawnList[i].set_ava(true,netID);
				return;
			}
		}
		Debug.LogError("Networkid not found");
	}
}
