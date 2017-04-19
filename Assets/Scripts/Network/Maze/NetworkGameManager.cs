using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkGameManager : NetworkBehaviour {

	public NetworkMaze mazePrefab;
	public GameObject spawners;

	private NetworkMaze mazeInstance;

	private Quaternion camera_spawn_rotation;

	private Vector3 playerPos;

	private void Start () {
		//StartCoroutine(BeginGame());

		if (isServer)
			BeginGame ();
	}
	private void Update () {
		if (Input.GetKeyDown(KeyCode.Space)) {
			//RestartGame();
		}
	}

	private void BeginGame () {
		Camera.main.clearFlags = CameraClearFlags.Skybox;
		Camera.main.rect = new Rect (0f, 0f, 1f, 1f);

		mazeInstance = Instantiate(mazePrefab) as NetworkMaze;
		NetworkServer.Spawn (mazeInstance.gameObject);
		mazeInstance.Generate ();
		//yield return StartCoroutine(mazeInstance.Generate());

		//playerPos = mazeInstance.start.transform.position;

		//Destroy (GameObject.Find ("First_Spawners"));
		GameObject spawn = Instantiate (spawners,GameObject.Find("StartCell").transform.position,Quaternion.identity) as GameObject;
		NetworkServer.Spawn (spawn);
		playerPos.y += 0.0125f;

		//cmdspawn_player (playerPos, playerRot);
		//playerPos.y += 0.0125f;
		//carInstance = Instantiate (carPrefab,playerPos,playerRot) as CarMovement;


		//playerInstance = Instantiate (playerPrefab) as Player;
		//playerInstance.SetLocation (mazeInstance.GetCell (mazeInstance.RandomCoordinates));//tenho de mudar isto

		/*Camera.main.clearFlags = CameraClearFlags.Depth;
		Camera.main.rect = new Rect(-0.04f,0.7f,0.25f,0.25f);
		Camera.main.transform.SetParent (carInstance.gameObject.transform);
		Camera.main.transform.localPosition = new Vector3 (0, 200.25f, 0);*/
	}
	 
}