using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// The Bird game object.
/// This class is responsible for generating poles 
/// with nest to collect eggs dropped by Bird on move.
/// </summary>
public class PillerGenerator : MonoBehaviour {

	/// <summary>
	/// The nest tree.
	/// Collects the egg dropped by Bird.
	/// </summary>
	public GameObject[] nestTree;


	/// <summary>
	/// The poles. All poles created
	/// </summary>
//	public static List <GameObject> poles;

	/// <summary>
	/// The tree root.
	/// Parent game object for all nestTree created.
	/// </summary>
	GameObject treeRoot;

	/// <summary>
	/// The tree root transfrom.
	/// </summary>
	Transform treeRootTransfrom;

	/// <summary>
	/// The half width of the World.
	/// </summary>
	float halfWidth;

	/// <summary>
	/// The half height of the World.
	/// </summary>
	float halfHeight;

	/// <summary>
	/// The minimum height.
	/// </summary>
	float minHeight;

	/// <summary>
	/// The minimum gap.
	/// </summary>
	float minGap;

	/// <summary>
	/// The max gap.
	/// </summary>
	float maxGap;

	/// <summary>
	/// The flying position.
	/// </summary>
	Vector3 flyingPos;

	float flyingSpeed;

	/// <summary>
	/// The pillar count.
	/// </summary>
	public int pillarCount;

	//TODO:
	int DieCountPillar;

	bool isAccelerated;


	private NestCreaterState m_eCurrentState;

	private NestCreaterState m_ePreviousState;


	GameObject currentPole;

	
	/// <summary>
	/// Occurs when first level of change in envirnment is required .
	/// </summary>
	public delegate void ActivateLevelOneChange();
	public static event ActivateLevelOneChange EnvirnmentChangeNow;
	
	/// <summary>
	/// Occurs when extreme change in Envirnment is required.
	/// </summary>
	public delegate void ActivateLevelTwoChange();
	public static event ActivateLevelTwoChange ExtremeChangeNow;


	public delegate void DeactivateEffects();
	public static event DeactivateEffects RemoveAllEffectsNow;


	void OnEnable()
	{
		GameEventManager.BirdReadyNow += BirdReadyToFlight;
		GameEventManager.RiviveGame += SetUpRevive;

	}
	
	void OnDisable()
	{
		GameEventManager.BirdReadyNow -= BirdReadyToFlight;
		GameEventManager.RiviveGame -= SetUpRevive;
		
	}


	// Use this for initialization
	void Start () 
	{
		pillarCount = 0;
		DieCountPillar = 0;
		flyingSpeed = Utility.GetBirdInitialSpeed();
		flyingPos = Utility.GetBirdFlyingPosition();
		treeRoot = new GameObject("TreeRoot");
		treeRootTransfrom = treeRoot.transform;
		halfWidth = Utility.WorldWidth()/2.0f;
		halfHeight = Utility.WorldHeight()/2.0f;
		minHeight = -halfHeight*0.5f;
		minGap = halfWidth*0.8f;
		maxGap = halfWidth*1.1f;
//		poles = new List<GameObject>();
		currentPole = Instantiate(nestTree[GameData.nestID ], new Vector3(-20, 0, 0), Quaternion.identity) as GameObject;
		currentPole.transform.parent = treeRoot.transform;
		CreateInitialPoles();
	}

	void OnDestroy()
	{
		pillarCount = 0;
		DieCountPillar = 0;
		if(GameData.poles != null)
		{
			for(int i = 0 ; i< GameData.poles.Count; i++)
				Destroy(GameData.poles[i]);
			GameData.poles.Clear();
//			GameData.poles = null;
		}

		if(treeRoot != null)
		{
			Destroy(treeRoot);
		}

	}

	// Update is called once per frame
	void Update () 
	{
		switch(m_eCurrentState)
		{
			
			
			case NestCreaterState.Spwan:
				break;
				
			case NestCreaterState.Ready:
//			treeRootTransfrom.Translate(Vector3.left*flyingSpeed*Time.fixedDeltaTime);
			if(transform.position.x > GameData.poles[GameData.poles.Count-2].transform.position.x)
				{
					if(GameData.isBirdLive)
					{
						AddNewPole();
					}
					
				}
				break;
		}

	}



	#region Pole Creation
	public void CreateTutorialPillar()
	{
		//Create initial Pole one
		Vector3 pole1Pos = new Vector3(0, Random.Range(minHeight+0.3f, minHeight+0.6f), Constants.Z_PLOES);
		GameObject pole1 = Instantiate(currentPole, pole1Pos, Quaternion.identity) as GameObject;
		pole1.name = "TutorialPole";
		pole1.transform.GetComponentInChildren<Nest>().enabled = true;
		pole1.transform.parent = treeRoot.transform;
//		poles.Add(pole1);
	}


	/// <summary>
	/// Creates the initial two poles.
	/// </summary>
	void CreateInitialPoles()
	{

		if(GameData.isTutorial)
		{
//			Debug.Log("CreateTutorialPillar");
			CreateTutorialPillar();
		}

//		Debug.Log("CreateInitialPolesCreateInitialPolesCreateInitialPoles");
		//Create initial Pole one
		Vector3 pole1Pos = new Vector3(flyingPos.x + maxGap*1.2f,
		                               Random.Range(minHeight, 0), Constants.Z_PLOES);
//		GameObject pole1 = Instantiate(nestTree[GameData.nestID ], pole1Pos, Quaternion.identity) as GameObject;
		GameObject pole1 = Instantiate(currentPole, pole1Pos, Quaternion.identity) as GameObject;
		pole1.name = "Pole_1";
		pole1.transform.GetComponentInChildren<Nest>().enabled = true;
//		pole1.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.Vertical;
		pole1.transform.parent = treeRoot.transform;
		GameData.poles.Add(pole1);

		//Create initial Pole Two
		Vector3 pole2Pos = new Vector3(Random.Range(pole1Pos.x + minGap, pole1Pos.x + maxGap),
		                               Random.Range(minHeight, 0), Constants.Z_PLOES);
//		GameObject pole2 = Instantiate(nestTree[GameData.nestID], pole2Pos, Quaternion.identity) as GameObject;
		GameObject pole2 = Instantiate(currentPole, pole2Pos, Quaternion.identity) as GameObject;
		pole2.name = "Pole_2";
		pole2.transform.GetComponentInChildren<Nest>().enabled = true;
//		pole2.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.Vertical;
		pole2.transform.parent = treeRoot.transform;
		GameData.poles.Add(pole2);

		//Create initial Pole Three
		Vector3 pole3Pos = new Vector3(Random.Range(pole2Pos.x + minGap, pole2Pos.x + maxGap),
		                               Random.Range(minHeight, 0), Constants.Z_PLOES);
//		GameObject pole3 = Instantiate(nestTree[GameData.nestID], pole3Pos, Quaternion.identity) as GameObject;
		GameObject pole3 = Instantiate(currentPole, pole3Pos, Quaternion.identity) as GameObject;
		pole3.name = "pole_3";
		pole3.transform.GetComponentInChildren<Nest>().enabled = true;
//		pole3.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.Sideways;
		pole3.transform.parent = treeRoot.transform;
		GameData.poles.Add(pole3);

		pillarCount = 3;
		DieCountPillar = 3;
	}

	/// <summary>
	/// Adds a new pole.
	/// </summary>
	void AddNewPole()
	{
		
		Vector3 prevPolePos = GameData.poles[GameData.poles.Count-1].transform.position;
		Vector3 newPolePos =  new Vector3(Random.Range(prevPolePos.x + minGap, prevPolePos.x + maxGap),
		                                  Random.Range(minHeight, 0), Constants.Z_PLOES);
//		GameObject newPole = Instantiate(nestTree[GameData.nestID], newPolePos, Quaternion.identity) as GameObject;
		GameObject newPole = Instantiate(currentPole, newPolePos, Quaternion.identity) as GameObject;
		newPole.transform.GetComponentInChildren<Nest>().enabled = true;
		pillarCount++;
		DieCountPillar++;
		newPole.name = "Pole_" + pillarCount; 

		if(pillarCount > 5 && pillarCount <= 11)
		{
			if(pillarCount %2 == 0)
			{
				newPole.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.Vertical;
			}
			else
			{
				newPole.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.None;
			}


		}
		else if(pillarCount > 11 && pillarCount <= 17)
		{
			if(pillarCount %2 == 0)
			{
				newPole.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.Sideways;
			}
			else
			{
				newPole.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.None;
			}
		}
		else if(pillarCount >  17 && pillarCount <= 27)
		{
			if(pillarCount %3 == 0)
			{
				newPole.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.Vertical;
			}
			else if(pillarCount %3 == 1)
			{
				newPole.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.Sideways;
			}
			else if(pillarCount %3 == 2)
			{
				newPole.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.None;
			}

		}
//		else if(pillarCount >= Constants.ACCELERATE_START_COUNT && !isAccelerated)
//		{
//			transform.GetComponent<Bird>().Accelerate();
//		}

		if(pillarCount == Constants.ENVIRNMENT_CHANGE_COUNT)
		{
			if(EnvirnmentChangeNow != null)
			{
				EnvirnmentChangeNow();
			}

			if(!isAccelerated)
			transform.GetComponent<Bird>().Accelerate();
		}
		if(pillarCount == Constants.EXTREME_CHANGE_COUNT)
		{
			if(ExtremeChangeNow != null)
			{
				ExtremeChangeNow();
			}
			if(!isAccelerated)
			transform.GetComponent<Bird>().Accelerate();
		}

		if(pillarCount == Constants.RESET_COUNT)
		{
			pillarCount = 0;

			if(RemoveAllEffectsNow != null)
			{
				RemoveAllEffectsNow();
			}
			if(!isAccelerated)
			{
				isAccelerated = true;
				transform.GetComponent<Bird>().Accelerate();
			}
				
		}

		newPole.transform.parent = treeRoot.transform;
		GameData.poles.Add(newPole);


	}
	#endregion


	#region Bird State Transitions
	void SetState(NestCreaterState newState)
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
		switch(m_eCurrentState)
		{

				
			case NestCreaterState.Spwan:
				break;
				
			case NestCreaterState.Ready:
				break;
		}
	}
	#endregion

	#region Bird Delegates
	void BirdReadyToFlight()
	{
		SetState(NestCreaterState.Ready);
	}

	void SetUpRevive()
	{
		SetState(NestCreaterState.WaitRevive);
		for(int i = 0 ; i< GameData.poles.Count; i++)
			Destroy(GameData.poles[i]);

		GameData.poles.Clear();

		AddNewPoleForRevive(transform.position.x + 15);
		for(int i = 0; i<2; i++)
		{
			AddNewPoleForRevive(0);
		}
		SetState(NestCreaterState.Ready);
	}
	#endregion


	void AddNewPoleForRevive(float startX)
	{
		
		Vector3 prevPolePos = Vector3.zero;
		Vector3 newPolePos = Vector3.zero;
		if(GameData.poles.Count == 0)
		{
			
			newPolePos =  new Vector3(startX,
				Random.Range(minHeight, 0), Constants.Z_PLOES);
		}
		else
		{
			
			prevPolePos = GameData.poles[GameData.poles.Count-1].transform.position;
			newPolePos =  new Vector3(Random.Range(prevPolePos.x + minGap, prevPolePos.x + maxGap),
				Random.Range(minHeight, 0), Constants.Z_PLOES);
		}

		//		GameObject newPole = Instantiate(nestTree[GameData.nestID], newPolePos, Quaternion.identity) as GameObject;
		GameObject newPole = Instantiate(currentPole, newPolePos, Quaternion.identity) as GameObject;
		newPole.transform.GetComponentInChildren<Nest>().enabled = true;
		pillarCount++;
		DieCountPillar++;
		newPole.name = "Pole_" + pillarCount; 

		if(pillarCount > 5 && pillarCount <= 11)
		{
			if(pillarCount %2 == 0)
			{
				newPole.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.Vertical;
			}
			else
			{
				newPole.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.None;
			}


		}
		else if(pillarCount > 11 && pillarCount <= 17)
		{
			if(pillarCount %2 == 0)
			{
				newPole.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.Sideways;
			}
			else
			{
				newPole.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.None;
			}
		}
		else if(pillarCount >  17 && pillarCount <= 27)
		{
			if(pillarCount %3 == 0)
			{
				newPole.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.Vertical;
			}
			else if(pillarCount %3 == 1)
			{
				newPole.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.Sideways;
			}
			else if(pillarCount %3 == 2)
			{
				newPole.transform.GetComponentInChildren<Nest>().NestMovementType = NestMoveType.None;
			}

		}
		//		else if(pillarCount >= Constants.ACCELERATE_START_COUNT && !isAccelerated)
		//		{
		//			transform.GetComponent<Bird>().Accelerate();
		//		}

		if(pillarCount == Constants.ENVIRNMENT_CHANGE_COUNT)
		{
			if(EnvirnmentChangeNow != null)
			{
				EnvirnmentChangeNow();
			}

			if(!isAccelerated)
				transform.GetComponent<Bird>().Accelerate();
		}
		if(pillarCount == Constants.EXTREME_CHANGE_COUNT)
		{
			if(ExtremeChangeNow != null)
			{
				ExtremeChangeNow();
			}
			if(!isAccelerated)
				transform.GetComponent<Bird>().Accelerate();
		}

		if(pillarCount == Constants.RESET_COUNT)
		{
			pillarCount = 0;

			if(RemoveAllEffectsNow != null)
			{
				RemoveAllEffectsNow();
			}
			if(!isAccelerated)
			{
				isAccelerated = true;
				transform.GetComponent<Bird>().Accelerate();
			}

		}

		newPole.transform.parent = treeRoot.transform;
		GameData.poles.Add(newPole);


	}

//	void TutorialOver()
//	{
//		Debug.Log("TutorialOver");
//	}
}

public enum NestCreaterState
{
	None = -1,
	Spwan,
	Ready,
	WaitRevive,
	Count
}
