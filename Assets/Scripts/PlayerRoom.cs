using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerRoom : MonoBehaviour {

	public Text room_name;
	public Image profile_picture;
	public string room_id;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	public void JoinRoom()
	{
		PhotonNetwork.JoinRoom(room_id, false);
		GameObject game_manager = GameObject.FindGameObjectWithTag("GameManager");
		APIManager api_manager = game_manager.GetComponent<APIManager>();
		api_manager.last_room_connected = room_id;
	}

	public IEnumerator GetFacebookPicture(string facebookId)
	{
		var www = new WWW("http://graph.facebook.com/" + facebookId + "/picture?width=210&height=210");
		
		yield return www;
		
		Texture2D new_profile_picture = new Texture2D(25,25);
		www.LoadImageIntoTexture(new_profile_picture);
		profile_picture.sprite = Sprite.Create(new_profile_picture, new Rect(0,0,new_profile_picture.width, new_profile_picture.height), new Vector2(0.5f, 0.5f));
	}
}
