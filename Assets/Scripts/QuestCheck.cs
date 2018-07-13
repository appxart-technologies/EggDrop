using UnityEngine;
using System.Collections;
using PlayerPrefs = PreviewLabs.PlayerPrefs;

public class QuestCheck : MonoBehaviour {

	[HideInInspector]
	public bool ACHIV_GAMES_10, ACHIV_GAMES_50, ACHIV_GAMES_100, ACHIV_GAMES_200, ACHIV_GAMES_500;
	[HideInInspector]
	public bool	ACHIV_EGGS_50, ACHIV_EGGS_100, ACHIV_EGGS_200, ACHIV_EGGS_500, ACHIV_EGGS_1000;
	[HideInInspector]
	public bool	ACHIV_BEST_10, ACHIV_BEST_20, ACHIV_BEST_40, ACHIV_BEST_80, ACHIV_BEST_100;
	[HideInInspector]
	public bool	ACHIV_REVIVE_5, ACHIV_REVIVE_25, ACHIV_REVIVE_50, ACHIV_REVIVE_100, ACHIV_REVIVE_150;
	[HideInInspector]
	public bool	ACHIV_UNLOCK_5, ACHIV_UNLOCK_9, ACHIV_SHARE;

	// Use this for initialization
	void Start () 
	{
		UpdateQuestStatus();
	}

	public void UpdateQuestStatus()
	{
		ACHIV_GAMES_10 = PlayerPrefs.GetBool(Constants.QUEST_GAMES_10, false);
		ACHIV_GAMES_50 = PlayerPrefs.GetBool(Constants.QUEST_GAMES_50, false);
		ACHIV_GAMES_100 = PlayerPrefs.GetBool(Constants.QUEST_GAMES_100, false);
		ACHIV_GAMES_200 = PlayerPrefs.GetBool(Constants.QUEST_GAMES_200, false);
		ACHIV_GAMES_500 = PlayerPrefs.GetBool(Constants.QUEST_GAMES_500, false);
		ACHIV_EGGS_50 = PlayerPrefs.GetBool(Constants.QUEST_EGGS_50, false);
		ACHIV_EGGS_100 = PlayerPrefs.GetBool(Constants.QUEST_EGGS_100, false);
		ACHIV_EGGS_200 = PlayerPrefs.GetBool(Constants.QUEST_EGGS_200, false);
		ACHIV_EGGS_500 = PlayerPrefs.GetBool(Constants.QUEST_EGGS_500, false);
		ACHIV_EGGS_1000 = PlayerPrefs.GetBool(Constants.QUEST_EGGS_1000, false);
		ACHIV_BEST_10 = PlayerPrefs.GetBool(Constants.QUEST_BEST_10, false);
		ACHIV_BEST_20 = PlayerPrefs.GetBool(Constants.QUEST_BEST_20, false);
		ACHIV_BEST_40 = PlayerPrefs.GetBool(Constants.QUEST_BEST_40, false);
		ACHIV_BEST_80 = PlayerPrefs.GetBool(Constants.QUEST_BEST_80, false);
		ACHIV_BEST_100 = PlayerPrefs.GetBool(Constants.QUEST_BEST_100, false);
		ACHIV_REVIVE_5 = PlayerPrefs.GetBool(Constants.QUEST_REVIVE_5, false);
		ACHIV_REVIVE_25 = PlayerPrefs.GetBool(Constants.QUEST_REVIVE_25, false);
		ACHIV_REVIVE_50 = PlayerPrefs.GetBool(Constants.QUEST_REVIVE_50, false);
		ACHIV_REVIVE_100 = PlayerPrefs.GetBool(Constants.QUEST_REVIVE_100, false);
		ACHIV_REVIVE_150 = PlayerPrefs.GetBool(Constants.QUEST_REVIVE_150, false);
		ACHIV_UNLOCK_5 = PlayerPrefs.GetBool(Constants.QUEST_UNLOCK_5, false);
		ACHIV_UNLOCK_9 = PlayerPrefs.GetBool(Constants.QUEST_UNLOCK_9, false);
		ACHIV_SHARE = PlayerPrefs.GetBool(Constants.QUEST_SHARE, false);

		PlayerPrefs.Flush();
	}

	public void PostAchievements()
	{
		if(ACHIV_GAMES_10)
		{
			Main.Instance.PostAchievement(Constants.QUEST_GAMES_10);
		}
		if(ACHIV_GAMES_50)
		{
			Main.Instance.PostAchievement(Constants.QUEST_GAMES_50);
		}
		if(ACHIV_GAMES_100)
		{
			Main.Instance.PostAchievement(Constants.QUEST_GAMES_100);
		}
		if(ACHIV_GAMES_200)
		{
			Main.Instance.PostAchievement(Constants.QUEST_GAMES_200);
		}
		if(ACHIV_GAMES_500)
		{
			Main.Instance.PostAchievement(Constants.QUEST_GAMES_500);
		}

		if(ACHIV_EGGS_50)
		{
			Main.Instance.PostAchievement(Constants.QUEST_EGGS_50);
		}
		if(ACHIV_EGGS_100)
		{
			Main.Instance.PostAchievement(Constants.QUEST_EGGS_100);
		}
		if(ACHIV_EGGS_200)
		{
			Main.Instance.PostAchievement(Constants.QUEST_EGGS_200);
		}
		if(ACHIV_EGGS_500)
		{
			Main.Instance.PostAchievement(Constants.QUEST_EGGS_500);
		}
		if(ACHIV_EGGS_1000)
		{
			Main.Instance.PostAchievement(Constants.QUEST_EGGS_1000);
		}

		if(ACHIV_BEST_10)
		{
			Main.Instance.PostAchievement(Constants.QUEST_BEST_10);
		}
		if(ACHIV_BEST_20)
		{
			Main.Instance.PostAchievement(Constants.QUEST_BEST_20);
		}
		if(ACHIV_BEST_40)
		{
			Main.Instance.PostAchievement(Constants.QUEST_BEST_40);
		}
		if(ACHIV_BEST_80)
		{
			Main.Instance.PostAchievement(Constants.QUEST_BEST_80);
		}
		if(ACHIV_BEST_100)
		{
			Main.Instance.PostAchievement(Constants.QUEST_BEST_100);
		}

		if(ACHIV_REVIVE_5)
		{
			Main.Instance.PostAchievement(Constants.QUEST_REVIVE_5);
		}
		if(ACHIV_REVIVE_25)
		{
			Main.Instance.PostAchievement(Constants.QUEST_REVIVE_25);
		}
		if(ACHIV_REVIVE_50)
		{
			Main.Instance.PostAchievement(Constants.QUEST_REVIVE_50);
		}
		if(ACHIV_REVIVE_100)
		{
			Main.Instance.PostAchievement(Constants.QUEST_REVIVE_100);
		}
		if(ACHIV_REVIVE_150)
		{
			Main.Instance.PostAchievement(Constants.QUEST_REVIVE_150);
		}

		if(ACHIV_UNLOCK_5)
		{
			Main.Instance.PostAchievement(Constants.QUEST_UNLOCK_5);
		}
		if(ACHIV_UNLOCK_9)
		{
			Main.Instance.PostAchievement(Constants.QUEST_UNLOCK_9);
		}

		if(ACHIV_SHARE)
		{
			Main.Instance.PostAchievement(Constants.QUEST_SHARE);
		}
	}
}
