using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour {

	//Game Services

	#if UNITY_ANDROID
	public const string LEADER_BOARD_ID = "CgkI8b-_5ZsaEAIQFw";
	public const string PRODUCT_ID = "android.test.purchased";

	public const string QUEST_GAMES_10 = "CgkI8b-_5ZsaEAIQAA";
	public const string QUEST_GAMES_50 = "CgkI8b-_5ZsaEAIQAQ";
	public const string QUEST_GAMES_100 = "CgkI8b-_5ZsaEAIQAg";
	public const string QUEST_GAMES_200 = "CgkI8b-_5ZsaEAIQAw";
	public const string QUEST_GAMES_500 = "CgkI8b-_5ZsaEAIQBA";
	public const string QUEST_EGGS_50 = "CgkI8b-_5ZsaEAIQBQ";
	public const string QUEST_EGGS_100 = "CgkI8b-_5ZsaEAIQBg";
	public const string QUEST_EGGS_200 = "CgkI8b-_5ZsaEAIQBw";
	public const string QUEST_EGGS_500 = "CgkI8b-_5ZsaEAIQCA";
	public const string QUEST_EGGS_1000 = "CgkI8b-_5ZsaEAIQCQ";
	public const string QUEST_BEST_10 = "CgkI8b-_5ZsaEAIQCg";
	public const string QUEST_BEST_20 = "CgkI8b-_5ZsaEAIQCw";
	public const string QUEST_BEST_40 = "CgkI8b-_5ZsaEAIQDA";
	public const string QUEST_BEST_80 = "CgkI8b-_5ZsaEAIQDQ";
	public const string QUEST_BEST_100 = "CgkI8b-_5ZsaEAIQDg";
	public const string QUEST_REVIVE_5 = "CgkI8b-_5ZsaEAIQDw";
	public const string QUEST_REVIVE_25 = "CgkI8b-_5ZsaEAIQEA";
	public const string QUEST_REVIVE_50 = "CgkI8b-_5ZsaEAIQEQ";
	public const string QUEST_REVIVE_100 = "CgkI8b-_5ZsaEAIQEg";
	public const string QUEST_REVIVE_150 = "CgkI8b-_5ZsaEAIQEw";
	public const string QUEST_UNLOCK_5 = "CgkI8b-_5ZsaEAIQFA";
	public const string QUEST_UNLOCK_9 = "CgkI8b-_5ZsaEAIQFQ";
	public const string QUEST_SHARE = "CgkI8b-_5ZsaEAIQFg";



	#elif UNITY_IOS

//	public const string LEADER_BOARD_ID = "com_voyager_games_eggdrop";
	public const string LEADER_BOARD_ID = "com.appsolutegames.dropbirddrop.leaderboard";
	public const string PRODUCT_ID = "com.appsolutegames.dropbirddrop.noads";
	#endif

	//Tags
	public const string TAG_EGG = "Egg";
	public const string TAG_CKECKED = "Checked";
	public const string TAG_BIRD = "Bird";
	public const string TAG_NEST = "Nest";
	public const string TAG_GAME_HUD = "Hud";

	//Noification related constants
	public const int H4 = 4;
	public const int H8 = 8;
	public const int D1 = 1;
	public const int D3 = 3;
	public const int D7 = 7;
	public const int D14 = 14;
	public const int D30 = 30;

	//Game play constants
	/// <summary>
	/// The BIRD's initial speed in terms of world width percentage.
	/// </summary>
	public const float BIRD_SPEED = 30.0f;	//25//35
	public const int ACCELERATE_START_COUNT = 29;
	public const float ACCELERATION = 0.1f;
	public const float GRAVITY_ACCELERATION = 0.2f;

	public const int ENVIRNMENT_CHANGE_COUNT = 10;
	public const int EXTREME_CHANGE_COUNT = 20;
	public const int RESET_COUNT = 30;


	// Z order of clouds & Water & Poles
	public const float Z_BACK_GROUND = 100;
	public const float Z_BACK_CLOUD = 95;
	public const float Z_FRONT_CLOUD = 90;
	public const float Z_BACK_WATER = 85;
	public const float Z_MID_WATER = 80;
	public const float Z_FRONT_WATER = 75;
	public const float Z_PLOES = 81;
	public const float Z_EGGS = 83;

	//Tiling of themes
	public static Vector2 WATER_TILING = new Vector2(7,1);
	public static Vector2 SNOW_TILING = new Vector2(3,1);
	public static Vector2 DESERT_TILING = new Vector2(3,1);



	//Keys for Data Save
	public const string KEY_BEST = "Best";
	public const string KEY_BIRD_ID = "PlayBird";
	public const string KEY_TOTAL_EGGS = "EggsT";
	public const string KEY_MUSIC = "Sound";
	public const string KEY_NOADS = "NoAds";
	public const string KEY_REVIVE_CNT = "Revive";
	public const string KEY_LIFE_TIME_EGGS = "LifeEgg";
	public const string KEY_UNLOCK_CNT = "BirdUnlock";


	public const string KEY_FIRST_TIME = "NewUser";
	public const string KEY_TUTORIAL = "Tutorial";
	public const string KEY_TOTAL_GAMES = "GamesCnt";

}
