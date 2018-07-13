using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using Prime31;
using Heyzap;

public class GameHud : MonoBehaviour {

	public RectTransform logoPanel;

	/// <summary>
	/// The tutorial panel for game start.
	/// </summary>
	public RectTransform tutorialPanel;

	public RectTransform tutorialReplay;

	public Text tutorialText;
	public Text hintText;
	public Text hintText2;

	/// <summary>
	/// The home panel.
	/// </summary>
	public RectTransform homePanel;

	/// <summary>
	/// The instruction panel, how to play.
	/// </summary>
	public RectTransform instructionPanel;

	/// <summary>
	/// The hud panel.
	/// </summary>
	public RectTransform hudPanel;

	/// <summary>
	/// The game over panel.
	/// </summary>
	public RectTransform gameOverPanel;

	/// <summary>
	/// The rate panel.
	/// </summary>
	public RectTransform ratePanel;

	public RectTransform inAppPanel;



	/// <summary>
	/// The sound button.
	/// </summary>
	public Button soundBtn;

	public Button noAddButtonHome;

	public Button noAddButtonGameOver;



	/// <summary>
	/// The sound on image.
	/// </summary>
	public SpriteRenderer soundOnImg;

	/// <summary>
	/// The sound off image.
	/// </summary>
	public SpriteRenderer soundOffImg;

	/// <summary>
	/// The egg icon of score text.
	/// </summary>
	public Image eggIcon;

	public Button rewardButton;

	public Image rewardConfirmImg;
	public Image rewardEggIcon;

	public Image eggSmall;
	public Image eggBig;

	/// <summary>
	/// The score text in HUD.
	/// </summary>
	public Text scoreText;


	public Image eggIconGO;
	public Text totalEggs;

	/// <summary>
	/// The game over score text.
	/// </summary>
	public Text gameOverScoreText;

	public Text newBestText;

	public Text bestScoreText;

	public Text gameOverInstText;

	public GameObject revivePanel;
	public Text timerText;


	/// <summary>
	/// The game score.
	/// </summary>
	private int gameScore;

	/// <summary>
	/// The best score.
	/// </summary>
	private int bestScore;

	bool isReviveBreaked;


#if UNITY_IOS
	private List<StoreKitProduct> _products;
#endif

	bool isRestoring, canRestored;

//	/// <summary>
//	/// Occurs when first level of change in envirnment is required .
//	/// </summary>
//	public delegate void ActivateLevelOneChange();
//	public static event ActivateLevelOneChange EnvirnmentChangeNow;
//
//	/// <summary>
//	/// Occurs when extreme change in Envirnment is required.
//	/// </summary>
//	public delegate void ActivateLevelTwoChange();
//	public static event ActivateLevelTwoChange ExtremeChangeNow;

	public static event Action HasWatchedVideo, HasTobeRevived;


	void OnEnable()
	{
		Nest.EggCatched += HandleEggCatched;
		UniRate.Instance.OnPromptedForRating += OnPromptedForRating;
		Bird.ReplayNow += Replay;
//		#if UNITY_IOS
//		StoreKitManager.productListReceivedEvent += productListReceivedEvent;
//		StoreKitManager.purchaseSuccessfulEvent += purchaseSuccessfulEvent;
//		#elif UNITY_ANDROID
//		GoogleIABManager.queryInventorySucceededEvent += queryInventorySucceededEvent;
//		GoogleIABManager.queryInventoryFailedEvent += queryInventoryFailedEvent;
//		GoogleIABManager.purchaseSucceededEvent += purchaseSucceededEvent;
//		#endif
		GameHud.HasWatchedVideo += AddEggCount;
		GameHud.HasTobeRevived += ReviveGame;

	}
	
	void OnDisable()
	{
		Nest.EggCatched -= HandleEggCatched;
		UniRate.Instance.OnPromptedForRating -= OnPromptedForRating;
		Bird.ReplayNow -= Replay;
//		#if UNITY_IOS
//		StoreKitManager.productListReceivedEvent -= productListReceivedEvent;
//		StoreKitManager.purchaseSuccessfulEvent -= purchaseSuccessfulEvent;
//		#elif UNITY_ANDROID
//		GoogleIABManager.queryInventorySucceededEvent -= queryInventorySucceededEvent;
//		GoogleIABManager.queryInventoryFailedEvent -= queryInventoryFailedEvent;
//		GoogleIABManager.purchaseSucceededEvent -= purchaseSucceededEvent;
//		#endif
		GameHud.HasWatchedVideo -= AddEggCount;
		GameHud.HasTobeRevived -= ReviveGame;
	}


	void OnDestroy()
	{
		Nest.EggCatched -= HandleEggCatched;
		UniRate.Instance.OnPromptedForRating -= OnPromptedForRating;
		Bird.ReplayNow -= Replay;
	}
	



	// Use this for initialization
	void Start () 
	{
		if(GameData.isComingFromStore && !GameData.isComingFromGameOverStore)
		{
			//On Main menu
//			Debug.Log("Create 2");
			BackGroundManager.Instance.SpwanBird();
		}

		canRestored = false;
		if(!logoPanel.gameObject.activeSelf)// && !GameData.isTutorial)
		{
			logoPanel.gameObject.SetActive(true);
		}
		if(tutorialPanel.gameObject.activeSelf)
		{
			tutorialPanel.gameObject.SetActive(false);
		}

		if(tutorialReplay.gameObject.activeSelf)
		{
			tutorialReplay.gameObject.SetActive(false);
		}

		hintText.gameObject.SetActive(false);
		hintText2.gameObject.SetActive(false);

		if(homePanel.gameObject.activeSelf)
		{
			homePanel.gameObject.SetActive(false);
		}
		if(instructionPanel.gameObject.activeSelf)
		{
			instructionPanel.gameObject.SetActive(false);
		}
		if(hudPanel.gameObject.activeSelf)
		{
			hudPanel.gameObject.SetActive(false);
		}
		if(gameOverPanel.gameObject.activeSelf)
		{
			gameOverPanel.gameObject.SetActive(false);
		}
		revivePanel.SetActive(false);
		inAppPanel.gameObject.SetActive(false);
		gameScore = 0;

		GameData.isMusic = PlayerPrefs.GetBool(Constants.KEY_MUSIC, true);
		GameData.reviveCount = PlayerPrefs.GetInt(Constants.KEY_REVIVE_CNT, 0);
		GameData.lifeTimeEggs = PlayerPrefs.GetInt(Constants.KEY_LIFE_TIME_EGGS, 0);
		GameData.unlockCount = PlayerPrefs.GetInt(Constants.KEY_UNLOCK_CNT, 0);

//		#if UNITY_IOS
//		// array of product ID's from iTunesConnect. MUST match exactly what you have there!
//		var productIdentifiers = new string[] {Constants.PRODUCT_ID};
//		StoreKitBinding.requestProductData( productIdentifiers );
//
//		#elif UNITY_ANDROID
//		var skus = new string[] {Constants.PRODUCT_ID};
//		GoogleIAB.queryInventory( skus );
//		#endif

		PlayerPrefs.SetInt(Constants.KEY_TOTAL_EGGS, Main.Instance.totalEggs);
		GameData.isComingFromStore = false;
		GameData.isComingFromGameOverStore = false;

		HZIncentivizedAd.setDisplayListener(listener);
		HZIncentivizedAd.fetch();
		#if UNITY_IOS
		AppTrackerIOS.startSession("FPxjfDrMNNVRJ0REcLqwHL45mG1HEnII");
		#endif

		if(!Main.Instance.isNoAdsPurchased)
		{
			HZBannerShowOptions showOptions = new HZBannerShowOptions();
			showOptions.Position = HZBannerShowOptions.POSITION_BOTTOM;
			HZBannerAd.ShowWithOptions(showOptions);
		}
	}
	



	#region Home Panel Actions
	public void ActivateHomePanel()
	{
		if(Main.Instance.isNoAdsPurchased)
		{
			if(noAddButtonHome != null)
			{
				noAddButtonHome.interactable = false;
			}

		}

		GameData.isComingFromGameOverStore = false;
		GameData.isComingFromStore = false;
		tutorialText.text = "Tap to start";
		if(!logoPanel.gameObject.activeSelf && !GameData.isRetry)
		{
			logoPanel.gameObject.SetActive(true);
		}
		if(!homePanel.gameObject.activeSelf && !GameData.isRetry)
		{


			if(GameData.isMusic)
			{
				soundBtn.image.sprite = soundOnImg.sprite;
			}
			else
			{
				soundBtn.image.sprite = soundOffImg.sprite;
			}
			homePanel.gameObject.SetActive(true);
			Invoke("ActivateTutorial", 0.3f);

		}

//		HZBannerAd.hide();

	}


	public void ActivateTutorial()
	{
		if(logoPanel.gameObject.activeSelf && GameData.isTutorial)
		{
			logoPanel.gameObject.SetActive(false);
		}

		{
			if(GameData.isTutorial)
			{
				tutorialText.text = "Tap to drop";
				hintText.text = "Try and aim to drop the egg in the nest!";
				hintText.gameObject.SetActive(true);
			}
			else
			{
				hintText.gameObject.SetActive(false);
				tutorialText.text = "Tap to start";
			}
			tutorialPanel.gameObject.SetActive(true);
		}
	}

	public void ChangeHintText()
	{
		tutorialText.text = "Tap to start";
		hintText.text = "Watch out for moving pillars and weather changes!";
		hintText2.gameObject.SetActive(true);
	}


	public void DeactivateTutorial()
	{
		if(tutorialPanel.gameObject.activeSelf)
		{
			tutorialPanel.gameObject.SetActive(false);
		}
		if(hintText.gameObject.activeSelf)
		{
			hintText.gameObject.SetActive(false);
		}
	}


	public void StarGame()
	{
		isReviveBreaked = false;

		#if UNITY_IOS
		AppTrackerIOS.loadModuleToCache("inapp");
		#endif	
		HZIncentivizedAd.fetch();
		GameData.isComingFromGameOverStore = false;

		//Deactivate Home panel
		if(homePanel.gameObject.activeSelf)
		{
			homePanel.gameObject.SetActive(false);
		}

		if(logoPanel.gameObject.activeSelf)
		{
			logoPanel.gameObject.SetActive(false);
		}

		if(tutorialPanel.gameObject.activeSelf)
		{
			tutorialPanel.gameObject.SetActive(false);
		}

		if(tutorialReplay.gameObject.activeSelf)
		{
			tutorialReplay.gameObject.SetActive(false);
		}

		if(hintText.gameObject.activeSelf)
		{
			hintText.gameObject.SetActive(false);
		}
		hintText2.gameObject.SetActive(false);

		CancelInvoke("ActivateTutorial");

		GameData.isRetry = false;

		bestScore = PlayerPrefs.GetInt(Constants.KEY_BEST, 0);
		bestScoreText.text = "" + bestScore;

		//Activate Game Hud panel
		ResetHud();
		if(!hudPanel.gameObject.activeSelf)
		{
//			int eggID = (int)BackGroundManager.Instance.GetThemeID();
			int eggId = 2;
			if(GameData.currentBirdId == 1 || GameData.currentBirdId == 3)
			{
				eggId = 3;
			}
			else if(GameData.currentBirdId == 2 || GameData.currentBirdId == 5)
			{
				eggId = 0;
			}
			else if(GameData.currentBirdId == 4 || GameData.currentBirdId == 6 )
			{
				eggId = 5;
			}
			else if(GameData.currentBirdId == 9)
			{
				eggId = 4;
			}
			else if(GameData.currentBirdId == 7 )
			{
				eggId = 1;
			}
			else if(GameData.currentBirdId == 8 )
			{
				eggId = 2;
			}
			string eggName = "egg_" + eggId;
			if(eggIcon != null)
			{
				eggIcon.sprite = Resources.Load <Sprite> ("Textures/Game/Eggs/" + eggName) as Sprite;
			}
			if(eggIconGO != null)
			{
				eggIconGO.sprite = Resources.Load <Sprite> ("Textures/Game/Eggs/" + eggName) as Sprite;
			}
			if(eggSmall != null)
			{
				eggSmall.sprite = Resources.Load <Sprite> ("Textures/Game/Eggs/" + eggName) as Sprite;
			}
			if(rewardEggIcon != null)
			{
				rewardEggIcon.sprite = Resources.Load <Sprite> ("Textures/Game/Eggs/" + eggName) as Sprite;
			}
			if(eggBig != null)
			{
				eggBig.sprite = Resources.Load <Sprite> ("Textures/Game/Eggs/" + eggName) as Sprite;
			}
			hudPanel.gameObject.SetActive(true);
		}
	}

	public void SoundOnOff()
	{
		if(GameData.isMusic)
		{
			GameData.isMusic = false;
			PlayerPrefs.SetBool(Constants.KEY_MUSIC, false);
			soundBtn.image.sprite = soundOffImg.sprite;
			Main.Instance.bgAudioSource.Stop();
		}
		else
		{
			GameData.isMusic = true;
			PlayerPrefs.SetBool(Constants.KEY_MUSIC, true);
			soundBtn.image.sprite = soundOnImg.sprite;
			Main.Instance.PlayButtonClickSound();
			Main.Instance.PlayBGMusic();
		}

		PlayerPrefs.Flush();
	}


	public void StoreClicked()
	{
		Main.Instance.PlayButtonClickSound();
		if(GameData.isComingFromGameOverStore)
		{
			HZBannerAd.hide();
			if(BackGroundManager.Instance != null)
			{
				BackGroundManager.Instance.transform.position = Vector3.zero;
			}
		}
		Application.LoadLevel(2);
	}

	public void LeadBoard()
	{
//		Debug.Log("LeadBoard");
		Main.Instance.PlayButtonClickSound();
//		HZBannerAd.hide();
		Main.Instance.ShowLeaderBoard();
//		HeyzapAds.ShowMediationTestSuite();
	}

	public void ShowAchiv()
	{
		Main.Instance.PlayButtonClickSound();
		Main.Instance.ShowAcivements();
	}

	public void RateMe()
	{
//		Debug.Log("RateMe");
		Main.Instance.PlayButtonClickSound();
		UniRate.Instance.RateIfNetworkAvailable();
	}

	public void Instruction()
	{
//		Debug.Log("Instruction");
		Main.Instance.PlayButtonClickSound();
//		if(!instructionPanel.gameObject.activeSelf)
//		{
//			instructionPanel.gameObject.SetActive(true);
//		}
		if(homePanel.gameObject.activeSelf)
		{
			homePanel.gameObject.SetActive(false);
		}
		GameObject gameBird = GameObject.FindGameObjectWithTag(Constants.TAG_BIRD);
		if(gameBird != null)
		{
			gameBird.SendMessage("StartTutorial");
		}
	}

	public void CloseInstruction()
	{
		Main.Instance.PlayButtonClickSound();
		if(instructionPanel.gameObject.activeSelf)
		{
			instructionPanel.gameObject.SetActive(false);
		}
	}

	public void FaceBookConnect()
	{
//		Debug.Log("FaceBookConnect");
		Main.Instance.PlayButtonClickSound();
		Main.Instance.FBConnect();

	}


	public void ShowIapPanel()
	{
		Main.Instance.PlayButtonClickSound();
		if(gameOverPanel.gameObject.activeSelf)
		{
			if(!Main.Instance.isNoAdsPurchased)
			{
				HZBannerAd.hide();
			}
		}
		isRestoring = false;
		inAppPanel.gameObject.SetActive(true);
	}

	public void CloseIapPanel()
	{
		Main.Instance.PlayButtonClickSound();
		isRestoring = false;
		if(gameOverPanel.gameObject.activeSelf)
		{
			if(!Main.Instance.isNoAdsPurchased)
			{
				HZBannerAd.show(HZBannerAd.POSITION_TOP);
			}
		}
		inAppPanel.gameObject.SetActive(false);
	}

	public void RestorePurchase()
	{
//		Main.Instance.PlayButtonClickSound();
//		isRestoring = true;
//		#if UNITY_IOS
//		StoreKitBinding.restoreCompletedTransactions();
//		#elif UNITY_ANDROID
//		if(canRestored)
//		{
//			Debug.Log("RestorePurchase");
//			GoogleIAB.purchaseProduct(Constants.PRODUCT_ID );
//		}
//		#endif

	}

	public void NoAddsAction()
	{
//		Main.Instance.PlayButtonClickSound();
//
//		#if UNITY_IOS
//		if( _products != null && _products.Count > 0 )
//		{
//			int productIndex = 0 ; //We have only one product	// Random.Range( 0, _products.Count );
//			var product = _products[productIndex];
//			
//			Debug.Log( "preparing to purchase product: " + product.productIdentifier );
//			StoreKitBinding.purchaseProduct( product.productIdentifier, 1 );
//		}
//		#elif UNITY_ANDROID
//		GoogleIAB.purchaseProduct(Constants.PRODUCT_ID );
//		#endif
	}

	
	#endregion


	#region Game Over Actions

	public void HomePressed()
	{
//		if(!Main.Instance.isNoAdsPurchased)
//		{
//			HZBannerAd.hide();
//		}
//		Debug.Log("HomePressed");
		Main.Instance.PlayButtonClickSound();
		GameData.isRetry = false;
		if(BackGroundManager.Instance != null)
		{
			BackGroundManager.Instance.ChangeTheme();
		}
		Camera.main.GetComponent<BirdFollow>().ResetCam();
		if(gameOverPanel.gameObject.activeSelf)
		{
			gameOverPanel.gameObject.SetActive(false);
		}
		if(hudPanel.gameObject.activeSelf)
		{
			hudPanel.gameObject.SetActive(false);
		}
//		if(!homePanel.gameObject.activeSelf)
//		{
//			homePanel.gameObject.SetActive(true);
//		}
		if(!logoPanel.gameObject.activeSelf )
		{
			logoPanel.gameObject.SetActive(true);
		}

		if(tutorialReplay.gameObject.activeSelf)
		{
			tutorialReplay.gameObject.SetActive(false);
		}

	}


	public void InviteFrnds()
	{
//		Debug.Log("InviteFrnds");
		Main.Instance.PlayButtonClickSound();
//		Main.Instance.InviteFriends();
	}

	public void ShareScore()
	{
		Main.Instance.PlayButtonClickSound();
		Main.Instance.ShareUniversal(gameScore);
	}

	public void Replay()
	{
//		Main.Instance.PlayButtonClickSound();

		//Go to Home/Lobby 
//		if(!Main.Instance.isNoAdsPurchased)
//		{
//			HZBannerAd.hide();
//		}


		GameData.isRetry = true;
		if(BackGroundManager.Instance != null)
		{
			BackGroundManager.Instance.ChangeTheme();
		}
		Camera.main.GetComponent<BirdFollow>().ResetCam();

		if(gameOverPanel.gameObject != null)
		{
			if(gameOverPanel.gameObject.activeSelf)
			{
				gameOverPanel.gameObject.SetActive(false);
			}
		}
		else
		{
			GameObject.FindGameObjectWithTag("GOver").SetActive(false);
		}

		if(hudPanel.gameObject.activeSelf)
		{
			hudPanel.gameObject.SetActive(false);
		}
		if(homePanel.gameObject.activeSelf)
		{
			homePanel.gameObject.SetActive(false);
		}
		if(logoPanel.gameObject.activeSelf )
		{
			logoPanel.gameObject.SetActive(false);
		}

	}

	public void ContinueNO()
	{
		isReviveBreaked = true;
		StopCoroutine(Timer());
		CancelInvoke("CntDown");
		Main.Instance.PlayButtonClickSound();
		revivePanel.SetActive(false);
		GameEventManager.TriggerReviveCancel();
	}

	public void ContinueYES()
	{
		isReviveBreaked = true;
		CancelInvoke("CntDown");
		StopCoroutine(Timer());
		Main.Instance.PlayButtonClickSound();
		if(HZIncentivizedAd.isAvailable())
		{
			HZIncentivizedAd.Show();
		}
	}


	#endregion


	#region Nest Delegate & Scoring Mechanism
	void HandleEggCatched (int count)
	{
//		Debug.Log("HUD EGG SCORE");
		gameScore += count;
		scoreText.text = "" + gameScore;

	}
	#endregion

	public void SetUpRevive()
	{
		timerText.gameObject.transform.localScale = Vector3.zero;
		revivePanel.SetActive(true);
		StartCoroutine(Timer());
	}

	int timeLeft = 8;
	IEnumerator Timer()
	{
		yield return null;
		float t = 0f;
		timeLeft = 8;
		timerText.text = timeLeft.ToString();
		while (t < 0.7f) 
		{
			t += Time.deltaTime / 0.7f;
			timerText.gameObject.transform.localScale = Vector3.Lerp (timerText.gameObject.transform.localScale, Vector3.one, t); 
			yield return null;
		}  

		InvokeRepeating("CntDown", 1, 1);
//		yield return new WaitForSeconds(1f);
//		for(int i = 0; i<8; i++)
//		{
//			timeLeft -= 1;
//			timerText.text = timeLeft.ToString();
//			yield return new WaitForSeconds(1);
//			if(isReviveBreaked)
//				break;
//		}
//
//		if(!isReviveBreaked)
//		{
//			revivePanel.SetActive(false);
//			GameEventManager.TriggerReviveCancel();
//		}

	}

	void CntDown()
	{
		timeLeft -= 1;
		timerText.text = timeLeft.ToString();

		if(timeLeft == 0)
		{
			CancelInvoke("CntDown");
			revivePanel.SetActive(false);
			GameEventManager.TriggerReviveCancel();
		}
	}

	void Counter()
	{
		
	}

	public void GameOver()
	{
		

		GameData.isComingFromGameOverStore = true;
		Main.Instance.gameCount++;
		Main.Instance.sessionGameCount++;
		PlayerPrefs.SetInt(Constants.KEY_TOTAL_GAMES, Main.Instance.gameCount);
		Main.Instance.totalScoreOfSession += gameScore;
//		Debug.Log("GameOver");

		if(hudPanel.gameObject.activeSelf)
		{
			hudPanel.gameObject.SetActive(false);
		}



		if(!gameOverPanel.gameObject.activeSelf)
		{
			if (HZIncentivizedAd.isAvailable())
			{
				rewardButton.gameObject.SetActive(true);
			}
			else
			{
				rewardButton.gameObject.SetActive(false);
			}
			rewardConfirmImg.gameObject.SetActive(false);

			gameOverScoreText.text = "" + gameScore;
			if(gameScore > bestScore)
			{
				bestScore = gameScore;
				Main.Instance.PlayBestScoreSound();
				newBestText.gameObject.SetActive(true);
				PlayerPrefs.SetInt(Constants.KEY_BEST, bestScore);
				bestScoreText.text = "" + bestScore;

			}
			else
			{
//				Main.Instance.PlayGameOverSound();
				newBestText.gameObject.SetActive(false);
				bestScoreText.text = "" + bestScore;
			}

			//Add total eggs
			if(gameScore > 0)
			{
				Main.Instance.totalEggs += gameScore;
				PlayerPrefs.SetInt(Constants.KEY_TOTAL_EGGS, Main.Instance.totalEggs);

				GameData.lifeTimeEggs += gameScore;
				PlayerPrefs.SetInt(Constants.KEY_LIFE_TIME_EGGS, GameData.lifeTimeEggs);
			}

			totalEggs.text = "" + Main.Instance.totalEggs;


			if(Main.Instance.isNoAdsPurchased)
			{
				if(noAddButtonGameOver != null)
				{
					noAddButtonGameOver.interactable = false;
				}

			}

			//Save Data
			PlayerPrefs.Flush();

			Main.Instance.PostScoreToLeaderBoard( gameScore );
//			gameOverInstText.text = GameData.gameOverInstructionText;
			gameOverPanel.gameObject.SetActive(true);


			if(!tutorialReplay.gameObject.activeSelf)
			{
				tutorialReplay.gameObject.SetActive(true);
			}
		}

		CheckForQuests();

		if(Main.Instance.gameCount > 1 && Main.Instance.gameCount%3 == 0)
		{
			if(!Main.Instance.isNoAdsPurchased)
			{
				HZInterstitialAd.show();

				#if UNITY_IOS
				AppTrackerIOS.loadModule("inapp");
				#endif
			}

		}

		UniRate.Instance.LogEvent(true);

	}

	void CheckForQuests()
	{	
		//Related to Best Score
		if(bestScore >= 10)
		{
			if(!Main.Instance.questChecker.ACHIV_BEST_10)
			{
				PlayerPrefs.SetBool(Constants.QUEST_BEST_10, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_BEST_10);
			}
		}
		if(bestScore >= 20)
		{
			if(!Main.Instance.questChecker.ACHIV_BEST_20)
			{
				PlayerPrefs.SetBool(Constants.QUEST_BEST_20, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_BEST_20);
			}
		}
		if(bestScore >= 40)
		{
			if(!Main.Instance.questChecker.ACHIV_BEST_40)
			{
				PlayerPrefs.SetBool(Constants.QUEST_BEST_40, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_BEST_40);
			}
		}
		if(bestScore >= 80)
		{
			if(!Main.Instance.questChecker.ACHIV_BEST_80)
			{
				PlayerPrefs.SetBool(Constants.QUEST_BEST_80, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_BEST_80);
			}
		}

		if(bestScore >= 100)
		{
			if(!Main.Instance.questChecker.ACHIV_BEST_100)
			{
				PlayerPrefs.SetBool(Constants.QUEST_BEST_100, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_BEST_100);
			}
		}


		//Check Game count achievements 
		if(Main.Instance.gameCount == 10)
		{
			if(!Main.Instance.questChecker.ACHIV_GAMES_10)
			{
				PlayerPrefs.SetBool(Constants.QUEST_GAMES_10, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_GAMES_10);
			}
		}
		else if(Main.Instance.gameCount == 50)
		{
			if(!Main.Instance.questChecker.ACHIV_GAMES_50)
			{
				PlayerPrefs.SetBool(Constants.QUEST_GAMES_50, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_GAMES_50);
			}
		}
		else if(Main.Instance.gameCount == 100)
		{
			if(!Main.Instance.questChecker.ACHIV_GAMES_100)
			{
				PlayerPrefs.SetBool(Constants.QUEST_GAMES_100, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_GAMES_100);
			}
		}
		else if(Main.Instance.gameCount == 200)
		{
			if(!Main.Instance.questChecker.ACHIV_GAMES_200)
			{
				PlayerPrefs.SetBool(Constants.QUEST_GAMES_200, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_GAMES_200);
			}
		}
		else if(Main.Instance.gameCount == 500)
		{
			if(!Main.Instance.questChecker.ACHIV_GAMES_500)
			{
				PlayerPrefs.SetBool(Constants.QUEST_GAMES_500, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_GAMES_500);
			}
		}


		//Check Revive achievements 
		if(GameData.reviveCount == 5)
		{
			if(!Main.Instance.questChecker.ACHIV_REVIVE_5)
			{
				PlayerPrefs.SetBool(Constants.QUEST_REVIVE_5, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_REVIVE_5);
			}
		}
		else if(GameData.reviveCount == 25)
		{
			if(!Main.Instance.questChecker.ACHIV_REVIVE_25)
			{
				PlayerPrefs.SetBool(Constants.QUEST_REVIVE_25, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_REVIVE_25);
			}
		}
		else if(GameData.reviveCount == 50)
		{
			if(!Main.Instance.questChecker.ACHIV_REVIVE_50)
			{
				PlayerPrefs.SetBool(Constants.QUEST_REVIVE_50, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_REVIVE_50);
			}
		}
		else if(GameData.reviveCount == 100)
		{
			if(!Main.Instance.questChecker.ACHIV_REVIVE_100)
			{
				PlayerPrefs.SetBool(Constants.QUEST_REVIVE_100, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_REVIVE_100);
			}
		}
		else if(GameData.reviveCount == 150)
		{
			if(!Main.Instance.questChecker.ACHIV_REVIVE_150)
			{
				PlayerPrefs.SetBool(Constants.QUEST_REVIVE_150, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_REVIVE_150);
			}
		}

		//Check Life time eggs collected achievements 
		if(GameData.lifeTimeEggs >= 1000)
		{
			if(!Main.Instance.questChecker.ACHIV_EGGS_1000)
			{
				PlayerPrefs.SetBool(Constants.QUEST_EGGS_1000, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_EGGS_1000);
			}
		}
		else if(GameData.lifeTimeEggs >= 500)
		{
			if(!Main.Instance.questChecker.ACHIV_EGGS_500)
			{
				PlayerPrefs.SetBool(Constants.QUEST_EGGS_500, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_EGGS_500);
			}
		}
		else if(GameData.lifeTimeEggs >= 200)
		{
			if(!Main.Instance.questChecker.ACHIV_EGGS_200)
			{
				PlayerPrefs.SetBool(Constants.QUEST_EGGS_200, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_EGGS_200);
			}
		}
		else if(GameData.lifeTimeEggs >= 100)
		{
			if(!Main.Instance.questChecker.ACHIV_EGGS_100)
			{
				PlayerPrefs.SetBool(Constants.QUEST_EGGS_100, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_EGGS_100);
			}
		}
		else if(GameData.lifeTimeEggs >= 50)
		{
			if(!Main.Instance.questChecker.ACHIV_EGGS_50)
			{
				PlayerPrefs.SetBool(Constants.QUEST_EGGS_50, true);
				Main.Instance.questChecker.UpdateQuestStatus();
				Main.Instance.PostAchievement(Constants.QUEST_EGGS_50);
			}
		}

	}

	#region Unirate Delegate
	void OnPromptedForRating()
	{
//		Debug.Log("OnPromptedForRating");
		if(!ratePanel.gameObject.activeSelf)
		{
			ratePanel.gameObject.SetActive(true);
		}
	}
	#endregion




	#region StoreKit Delegates

	#if UNITY_IOS
	void productListReceivedEvent( List<StoreKitProduct> productList )
	{
		_products = productList;
	}

	void purchaseSuccessfulEvent( StoreKitTransaction transaction )
	{
		if(transaction.productIdentifier == Constants.PRODUCT_ID)
		{
			HZBannerAd.hide();
			Main.Instance.isNoAdsPurchased = true;
			PlayerPrefs.SetBool(Constants.KEY_NOADS, Main.Instance.isNoAdsPurchased);
			PlayerPrefs.Flush();

			if(Main.Instance.isNoAdsPurchased)
			{
				if(noAddButtonHome != null)
				{
					noAddButtonHome.interactable = false;
				}
			}

			if(Main.Instance.isNoAdsPurchased)
			{
				if(noAddButtonGameOver != null)
				{
					noAddButtonGameOver.interactable = false;
				}
			}
		}
	}

	#elif UNITY_ANDROID

//	void queryInventorySucceededEvent( List<GooglePurchase> purchases, List<GoogleSkuInfo> skus )
//	{
//		Debug.Log( string.Format( "queryInventorySucceededEvent. total purchases: {0}, total skus: {1}", purchases.Count, skus.Count ) );
//		Prime31.Utils.logObject( purchases );
//		Prime31.Utils.logObject( skus );
//		for(int i = 0; i < purchases.Count; i++)
//		{
//			Debug.Log("Order id = " + purchases[i].productId +"   State = "+ purchases[i].purchaseState); 
//			if(purchases[i].purchaseState == GooglePurchase.GooglePurchaseState.Purchased)
//			{
//				//Can Restore purchse
//				Debug.Log("Can Restore purchse");
//				canRestored = true;
//			}
//		}
//
//	}
//	
//	
//	void queryInventoryFailedEvent( string error )
//	{
//		Debug.Log( "queryInventoryFailedEvent: " + error );
//	}
//
//	void purchaseSucceededEvent( GooglePurchase purchase )
//	{
//		Debug.Log( "purchaseSucceededEvent: " + purchase );
//		if(purchase.productId == Constants.PRODUCT_ID)
//		{
//			Debug.Log("No ads purchased");
//			HZBannerAd.hide();
//			Main.Instance.isNoAdsPurchased = true;
//			PlayerPrefs.SetBool(Constants.KEY_NOADS, Main.Instance.isNoAdsPurchased);
//			PlayerPrefs.Flush();
//			
//			
//			if(isRestoring)
//			{
//				#if UNITY_ANDROID
//				EtceteraAndroid.showAlert("Purchase Restored", "Ads removed", "Ok");
//				#elif UNITY_IPHONE
//				CreateAlertForRestore("Purchase Restored","Ads removed", "Ok");
//				#endif
//				
//			}
//			
//			if(Main.Instance.isNoAdsPurchased)
//			{
//				if(noAddButtonHome != null)
//				{
//					noAddButtonHome.interactable = false;
//				}
//			}
//			
//			if(Main.Instance.isNoAdsPurchased)
//			{
//				if(noAddButtonGameOver != null)
//				{
//					noAddButtonGameOver.interactable = false;
//				}
//			}
//		}
//
//
//	}


	#endif
	#endregion




	#region Reward Video
	public void ShowRewardVideo()
	{
		if (HZIncentivizedAd.isAvailable()) 
		{
			GameData.isFreeGift = true;
			Main.Instance.MuteSounds();
			HZIncentivizedAd.show();
		}
	}

	HZIncentivizedAd.AdDisplayListener listener = delegate(string adState, string adTag)
	{
		if (adState.Equals ("incentivized_result_complete")) 
		{
			// The user has watched the entire video and should be given a reward.
//			Debug.Log("incentivized_result_complete");
			if(GameData.isFreeGift)
				HasWatchedVideo();
			else
				HasTobeRevived();

			GameData.isFreeGift = false;
			HZIncentivizedAd.fetch();
		}
		if (adState.Equals ("incentivized_result_incomplete")) 
		{
			// The user did not watch the entire video and should not be given a reward.
			Debug.Log("incentivized_result_incomplete");
		}
	};
	
	void AddEggCount()
	{
		rewardButton.gameObject.SetActive(false);
		rewardConfirmImg.gameObject.SetActive(true);
		Main.Instance.totalEggs += 10;
		totalEggs.text = "" + Main.Instance.totalEggs;
		PlayerPrefs.SetInt(Constants.KEY_TOTAL_EGGS, Main.Instance.totalEggs);
		PlayerPrefs.Flush();
		Main.Instance.UnMuteSounds();

	}

	void ReviveGame()
	{
		GameData.reviveCount++;
		PlayerPrefs.SetInt(Constants.KEY_REVIVE_CNT, GameData.reviveCount);
		Main.Instance.UnMuteSounds();
		revivePanel.SetActive(false);
		GameEventManager.TriggerReviveGame();
	}

	#endregion

	void ResetHud()
	{
		gameScore = 0;
		scoreText.text = "" + gameScore;
	}
}
