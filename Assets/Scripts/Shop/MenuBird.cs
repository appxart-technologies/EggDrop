using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class MenuBird : MonoBehaviour {

	public int birdId;
	public int price;
	public bool isPurchased;
	public Image bgImage;
	public Image birdIcon;
	private Button birdButton;

	public static int currentSelction;

	private bool isSelected;
	string purchaseKey;

	public delegate void BirdSelected (int price);
	public static event BirdSelected BirdSelectedNow;

	public static event Action DontHaveEnoughEggs;

	public void OnEnable ()
	{
		MenuBird.BirdSelectedNow += BirdSelectionChange;
		purchaseKey = "Bird_" + birdId;
		GetPurchaseStatus();
	}
	
	void OnDisable()
	{
		MenuBird.BirdSelectedNow -= BirdSelectionChange;
		
	}

	// Use this for initialization
	void Start () 
	{
		birdButton = transform.GetComponent<Button>();
		purchaseKey = "Bird_" + birdId;
		GetPurchaseStatus();

	}

	void GetPurchaseStatus()
	{
		if(birdId == 0)
		{
			isPurchased = true;
		}
		else
		{
			isPurchased = PlayerPrefs.GetBool(purchaseKey, false);
		}
		
		if(!isPurchased)
		{
			birdIcon.color = new Color(0, 0, 0, 100/255.0f); 
		}
		else
		{
			birdIcon.color = new Color(1, 1, 1); 
			if(GameData.currentBirdId == birdId)
			{
				currentSelction = GameData.currentBirdId;
				isSelected = true;
				bgImage.color = new Color(58/255.0f, 191/255.0f, 236/255.0f);
			}
		}
	}

	
	public void BirdSelect(int id)
	{
		Main.Instance.PlayButtonClickSound();
		if(birdId == id && !isSelected)
		{
			isSelected = true;
			currentSelction = id;
			bgImage.color = new Color(58/255.0f, 191/255.0f, 236/255.0f);
//			birdIcon.color = new Color(0, 0, 0, 100/255.0f); 
			birdIcon.color = new Color(1, 1, 1); 
			if(isPurchased)
			{
				GameData.currentBirdId = birdId;
				PlayerPrefs.SetInt(Constants.KEY_BIRD_ID, GameData.currentBirdId);
				if(GameData.isComingFromGameOverStore)
				{
					BackGroundManager.Instance.ChangeTheme();
				}
				Application.LoadLevel(1);
			}
			else
			{
				BirdSelectedNow(price);
			}
			
		}
		else if(birdId == id && isSelected )
		{
			if(!isPurchased && Main.Instance.totalEggs >= price)
			{
				isPurchased = true;
				Main.Instance.totalEggs -= price;
				PlayerPrefs.SetInt(Constants.KEY_TOTAL_EGGS, Main.Instance.totalEggs);
				PlayerPrefs.SetBool(purchaseKey, isPurchased);
				GameData.currentBirdId = birdId;
				PlayerPrefs.SetInt(Constants.KEY_BIRD_ID, GameData.currentBirdId);
				if(GameData.isComingFromGameOverStore)
				{
					BackGroundManager.Instance.ChangeTheme();
				}
				GameData.unlockCount++;
				PlayerPrefs.SetInt(Constants.KEY_UNLOCK_CNT, GameData.unlockCount);
				if(GameData.unlockCount == 9)
				{
					if(!Main.Instance.questChecker.ACHIV_UNLOCK_9)
					{
						PlayerPrefs.SetBool(Constants.QUEST_UNLOCK_9, true);
						Main.Instance.questChecker.UpdateQuestStatus();
						Main.Instance.PostAchievement(Constants.QUEST_UNLOCK_9);
					}
				}
				else if(GameData.unlockCount >= 5)
				{
					if(!Main.Instance.questChecker.ACHIV_UNLOCK_5)
					{
						PlayerPrefs.SetBool(Constants.QUEST_UNLOCK_5, true);
						Main.Instance.questChecker.UpdateQuestStatus();
						Main.Instance.PostAchievement(Constants.QUEST_UNLOCK_5);
					}
				}
				Application.LoadLevel(1);
			}
			else if(isPurchased)
			{
				GameData.currentBirdId = birdId;
				PlayerPrefs.SetInt(Constants.KEY_BIRD_ID, GameData.currentBirdId);
				if(GameData.isComingFromGameOverStore)
				{
					BackGroundManager.Instance.ChangeTheme();
				}
				Application.LoadLevel(1);
			}
			else
			{
				DontHaveEnoughEggs();
			}

		}


	}

	void BirdSelectionChange(int price)
	{
		if(birdId != currentSelction)
		{
			isSelected = false;
			bgImage.color = new Color(209/255.0f, 212/255.0f, 217/255.0f);
			if(!isPurchased)
			{
				birdIcon.color = new Color(0, 0, 0, 100/255.0f); 
			}
		}
	}
}
