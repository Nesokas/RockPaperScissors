using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerRoom : MonoBehaviour {

	public Text room_name;
	public Image profile_picture;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void JoinRoom()
	{
		PhotonNetwork.JoinRoom(room_name.text, false);
	}
}
