using UnityEngine;
using System.Collections;

public class PlayerManager : Photon.MonoBehaviour {
	
	public bool is_local_player;
	public Manager.Move move;
	public bool ready_for_new_round;

	// Use this for initialization
	void Start () {
		if(photonView.isMine){
			is_local_player = true;
			ready_for_new_round = true;
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
			stream.SendNext(ready_for_new_round);
		} else {
			move = (Manager.Move)stream.ReceiveNext();
			ready_for_new_round = (bool)stream.ReceiveNext();
		}
	}
}
