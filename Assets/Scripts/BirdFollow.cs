using UnityEngine;
using System.Collections;

public class BirdFollow : MonoBehaviour {

	/// <summary>
	/// The target object.
	/// This should be our bird object
	/// </summary>
	GameObject targetObject;	


	/// <summary>
	/// The distance to target.
	/// Gap between position of Bird & Center of screen
	/// </summary>
	private float distanceToTarget;	

	public static bool isBirdReady;

	private Vector3 initialPos;

	void OnEnable()
	{
		GameEventManager.BirdReadyNow += BirdReadyToFlight;
	}
	
	void OnDisable()
	{
		GameEventManager.BirdReadyNow -= BirdReadyToFlight;

	}

	void OnDestroy()
	{
		GameEventManager.BirdReadyNow -= BirdReadyToFlight;
	}

	// Use this for initialization
	void Start () 
	{
		isBirdReady = false;
//		targetObject = GameObject.FindGameObjectWithTag(Constants.TAG_BIRD);
//		distanceToTarget = transform.position.x - targetObject.transform.position.x;

//		Invoke("FindBird", 0.2f);

		initialPos = transform.position;
	}

	void FindBird()
	{
		targetObject = GameObject.FindGameObjectWithTag(Constants.TAG_BIRD);
		distanceToTarget = transform.position.x - targetObject.transform.position.x;
		if(targetObject == null)
		{
			//Attempt one more time to find
			string birdName = "Bird_" + GameData.currentBirdId;
			targetObject = GameObject.Find(birdName);
			distanceToTarget = transform.position.x - targetObject.transform.position.x;
		}
//		isBirdFound = true;
	}
	
	// Update is called once per frame
//	void Update () 
//	{
//		//Take the x-coordinate of the target object and moves the camera to that position.
//		float targetObjectX = targetObject.transform.position.x;
//		
//		Vector3 newCameraPosition = transform.position;
//		newCameraPosition.x = targetObjectX + distanceToTarget;
//		transform.position = newCameraPosition;
//	}

	void LateUpdate()
	{
		if(isBirdReady)
		{
			//Take the x-coordinate of the target object and moves the camera to that position.
			float targetObjectX = targetObject.transform.position.x;
			
			Vector3 newCameraPosition = transform.position;
			newCameraPosition.x = targetObjectX + distanceToTarget;
			transform.position = newCameraPosition;
		}

	}

	#region Bird Delegates
	void BirdReadyToFlight()
	{
		FindBird();
		distanceToTarget = transform.position.x - targetObject.transform.position.x;
		isBirdReady = true;
	}

	public void ResetCam()
	{
		isBirdReady = false;
		transform.position = initialPos;
	}
	#endregion

}
