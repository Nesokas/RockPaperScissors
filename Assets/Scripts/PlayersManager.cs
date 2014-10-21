using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Requests;

public class PlayersManager : MonoBehaviour {

	public GameObject opponents_prefab;
	public GameObject opponents_panel;

	public List<GameObject> friends = new List<GameObject>();

	public void GetFriends()
	{
		for(int i = 0; i < friends.Count; i++) {
			Destroy(friends[i]);
		}
		friends.Clear();

		new ListGameFriendsRequest().Send((response) =>
		{
			foreach(var friend in response.Friends){
				Debug.Log(friend.DisplayName);
			}
		});
	}
}
