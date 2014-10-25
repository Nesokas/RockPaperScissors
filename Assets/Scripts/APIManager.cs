using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Facebook;
using PlayFab;
using PlayFab.ClientModels;

public class APIManager : MonoBehaviour {

	public bool has_logged_in_before = false;
	public string playfab_id;
	bool try_reconnect = false;
	public string last_room_connected;

	public bool is_logged_in_playfab = false;
	public bool is_logged_in_photon = false;

	void Awake()
	{
		PhotonNetwork.ConnectUsingSettings("0.1");
		if(FB.IsInitialized) {
			FacebookLogin();
		}

		DontDestroyOnLoad(this);
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
		NotificationCenter.DefaultCenter.PostNotification(this, "TryingGameFabLogin");
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

	}

	void OnLoginResult(LoginResult result)
	{
		PlayFabData.AuthKey = result.SessionTicket;
		playfab_id = result.PlayFabId;
		is_logged_in_playfab = true;
		has_logged_in_before = true;
	}

	void OnPlayFabError(PlayFabError error)
	{
		Debug.Log ("Got an error: " + error.Error);
	}

	void Update()
	{
		if(try_reconnect) {
			PhotonNetwork.ConnectUsingSettings("0.1");
			try_reconnect = false;
		}
	}

	void OnJoinedLobby()
	{
		is_logged_in_photon = true;
		if(has_logged_in_before)
			FacebookLogin();
	}

	void OnGUI()
	{
		#if UNITY_EDITOR
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
		#endif
	}

	void OnDisconnectedFromPhoton()
	{
		Debug.Log("Disconnected from Photon"); 
		try_reconnect =true;

		is_logged_in_photon = false;
		is_logged_in_playfab = false;
	}

}
