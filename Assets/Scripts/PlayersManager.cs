using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameSparks.Api.Requests;
using MyExtensions;

public class PlayersManager : MonoBehaviour {

	public GameObject opponents_prefab;
	public GameObject opponents_panel;

	public List<GameObject> friends = new List<GameObject>();

	public int x, y;
	public int height_next;

	public void GetOpponents()
	{
		for(int i = 0; i < friends.Count; i++) {
			Destroy(friends[i]);
		}
		friends.Clear();

		new ListGameFriendsRequest().Send((response) =>
		{
			int i = 0;
			foreach(var friend in response.Friends){
				GameObject friend_obj = (GameObject)Instantiate(opponents_prefab);
				friend_obj.transform.parent = opponents_panel.transform;

				RectTransform rect_transform = (RectTransform)friend_obj.GetComponent("RectTransform");
				Vector2 new_position = new Vector2(x, y - height_next*i);
				Vector2 new_offset = new Vector2(x,x);
				rect_transform.offsetMin = new_offset;
				rect_transform.offsetMax = -new_offset;
				rect_transform.anchoredPosition = new_position;
				i++;

				RectTransform opponents_transform =(RectTransform)opponents_panel.GetComponent("RectTransform");
				if (opponents_transform.rect.height < Mathf.Abs(y - height_next*i)){
					RectTransformExtensions.SetHeight(opponents_transform, Mathf.Abs(y - height_next*i));
				}

				friend_obj.GetComponent<PlayerEntry>().player_name = friend.DisplayName;
				friend_obj.GetComponent<PlayerEntry>().id = friend.Id;
				friend_obj.GetComponent<PlayerEntry>().isOnline = friend.Online.Value;
				friend_obj.GetComponent<PlayerEntry>().facebookId = friend.ExternalIds.GetString("FB");

				friends.Add(friend_obj);
			}
		});
	}
}
