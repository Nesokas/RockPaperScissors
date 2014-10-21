using UnityEngine;
using System.Collections;
using GameSparks.Api.Requests;
using UnityEngine.UI;

public class UserManager : MonoBehaviour {

	public static UserManager instance;

	public string username;
	public string user_id;
	private string facebook_id;

	public GameObject player_name;
	public GameObject player_profile;

	private Text player_name_text;
	private Image player_profile_image;

	// Use this for initialization
	void Start () {
		instance = this;
		player_name_text = (Text)player_name.GetComponent("Text");
		player_profile_image = (Image)player_profile.GetComponent("Image");
	}
	
	public void UpdateInformation()
	{
		new AccountDetailsRequest().Send((response) =>
		{
			UpdateGUI(response.DisplayName, response.UserId, response.ExternalIds.GetString("FB").ToString());
		});
	}

	public void UpdateGUI(string name, string uId, string fbId)
	{
		username = name;
		player_name_text.text = name;
		user_id = uId;
		facebook_id = fbId;
		StartCoroutine(GetFBPicture());
	}

	public IEnumerator GetFBPicture()
	{
		var www = new WWW("http://graph.facebook.com/" + facebook_id + "/picture?width=210&height=210");
		yield return www;

		Texture2D new_profile_picture = new Texture2D(25,25);
		www.LoadImageIntoTexture(new_profile_picture);
		player_profile_image.sprite = Sprite.Create(new_profile_picture, new Rect(0,0,new_profile_picture.width, new_profile_picture.height), new Vector2(0.5f, 0.5f));
	}
}
