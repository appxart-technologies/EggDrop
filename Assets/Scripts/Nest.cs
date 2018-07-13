using UnityEngine;
using System.Collections;

/// <summary>
/// Pole.
/// Handles behavior of pole.
/// </summary>
public class Nest : MonoBehaviour {

	/// <summary>
	/// The star effect.
	/// </summary>
	public GameObject starEffect;

	public GameObject comboEffect;

	/// <summary>
	/// The have egg.
	/// </summary>
	private bool haveEgg;

	public bool HaveEgg
	{
		set{ haveEgg = value; }
		get{ return haveEgg; }
	}

	/// <summary>
	/// The type of the nest movment.
	/// </summary>
	private NestMoveType nestMovmentType;

	public NestMoveType NestMovementType
	{
		set{ nestMovmentType = value; }
		get{ return nestMovmentType; }
	}


	/// <summary>
	/// The egg count.
	/// </summary>
	private int eggCount;

	private bool isEggsChecked;

	private Camera mainCamera;
	private Transform parent;

	/// <summary>
	/// The initial position of pole.
	/// </summary>
	Vector3 initialPosPole;

	/// <summary>
	/// The range horizontal.
	/// </summary>
	float rangeHorizontal;

	/// <summary>
	/// The range vertical.
	/// </summary>
	float rangeVertical;

	/// <summary>
	/// Occurs when egg catched.
	/// </summary>
	public delegate void EggCollected(int count);
	public static event EggCollected EggCatched;


	// Use this for initialization
	void Start () 
	{
		eggCount = 0;
		rangeHorizontal = Random.Range(0.5f, 1.0f);
		rangeVertical = Random.Range(0.5f, 1.3f);
		isEggsChecked = false;
		parent = transform.parent;
		mainCamera = Camera.main;
		initialPosPole = parent.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 viewPos = mainCamera.WorldToViewportPoint(transform.position);
		if (viewPos.x < -0.1F)
		{
			//Deactivate pole (Parent of nest) as it is no more visible on screen
			parent.gameObject.SetActive(false);
			Destroy(parent.gameObject);
			GameData.poles.Remove(parent.gameObject);
		}

		if(!isEggsChecked && viewPos.x < 0.1F)
		{
			isEggsChecked = true;
			Check();
		}

		//Movement of nest
		switch(nestMovmentType)
		{
			case NestMoveType.Sideways:
			parent.position = initialPosPole + new Vector3(Mathf.PingPong(Time.time, rangeHorizontal), 0, 0);
				break;

			case NestMoveType.Vertical:
			parent.position = initialPosPole + new Vector3(0, Mathf.PingPong(Time.time, rangeVertical), 0);
				break;
		}
			
	}


	#region Collision Delegates
//	void OnCollisionEnter2D(Collision2D collInfo)
//	{
//
//	}
//
//	void OnCollisionExit2D(Collision2D collInfo)
//	{
//
//
//	}

	void OnTriggerEnter2D(Collider2D collInfo)
	{
		if(collInfo.gameObject.tag == Constants.TAG_EGG)
		{
//			Debug.Log("Egg enterr =  " + transform.parent.name);
			Main.Instance.PlayEggCatchedSound();
			Rigidbody2D eggBody =  collInfo.gameObject.transform.GetComponent<Rigidbody2D>();
			if(eggBody != null)
			{
				starEffect.SetActive(true);
				Invoke("RemoveEffect",2.0f);
				eggBody.velocity = Vector2.zero;
				collInfo.gameObject.tag = Constants.TAG_CKECKED;
				collInfo.gameObject.transform.parent = transform;
//				Destroy()

				if(!GameData.isTutorial && EggCatched != null)
				{
					if(GameData.isBirdLive)
					{
						EggCatched(1);
					}
				}
				else
				{
					GameObject gameBird = GameObject.FindGameObjectWithTag(Constants.TAG_BIRD);
					if(gameBird != null)
					{
						gameBird.SendMessage("TutorialOver");
					}
				}

			}
			eggCount++;
			if(eggCount > 1 && GameData.isBirdLive &&  !GameData.isTutorial)
			{
				comboEffect.GetComponent<TextMesh>().text = "+ " + eggCount*eggCount;
				Main.Instance.PlayComboSound();
				comboEffect.SetActive(true);

				if(eggCount == 2)
				{
					int pointCombo = eggCount*eggCount - eggCount;
					if(EggCatched != null)
					{
						EggCatched(pointCombo);
					}
				}
				else if(eggCount == 3)
				{
					int pointTripplet = eggCount*eggCount - eggCount - 2;
					if(EggCatched != null)
					{
						EggCatched(pointTripplet);
					}
				}

			}
		}
		
	}
	#endregion


	/// <summary>
	/// Check if egg is catched or not by nest.
	/// </summary>
	void Check()
	{

//		Debug.Log("Check egg count");
		if(eggCount <= 0)
		{
			Main.Instance.PlayPoleMissSound();
			GameData.isBirdLive = false;
			starEffect.SetActive(false);

//			Blink();
			GameObject gameBird = GameObject.FindGameObjectWithTag(Constants.TAG_BIRD);
			if(gameBird != null)
			{
//				Debug.Log("Egg die = " + transform.parent.name);
				GameData.gameOverInstructionText = "You forgot drop egg in a nest";
//				Debug.Log("Die from here nest - " + parent.name);
				gameBird.SendMessage("Die");
			}
		}
//		else
//		{
//			int pointCombo = eggCount*eggCount - eggCount;
//			if(EggCatched != null)
//			{
//				EggCatched(pointCombo);
//			}
//		}
	}


	/// <summary>
	/// Removes the star effect.
	/// </summary>
	void RemoveEffect()
	{
		starEffect.SetActive(false);
	}

	void Blink()
	{
		if(!parent.gameObject.activeSelf)
		{
			parent.gameObject.SetActive(true);
		}
		else
		{
			parent.gameObject.SetActive(false);
		}

		Invoke("Blink", 0.01f);
	}
}

public enum NestMoveType
{
	None = 0,
	Sideways,
	Vertical,
	Count
}
