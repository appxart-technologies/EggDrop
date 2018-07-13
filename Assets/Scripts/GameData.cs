using UnityEngine;
using System.Collections.Generic;

public class GameData : MonoBehaviour {

	public static bool isMusic;
	public static bool isTutorial;
	public static int currentTheme;
	public static int currentBirdId;
	public static bool isBirdLive, isFreeGift;
	public static int nestID, reviveCount, lifeTimeEggs, unlockCount;
	public static int gameScore;
	public static bool isRetry;
	public static bool isComingFromStore;
	public static bool isComingFromGameOverStore;
	public static string gameOverInstructionText;
	public static List <GameObject> poles = new List<GameObject>();

}
