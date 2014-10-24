using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Requests;
using System;

public class ChallengeManager : MonoBehaviour {

	public static ChallengeManager instance;

	public GameObject invite;
	public Text challenge_wating;
	public GameObject challenge_waiting_obj;
	public GameObject canvas;

	public bool is_challenge_running = false;
	public string challenge_id;
	public List<string> players_id = new List<string>(); 
	public List<string> players_name = new List<string>();
//	private List<string> players_FB_id = new List<string>();

	private bool waiting_for_challenges = true;


	// Use this for initialization
	void Start () {
		instance = this;
		DontDestroyOnLoad(transform.gameObject);
	}

	bool waiting_accept_challenge = false;

	public IEnumerator GetChallengeInvites()
	{
		if(!waiting_accept_challenge) {
			new ListChallengeRequest().SetShortCode("NewGame").SetState("RECEIVED").SetEntryCount(1).Send((response) => 
			                                                                                               {
				foreach(var challenge in response.ChallengeInstances) {
					invite.SetActive(true);

					invite.GetComponent<GameInvite>().challengeId = challenge.ChallengeId;

					invite.GetComponent<GameInvite>().invite_name = challenge.Challenger.Name;
					invite.GetComponent<GameInvite>().invite_expire = challenge.EndDate.ToString();

					waiting_accept_challenge = true;
				}

			});
			StartCoroutine(RemoveInvite());
		}
		yield return new WaitForSeconds(5);

		if(!waiting_accept_challenge)
			waiting_for_challenges = true;

	}

	IEnumerator RemoveInvite()
	{
		yield return new WaitForSeconds(10);
		if(invite != null) {
			new DeclineChallengeRequest().SetChallengeInstanceId(invite.GetComponent<GameInvite>().challengeId)
				.Send((response) => {
					if(response.HasErrors)
						Debug.Log("Couldn't decline the challenge: " + response.Errors);
				});
			invite.SetActive(false);
			waiting_accept_challenge = false;
			waiting_for_challenges = true;
		}
	}

	public IEnumerator ChallengeUser(List<string> userIds, string player_name)
	{
		waiting_for_challenges = false;
		new CreateChallengeRequest().SetChallengeShortCode("NewGame")
			.SetEndTime(System.DateTime.Today.AddDays(1))
			.SetUsersToChallenge(userIds)
			.Send((response) => 
				{
					if(response.HasErrors) {
						Debug.Log("Could not create challenge: " + response.Errors.ToString());
					} else {
						challenge_id = response.JSONData["challengeInstanceId"].ToString();
						challenge_waiting_obj.SetActive(true);
						challenge_wating.text = "Wainting for " + player_name + " to accept your challenge";
					}
				});


		yield return new WaitForSeconds(10);

		challenge_waiting_obj.SetActive(false);
		yield return new WaitForSeconds(0.2f);
		challenge_waiting_obj.SetActive(true);

		new WithdrawChallengeRequest().SetChallengeInstanceId(challenge_id)
			.Send((response2) =>
			      {
				if(response2.HasErrors){
					Debug.Log("Could not withdraw challenge: " + response2.Errors.ToString());
				}
			});
		challenge_wating.text = player_name + " didn't accept your challenge";
		yield return new WaitForSeconds(3);
		challenge_waiting_obj.SetActive(false);
		waiting_for_challenges = true;

	}

	public void GetRunningChallenges()
	{
		new ListChallengeRequest().SetShortCode("NewGame")
			.SetState("RUNNING")
			.SetEntryCount(1)
			.Send((response) =>
				{
					foreach(var challenge in response.ChallengeInstances) {
						is_challenge_running = true;
						challenge_id = challenge.ChallengeId;
						foreach(var player in challenge.Accepted) {
							players_id.Add(player.Id);
							players_name.Add(player.Name);
							Debug.Log(player.JSONData);
						}
						Application.LoadLevel(1);
						has_game_started = true;
					}
				});
	}

	public void Update()
	{
		if(is_challenge_running && Application.loadedLevelName == "Game") {
			is_challenge_running = false;
			GameObject gamecontroller = GameObject.FindGameObjectWithTag("GameController");
			RockPaperScissor rock_paper_scissor = gamecontroller.GetComponent<RockPaperScissor>();

			rock_paper_scissor.players_id = players_id;
			rock_paper_scissor.players_name = players_name;
			rock_paper_scissor.challenge_id = challenge_id;

			rock_paper_scissor.StartGame();

		} else if (Application.loadedLevelName != "Game" && waiting_for_challenges){
			waiting_for_challenges = false;
			StartCoroutine(GetChallengeInvites());
		}

		if(has_game_started && Application.loadedLevelName == "Game"){
			has_game_started = false;
			GameHasStarted();
		}
	}

	public bool has_game_started = false;

	public void GameHasStarted()
	{
		GameObject gamecontroller = GameObject.FindGameObjectWithTag("GameController");
		RockPaperScissor rock_paper_scissor = gamecontroller.GetComponent<RockPaperScissor>();

		rock_paper_scissor.GetTurnInfo();
	}
	
}
