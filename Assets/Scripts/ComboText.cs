using UnityEngine;
using System.Collections;

public class ComboText : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
		Destroy(gameObject, 1.50f);
	}
	
	// Update is called once per frame
	void Update () 
	{
	
		transform.Translate(Vector3.up*0.08f);
	}
}
