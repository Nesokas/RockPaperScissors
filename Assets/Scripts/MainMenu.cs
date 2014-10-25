using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using Facebook;
using PlayFab;
using PlayFab.ClientModels;

public class MainMenu : MonoBehaviour {

	public GameObject lobby;
	public GameObject room_prefab;
	public GameObject waiting;
	public GameObject all_lobby;
	public GameObject welcome;
	public GameObject login;

	public string player_name;
	bool is_playfab_login;
	bool is_photon_login;
	string playfab_id;
	bool has_logged_in_before;

	bool try_reconnect = false;

	void Awake() {
		is_playfab_login = false;
		is_photon_login = false;
		PhotonNetwork.ConnectUsingSettings("0.1");
		if(FB.IsInitialized) {
			FacebookLogin();
		}
	}

	public void InitializeFacebook()
	{
		FB.Init(FacebookLogin);
	}

	void FacebookLogin()
	{
		if(!FB.IsLoggedIn){
			FB.Login("", PlayFabLogin);
		} else {
			PlayFabLogin(null);
		}
		login.SetActive(false);
		welcome.SetActive(true);
	}

	void PlayFabLogin(FBResult result)
	{
		if(FB.IsLoggedIn){
			LoginWithFacebookRequest request = new LoginWithFacebookRequest();
			request.AccessToken = FB.AccessToken;
			request.TitleId = PlayFabData.TitleId;
			request.CreateAccount = true;
			PlayFabClientAPI.LoginWithFacebook(request, OnLoginResult, OnPlayFabError);
		}
		has_logged_in_before = true;
	}

	void OnLoginResult(LoginResult result)
	{
		PlayFabData.AuthKey = result.SessionTicket;
		is_playfab_login = true;
		playfab_id = result.PlayFabId;
	}

	void OnPlayFabError(PlayFabError error)
	{
		Debug.Log ("Got an error: " + error.Error);
	}

	void Update()
	{
		if(is_photon_login && is_playfab_login && Application.loadedLevelName != "Game") {
			all_lobby.SetActive(true);
			welcome.SetActive(false);
			login.SetActive(false);
		}

		if(try_reconnect) {
			PhotonNetwork.ConnectUsingSettings("0.1");
			try_reconnect = false;
		}
	}

	void InstanciateRoom(RoomInfo room, int index, UserFacebookInfo facebook)
	{
		GameObject new_room = (GameObject)Instantiate(room_prefab, room_prefab.transform.position, room_prefab.transform.rotation);
		new_room.transform.SetParent(lobby.transform, false);
		
		new_room.GetComponent<PlayerRoom>().room_name.text = facebook.FacebookUsername;
		new_room.GetComponent<PlayerRoom>().room_id = room.name;
		StartCoroutine(new_room.GetComponent<PlayerRoom>().GetFacebookPicture(facebook.FacebookId));

		RectTransform rect_transform = new_room.GetComponent<RectTransform>();
		Vector2 new_position = rect_transform.anchoredPosition;
		new_position.y -= 48*index;
		rect_transform.anchoredPosition = new_position;
		
		RectTransform lobby_rect = lobby.GetComponent<RectTransform>();
		if(Mathf.Abs(new_position.y) > lobby_rect.rect.height) {
			Vector2 new_lobby_size = lobby_rect.sizeDelta;
			new_lobby_size.y = Mathf.Abs(new_position.y) + rect_transform.rect.height;
			lobby_rect.sizeDelta = new_lobby_size;
			Vector2 new_lobby_position = new Vector2(0,0);
			lobby_rect.offsetMax = new_lobby_position;
		}
	}

	int i = 0;
	Dictionary<RoomInfo,int> rooms = new Dictionary<RoomInfo,int>();
	void OnReceivedRoomListUpdate()
	{
		foreach(Transform child in lobby.transform){
			GameObject.Destroy(child.gameObject);
		}
		
		RoomInfo[] all_rooms = PhotonNetwork.GetRoomList();

		foreach(RoomInfo key in rooms.Keys){
			rooms.Remove(key);
		}
		rooms.Clear();
		
		i = 0;
		foreach(RoomInfo room in all_rooms) {
			if(room.playerCount < 2) {
				Debug.Log("new room request: " + room.name);
				GetAccountInfoRequest request = new GetAccountInfoRequest();
				request.PlayFabId = room.name;
				rooms.Add(room, i);
				PlayFabClientAPI.GetAccountInfo(request, CreateNewRoom, OnPlayFabError);
				i++;
			}
		}
	}

	void CreateNewRoom(GetAccountInfoResult result)
	{
		Debug.Log("creating new room: " + result.AccountInfo.Username);
		foreach(KeyValuePair<RoomInfo, int> room in rooms){
			if(room.Key.name == result.AccountInfo.PlayFabId) {
				InstanciateRoom(room.Key, room.Value, result.AccountInfo.FacebookInfo);
				break;
			}
		}
	}

	void OnJoinedLobby()
	{
		is_photon_login = true;
		welcome.SetActive(false);
		if(has_logged_in_before){
			all_lobby.SetActive(true);
			FacebookLogin();
		} else {
			login.SetActive(true);
		}
	}
	
	void OnGUI()
	{
#if UNITY_EDITOR
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
#endif
	}

	public void CreateRoom()
	{
		PhotonNetwork.CreateRoom(playfab_id, new RoomOptions(){maxPlayers=2}, null);
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

	void OnConnectionFail(DisconnectCause cause)
	{
		Debug.Log(cause.ToString());
	}

	void OnDisconnectedFromPhoton()
	{
		Debug.Log("Disconnected from Photon");
		try_reconnect =true;
		all_lobby.SetActive(false);
		login.SetActive(false);
		welcome.SetActive(true);
		is_photon_login = false;
		is_playfab_login = false;
	}

	void OnFailedToConnectToPhoton(DisconnectCause cause)
	{
		Debug.Log(cause.ToString());
	}

}


