using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
//using UnityStandardAssets.Utility;

[RequireComponent (typeof(Rigidbody))]
public class NetworkCarMovement : NetworkBehaviour {
	public float aceleration,maxSpeed;
	public float del_start;

	private Rigidbody rg;
	void Start () {
		transform.FindChild ("Camera").gameObject.SetActive (false);
		//GetComponent<SimpleMouseRotator> ().enabled = false;

		rg = gameObject.GetComponent<Rigidbody> ();
		Invoke ("Delay_Start", del_start);


	}
	private void Delay_Start()
	{
		Debug.Log("@ NetworkCarMovement");
		if (!isLocalPlayer)
			return;
		Debug.Log ("IM LOCAL PLAYER");
		transform.FindChild ("Camera").gameObject.SetActive (true);
		//GetComponent<SimpleMouseRotator> ().enabled = true;
		this.transform.position = GameObject.Find ("NetworkSpawners").GetComponent<NetworkSpawnersHandler> ().Get_Spawn ();
	}
	// Update is called once per frame
	void Update ()	
	{
		if (!isLocalPlayer)
			return;
		
		//Debug.Log (rg.velocity.magnitude);
		if((Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.W)) && rg.velocity.magnitude < maxSpeed) {
			//transform.position += transform.forward * Time.deltaTime * aceleration;
			gameObject.GetComponent<Rigidbody>().AddForce(
				new Vector3(transform.forward.x,
					0,
					transform.forward.z)
				* aceleration);
		}
		else if(Input.GetAxis("Vertical")<0|| Input.GetKey(KeyCode.S)) {
			gameObject.GetComponent<Rigidbody>().AddForce(
				new Vector3(transform.forward.x,
					0,
					transform.forward.z)
				* -aceleration);
		}
		else if(Input.GetKey(KeyCode.A)) {
		}
		else if(Input.GetKey(KeyCode.D)) {

		}
			
	}
}
