using UnityEngine;
using System.Collections;

/// <summary>
/// Utility.
/// 
/// </summary>
public class Utility : MonoBehaviour {

	/// <summary>
	/// World's width in Unity units.
	/// </summary>
	/// <returns>The width of visible world.</returns>
	public static float WorldWidth()
	{
		float height = 2.0f * Camera.main.orthographicSize;
		float worldWidth = height * Camera.main.aspect;
		return worldWidth;
	}

	/// <summary>
	/// World's  height in Unity units.
	/// </summary>
	/// <returns>The height of visible world.</returns>
	public static float WorldHeight()
	{
		float height = 2.0f * Camera.main.orthographicSize;
		return height;
	}

	/// <summary>
	/// Gets the bird's initial position.
	/// </summary>
	/// <returns>The bird position.</returns>
	public static Vector3 GetBirdFlyingPosition()
	{
		float x = WorldWidth() * 0.05f;
		float y = WorldHeight()/2.0f - WorldHeight() * 0.2f;
		float z = 0;
		return new Vector3(x, y, z);
	}

	public static Vector3 GetBirdMenuPosition()
	{
		float x = 0;
		float y = WorldHeight()/2.0f - WorldHeight() * 0.38f;
		float z = 0;
		return new Vector3(x, y, z);
	}


	/// <summary>
	/// Gets the egg sensor offset.
	/// </summary>
	/// <returns>The egg sensor offset.</returns>
	public static float GetEggSensorOffset()
	{
		return WorldWidth()*0.3f;
	}

	public static float GetBirdInitialSpeed()
	{
		return (Constants.BIRD_SPEED/100)*WorldWidth();
	}

	public static Vector3 GetGroundPosition()
	{
		float x = 0;
		float y = - WorldHeight()/2.0f + WorldHeight() * 0.02f;
		float z = 0;
		return new Vector3(x, y, z);
	}
}
