public static class GameEventManager 
{
	
	public delegate void GameEvent();
	
	public static event GameEvent GameInit, BirdReadyNow, RiviveCancel, RiviveGame,  GameReplay, GameOver, VideoWateched;

	public delegate void ShopEvent(int a);
	public static event ShopEvent CharacterSelected;



	public static void TriggerGameInit()
	{
		if(GameInit != null){
			GameInit();
		}
	}


	public static void TriggerBirdReadyNow()
	{
		if(BirdReadyNow != null)
		{
			BirdReadyNow();
		}
	}


	public static void TriggerGameOver()
	{
		if(GameOver != null)
		{
			GameOver();
		}
	}
		

	public static void TriggerVideoWateched()
	{
		if(VideoWateched != null)
		{
			VideoWateched();
		}
	}

	public static void TriggerReviveCancel()
	{
		if(RiviveCancel != null)
		{
			RiviveCancel();
		}
	}

	public static void TriggerReviveGame()
	{
		if(RiviveGame != null)
		{
			RiviveGame();
		}
	}
}
