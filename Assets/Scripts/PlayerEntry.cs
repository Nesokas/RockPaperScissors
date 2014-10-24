using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerEntry : MonoBehaviour {

	public string player_name, id, facebookId;
	public bool isOnline;

	public GameObject name_obj;
	public GameObject profile_picture_obj;
	public GameObject online_obj;

	private Text name_text;
	private Image profile_picture;
	private Image online;

	// Use this for initialization
	void Awake () 
	{
		name_text = (Text)name_obj.GetComponent("Text");
		profile_picture = (Image)profile_picture_obj.GetComponent("Image");
		online = (Image)online_obj.GetComponent("Image");
	}

	void Start()
	{
		UpdateFriend();
	}
	
	// Update is called once per frame
	public void UpdateFriend () 
	{
		name_text.text = player_name;
		online.color = isOnline? Color.green : Color.gray;
		StartCoroutine(GetFBPicture());
	}

	public IEnumerator GetFBPicture()
	{
		var www = new WWW("http://graph.facebook.com/" + facebookId + "/picture?width=210&height=210");

		yield return www;

		Texture2D new_profile_picture = new Texture2D(25,25);
		www.LoadImageIntoTexture(new_profile_picture);
		profile_picture.sprite = Sprite.Create(new_profile_picture, new Rect(0,0,new_profile_picture.width, new_profile_picture.height), new Vector2(0.5f, 0.5f));
	}

	public void StartChallenge()
	{
		List<string> gsId = new List<string>();
		gsId.Add(id);

		StartCoroutine(ChallengeManager.instance.ChallengeUser(gsId, player_name));
	}
}
