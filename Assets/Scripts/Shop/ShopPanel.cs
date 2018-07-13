using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour 
{


	public Text totalEggs;
	public Text priceText;
	public Text tapText;
	public Image priceEggIcon;
	public Image totalEggIcon;


	void OnEnable()
	{
		totalEggs.text = Main.Instance.totalEggs + "";
		GameData.isComingFromStore = true;
		BackGroundManager.Instance.HandleRemoveAllEffectsNow();
		MenuBird.BirdSelectedNow += UpdatePriceText;
		MenuBird.DontHaveEnoughEggs += ShowLessMoneyText;

		priceText.gameObject.SetActive(false);
		tapText.gameObject.SetActive(false);
		priceEggIcon.gameObject.SetActive(false);
		UpdateEggIcon();
	}

	void OnDisable()
	{
		MenuBird.BirdSelectedNow -= UpdatePriceText;
		MenuBird.DontHaveEnoughEggs -= ShowLessMoneyText;
	}


	// Use this for initialization
	void Start () 
	{
		totalEggs.text = Main.Instance.totalEggs + "";
		GameData.isComingFromStore = true;
		BackGroundManager.Instance.HandleRemoveAllEffectsNow();

	}
	

	public void PlayGame()
	{
		Main.Instance.PlayButtonClickSound();
		if(GameData.isComingFromGameOverStore)
		{
			BackGroundManager.Instance.ChangeTheme();
		}
		Application.LoadLevel(1);

	}

	void UpdatePriceText(int price)
	{
		UpdateEggIcon();
		priceEggIcon.gameObject.SetActive(true);
		priceText.gameObject.SetActive(true);
		tapText.text = "Tap the bird to purchase";
		tapText.gameObject.SetActive(true);
		priceText.text = price + " required to unlock";
	}

	void ShowLessMoneyText()
	{
		tapText.text = "Need more eggs to purchase";
//		tapText.gameObject.SetActive(true);
	}

	void UpdateEggIcon()
	{
		int eggId = 2;
		if(MenuBird.currentSelction == 1 || MenuBird.currentSelction == 3)
		{
			eggId = 3;
		}
		else if(MenuBird.currentSelction == 2 || MenuBird.currentSelction == 5)
		{
			eggId = 0;
		}
		else if(MenuBird.currentSelction == 4 || MenuBird.currentSelction == 6 )
		{
			eggId = 5;
		}
		else if(MenuBird.currentSelction == 9)
		{
			eggId = 4;
		}
		else if(MenuBird.currentSelction == 7 )
		{
			eggId = 1;
		}
		else if(MenuBird.currentSelction == 8 )
		{
			eggId = 2;
		}
		string eggName = "egg_" + eggId;
		if(priceEggIcon != null)
		{
//			Debug.Log("Textures/Game/Eggs/" + eggName);
			priceEggIcon.sprite = Resources.Load <Sprite> ("Textures/Game/Eggs/" + eggName) as Sprite;
		}

		if(totalEggIcon != null)
		{
			//			Debug.Log("Textures/Game/Eggs/" + eggName);
			totalEggIcon.sprite = Resources.Load <Sprite> ("Textures/Game/Eggs/" + eggName) as Sprite;
		}
	}

}
