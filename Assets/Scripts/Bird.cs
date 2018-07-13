using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using PlayerPrefs = PreviewLabs.PlayerPrefs;
using Heyzap;

/// <summary>
/// The Bird game object.
/// This class is responsible all actions by Bird. 
/// </summary>
public class Bird : MonoBehaviour {



	/// <summary>
	/// The egg root.
	/// Parent object of all eggs.
	/// </summary>
	GameObject eggRoot;

	/// <summary>
	/// The gravity scale.
	/// </summary>
	float gravityScale;

	/// <summary>
	/// The forward movement speed.
	/// Bird flight speed.
	/// </summary>
	float forwardMovementSpeed; 

	/// <summary>
	/// Current state of the Bird.
	/// </summary>
	BirdState m_eCurrentState;

	/// <summary>
	/// Previous state of the Bird.
	/// </summary>
	BirdState m_ePreviousState;

	GameHud gameHudMenu;


	public delegate void ReplayGame();
	public static event ReplayGame ReplayNow;

	/// <summary>
	/// The flying position.
	/// </summary>
	Vector3 flyingPos;

	/// <summary>
	/// The menu bird position.
	/// </summary>
	Vector3 menuBirdPosition;

	/// <summary>
	/// The transition direction to take flying position.
	/// </summary>
	Vector3 transitionDirection;

	Vector3 dyingDirection;

	/// <summary>
	/// The egg ID.
	/// </summary>
	int eggID;

	/// <summary>
	/// The bird transform.
	/// </summary>
	Transform birdTransform;

	/// <summary>
	/// The bird animator.
	/// </summary>
	Animator birdAnimator;

	int eggCount;

	GameObject currentEgg;

	bool isDieDelay, isRevived;

	void OnEnable()
	{
		GameEventManager.RiviveCancel += MoveToDie;
		GameEventManager.RiviveGame += SetUpRevive;
	}

	void OnDisable()
	{
		GameEventManager.RiviveCancel -= MoveToDie;
		GameEventManager.RiviveGame -= SetUpRevive;
	}

	// Use this for initialization
	void Start () 
	{
		
//		SetState(BirdState.Init);
		Init();
	}

	void OnDestroy()
	{
		GameEventManager.RiviveCancel -= MoveToDie;
		GameEventManager.RiviveGame -= SetUpRevive;
		Destroy(eggRoot);
	}


	/// <summary>
	/// Init this instance.
	/// </summary>
	public void Init()
	{
		isRevived = false;
		GameData.isBirdLive = true;
		isDieDelay = false;
		eggRoot = new GameObject("EggRoot");
		GameObject hudMenuObject = GameObject.FindGameObjectWithTag(Constants.TAG_GAME_HUD);
		if(hudMenuObject != null)
		{
			gameHudMenu = hudMenuObject.GetComponent<GameHud>();
		}

//		eggID = (int)BackGroundManager.Instance.GetThemeID();
		string eggName = "Egg_" + GameData.currentBirdId;
		currentEgg = Instantiate (Resources.Load ("Prefabs/AllEggs/" + eggName ),
		                           new Vector3(0, 20, 0), Quaternion.identity ) as GameObject;
		currentEgg.name = "EggMasterCopy";
		currentEgg.transform.parent = eggRoot.transform;

		eggCount = 0;
		birdTransform = transform;
		birdAnimator = transform.GetComponent<Animator>();
		forwardMovementSpeed = Utility.GetBirdInitialSpeed();
//		Debug.Log("Witdth  = " + Screen.width +" Screen hieght = "+ Screen.height);
//		Debug.Log("forwardMovementSpeed = " + forwardMovementSpeed + "World wodth = "+  Utility.WorldWidth());
		//Adjust Speed w.r.t screen aspect ratio
		float aspectRatio = (float)Screen.width/(float)Screen.height;
		float referenceRatio = 1280/800.0f;
//		Debug.Log("aspectRatio= " + aspectRatio + "  referenceRatio = "+ referenceRatio );
		if(aspectRatio < referenceRatio)
		{
//			forwardMovementSpeed = forwardMovementSpeed*(referenceRatio/aspectRatio);
//			Debug.Log("Modified forwardMovementSpeed = "+ forwardMovementSpeed); 
		}
		flyingPos = Utility.GetBirdFlyingPosition();
		menuBirdPosition = Utility.GetBirdMenuPosition();
		transitionDirection = flyingPos - menuBirdPosition;

		gravityScale = 	2.5f;  //		1.35f;
//		if(GameData.isRetry)
//		{
//			Invoke("Menu", 0.10f);
//		}
//		else
//		{
//			Invoke("Menu", 1.0f);
//		}

	}

//	void FixedUpdate () 
//	{
//	}

	// Update is called once per frame
	void Update () 
	{

		switch(m_eCurrentState)
		{
			case BirdState.Spwan:

				if(birdTransform.position.x < menuBirdPosition.x)
				{
					birdTransform.Translate(Vector3.right*forwardMovementSpeed*1.0F*Time.fixedDeltaTime);
				}
				else
				{
					if(GameData.isTutorial)
					{
						SetState(BirdState.Tutorial);
					}
					else
					{
						if(GameData.isRetry)
						{
							SetState(BirdState.Transition);
						}
						else
						{
							SetState(BirdState.Init);
						}
					}
					
				}

			break;

			case BirdState.Tutorial:
				if(Input.GetMouseButtonDown(0))
				{
					DropAnEgg();
				}
				break;

			case BirdState.Init:
				if(Input.GetMouseButtonDown(0))
				{
					#if UNITY_EDITOR
					if(!EventSystem.current.IsPointerOverGameObject())
					{
						SetState(BirdState.Transition);
					}
					#else 
					if(!EventSystem.current.IsPointerOverGameObject(0))
					{
						SetState(BirdState.Transition);
					}
					#endif

				}
				break;

			case BirdState.Transition:
				if(birdTransform.position.y < flyingPos.y)
				{
	//				birdTransform.position = Vector3.MoveTowards(birdTransform.position,flyingPos, 0.1f);
					birdTransform.Translate(transitionDirection*forwardMovementSpeed *0.4f*Time.fixedDeltaTime);
				}
				else
				{
					SetState(BirdState.Flying);
				}
				break;
			
			case BirdState.Flying:
				birdTransform.Translate(Vector3.right*forwardMovementSpeed*Time.fixedDeltaTime);
				if(Input.GetMouseButtonDown(0))
				{
					DropAnEgg();
				}
				break;
				
			case BirdState.Died:
				
//				Debug.Log("Application.platform = " + Application.platform);
				if ( Application.platform == RuntimePlatform.Android ||  Application.platform == RuntimePlatform.IPhonePlayer) 
				{
					if(isDieDelay && Input.GetMouseButtonDown(0))
					{
						if(!EventSystem.current.IsPointerOverGameObject(0))
						{
							if(ReplayNow != null)
							{
								ReplayNow();
							}
						}
					}
					
				}
				else if (Application.platform == RuntimePlatform.WindowsEditor 
				         || Application.platform == RuntimePlatform.OSXEditor) 
				{

					if(isDieDelay && Input.GetMouseButtonDown(0))
					{
						if(!EventSystem.current.IsPointerOverGameObject())
						{
							if(ReplayNow != null)
							{
								ReplayNow();
							}
						}
					}
				}

				break;
		}


	}


	#region Bird State Transitions
	void SetState(BirdState newState)
	{
		m_ePreviousState = m_eCurrentState;
		m_eCurrentState = newState;
		if(m_eCurrentState != m_ePreviousState)
		{
			StateChange();
		}

	}

	void StateChange()
	{
//		Debug.Log("m_eCurrentState = " + m_eCurrentState);
		switch(m_eCurrentState)
		{
		case BirdState.Spwan:
			break;

		case BirdState.Init:

				if(m_ePreviousState != BirdState.TutorialComplete && gameHudMenu != null)
				{
					gameHudMenu.ActivateHomePanel();
				}

			break;

		case BirdState.Tutorial:
			if(GameData.isTutorial)
			{
				if(gameHudMenu != null)
				{
					gameHudMenu.ActivateTutorial();
				}
			}
			break;

		case BirdState.TutorialComplete:
			if(gameHudMenu != null)
			{
				gameHudMenu.ChangeHintText();
			}

			SetState(BirdState.Init);
			break;

		case BirdState.Transition:
			if(gameHudMenu != null)
			{
				Main.Instance.PlayButtonClickSound();
				gameHudMenu.StarGame();
			}
			break;


		case BirdState.Flying:
			GameEventManager.TriggerBirdReadyNow();
			//Start background move also
			if(BackGroundManager.Instance != null)
			{
				BackGroundManager.Instance.StartMove();
			}

			break;


		case BirdState.WaitForRevive:
			foreach (Transform childegg in eggRoot.transform)
			{
				childegg.tag = "Untagged";
			}
			isRevived = true;
//			Debug.Log("WaitForRevive");
			BirdFollow.isBirdReady = false;
			if(gameHudMenu != null)
			{
				gameHudMenu.SetUpRevive();
			}
			if(BackGroundManager.Instance != null)
			{
				BackGroundManager.Instance.StopMove();
			}

			break;

		case BirdState.Died:
			Vector3 dyinPos = new Vector3(birdTransform.position.x + 15, 5, birdTransform.position.z);
			dyingDirection = dyinPos - birdTransform.position;

			Hashtable param = new Hashtable();
			param.Add("position", dyinPos);
//			param.Add("rotation", new Vector3(0, 0, 55));//easeOutBounce
			param.Add("easetype", iTween.EaseType.easeOutBounce);	//easeInExpo, easeInOutBack
			param.Add("delay", 0.0f);
			param.Add("time", 6.0f);
			iTween.MoveTo(birdTransform.gameObject, param);
			iTween.RotateTo(birdTransform.gameObject, new Vector3(0, 0, 15), 2);
//			iTween.RotateAdd(birdTransform.gameObject, new Vector3(0, 0, 55), 5);
			BirdFollow.isBirdReady = false;
			if(BackGroundManager.Instance != null)
			{
				BackGroundManager.Instance.StopMove();
			}
			if(gameHudMenu != null)
			{
				gameHudMenu.GameOver();
			}
			Invoke("TakeInput", 0.6f);
			break;
		}
	}
	#endregion


	void MoveToDie()
	{
		SetState(BirdState.Died);
	}


	void SetUpRevive()
	{
		SetState(BirdState.Flying);
		BirdFollow.isBirdReady = true;
		GameData.isBirdLive = true;
	}

	void TakeInput()
	{
		isDieDelay = true;
	}

	/// <summary>
	/// Drops an egg.
	/// </summary>
	void DropAnEgg()
	{
		Main.Instance.PlayEggDropSound();
		birdAnimator.SetTrigger("Egg");
		eggCount++;
		Vector3 dropPos = birdTransform.position;
		dropPos.x -= 0.2f;
		dropPos.z = Constants.Z_EGGS;
		dropPos.y -= birdTransform.localScale.y*0.5f;
		Quaternion eggRotation = Quaternion.Euler(new Vector3(0, 0, -2));
		GameObject egg = Instantiate(currentEgg, dropPos, eggRotation) as GameObject;
		egg.name = "Egg" + eggCount;
		egg.tag = Constants.TAG_EGG;
		egg.transform.parent = eggRoot.transform;
		Rigidbody2D eggBody = egg.transform.GetComponent<Rigidbody2D>();
		if(eggBody != null)
		{
			eggBody.isKinematic = false;
			eggBody.gravityScale = gravityScale;
		}

	}



	void Menu()
	{
		if(gameHudMenu != null)
		{
			gameHudMenu.ActivateHomePanel();
		}
	}

	public void Accelerate()
	{
		forwardMovementSpeed += forwardMovementSpeed*Constants.ACCELERATION;
		gravityScale += Constants.GRAVITY_ACCELERATION;
		BackGroundManager.Instance.Accelerate();
	}

	public void Die()
	{
		if(!isRevived && HZIncentivizedAd.isAvailable() )
		{
//			Debug.Log("Set revive");
			SetState(BirdState.WaitForRevive);
		}
		else
		{
			SetState(BirdState.Died);
		}
	}

	void StartTutorial()
	{
//		Debug.Log("Start tutorial");
		GameData.isTutorial = true;
		transform.GetComponent<PillerGenerator>().CreateTutorialPillar();
		SetState(BirdState.Tutorial);
	}


	void TutorialOver()
	{
//		Debug.Log("TutorialOver111111111111");
		GameData.isTutorial = false;
		PlayerPrefs.SetBool(Constants.KEY_TUTORIAL, false);
		PlayerPrefs.Flush();
		SetState(BirdState.TutorialComplete);
	}

}

/// <summary>
/// Bird state.
/// Bird's different states
/// </summary>
public enum BirdState
{
	None = -1,
	Spwan,
	Tutorial,
	TutorialComplete,
	Init,
	Transition,
	Flying,
	WaitForRevive,
	Died,
	Count
}