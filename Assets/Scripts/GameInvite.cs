using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GameSparks.Api.Requests;

public class GameInvite : MonoBehaviour {

	public string invite_name, invite_id, invite_facebookId, invite_expire, challengeId;

	public bool can_destroy = false;

	public GameObject invite_message_obj;
	public GameObject invite_expire_obj;
	public GameObject invite_image_obj;

	Text invite_message_text;
	Text invite_expire_text;
	Image invite_image;

	// Use this for initialization
	void Start () {
		invite_message_text = (Text)invite_message_obj.GetComponent("Text");
		invite_expire_text = (Text)invite_expire_obj.GetComponent("Text");
		invite_image = (Image)invite_image_obj.GetComponent("Image");

		invite_message_text.text = invite_name + " has challenge you!";
		invite_expire_text.text = "Challenge ends on " + invite_expire;
		new ListGameFriendsRequest().Send((response) => {
			foreach(var friend in response.Friends) {
				if(friend.Id == invite_id){
					invite_facebookId = friend.ExternalIds.GetString("FB");
					StartCoroutine(GetFBPicture());
				}
			}
		});
		//StartCoroutine(GetFBPicture());
	}
	
	public IEnumerator GetFBPicture()
	{
		var www = new WWW("http://graph.facebook.com/" + invite_facebookId + "/picture?width=210&height=210");
		
		yield return www;
		
		Texture2D new_profile_picture = new Texture2D(25,25);
		www.LoadImageIntoTexture(new_profile_picture);
		invite_image.sprite = Sprite.Create(new_profile_picture, new Rect(0,0,new_profile_picture.width, new_profile_picture.height), new Vector2(0.5f, 0.5f));
	}

	public void AcceptChallenge()
	{
		new AcceptChallengeRequest().SetChallengeInstanceId(challengeId).Send((response) => {
			ChallengeManager.instance.has_game_started = true;
		});

		new ListChallengeRequest().SetShortCode("NewGame")
			.SetState("RUNNING")
			.SetEntryCount(1)
			.Send((response) => {
				foreach(var challenge in response.ChallengeInstances) {
					ChallengeManager.instance.is_challenge_running = true;
					ChallengeManager.instance.challenge_id = challenge.ChallengeId;
					foreach(var player in challenge.Accepted) {
						ChallengeManager.instance.players_id.Add(player.Id);
						ChallengeManager.instance.players_name.Add(player.Name);
						Debug.Log(player.JSONData);
					}
					Application.LoadLevel(1);
				}
			});
	}

	public void DeclineChallenge()
	{
		new DeclineChallengeRequest().SetChallengeInstanceId(challengeId).Send((response) =>
		{
			if(response.HasErrors) {
				Debug.Log(response.Errors);
			} else {
				can_destroy = true;
			}
		});
	}

	public void Update()
	{
		if(can_destroy){
			Destroy(gameObject);
		}
	}
}
