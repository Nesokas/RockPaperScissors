using UnityEngine;
using System.Collections;

public class PlayerManager : Photon.MonoBehaviour {
	
	public bool is_local_player;
	public RockPaperScissor.Move move;
	public bool ready_for_new_round;

	// Use this for initialization
	void Awake() {
		if(photonView.isMine)
			is_local_player = true;
		ready_for_new_round = true;
		move = RockPaperScissor.Move.notReady;
	}

	void Start()
	{
		GameObject rps_object = GameObject.FindGameObjectWithTag("GameController");
		RockPaperScissor rps = rps_object.GetComponent<RockPaperScissor>();
		Debug.Log("registering: " + is_local_player);
		rps.RegisterNewPlayer(this);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
	{
		if(stream.isWriting){
			stream.SendNext(move);
			stream.SendNext(ready_for_new_round);
		} else {
			move = (RockPaperScissor.Move)stream.ReceiveNext();
			ready_for_new_round = (bool)stream.ReceiveNext();
		}
	}
}
