using UnityEngine;
using System.Collections;

public class PlayerManager : Photon.MonoBehaviour {
	
	public bool is_local_player;
	public Manager.Move move;
	public bool player_time_out;

	// Use this for initialization
	void Start () {
		if(photonView.isMine){
			is_local_player = true;
			player_time_out = false;
		}

		GameObject manager_object = GameObject.FindGameObjectWithTag("GameManager");
		Manager manager = manager_object.GetComponent<Manager>();
		manager.RegisterNewPlayer(this);

		move = Manager.Move.notReady;
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) 
	{
		if(stream.isWriting){
			stream.SendNext(move);
			stream.SendNext(player_time_out);
		} else {
			move = (Manager.Move)stream.ReceiveNext();
			player_time_out = (bool)stream.ReceiveNext();
		}
	}
}
