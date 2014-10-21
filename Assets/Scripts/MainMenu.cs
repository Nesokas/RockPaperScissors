using UnityEngine;
using System.Collections;
using Facebook;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public GameObject facebook_login;
	public GameObject lobby;

	public GameObject debug_text_object;
	private Text debug_text;

	void Awake()
	{
		if(!FB.IsInitialized){
			FB.Init(SetInit);
		} else {
			SetInit();
		}

		debug_text = (Text)debug_text_object.GetComponent("Text");
	}

	private void SetInit()
	{
		enabled = true;
		if (FB.IsLoggedIn)
		{
			Debug.Log("SetInit()");
			OnLoggedIn();
		}
	}

	void Update()
	{
		debug_text.text = PhotonNetwork.connectionStateDetailed.ToString();
		debug_text.text = "Update";
		if(PhotonNetwork.connectionStateDetailed == PeerState.JoinedLobby){
			Debug.Log("joined lobby");
			facebook_login.SetActive(false);
			lobby.SetActive(true);
		}
	}

	private void OnHideUnity(bool isGameShown)
	{
		Debug.Log("OnHideUnity()");
	} 

	void LoginCallback(FBResult result)
	{
		if (FB.IsLoggedIn)
		{
			OnLoggedIn();
		}
	}

	public void StartOfflineGame()
	{
		Application.LoadLevel(1);
	}

	public void LoginFacebook()
	{
		Debug.Log("Button Pressed");
		FB.Login("email", LoginCallback);
	}

	void OnLoggedIn()
	{
		PhotonNetwork.AuthValues = new AuthenticationValues();
		PhotonNetwork.AuthValues.AuthType = CustomAuthenticationType.Facebook;
		PhotonNetwork.AuthValues.SetAuthParameters(FB.UserId, FB.AccessToken);
		PhotonNetwork.ConnectUsingSettings("0.1");
	}
}


