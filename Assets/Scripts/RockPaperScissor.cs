using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;


public class RockPaperScissor : MonoBehaviour {

	public enum Move {
		rock,
		paper,
		scissor,
		notReady,
		time_out
	};

	private enum State {
		choose_move,
		play_animation,
		show_result,
		idle,
		end_game
	};

	private enum Result {
		win,
		lose,
		draw
	}

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

	public GameObject canvas;

	public GameObject end_game_panel;
	public GameObject end_game_win;
	public GameObject end_game_loose;

	public GameObject timer;
	public int turn_time;

	public GameObject player_ready;
	public GameObject opponent_ready;

	public GameObject lost_connection_obj;
	public Text lost_connection_text;

	private float lost_connection_remaining;
	private bool waiting_to_reconnect = false;

	private Image[] player_stars;
	private Image[] opponent_stars;

	private int player_score;
	private Animator player_animator;
	private int opponent_score;
	private Animator opponent_animator;

	private Animator canvas_animator;

	private Move player_move;
	private Move opponent_move;

	private Result turn_result;
	private Text outcome_text; 

	private State game_state;

	private float start_time;
	private int rest_seconds;
	private Text text_timer;
	private bool stop_timer;

	private PlayerManager player_manager;
	private PlayerManager opponent_manager;

	private GameObject player_manager_obj;

	// Use this for initialization
	protected void Start () {
		game_state = State.choose_move;

		player_animator = (Animator)player.GetComponent("Animator");
		opponent_animator = (Animator)opponent.GetComponent("Animator");

		canvas_animator = (Animator)canvas.GetComponent("Animator");

		player_stars = new Image[5];
		opponent_stars = new Image[5];
		for(int i = 0; i < 5; i++){
			player_stars[i] = (Image)player_stars_obj[i].GetComponent("Image");
			opponent_stars[i] = (Image)opponent_stars_obj[i].GetComponent("Image");
		}

		player_score = 0;
		opponent_score = 0;

		outcome_text = (Text)outcome_text_obj.GetComponent("Text");

		start_time = Time.time;
		text_timer = (Text)timer.GetComponent("Text");
		stop_timer = false;

		player_manager_obj = PhotonNetwork.Instantiate("Prefabs/PlayerManager", Vector3.zero, Quaternion.identity, 0);
		player_move = Move.notReady;
		opponent_move = Move.notReady;
	}
	
	// Update is called once per frame
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
			ShowResults();
			break;
		case State.idle: //don't do nothing just wait
			break;
		case State.end_game:
			StartCoroutine(EndGame());
			break;
		}

		if(!waiting_to_reconnect){
			CountDown();
		}
	}


	/* public function to be used by the buttons and set the player choice.
	 * Parameters:
	 * 		player_choice:
	 * 			0 - player choose rock
	 * 			1 - player choose paper
	 * 			2 - player choose scissors
	 */
	public void PlayerChoose(int player_choice)
	{
		stop_timer = true;
		switch(player_choice){
		case 0:
			player_move = Move.rock;
			break;
		case 1:
			player_move = Move.paper;
			break;
		case 2:
			player_move = Move.scissor;
			break;
		}

		player_manager.move = player_move;
		player_ready.SetActive(true);

		/* // random opponent decision
		 * Array possible_moves = Enum.GetValues(typeof(Move));
		 * System.Random random = new System.Random();
		 * opponent_move = (Move)possible_moves.GetValue(random.Next(possible_moves.Length));
		 */

		canvas_animator.SetBool("Hide", true);
	}

	// Show window to choose player move
	void ChooseMove ()
	{
		//choose_your_move.SetActive(true);
		if(player_manager != null){
			if(opponent_manager != null){
				if(!player_manager.ready_for_new_round)
					if(!opponent_manager.ready_for_new_round)
						return;

				Debug.Log("chosing move");
				canvas_animator.SetBool("Hide", false);
				StartCoroutine(WaitForOpponentMove());
				StartCoroutine(WaitPlayersReady());
				game_state = State.idle;
			}
		}
	}

	string debug = "";
	IEnumerator WaitForOpponentMove()
	{
		while(opponent_manager == null)
			yield return null;

		while(opponent_manager.move == Move.notReady){
			debug = "player_move: " + player_manager.move + "; opponent_move: " + opponent_manager.move;
			yield return null;
		}

		debug = "player_move: " + player_manager.move + "; opponent_move: " + opponent_manager.move;

		opponent_move = opponent_manager.move;
		opponent_ready.SetActive(true);
	}

	IEnumerator WaitPlayersReady()
	{
		while(!player_ready.activeSelf || !opponent_ready.activeSelf)
			yield return null;

		player_ready.SetActive(false);
		opponent_ready.SetActive(false);

		if(player_move != Move.time_out && opponent_move != Move.time_out){
			player_animator.SetBool("hand_move", true); // start player and opponent animations
			opponent_animator.SetBool("hand_move", true);
			
			game_state = State.play_animation; // change game state
		} else {
			game_state = State.show_result;
		}
	}


	// Function that changes the game state after animation
	IEnumerator PlayAnimation ()
	{
		yield return new WaitForSeconds(1);
		player_animator.SetBool("hand_move", false);
		opponent_animator.SetBool("hand_move", false);
		game_state = State.show_result;
	}

	// Show turn results
	void ShowResults ()
	{
		player_manager.ready_for_new_round = false;
		Image player_image = (Image)player.GetComponent("Image");
		Image opponent_image = (Image)opponent.GetComponent("Image");

		switch(player_move){
		case Move.rock:
			player_image.sprite = rock;
			break;
		case Move.paper:
			player_image.sprite = paper;
			break;
		case Move.scissor:
			player_image.sprite = scissor;
			break;
		default:
			player_image.sprite = rock;
			break;
		}

		switch(opponent_move){
		case Move.rock:
			opponent_image.sprite = rock;
			break;
		case Move.paper:
			opponent_image.sprite = paper;
			break;
		case Move.scissor:
			opponent_image.sprite = scissor;
			break;
		default:
			player_image.sprite = rock;
			break;
		}

		if(player_move == Move.time_out && opponent_move == Move.time_out) {
			turn_result = Result.draw;
		} else 	if(player_move == Move.rock) {
			switch(opponent_move){
			case Move.rock:
				turn_result = Result.draw;
				break;
			case Move.paper:
				turn_result = Result.lose;
				break;
			case Move.scissor:
				turn_result = Result.win;
				break;
			case Move.time_out:
				turn_result = Result.win;
				break;
			}
		} else if(player_move == Move.paper) {
			switch(opponent_move){
			case Move.rock:
				turn_result = Result.win;
				break;
			case Move.paper:
				turn_result = Result.draw;
				break;
			case Move.scissor:
				turn_result = Result.lose;
				break;
			case Move.time_out:
				turn_result = Result.win;
				break;
			}
		} else if(player_move == Move.scissor){
			switch(opponent_move){
			case Move.rock:
				turn_result = Result.lose;
				break;
			case Move.paper:
				turn_result = Result.win;
				break;
			case Move.scissor:
				turn_result = Result.draw;
				break;
			case Move.time_out:
				turn_result = Result.win;
				break;
			}
		} else { // opponent played something but not the player
			turn_result = Result.lose;
			player_image.sprite = rock;
			opponent_image.sprite = rock;
		}

		//opponent_manager.move = Move.notReady;
		StartCoroutine(ShowOutcome(turn_result));

		game_state = State.idle;
	}

	IEnumerator ShowOutcome(Result result){

		yield return new WaitForSeconds(0.7f);

		if(result == Result.lose){
			outcome_text.text = "LOSE";
			if(player_move == Move.time_out)
				outcome_text.text += "\nyour time was up";
			opponent_stars[opponent_score].sprite = star_filled;
			opponent_score++;
		} else if(result == Result.win) {
			outcome_text.text = "WIN";
			if(opponent_move == Move.time_out)
				outcome_text.text += "\nopponent time was up";
			player_stars[player_score].sprite = star_filled;
			player_score++;
		} else {
			outcome_text.text = "DRAW";
			if(player_move == Move.time_out && opponent_move == Move.time_out)
				outcome_text.text += "\nyours and opponents time was up";
		}

		player_move = Move.notReady;
		opponent_move = Move.notReady;
		player_manager.move = Move.notReady;

		outcome_obj.SetActive(true);
		yield return new WaitForSeconds(2.5f);
		outcome_obj.SetActive(false);

		// return hands to normal state
		Image player_image = (Image)player.GetComponent("Image");
		Image opponent_image = (Image)opponent.GetComponent("Image");

		player_image.sprite = rock;
		opponent_image.sprite = rock;

		stop_timer = false;
		start_time = Time.time;
		if(player_score == 5 || opponent_score == 5)
			game_state = State.end_game;
		else
			game_state = State.choose_move;

		player_manager.ready_for_new_round = true;
	}

	IEnumerator EndGame ()
	{
		end_game_panel.SetActive(true);
		if(player_score == 5)
			end_game_win.SetActive(true);
		else
			end_game_loose.SetActive(true);

		game_state = State.idle;
		yield return new WaitForSeconds(1);

		Application.LoadLevel(0);
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
				player_move = Move.time_out;
				player_manager.move = player_move;
				player_ready.SetActive(true);
			}
		} else {
			text_timer.text = "";
		}
	}

	public void ExitGame()
	{
		PhotonNetwork.LeaveRoom();
		Application.LoadLevel(0);
	}

	void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		if(otherPlayer.isMasterClient)
			PhotonNetwork.SetMasterClient(PhotonNetwork.player);
		waiting_to_reconnect = true;
		StartCoroutine(WaitForPlayerReconnect("Waiting for opponent to reconnect: "));
	}

	IEnumerator WaitForPlayerReconnect(string message)
	{
		Debug.Log("tryong to reconnect");
		lost_connection_obj.SetActive(true);
		float time_left = 60; // wait a minute for other player reconnection
		while(waiting_to_reconnect) {
			lost_connection_text.text = message + (int)time_left;
			time_left -= Time.deltaTime;
			if(time_left < 0){
				PhotonNetwork.LeaveRoom();
				Application.LoadLevel(0); // if time went off return to lobby
			}
			yield return null;
		}
		lost_connection_obj.SetActive(false);
	}

	void OnJoinedLobby()
	{
		GameObject game_manager = GameObject.FindGameObjectWithTag("GameManager");
		APIManager api_manager = game_manager.GetComponent<APIManager>();

		PhotonNetwork.JoinRoom(api_manager.last_room_connected);
	}

	void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		waiting_to_reconnect = false;
	}

	void OnJoinedRoom()
	{
		waiting_to_reconnect = false;
		player_manager_obj = PhotonNetwork.Instantiate("Prefabs/PlayerManager", Vector3.zero, Quaternion.identity, 0);
	}

	void OnDisconnectedFromPhoton()
	{
		player_ready_for_new_round = player_manager.ready_for_new_round;
		Destroy(player_manager_obj);
		Destroy(opponent_manager.gameObject);
		waiting_to_reconnect = true;
		StartCoroutine(WaitForPlayerReconnect("Trying to reconnect: "));
	}

	bool player_ready_for_new_round = true;

	public void RegisterNewPlayer(PlayerManager player_m)
	{
		if(player_m.is_local_player) {
			player_manager = player_m;
			player_manager.move = player_move;
			player_manager.ready_for_new_round = player_ready_for_new_round;
		} else {
			opponent_manager = player_m;
		}
	}

	void OnGUI()
	{
#if UNITY_EDITOR
		GUILayout.Label(debug);
		if(player_manager != null && opponent_manager != null)
			GUILayout.Label("player_ready: " + player_manager.ready_for_new_round + " opponent_ready: " + opponent_manager.ready_for_new_round);
		GUILayout.Label(debug);
#endif
	}

}
