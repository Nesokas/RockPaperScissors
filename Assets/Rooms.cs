using UnityEngine;
using System.Collections;
using Facebook;

public class Rooms : MonoBehaviour {

	private Room[] rooms;
	private Vector2 scrollbar_position;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnGUI()
	{
		GUI.Box(new Rect(Screen.width*0.1f, Screen.height*0.25f, Screen.width*0.8f, Screen.height*0.6f), "");
		GUILayout.BeginArea(new Rect(Screen.width*0.1f, Screen.height*0.25f, Screen.width*0.8f, Screen.height*0.6f));
		scrollbar_position = GUILayout.BeginScrollView(scrollbar_position, false, true, GUILayout.Width(Screen.width*0.8f), GUILayout.Height(Screen.height*0.6f));

		foreach(RoomInfo room in PhotonNetwork.GetRoomList()) {
			GUILayout.Box(room.name);
			if(GUILayout.Button("Join"))
				PhotonNetwork.JoinRoom(room.name);
		}

		GUILayout.EndScrollView();
		GUILayout.EndArea();
	}

	public void JoinRandomRoom()
	{
		PhotonNetwork.JoinRandomRoom();
	}
	
	public void OnPhotonRandomJoinFailed()
	{
		PhotonNetwork.CreateRoom(FB.UserId);
	}

	public void CreateRoom()
	{
		PhotonNetwork.CreateRoom(FB.UserId, true, true, 2);
	}
}
