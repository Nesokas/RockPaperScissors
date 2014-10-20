using UnityEngine;
using System.Collections;
using Facebook;
using GameSparks.Api.Requests;

public class MainMenu : MonoBehaviour {

	public void Awake() 
	{
		if(!FB.IsInitialized){
			FB.Init(FacebookLogin);
		} else {
			FacebookLogin();
		}
	}

	public void FacebookLogin()
	{
		if(!FB.IsLoggedIn){
			FB.Login("", GameSparksLogin);
		}
	}

	public void GameSparksLogin(FBResult result)
	{
		if(FB.IsLoggedIn){
			new FacebookConnectRequest().SetAccessToken(FB.AccessToken).Send((response) => 
			{
				if(response.HasErrors){
					Debug.Log("Something went wrong with Facebook login");
				} else {
					Debug.Log("GameSparks Facebook login successful");
				}
			});
		}
	}

	public void StartOfflineGame()
	{
		Application.LoadLevel(1);
	}
}
