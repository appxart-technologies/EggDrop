using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.IO;
using System;
using System.Linq;
#if UNITY_ANDROID
//using Prime31;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
#endif
using UnityEngine.SocialPlatforms;
using UnityEngine.SocialPlatforms.GameCenter;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using System.IO;
using System.Runtime.InteropServices;
using Prime31;
using Heyzap;



public class Main : MonoBehaviour {


	const float splashHoldTime = 2.0f;

	//Android Notifications
	private int _fourHrNotificationId;
	private int _eightHrNotificationId;
	private int _oneDayNotificationId;
	private int _threeDayNotificationId;
	private int _sevenDayNotificationId;
	private int _fourteenDayNotificationId;
	private int _thirtyDayNotificationId;
	
	long fourHr = 14400;	
	long eightHr = 28800;
	long oneDay = 86400;
	long threeDay = 259200;
	long sevenDay = 604800;
	long fourteenDay = 1209600;
	long thirtyDay = 2592000;
	
	string msg_h4 = "Ready to continue the challenge? Don’t crack the eggs, play Egg Drop!";
	string msg_h8 = "Take a break, set a new high score. Play Egg Drop!";
	string msg_d1 = "Play Egg Drop and score some baskets!";
	string msg_d3 = "Can you beat your friend’s high score on Egg Drop? Play now!";
	string msg_d7 = "Speed and skill is what it takes to win.  Play Egg Drop!";
	string msg_d14 = "The birds are chirping, Egg Drop awaits!";
	string msg_d30 = "We’ve added new levels, check out Egg Drop!";



	Texture2D MyImage;
	public QuestCheck questChecker;

	public AudioSource bgAudioSource;
	public AudioSource effectsSource;

	public AudioClip forestMusic;
//	public AudioClip snowMusic;
//	public AudioClip desertMusic;

	public AudioClip thunderBolt;
	public AudioClip sandStorm;

	public AudioClip eggDropSound;
	public AudioClip eggCatchedSound;
	public AudioClip eggMissForest;
	public AudioClip eggMissSnow;
	public AudioClip eggMissDesert;
	public AudioClip poleMissSound;
	public AudioClip eggComboSound;
	public AudioClip buttonSound;
	public AudioClip gameOverSound;
	public AudioClip bestScoreSound;


	public AudioSource audioSource;


	private int bestScore;

	public bool isNoAdsPurchased;
	public int totalEggs;
	public bool isFirstTimeUser;
	private bool hasFbShared;
	public int gameCount;
	public int sessionGameCount;
	public int totalScoreOfSession;
	public bool hasScored_20;
	public bool hasScored_40;
	public bool hasScored_60;



	private static Main instance = null;
	public static Main Instance 
	{
		get { return instance; }
	}
	
	void Awake() 
	{
		if (instance != null && instance != this) 
		{
			Destroy(this.gameObject);
			return;
		} 
		else 
		{
			instance = this;
		}

	}


	public void OnEnable ()
	{
//		ScreenshotHandler.ScreenshotFinishedSaving += ScreenshotSaved;
		#if UNITY_IOS
		StoreKitManager.restoreTransactionsFailedEvent += restoreTransactionsFailedEvent;
		StoreKitManager.restoreTransactionsFinishedEvent += restoreTransactionsFinishedEvent;
		StoreKitManager.purchaseSuccessfulEvent += purchaseSuccessfulEvent;
		StoreKitManager.purchaseCancelledEvent += purchaseCancelledEvent;
		StoreKitManager.purchaseFailedEvent += purchaseFailedEvent;
		#endif
	}
	
	void OnDisable()
	{
//		ScreenshotHandler.ScreenshotFinishedSaving -= ScreenshotSaved;
		#if UNITY_IOS
		StoreKitManager.restoreTransactionsFailedEvent -= restoreTransactionsFailedEvent;
		StoreKitManager.restoreTransactionsFinishedEvent -= restoreTransactionsFinishedEvent;
		StoreKitManager.purchaseSuccessfulEvent -= purchaseSuccessfulEvent;
		StoreKitManager.purchaseCancelledEvent -= purchaseCancelledEvent;
		StoreKitManager.purchaseFailedEvent -= purchaseFailedEvent;
		#endif
	}



#if UNITY_IOS

	#region Store Kit Delegates
	void restoreTransactionsFailedEvent( string error )
	{
		Debug.Log( "restoreTransactionsFailedEvent: " + error );
	}
	
	
	void restoreTransactionsFinishedEvent()
	{
		Debug.Log( "restoreTransactionsFinished" );
	}

	void purchaseSuccessfulEvent( StoreKitTransaction transaction )
	{
		if(transaction.productIdentifier == Constants.PRODUCT_ID)
		{
			Main.Instance.isNoAdsPurchased = true;
			PlayerPrefs.SetBool(Constants.KEY_NOADS, Main.Instance.isNoAdsPurchased);
			PlayerPrefs.Flush();
		}
	}
	
	void purchaseFailedEvent( string error )
	{
		Debug.Log( "purchaseFailedEvent: " + error );
	}
	
	
	void purchaseCancelledEvent( string error )
	{
		Debug.Log( "purchaseCancelledEvent: " + error );
	}

	#endregion

#endif







	// Use this for initialization
	void Start () 
	{
//		Application.targetFrameRate = 60;

		//Get all data
		PlayerPrefs.EnableEncryption(true);
		Instance.isNoAdsPurchased = PlayerPrefs.GetBool(Constants.KEY_NOADS, false);
		GameData.currentBirdId = PlayerPrefs.GetInt(Constants.KEY_BIRD_ID, 0);
		Instance.bestScore = PlayerPrefs.GetInt(Constants.KEY_BEST, 0);
		Instance.totalEggs = PlayerPrefs.GetInt(Constants.KEY_TOTAL_EGGS, 0);
		Instance.isFirstTimeUser = PlayerPrefs.GetBool(Constants.KEY_FIRST_TIME, true);
		GameData.isTutorial =  PlayerPrefs.GetBool(Constants.KEY_TUTORIAL, true);
		Instance.gameCount = PlayerPrefs.GetInt(Constants.KEY_TOTAL_GAMES, 0);
		Instance.totalScoreOfSession = 0;
		Instance.sessionGameCount = 0;
		Instance.audioSource = transform.GetComponent<AudioSource>();
		GameData.isMusic = PlayerPrefs.GetBool(Constants.KEY_MUSIC, true);
//		Debug.Log("MAIN START");
		{
			Invoke("PlayGame", splashHoldTime);
//			Invoke("SignIn", 2.0f);
		}

		#if UNITY_ANDROID
		PlayGamesPlatform.Activate();
		EtceteraAndroid.cancelNotification( Constants.H4 );
//		EtceteraAndroid.cancelNotification( Constants.H8 );
		EtceteraAndroid.cancelNotification( Constants.D1 );
		EtceteraAndroid.cancelNotification( Constants.D3 );
		EtceteraAndroid.cancelNotification( Constants.D7 );
		EtceteraAndroid.cancelNotification( Constants.D14 );
		EtceteraAndroid.cancelNotification( Constants.D30 );

//		var key = "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAn+MGqVUOeZYeOpAHzh+2fjA/SFHsZtMRfsn+jPf24VyPpj3stWeHMVCQY6lC18khZZdycuXHdtGp5enS/qdw02QoIhJUOZNSP8EXOpHxqfL4MaQ4G2AEgYxXV/Au2WXrfQ8cfC8xuE+n/MmER9b3dBHMRyIYeoafacqcrHdjmhLJy+vSL05NpJcURVTaRjAlwUL3PWa2ViJMTWkEohWIDCZpdeyFrduKCA+TBJcosqiMIX0hNN953ckpEkR3PUdUwIlIkr1GyzCjcrCIXR3qbpQXa5416O7u1bPA0LFo+5fx36kI6YNQGY8R+hiz3VbVzCMcvXifZXlVtAGYPAvRSQIDAQAB";
//		GoogleIAB.init( key );
		
		#elif UNITY_IOS 
		if(Instance.isFirstTimeUser)
		{
			//Restore Purchase in case ap deleted
//			StoreKitBinding.restoreCompletedTransactions();
		}

		UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert|UnityEngine.iOS.NotificationType.Badge);
		
		UnityEngine.iOS.NotificationServices.ClearLocalNotifications();
		UnityEngine.iOS.NotificationServices.CancelAllLocalNotifications ();

		// clear badge number
		UnityEngine.iOS.LocalNotification temp = new UnityEngine.iOS.LocalNotification();
		temp.fireDate = DateTime.Now;
		temp.applicationIconBadgeNumber = -1;
		temp.alertBody = "";
		UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(temp);

		#endif

		questChecker  = transform.GetComponent<QuestCheck>();

		HeyzapAds.start("790edd987b2facf1eb7117dfc97d354b", HeyzapAds.FLAG_NO_OPTIONS);


		DontDestroyOnLoad(gameObject);
	}
	

	void PlayGame()
	{
		SignIn();
		Application.LoadLevel("Play");
	}

	
	void SignIn()
	{
		#if UNITY_ANDROID
		Social.localUser.Authenticate((bool success) => {
			// handle success or failure
			if(success)
			{
//				Debug.Log("LOGGED In Google");
				if(Main.Instance.isLeaderBoard)
				{
					ShowLeaderBoard();
				}
				if(questChecker != null)
				{
					questChecker.PostAchievements();
				}
				//Post best score of user to leaderboard
				Main.Instance.PostScoreToLeaderBoard(Main.Instance.bestScore);

//				string UserName  = Social.localUser.userName; // UserName
//				string UserID      = Social.localUser.id; // UserID
//				Texture2D  userIMg  =Social.localUser.image;
//				Debug.Log(" UserName = " + UserName + UserID);
//				if(userIMg != null)
//				{
//					Debug.Log("Image found");
//				}
//				else 
//					Debug.Log("Image is NULL");
			}
			else
			{
//				Debug.Log("LOG_IN FALED Play Service Google");

			}


		});

		#elif UNITY_IOS
		Social.localUser.Authenticate (Main.Instance.ProcessAuthentication);
		#endif

	}


	void OnApplicationPause(bool pauseStatus) 
	{
		if(pauseStatus)
		{
			Main.Instance.isLeaderBoard = false;
			{
				#if UNITY_ANDROID
				Main.Instance._fourHrNotificationId = EtceteraAndroid.scheduleNotification( Main.Instance.fourHr, "Egg Drop", Main.Instance.msg_h4, "Egg Drop", "four-hour-note", "small_icon", "large_icon", Constants.H4 );
//				Main.Instance._eightHrNotificationId = EtceteraAndroid.scheduleNotification( Main.Instance.eightHr, "Egg Drop", Main.Instance.msg_h8, "Egg Drop", "eight-hour-note", "small_icon", "large_icon", Constants.H8 );
				Main.Instance._oneDayNotificationId = EtceteraAndroid.scheduleNotification( Main.Instance.oneDay, "Egg Drop", Main.Instance.msg_d1, "Egg Drop", "one-day-note", "small_icon", "large_icon", Constants.D1 );
				Main.Instance._threeDayNotificationId = EtceteraAndroid.scheduleNotification( Main.Instance.threeDay, "Egg Drop", Main.Instance.msg_d3, "Egg Drop", "three-day-note", "small_icon", "large_icon", Constants.D3 );
				Main.Instance._sevenDayNotificationId = EtceteraAndroid.scheduleNotification( Main.Instance.sevenDay, "Egg Drop", Main.Instance.msg_d7, "Egg Drop", "seven-day-note", "small_icon", "large_icon", Constants.D7 );
				Main.Instance._fourteenDayNotificationId = EtceteraAndroid.scheduleNotification( Main.Instance.fourteenDay, "Egg Drop", Main.Instance.msg_d14, "Egg Drop", "fourteen-day-note", "small_icon", "large_icon", Constants.D14 );
				Main.Instance._thirtyDayNotificationId = EtceteraAndroid.scheduleNotification( Main.Instance.thirtyDay, "Egg Drop", Main.Instance.msg_d30, "Egg Drop", "thirty-day-note", "small_icon", "large_icon", Constants.D30 );
				#elif UNITY_IOS 
				UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(getNotification(Main.Instance.msg_h4, 4));
				UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(getNotification(Main.Instance.msg_h8, 8));
				UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(getNotification(Main.Instance.msg_d1, 24));
				UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(getNotification(Main.Instance.msg_d3, 72));
				UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(getNotification(Main.Instance.msg_d7, 168));
				UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(getNotification(Main.Instance.msg_d14, 336));
				UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(getNotification(Main.Instance.msg_d30, 720));
				#endif
			}
		}
		else
		{
			#if UNITY_ANDROID
			EtceteraAndroid.cancelNotification( Constants.H4 );
			EtceteraAndroid.cancelNotification( Constants.H8 );
			EtceteraAndroid.cancelNotification( Constants.D1 );
			EtceteraAndroid.cancelNotification( Constants.D3 );
			EtceteraAndroid.cancelNotification( Constants.D7 );
			EtceteraAndroid.cancelNotification( Constants.D14 );
			EtceteraAndroid.cancelNotification( Constants.D30 );
			//			EtceteraAndroid.cancelAllNotifications();
			
			#elif UNITY_IOS
			NotificationServices.ClearLocalNotifications();
			NotificationServices.CancelAllLocalNotifications ();
			#endif
		}

		Main.Instance.isLeaderBoard = false;
	}
	
	void OnApplicationQuit()
	{
		Main.Instance.isLeaderBoard = false;
		if(Main.Instance.isFirstTimeUser)
		{
			PlayerPrefs.SetBool(Constants.KEY_FIRST_TIME, false);
			float avgScore = 0;
			if(Main.Instance.sessionGameCount > 0)
			{
				avgScore =  Main.Instance.totalScoreOfSession/Main.Instance.sessionGameCount;
			}


		}
		else
		{
			float avgScore = 0;
			if(Main.Instance.sessionGameCount > 0)
			{
				avgScore =  Main.Instance.totalScoreOfSession/Main.Instance.sessionGameCount;
			}

		}

		{
			#if UNITY_ANDROID
			Main.Instance._fourHrNotificationId = EtceteraAndroid.scheduleNotification( Main.Instance.fourHr, "Egg Drop", Main.Instance.msg_h4, "Egg Drop", "four-hour-note", "small_icon", "large_icon", Constants.H4 );
//			Main.Instance._eightHrNotificationId = EtceteraAndroid.scheduleNotification( Main.Instance.eightHr, "Egg Drop", Main.Instance.msg_h8, "Egg Drop", "eight-hour-note", "small_icon", "large_icon", Constants.H8 );
			Main.Instance._oneDayNotificationId = EtceteraAndroid.scheduleNotification( Main.Instance.oneDay, "Egg Drop", Main.Instance.msg_d1, "Egg Drop", "one-day-note", "small_icon", "large_icon", Constants.D1 );
			Main.Instance._threeDayNotificationId = EtceteraAndroid.scheduleNotification( Main.Instance.threeDay, "Egg Drop",Main.Instance.msg_d3, "Egg Drop", "three-day-note", "small_icon", "large_icon", Constants.D3 );
			Main.Instance._sevenDayNotificationId = EtceteraAndroid.scheduleNotification( Main.Instance.sevenDay, "Egg Drop", Main.Instance.msg_d7, "Egg Drop", "seven-day-note", "small_icon", "large_icon", Constants.D7 );
			Main.Instance._fourteenDayNotificationId = EtceteraAndroid.scheduleNotification( Main.Instance.fourteenDay, "Egg Drop", Main.Instance.msg_d14, "Egg Drop", "fourteen-day-note", "small_icon", "large_icon", Constants.D14 );
			Main.Instance._thirtyDayNotificationId = EtceteraAndroid.scheduleNotification( Main.Instance.thirtyDay, "Egg Drop", Main.Instance.msg_d30, "Egg Drop", "thirty-day-note", "small_icon", "large_icon", Constants.D30 );
			#elif UNITY_IOS 
			UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(getNotification(Main.Instance.msg_h4, 4));
			UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(getNotification(Main.Instance.msg_h8, 8));
			UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(getNotification(Main.Instance.msg_d1, 24));
			UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(getNotification(Main.Instance.msg_d3, 72));
			UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(getNotification(Main.Instance.msg_d7, 168));
			UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(getNotification(Main.Instance.msg_d14, 336));
			UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(getNotification(Main.Instance.msg_d30, 720));
			#endif
		}
		PlayerPrefs.Flush();


	}




	public void FBConnect()
	{
#if UNITY_IOS
		GotoFacebookPage("478546532205724");	//Appsolute Page
#elif UNITY_ANDROID
//		Application.OpenURL ("https://www.facebook.com/1touchstudios");
		Application.OpenURL("fb://page/135020303263673");	//Puissant page

		//1413755088947982 //1Touch Page
		#endif

	}


	#region Game Center & Service
	bool isLeaderBoard = false;

	public void ShowAcivements()
	{
		if (Social.localUser.authenticated) 
		Social.ShowAchievementsUI();
		else
		{
			SignIn();
		}
	}

	public void PostAchievement(string ID)
	{
		Social.ReportProgress(ID, 100.0f, (bool success) => {
			// handle success or failure
			if (success) 
			{
				//				Debug.Log("PostAchievement = " + ID);
			} 
			else 
			{
				//				Debug.Log ("PostAchievement Fail");
			}
		});
	}

	public void ShowLeaderBoard()
	{
		Main.Instance.isLeaderBoard = true;
		if (Social.localUser.authenticated) 
		{
//			Debug.Log("showing LB UI");
			#if UNITY_ANDROID
			((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(Constants.LEADER_BOARD_ID);
			#elif UNITY_IOS
			Social.ShowLeaderboardUI();
			#endif
		}
		else
		{
//			Debug.Log("Not signed so login");
			SignIn();
		}


	}

	public void PostScoreToLeaderBoard( int score )
	{

		if (Social.localUser.authenticated) 
		{
			#if UNITY_ANDROID
			Social.ReportScore (score, Constants.LEADER_BOARD_ID, (bool success) =>
			{
				if (success) 
				{

				} 
				else 
				{
//					Debug.Log ("Add Score Fail");
				}
			});

			#elif UNITY_IOS
			Social.ReportScore (score, Constants.LEADER_BOARD_ID, (bool success) =>
			{
				if (success) 
				{
					
				} 
				else 
				{
					//					Debug.Log ("Add Score Fail");
				}
			});
			#endif
		} 

	}
	#endregion
	
	
	
	
	#if UNITY_IOS
	UnityEngine.iOS.LocalNotification getNotification(string notif, int time)
	{
		UnityEngine.iOS.LocalNotification notification = new UnityEngine.iOS.LocalNotification();
		notification.fireDate = DateTime.Now.AddHours(time);
//		notification.applicationIconBadgeNumber = 1;
		notification.alertBody = notif;
		notification.soundName = UnityEngine.iOS.LocalNotification.defaultSoundName;
		return notification;
	}

	void ProcessAuthentication (bool success) 
	{
		if (success) 
		{
//			Debug.Log ("Authenticated, checking achievements");
//			if(Main.Instance.isLeaderBoard)
//			{
//				ShowLeaderBoard();
//			}
			//Post best score of user to leaderboard
			Main.Instance.PostScoreToLeaderBoard(Main.Instance.bestScore);
		}
		else
			Debug.Log ("Failed to authenticate");
	}

	#endif





	#region Sounds

	public void MuteSounds()
	{
		if(Instance.bgAudioSource != null && GameData.isMusic)
			Instance.bgAudioSource.volume = 0;
	}

	public void UnMuteSounds()
	{
		if(Instance.bgAudioSource != null && GameData.isMusic)
			Instance.bgAudioSource.volume = 0.3f;
	}

	public void PlayButtonClickSound()
	{
		if(Instance.audioSource != null && GameData.isMusic)
		{
			Instance.audioSource.PlayOneShot(Main.Instance.buttonSound);
		}
			
	}

	public void PlayEggDropSound()
	{
		if(Instance.audioSource != null && GameData.isMusic)
		{
			Instance.audioSource.PlayOneShot(Main.Instance.eggDropSound);
		}

	}


	public void PlayEggCatchedSound()
	{
		if(Instance.audioSource != null && GameData.isMusic)
		{
			Instance.audioSource.PlayOneShot(Main.Instance.eggCatchedSound);
		}

	}

	public void PlayComboSound()
	{
		if(Instance.audioSource != null && GameData.isMusic)
		{
			Instance.audioSource.PlayOneShot(Main.Instance.eggComboSound);
		}
		
	}


	public void PlayEggMissSound()
	{
		if(Instance.audioSource != null && GameData.isMusic)
		{
			switch(BackGroundManager.Instance.GetThemeID())
			{
			case BGTheme.Forest:
				Instance.audioSource.PlayOneShot(Main.Instance.eggMissForest);
				break;
				
			case BGTheme.Snow:
				Instance.audioSource.PlayOneShot(Main.Instance.eggMissSnow);
				break;

			case BGTheme.Desert:
				Instance.audioSource.PlayOneShot(Main.Instance.eggMissDesert);
				break;
			}
		}

	}

	public void PlayPoleMissSound()
	{
		if(Instance.audioSource != null && GameData.isMusic)
		{
			Instance.audioSource.PlayOneShot(Main.Instance.poleMissSound);
		}
	}

	public void PlayGameOverSound()
	{
		if(Instance.audioSource != null && GameData.isMusic)
		{
			Instance.audioSource.PlayOneShot(Main.Instance.gameOverSound);
		}

	}

	public void PlayBestScoreSound()
	{
		if(Instance.audioSource != null && GameData.isMusic)
		{
			Instance.audioSource.PlayOneShot(Main.Instance.bestScoreSound);
		}

	}

	public void PlayBGMusic()
	{
		AudioClip bgClip = null;
//		switch(BackGroundManager.Instance.GetThemeID())
//		{
//		case BGTheme.Forest:
//			bgClip = Instance.forestMusic;
//			break;
//
//		case BGTheme.Snow:
//			bgClip = Instance.snowMusic;
//			break;
//
//		case BGTheme.Desert:
//			bgClip = Instance.desertMusic;
//			break;
//		}
		if(Instance.bgAudioSource != null && GameData.isMusic)
		{
			Instance.bgAudioSource.volume = 0.3f;
//			Instance.bgAudioSource.clip = bgClip;
			Instance.bgAudioSource.clip = Instance.forestMusic;
			Instance.bgAudioSource.Play();
		}

	}

	public void PlayEffectsSounds()
	{
		if(Instance.effectsSource != null && GameData.isMusic)
		{
			switch(BackGroundManager.Instance.GetThemeID())
			{
				case BGTheme.Forest:
					Instance.effectsSource.clip = Instance.thunderBolt;
					Instance.effectsSource.volume = 0.5f;
					Instance.effectsSource.Play();
					break;
					
				case BGTheme.Snow:
					break;
					
				case BGTheme.Desert:
					Instance.effectsSource.clip = Instance.sandStorm;
					Instance.effectsSource.volume = 0.5f;
					Instance.effectsSource.Play();
					break;
			}

		}
	}
	#endregion


	#region General Share Delegates
	
	#if UNITY_IPHONE
	
	[DllImport("__Internal")]
	private static extern void MediaShareIos (string iosPath, string message);
	
	[DllImport("__Internal")]
	private static extern void TextShareIos (string message);

	[DllImport("__Internal")]
	private static extern void GotoFacebookPage(string pageID);
	#endif
	
	
	string pathToShareImg;
	string subject = "Come & Play this Awesome Game with Me!";
	string appUrl = "http://onelink.to/eggdrop";
	string body = "" ;

	IEnumerator SaveScreenShot()
	{

		// create the texture
		yield return new WaitForEndOfFrame();
		MyImage = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24,true);//* 0.38f
		// put buffer into texture
		MyImage.ReadPixels(new Rect(0f, 0f , Screen.width, Screen.height),0,0);// * 0.8f
		// apply
		MyImage.Apply();
		yield return new WaitForEndOfFrame();
		byte[] bytes = MyImage.EncodeToPNG();
		pathToShareImg = Application.persistentDataPath + "/dbdShare.png";
		File.WriteAllBytes(pathToShareImg, bytes);
		yield return new WaitForEndOfFrame();

		#if UNITY_ANDROID
		StartCoroutine(Main.Instance.ShareAndroidMedia());

		#elif UNITY_IPHONE
		MainDriver.Instance.ShareIosMedia();


		#endif

		//Check SHare Quest
		if(!questChecker.ACHIV_SHARE)
		{
			PlayerPrefs.SetBool(Constants.QUEST_SHARE, true);
			questChecker.UpdateQuestStatus();
			Main.Instance.PostAchievement(Constants.QUEST_SHARE);
		}
	}


	public  void ShareUniversal(int score)
	{
		Main.Instance.body = "I Scored " + score + " in #eggdrop. Can you beat my score? @puissantapps " ;
		Main.Instance.body += Main.Instance.appUrl;
		StartCoroutine(Main.Instance.SaveScreenShot());

	}


	
	IEnumerator ShareAndroidText()
	{
		yield return new WaitForEndOfFrame();
		#if UNITY_ANDROID
		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		intentObject.Call<AndroidJavaObject>("setType", "text/plain");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), Main.Instance.subject);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "Egg Drop");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), Main.Instance.body);

		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		currentActivity.Call("startActivity", intentObject);
		#endif
	}
	
	
	IEnumerator ShareAndroidMedia ()
	{

		yield return new WaitForEndOfFrame();
		#if UNITY_ANDROID

		Debug.Log("ShareAndroidMedia");
//		byte[] bytes = MyImage.EncodeToPNG();
//		pathToShareImg = Application.persistentDataPath + "/zigzagjump.png";
//		File.WriteAllBytes(pathToShareImg, bytes);

		AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
		AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

		intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
		intentObject.Call<AndroidJavaObject>("setType", "image/*");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_SUBJECT"), Main.Instance.subject);
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TITLE"), "Egg Drop");
		intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), Main.Instance.body);

		AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
		AndroidJavaClass fileClass = new AndroidJavaClass("java.io.File");

		AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", pathToShareImg);// Set Image Path Here

		AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);

		bool fileExist = fileObject.Call<bool>("exists");
		if (fileExist)
			intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);

		AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
		AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
		currentActivity.Call("startActivity", intentObject);


		#endif

	}
	
	void ShareIosMedia()
	{
		#if UNITY_IPHONE
		byte[] bytes = MyImage.EncodeToPNG();
//		string path = Application.persistentDataPath + "/MyImage.png";
		File.WriteAllBytes(pathToShareImg, bytes);
		//		string path_ =  "MyImage.png";
		//		StartCoroutine(ScreenshotHandler.Save(path_, "Media Share", true));
		string shareMessage = MainDriver.Instance.body;
		MediaShareIos (path, shareMessage);
		#endif

	}

	void ShareIosText()
	{
		string shareMessage = Main.Instance.body;
		#if UNITY_IPHONE
		TextShareIos(shareMessage);
		#endif
	}

	
	#endregion

}
