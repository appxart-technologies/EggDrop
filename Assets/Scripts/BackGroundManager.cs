using UnityEngine;
using System.Collections;

/// <summary>
/// Back ground manager.
/// </summary>
public class BackGroundManager : MonoBehaviour {


	/// <summary>
	/// The back ground.
	/// </summary>
	public GameObject backGround;

	/// <summary>
	/// The Front cloud.
	/// </summary>
	public GameObject frontCloud;

	/// <summary>
	/// The back cloud.
	/// </summary>
	public GameObject backCloud;

	/// <summary>
	/// The back layer of water/snow/sand.
	/// </summary>
	public GameObject backWater;

	/// <summary>
	/// The middle layer of water/snow/sand.
	/// </summary>
	public GameObject midWater;

	/// <summary>
	/// The front layer of water/snow/sand.
	/// </summary>
	public GameObject frontWater;

	/// <summary>
	/// The rain effect.
	/// </summary>
	public GameObject rainEffect;

	/// <summary>
	/// The lightening effect. Second level of Rain effect
	/// </summary>
	public GameObject lighteningEffect;

	/// <summary>
	/// The snow effect.
	/// </summary>
	public GameObject snowEffect;

	/// <summary>
	/// The fog effect. Second level of Snow effect
	/// </summary>
	public GameObject fogEffect;

	/// <summary>
	/// The dust fog effect.
	/// </summary>
	public GameObject dustFogEffect;

	public GameObject sandEffect;

	/// <summary>
	/// All birds.
	/// </summary>
	public GameObject[] allBirds;

	private GameObject currentBird;

	/// <summary>
	/// The back ground scroll speed.
	/// </summary>
	float backGroundScrollSpeed;

	float backWaterSpeed;
	float midWaterSpeed;
	float frontWaterSpeed;

	float frontCloudSpeeed;
	float backCloudSpeed;

	/// <summary>
	/// The background root tranform.
	/// </summary>
	Transform bgRootTranform;

	/// <summary>
	/// The background renderer.
	/// </summary>
	private Renderer bgRenderer;

	/// <summary>
	/// The front cloud renderer.
	/// </summary>
	private Renderer frontCloudRenderer;

	/// <summary>
	/// The back cloud renderer.
	/// </summary>
	private Renderer backCloudRenderer;

	private Renderer backWaterRenderer;

	private Renderer midWaterRenderer;

	private Renderer frontWaterRenderer;

	/// <summary>
	/// The saved background offset.
	/// </summary>
	private Vector2 savedBGOffset;

	/// <summary>
	/// The saved Front cloud offset.
	/// </summary>
	private Vector2 savedFCOffset;

	/// <summary>
	/// The saved Back Cloud offset.
	/// </summary>
	private Vector2 savedBCOffset;

	/// <summary>
	/// The saved Back water offset.
	/// </summary>
	private Vector2 savedBWoffset;

	/// <summary>
	/// The saved Midlle water offset.
	/// </summary>
	private Vector2 savedMWoffset;

	/// <summary>
	/// The saved Front water offset.
	/// </summary>
	private Vector2 savedFWoffset;


	/// <summary>
	/// The tilling back ground.
	/// </summary>
	private Vector2 tillingBackGround;

	/// <summary>
	/// The initial position.
	/// </summary>
	private Vector3 initialPosition;

	private BGTheme currentTheme;

	/// <summary>
	/// The forward movement speed.
	/// Bird flight speed.
	/// </summary>
	float forwardMovementSpeed; 


	private bool isGameRunning;

	float bgTimer = 0;

	float waterTimer = 0;

	private Color color_theme_one, color_theme_two, color_theme_three, color_theme_four;
	private Color rainColor, snowColor;
	float duration = 0.010f;



	private static BackGroundManager instance = null;
	public static BackGroundManager Instance 
	{
		get { return instance; }
	}


	void OnEnable()
	{
		PillerGenerator.EnvirnmentChangeNow += HandleEnvirnmentChange;
		PillerGenerator.ExtremeChangeNow += HandleExtremeChangeNow;
		PillerGenerator.RemoveAllEffectsNow += HandleRemoveAllEffectsNow;
	}


	void OnDisable()
	{
		PillerGenerator.EnvirnmentChangeNow -= HandleEnvirnmentChange;
		PillerGenerator.ExtremeChangeNow -= HandleExtremeChangeNow;
		PillerGenerator.RemoveAllEffectsNow -= HandleRemoveAllEffectsNow;
		SetSavedTextureOffset();
		HandleRemoveAllEffectsNow();
//		rainEffect.SetActive(false);
//		lighteningEffect.SetActive(false);
//		snowEffect.SetActive(false);
//		fogEffect.SetActive(false);
//		dustFogEffect.SetActive(false);
//		sandEffect.SetActive(false);
	}

	void Awake() 
	{
		if (instance != null && instance != this) 
		{
			Destroy(this.gameObject);
			return;
		} 
		else 
		{
			instance = this;
		}

		
	}



	void Start () 
	{
//		Init();
		ChangeTheme();
		DontDestroyOnLoad(gameObject);
	}

	void Init()
	{
//		rainEffect.SetActive(false);
//		lighteningEffect.SetActive(false);
//		snowEffect.SetActive(false);
//		fogEffect.SetActive(false);
//		dustFogEffect.SetActive(false);
//		sandEffect.SetActive(false);
		HandleRemoveAllEffectsNow();
		Main.Instance.effectsSource.Stop();
		StopAllCoroutines();
		//Scale all backgrounds as per screen 
		Instance.isGameRunning = false;
		float width = Utility.WorldWidth() + 0.3f;
		float height = Utility.WorldHeight() + 0.1f;
		Instance.backGroundScrollSpeed = 0.037f;	//0.025f;
		Instance.frontCloudSpeeed = 0.067f;			//0.045f;
		Instance.backCloudSpeed = 0.03f;			//0.02f;
		Instance.transform.position = Vector3.zero;
		Instance.initialPosition = transform.position;
		Instance.bgRootTranform = transform;
		Instance.forwardMovementSpeed = Utility.GetBirdInitialSpeed();

		//BG image setup
		Instance.backGround.transform.localScale = new Vector3(width, height/2, 1);
		Instance.backGround.transform.localPosition = new Vector3(0, -height/4, Constants.Z_BACK_GROUND);
		Instance.bgRenderer = Instance.backGround.transform.GetComponent<Renderer>();
		Instance.savedBGOffset = Vector2.zero;

		//Front Cloud Setup
		Instance.frontCloud.transform.localScale = new Vector3(width, height/4, 1);
		Instance.frontCloud.transform.localPosition = new Vector3(0, height*0.35f, Constants.Z_FRONT_CLOUD);
		Instance.frontCloudRenderer = Instance.frontCloud.transform.GetComponent<Renderer>();
		Instance.savedFCOffset = Vector2.zero;

		//Back Cloud Setup
		Instance.backCloud.transform.localScale = new Vector3(width, height/4, 1);
		Instance.backCloud.transform.localPosition = new Vector3(0, height*0.32f, Constants.Z_BACK_CLOUD);
		Instance.backCloudRenderer = Instance.backCloud.transform.GetComponent<Renderer>();
		Instance.savedBCOffset = Vector2.zero;

		//Water Setup
		Instance.backWater.transform.localScale = new Vector3(width, height*0.16f, 1);
		Instance.backWater.transform.localPosition = new Vector3(0, -height*0.45f, Constants.Z_BACK_WATER);
		Instance.backWaterRenderer = Instance.backWater.transform.GetComponent<Renderer>();
		Instance.savedBWoffset = Vector2.zero;

		Instance.midWater.transform.localScale = new Vector3(width, height*0.16f, 1);
		Instance.midWater.transform.localPosition = new Vector3(0, -height*0.45f, Constants.Z_MID_WATER);
		Instance.midWaterRenderer = Instance.midWater.transform.GetComponent<Renderer>();
		Instance.savedMWoffset = Vector2.zero;

		Instance.frontWater.transform.localScale = new Vector3(width, height*0.16f, 1);
		Instance.frontWater.transform.localPosition = new Vector3(0, -height*0.45f, Constants.Z_FRONT_WATER);
		Instance.frontWaterRenderer = Instance.frontWater.transform.GetComponent<Renderer>();
		Instance.savedFWoffset = Vector2.zero;

		//For BG Theme one
		Instance.color_theme_one = new Color(198/255.0f, 229/255.0f, 224/255.0f);
		Instance.color_theme_two = new Color(192/255.0f, 212/255.0f, 209/255.0f);
		Instance.color_theme_three = new Color(245/255.0f, 231/255.0f, 185/255.0f);
		Instance.rainColor = new Color(156/255.0f, 189/255.0f, 184/255.0f);
		Instance.snowColor = new Color(165/255.0f, 178/255.0f, 176/255.0f);
	}

	void OnDestroy()
	{
	}


	public void SpwanBird()
	{
		Instance.StartCoroutine( CreateBird());
	}

	IEnumerator  CreateBird()
	{
		if(currentBird != null)
		{
//			Debug.Log("DEstroying bit");
//			currentBird.tag = "Untagged";
			Destroy(currentBird);
			currentBird = null;
		}

		yield return new WaitForSeconds(0.2f);
//		else
		{
			float startY = Utility.GetBirdMenuPosition().y;
			float startX = -Utility.WorldWidth()/2;
			if(GameData.isRetry)
			{
				startX = 0f;
			}
//			int n = Random.Range(0, allBirds.Length);
//			n = GameData.currentBirdId;
			string birdName = "Bird_" + GameData.currentBirdId;
//			currentBird = Instantiate(allBirds[n], new Vector3(startX, startY, 0), Quaternion.identity) as GameObject;
			currentBird = Instantiate (Resources.Load ("Prefabs/AllBirds/" + birdName ),
			                           new Vector3(startX, startY, 0), Quaternion.identity ) as GameObject;
			currentBird.name = birdName;
			currentBird.tag = Constants.TAG_BIRD;
		}
	}



	// Update is called once per frame
	void Update () 
	{
		Instance.waterTimer += Time.fixedDeltaTime;

		if(Instance.isGameRunning)
		{
			//BG Scroll offset
			Instance.bgTimer += Time.fixedDeltaTime;
			//						float x = Mathf.Repeat (Time.time * Instance.backGroundScrollSpeed, 1);
			float x = Mathf.Repeat (Instance.bgTimer * Instance.backGroundScrollSpeed, 1);
			Vector2 offset = new Vector2 (x, Instance.savedBGOffset.y);
			Instance.bgRenderer.sharedMaterial.SetTextureOffset ("_MainTex", offset);
			Instance.bgRootTranform.Translate(Vector3.right*Instance.forwardMovementSpeed*Time.fixedDeltaTime);
			
		}

		//Front Cloud scroll offset
		float xF = Mathf.Repeat (Instance.waterTimer * Instance.frontCloudSpeeed, 1);
		Vector2 offsetFC = new Vector2 (xF, Instance.savedFCOffset.y);
		Instance.frontCloudRenderer.sharedMaterial.SetTextureOffset ("_MainTex", offsetFC);
		
		//Back Cloud scroll offset
		float xB = Mathf.Repeat (Instance.waterTimer * Instance.backCloudSpeed, 1);
		Vector2 offsetBC = new Vector2 (xB, Instance.savedBCOffset.y);
		Instance.backCloudRenderer.sharedMaterial.SetTextureOffset ("_MainTex", offsetBC);


		//Back water scroll
		float xBW = Mathf.Repeat (Instance.waterTimer * Instance.backWaterSpeed, 1);
		Vector2 offsetBW = new Vector2 (xBW, Instance.savedBWoffset.y);
		Instance.backWaterRenderer.sharedMaterial.SetTextureOffset ("_MainTex", offsetBW);
		
		//Mid Water Scroll
		float xMW = Mathf.Repeat (Instance.waterTimer * Instance.midWaterSpeed, 1);
		Vector2 offsetMW = new Vector2 (xMW, Instance.savedMWoffset.y);
		Instance.midWaterRenderer.sharedMaterial.SetTextureOffset ("_MainTex", offsetMW);
		
		//Front Water scroll
		float xFW = Mathf.Repeat (Instance.waterTimer * Instance.frontWaterSpeed, 1);
		Vector2 offsetFW = new Vector2 (xFW, Instance.savedFWoffset.y);
		Instance.frontWaterRenderer.sharedMaterial.SetTextureOffset ("_MainTex", offsetFW);
	}



	void OnLevelWasLoaded(int level)
	{
//		if(level == 1)
//		{
//			if(currentBird == null && GameData.isComingFromStore)
//			{
//				//On Main menu
//				GameData.isComingFromStore = false;
//				Debug.Log("Create 2");
//				CreateBird();
//			}
//		}
//		else if(level == 2)
//		{
//			if(currentBird != null)
//			{
//				Destroy(currentBird);
//				currentBird = null;
//			}
//
//		}

	}



	public void StartMove()
	{
		Instance.isGameRunning = true;
		switch(Instance.currentTheme)
		{
			case BGTheme.Forest:
				Instance.backWaterSpeed = 0.77f;		//0.7f;
			Instance.midWaterSpeed = 0.847f;				//0.77f;
			Instance.frontWaterSpeed = 1.1f;			//0.95f;
				break;

			case BGTheme.Snow:
			Instance.backWaterSpeed = 0.3f;			//0.2f;
			Instance.midWaterSpeed = 0.242f;			//0.22f;
			Instance.frontWaterSpeed = 0.275f;		//0.25f;
				break;

			case BGTheme.Desert:
			Instance.backWaterSpeed = 0.3f;			//0.2f;
			Instance.midWaterSpeed = 0.242f;		//0.22f;
			Instance.frontWaterSpeed = 0.275f;	////0.25f;
				break;
		}
//		Instance.bgRenderer.sharedMaterial.SetTextureOffset ("_MainTex", Instance.savedBGOffset);
	}

	public void StopMove()
	{
		Instance.bgTimer = 0;
		Instance.isGameRunning = false;
		switch(Instance.currentTheme)
		{
			case BGTheme.Forest:
				Instance.backWaterSpeed = 0.77f;		//0.7f;
				Instance.midWaterSpeed = 0.847f;				//0.77f;
				Instance.frontWaterSpeed = 1.1f;			//0.95f;
				break;
				
			case BGTheme.Snow:
			Instance.backWaterSpeed = 0;//	0.3f;			//0.2f;
			Instance.midWaterSpeed = 0;//0.242f;			//0.22f;
			Instance.frontWaterSpeed = 0;//0.275f;		//0.25f;
				break;
				
			case BGTheme.Desert:
			Instance.backWaterSpeed = 0;//	0.3f;			//0.2f;
			Instance.midWaterSpeed = 0;//0.242f;			//0.22f;
			Instance.frontWaterSpeed = 0;//0.275f;		//0.25f;
				break;
		}
	}


	public void ChangeTheme()
	{
		Init();
		Instance.currentTheme = (BGTheme)GameData.currentTheme;
		
		Texture _bgTex = Resources.Load("Textures/Game/BG/" + "BG_" + GameData.currentTheme) as Texture;
		if(_bgTex != null)
		{
			Instance.bgRenderer.sharedMaterial.SetTexture("_MainTex",_bgTex);
		}
		else
		{
			Debug.Log("BG texture not loaded");
		}

		Texture _waterBackTex = Resources.Load("Textures/Game/Water/" + "WB_" + GameData.currentTheme) as Texture;
		if(_waterBackTex != null)
		{
			Instance.backWaterRenderer.sharedMaterial.SetTexture("_MainTex",_waterBackTex);
		}
		else
		{
			Debug.Log("_waterBackTex texture not loaded");
		}

		Texture _waterMidTex = Resources.Load("Textures/Game/Water/" + "WM_" + GameData.currentTheme) as Texture;
		if(_waterMidTex != null)
		{
			Instance.midWaterRenderer.sharedMaterial.SetTexture("_MainTex",_waterMidTex);
		}
		else
		{
			Debug.Log("_waterMidTex texture not loaded");
		}

		Texture _waterFrontTex = Resources.Load("Textures/Game/Water/" + "WF_" + GameData.currentTheme) as Texture;
		if(_waterFrontTex != null)
		{
			Instance.frontWaterRenderer.sharedMaterial.SetTexture("_MainTex",_waterFrontTex);
		}
		else
		{
			Debug.Log("_waterFrontTex texture not loaded");
		}


		switch(Instance.currentTheme)
		{
			case BGTheme.Forest:
				Camera.main.backgroundColor = Instance.color_theme_one;
				GameData.nestID = (int)Instance.currentTheme;
				Instance.backWaterSpeed = 0.77f;		//0.7f;
				Instance.midWaterSpeed = 0.847f;				//0.77f;
				Instance.frontWaterSpeed = 1.1f;		//0.95f;
				Instance.backWaterRenderer.sharedMaterial.mainTextureScale = Constants.WATER_TILING;
				Instance.midWaterRenderer.sharedMaterial.mainTextureScale = Constants.WATER_TILING;
				Instance.frontWaterRenderer.sharedMaterial.mainTextureScale = Constants.WATER_TILING;
				break;
				
			case BGTheme.Snow:
				Camera.main.backgroundColor = Instance.color_theme_two;
				GameData.nestID = (int)Instance.currentTheme;
				Instance.backWaterSpeed = 0;
				Instance.midWaterSpeed = 0;
				Instance.frontWaterSpeed = 0;
				Instance.backWaterRenderer.sharedMaterial.mainTextureScale = Constants.SNOW_TILING;
				Instance.midWaterRenderer.sharedMaterial.mainTextureScale = Constants.SNOW_TILING;
				Instance.frontWaterRenderer.sharedMaterial.mainTextureScale = Constants.SNOW_TILING;
				break;

			case BGTheme.Desert:
				Camera.main.backgroundColor = Instance.color_theme_three;
				GameData.nestID = (int)Instance.currentTheme;
				Instance.backWaterSpeed = 0;
				Instance.midWaterSpeed = 0;
				Instance.frontWaterSpeed = 0;
				Instance.backWaterRenderer.sharedMaterial.mainTextureScale = Constants.DESERT_TILING;
				Instance.midWaterRenderer.sharedMaterial.mainTextureScale = Constants.DESERT_TILING;
				Instance.frontWaterRenderer.sharedMaterial.mainTextureScale = Constants.DESERT_TILING;
				break;
		}
		
		GameData.currentTheme++;
		if(GameData.currentTheme == (int)BGTheme.Count)
		{
			GameData.currentTheme = 0;
		}

//		CreateBird();
//		Debug.Log("Create 1");
		Instance.StartCoroutine( CreateBird());
		SetSavedTextureOffset();

		Main.Instance.PlayBGMusic();

	}

	public BGTheme GetThemeID()
	{
		return Instance.currentTheme;
	}

	/// <summary>
	/// Sets the saved texture offset.
	/// </summary>
	void SetSavedTextureOffset()
	{
		Instance.bgRenderer.sharedMaterial.SetTextureOffset ("_MainTex", Instance.savedBGOffset);
		Instance.frontCloudRenderer.sharedMaterial.SetTextureOffset ("_MainTex", Instance.savedFCOffset);
		Instance.backCloudRenderer.sharedMaterial.SetTextureOffset ("_MainTex", Instance.savedBCOffset);
		Instance.backWaterRenderer.sharedMaterial.SetTextureOffset ("_MainTex", Instance.savedBWoffset);
		Instance.midWaterRenderer.sharedMaterial.SetTextureOffset ("_MainTex", Instance.savedMWoffset);
		Instance.frontWaterRenderer.sharedMaterial.SetTextureOffset ("_MainTex", Instance.savedFWoffset);
	}

	public void Accelerate()
	{
		Instance.forwardMovementSpeed += Instance.forwardMovementSpeed*Constants.ACCELERATION;

	}

	#region Envirnment Change Delegate
	void HandleEnvirnmentChange ()
	{
		switch(Instance.currentTheme)
		{
			case BGTheme.Forest:
				rainEffect.SetActive(true);
				break;

			case BGTheme.Snow:
				snowEffect.SetActive(true);
				break;

			case BGTheme.Desert:
				dustFogEffect.SetActive(true);
				break;
		}
		StartCoroutine(ColorChangeRoutine());
		Invoke("StopCR", 2.0f);
	}

	void StopCR()
	{
		StopAllCoroutines();
	}


	private IEnumerator ColorChangeRoutine()
	{
		float timer = 0.0f;
//		Color32 color = col;
		
		while (timer < 1)
		{
			switch(Instance.currentTheme)
			{ 

				case BGTheme.Forest:
				timer += Time.deltaTime * duration ;
				Camera.main.backgroundColor = Color32.Lerp(Camera.main.backgroundColor, Instance.rainColor, timer);
				break;
				
				case BGTheme.Snow:

				timer += Time.deltaTime * duration ;
				Camera.main.backgroundColor = Color32.Lerp(Camera.main.backgroundColor, Instance.snowColor, timer);
				break;

				case BGTheme.Desert:
				timer += Time.deltaTime * duration ;
//				Camera.main.backgroundColor = Color32.Lerp(Camera.main.backgroundColor, Instance.snowColor, timer);
				break;
			}

			
			yield return null;
		}
	}

	void HandleExtremeChangeNow ()
	{
		switch(Instance.currentTheme)
		{
		case BGTheme.Forest:
			lighteningEffect.SetActive(true);
			break;
			
		case BGTheme.Snow:
			fogEffect.SetActive(true);
			break;
			
		case BGTheme.Desert:
			sandEffect.SetActive(true);
			break;
		}

		Main.Instance.PlayEffectsSounds();
	}

	public void HandleRemoveAllEffectsNow ()
	{
		rainEffect.SetActive(false);
		lighteningEffect.SetActive(false);
		snowEffect.SetActive(false);
		fogEffect.SetActive(false);
		dustFogEffect.SetActive(false);
		sandEffect.SetActive(false);
		switch(Instance.currentTheme)
		{
			case BGTheme.Forest:
				Camera.main.backgroundColor = Instance.color_theme_one;
				break;
				
			case BGTheme.Snow:
				Camera.main.backgroundColor = Instance.color_theme_two;
				break;
			
			case BGTheme.Desert:
				Camera.main.backgroundColor = Instance.color_theme_three;
				break;
		}

		Main.Instance.effectsSource.Stop();
	}


	#endregion
}

public enum BGTheme
{
	None = -1,
	Forest,
	Snow,
	Desert,
	Count
}
