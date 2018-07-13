using UnityEngine;
using System.Collections;

public class Ground : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		transform.position = Utility.GetGroundPosition();
	}
	
	void OnCollisionEnter2D(Collision2D collInfo)
	{

		if(collInfo.gameObject.tag == Constants.TAG_EGG && !GameData.isTutorial && BirdFollow.isBirdReady)
		{
			collInfo.gameObject.tag = Constants.TAG_CKECKED;
			Main.Instance.PlayEggMissSound();
//			Debug.Log("Ground    = " + collInfo.gameObject.name);
//			collInfo.gameObject.transform.GetComponent<Collider2D>().isTrigger = true;

			if(BackGroundManager.Instance.GetThemeID() == BGTheme.Forest)
			{
				Vector3 dyinPos = new Vector3(collInfo.transform.position.x - 10, collInfo.transform.position.y, collInfo.transform.position.z);
				
				Hashtable param = new Hashtable();
				param.Add("position", dyinPos);
				//			param.Add("rotation", new Vector3(0, 0, 55));//easeOutBounce
				param.Add("easetype", iTween.EaseType.easeOutBounce);	//easeInExpo, easeInOutBack
				param.Add("delay", 0.0f);
				param.Add("time", 4.0f);
				iTween.MoveTo(collInfo.gameObject, param);
				iTween.RotateTo(collInfo.gameObject, new Vector3(0, 0, 95), 3);
			}



			GameObject gameBird = GameObject.FindGameObjectWithTag(Constants.TAG_BIRD);
			if(gameBird != null)
			{
//				Debug.Log("Ground Die = " + collInfo.gameObject.name);
				Vector3 viewPos = Camera.main.WorldToViewportPoint(collInfo.gameObject.transform.position);
				if (viewPos.x > 0)
				{
//					GameData.isBirdLive = false;
					GameData.gameOverInstructionText = "Drop egg in the nest";
//					Debug.Log("Die from here ground - " + collInfo.gameObject.name);
					gameBird.SendMessage("Die");
				}

			}

//			collInfo.gameObject.SetActive(false);
		}
	}
}
