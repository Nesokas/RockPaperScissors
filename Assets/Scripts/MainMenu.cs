using UnityEngine;
using System.Collections;
using Facebook;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public GameObject lobby;
	public GameObject room_prefab;
	public GameObject waiting;
	public GameObject all_lobby;
	public GameObject welcome;

	public string player_name;

	void Start() {
		PhotonNetwork.ConnectUsingSettings("0.1");
	}
	
	void OnReceivedRoomListUpdate()
	{
		foreach(Transform child in lobby.transform){
			GameObject.Destroy(child.gameObject);
		}
		
		RoomInfo[] all_rooms = PhotonNetwork.GetRoomList();
		
		int i = 0;
		foreach(RoomInfo room in all_rooms) {
			if(room.playerCount < 2) {
				GameObject new_room = (GameObject)Instantiate(room_prefab, room_prefab.transform.position, room_prefab.transform.rotation);
				new_room.transform.SetParent(lobby.transform, false);
				
				new_room.GetComponent<PlayerRoom>().room_name.text = room.name;
				
				RectTransform rect_transform = new_room.GetComponent<RectTransform>();
				Vector2 new_position = rect_transform.anchoredPosition;
				new_position.y -= 48*i;
				rect_transform.anchoredPosition = new_position;
				
				RectTransform lobby_rect = lobby.GetComponent<RectTransform>();
				if(Mathf.Abs(new_position.y) > lobby_rect.rect.height) {
					Vector2 new_lobby_size = lobby_rect.sizeDelta;
					new_lobby_size.y = Mathf.Abs(new_position.y) + rect_transform.rect.height;
					lobby_rect.sizeDelta = new_lobby_size;
					Vector2 new_lobby_position = new Vector2(0,0);
					lobby_rect.offsetMax = new_lobby_position;
				}
				i++;
			}
		}
	}

	void OnJoinedLobby()
	{
		welcome.SetActive(false);
		all_lobby.SetActive(true);
	}
	
	void OnGUI()
	{
#if UNITY_EDITOR
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
#endif
	}

	public void CreateRoom()
	{
		PhotonNetwork.CreateRoom(player_name, new RoomOptions(){maxPlayers=2}, null);
		waiting.SetActive(true);
	}

	public void CancelRoom()
	{
		PhotonNetwork.LeaveRoom();
		waiting.SetActive(false);
	}

	public void JoinRandom()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	// Called by the player who created the room when another player connects to the room
	public void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		Debug.Log("Joined room: " + PhotonNetwork.room.playerCount);
		if(PhotonNetwork.room.playerCount == 2)
			Application.LoadLevel(1);
	}

	// Called by the player joining the room when he connects to the room
	public void OnJoinedRoom()
	{
		Debug.Log("Joined room: " + PhotonNetwork.room.playerCount);
		if(PhotonNetwork.room.playerCount == 2)
			Application.LoadLevel(1);
	}

}


