using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Requests;
using System.Text.RegularExpressions;

public class RockPaperScissor : MonoBehaviour {

	private enum MOVE {
		ROCK,
		PAPER,
		SCISSOR,
		NEITHER
	};

	private enum State {
		choose_move,
		play_animation,
		show_result,
		idle,
		end_game,
		wait_opponent
	};
	
	private enum Result {
		win,
		lose,
		draw
	}

	struct PlayerData {
		public string id;
		public bool ready;
		public MOVE move;
		public int score;
	};

	public List<string> players_name = new List<string>();
	public List<string> players_id = new List<string>();
	public List<string> players_FB_id = new List<string>();

	public Text player_name;
	public Text opponent_name;
	public Image player_picture;
	public Image opponent_picture;

	public GameObject player_ready;
	public GameObject opponent_ready;

	public string challenge_id;
	public string challenge_turn;

	private float start_time;
	private int rest_seconds;
	public Text text_timer;
	private bool stop_timer;

	private MOVE player_move;
	private MOVE opponent_move;

	public GameObject player;
	public GameObject opponent;
	public GameObject choose_your_move;
	
	public GameObject outcome_obj;
	public GameObject outcome_text_obj;
	
	public Sprite rock;
	public Sprite paper;
	public Sprite scissor;
	
	public Sprite star_filled;
	
	public GameObject[] player_stars_obj;
	public GameObject[] opponent_stars_obj;
	
	public GameObject end_game_panel;
	public GameObject end_game_win;
	public GameObject end_game_loose;
	
	public GameObject timer;
	public int turn_time;
	
	public Image[] player_stars;
	public Image[] opponent_stars;
	
	private int player_score;
	public Animator player_animator;
	private int opponent_score;
	public Animator opponent_animator;
	
	public Animator canvas_animator;

	private bool gameOver = false;
	private State game_state;

	private bool waiting_opponents_response = false;

	private PlayerData player_data = new PlayerData();
	private PlayerData opponent_data = new PlayerData();

	void Start()
	{
		game_state = State.idle;
	}

	// Use this for initialization
	public void StartGame() {
		if(UserManager.instance.user_id == players_id[0]) {
			player_name.text = players_name[0];
			opponent_name.text = players_name[1];

//			StartCoroutine(GetFBPicture(players_FB_id[0], player_picture));
//			StartCoroutine(GetFBPicture(players_FB_id[1], opponent_picture));

		} else {
			player_name.text = players_name[1];
			opponent_name.text = players_name[0];

//			StartCoroutine(GetFBPicture(players_FB_id[1], player_picture));
//			StartCoroutine(GetFBPicture(players_FB_id[0], opponent_picture));
		}
		game_state = State.idle;

	}

	public void GetTurnInfo()
	{
		new GetChallengeRequest().SetChallengeInstanceId(challenge_id)
			.Send((response) => {
				//challenge_turn = response.Challenge.JSONData["turn"];
				game_state = State.choose_move;

				string json = response.Challenge.ScriptData.JSON;

				GetInformationFromJSONString(json);
			});
	}

	public void GetInformationFromJSONString(string JSON)
	{
		char[] delimiters = {'[', ']'};
		string[] data = JSON.Split(delimiters);

		char[] delimiters2 = {'{', '}'};
		string[] data2 = data[1].Split(delimiters2);

		char[] delimiters3 = {',', ':'};
		string[] p1_string = data2[1].Split(delimiters3);
		string[] p2_string = data2[3].Split(delimiters3);

		PlayerData p1 = StringToPlayerData(p1_string);
		PlayerData p2 = StringToPlayerData(p2_string);

		Debug.Log("----------------------------   ID:" + p1.id + ", READY:" + p1.ready + ", MOVE:" + p1.move + ", SCORE:" + p1.score);
	}

	/* string[] example: ["id","xxxxxxxxxxxxx","ready","true","move","1","score","0"]*/
	PlayerData StringToPlayerData(string[] player_string)
	{
		PlayerData p = new PlayerData();
		p.id = player_string[1];
		if(player_string[3] == "true")
			p.ready = true;
		else
			p.ready = false;
		
		switch(player_string[5]) {
		case "0":
			p.move = MOVE.ROCK;
			break;
		case "1":
			p.move = MOVE.PAPER;
			break;
		case "2":
			p.move = MOVE.SCISSOR;
			break;
		default:
			p.move = MOVE.NEITHER;
			break;
		}
		
		p.score = int.Parse(player_string[7]);

		return p;
	}

	public IEnumerator GetFBPicture(string facebookId, Image profile_picture)
	{
		var www = new WWW("http://graph.facebook.com/" + facebookId + "/picture?width=210&height=210");
		
		yield return www;
		
		Texture2D new_profile_picture = new Texture2D(25,25);
		www.LoadImageIntoTexture(new_profile_picture);
		profile_picture.sprite = Sprite.Create(new_profile_picture, new Rect(0,0,new_profile_picture.width, new_profile_picture.height), new Vector2(0.5f, 0.5f));
	}

	// After the animation of the popup, when the player chooses a move on that popup, this function will be called
	public void PlayerChoose(int player_choice)
	{
		stop_timer = true;
		switch(player_choice){
		case 0:
			player_move = MOVE.ROCK;
			break;
		case 1:
			player_move = MOVE.PAPER;
			break;
		case 2:
			player_move = MOVE.SCISSOR;
			break;
		}

		 /* 
		  * For offline games
		  * 
		  * // random opponent decision
		  * Array possible_moves = Enum.GetValues(typeof(MOVE));
		  * System.Random random = new System.Random();
		  * opponent_move = (MOVE)possible_moves.GetValue(random.Next(possible_moves.Length));
		  * 
		  * //choose_your_move.SetActive(false); // disable menu
		  * canvas_animator.SetBool("Hide", true);
		  * 
		  * player_animator.SetBool("hand_move", true); // start player and opponent animations
		  * opponent_animator.SetBool("hand_move", true);
		  * 
		  * game_state = State.play_animation; // change game state
		  * 
		  */

		new LogChallengeEventRequest().SetChallengeInstanceId(challenge_id)
			.SetEventKey("MakeMove")
			.SetEventAttribute("move", player_choice)
			.Send((response) => {});

		game_state = State.wait_opponent;
		waiting_opponents_response = false;
	}
	
	// Show window to choose player move
	void ChooseMove ()
	{
		//choose_your_move.SetActive(true);
		canvas_animator.SetBool("Hide", false);
	}

	void GetOpponentsResponse()
	{
		waiting_opponents_response = true;
		new GetChallengeRequest().SetChallengeInstanceId(challenge_id)
			.Send((response) => {
				challenge_turn = response.ScriptData.GetString("turn");
			});
	}

	void CountDown()
	{
		if(!stop_timer){
			float time = Time.time - start_time;
			rest_seconds = (int)(turn_time - time); // 10 seconds turn;
			
			text_timer.text = rest_seconds.ToString();
			
			if(rest_seconds <= 0){
				stop_timer = true;
				game_state = State.idle;
				canvas_animator.SetBool("Hide", true);
				//StartCoroutine(ShowOutcome(Result.lose));
			}
		} else {
			text_timer.text = "";
		}
	}

	void Update () {
		
		switch(game_state) {
		case State.choose_move:
			ChooseMove();
			break;
		case State.play_animation:
			StartCoroutine(PlayAnimation());
			game_state = State.idle;
			break;
		case State.show_result:
			//ShowResults();
			break;
		case State.idle: //don't do nothing just wait
			break;
		case State.end_game:
			StartCoroutine(EndGame());
			break;
		}
		
		CountDown();
	}

	IEnumerator PlayAnimation ()
	{
		yield return new WaitForSeconds(1);
		player_animator.SetBool("hand_move", false);
		opponent_animator.SetBool("hand_move", false);
		game_state = State.show_result;
	}

	IEnumerator EndGame ()
	{
		end_game_panel.SetActive(true);
		if(player_score == 5)
			end_game_win.SetActive(true);
		else
			end_game_panel.SetActive(true);
		
		game_state = State.idle;
		yield return new WaitForSeconds(1);
		
		Application.LoadLevel(0);
	}

}
