using UnityEngine;
using System.Collections.Generic;
using System;
using GameSparks;
using GameSparks.Core;
 
public class GameSparksUnity : MonoBehaviour , GameSparks.Core.IGSPlatform {
	
	static string PLAYER_PREF_KEY = "gamesparks.authtoken";
	
	void Start()
	{

		DeviceName = SystemInfo.deviceName.ToString();
		DeviceType = SystemInfo.deviceType.ToString();
		DeviceId = SystemInfo.deviceUniqueIdentifier.ToString();
		AuthToken = PlayerPrefs.GetString(PLAYER_PREF_KEY);
		Platform = Application.platform.ToString();
		ExtraDebug = GameSparksSettings.DebugBuild;
		ServiceUrl = GameSparksSettings.ServiceUrl;
		GameSparksSecret = GameSparksSettings.ApiSecret;

		RequestTimeoutSeconds = 5;

		GS.Initialise(this);

		DontDestroyOnLoad (this);

	}

	private List<Action> _actions = new List<Action>();
	
	public void ExecuteOnMainThread(Action action){
		_actions.Add(action);
	}

	void Update(){
		List<Action> _currentActions = new List<Action>();
		lock (_actions)
		{
			_currentActions.AddRange(_actions);
			_actions.Clear();
		}
		foreach(var a in _currentActions)
		{
			a();
		}
	}

	void OnApplicationPause(bool paused) 
	{
		if(paused)
		{
			GS.Disconnect();
		}
		else
		{
			GS.Reconnect();
		}
	}
	
	void OnDestroy () {
		GS.ShutDown();
	}
	
	public String DeviceOS {
		get{
			#if UNITY_IOS
			return "IOS";
			#elif UNITY_ANDROID
			return "ANDROID";
			#elif UNITY_METRO
			return "W8";
			#else
			return "WP8";
			#endif
		}
	}

	public String DeviceName  {get; private set;}
	public String DeviceType {get; private set;}
	public String DeviceId  {get; private set;}
	public String Platform {get; private set;}
	public bool ExtraDebug {get; private set;}
	public String ServiceUrl  {get; private set;}
	public String GameSparksSecret  {get; private set;}
	public int RequestTimeoutSeconds  {get; set;}

	public void DebugMsg(String message){
		if(message.Length < 1500){
			Debug.Log("GS: "+message);
		} else {
			Debug.Log("GS: " + message.Substring(0, 1500) + "...");
		}
	}

	public String SDK{get;set;}

	private String m_authToken="0";
	public String AuthToken {
		get {return m_authToken;}
		set {
			m_authToken = value;
			PlayerPrefs.SetString(PLAYER_PREF_KEY, value);
		}
	}

}
