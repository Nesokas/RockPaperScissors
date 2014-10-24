using UnityEngine;
using System.Collections;
using Facebook;
using GameSparks.Api.Requests;

public class MainMenu : MonoBehaviour {

	public void Awake() 
	{
		CallFBInit();
	}

	private void CallFBInit()
	{
		FB.Init(OnInitComplete, OnHideUnity);
	}

	private void OnInitComplete()
	{
		Debug.Log("FB.Init completed: is user logged in? " + FB.IsLoggedIn);
		CallFBLoginIn();
	}

	private void OnHideUnity(bool is_game_shown)
	{
		Debug.Log("Is game shownig? " + is_game_shown);
	}

	private void CallFBLoginIn()
	{
		FB.Login("email,user_friends", GameSparksLogin);
	}

	private void GameSparksLogin(FBResult result)
	{
		if(FB.IsLoggedIn) {
			new FacebookConnectRequest().SetAccessToken(FB.AccessToken).Send ((response) =>
			{
				if(response.HasErrors)
					Debug.Log("Something went wrong when logging to facebook: " + result.Error);
				else {
					Debug.Log("Gamesparks facebook login succeful.");
					UserManager.instance.UpdateInformation();
					this.GetComponent<PlayersManager>().GetOpponents();
				}
			});
		}
	}

	public void ExitGame()
	{
		Application.Quit();
	}
}
