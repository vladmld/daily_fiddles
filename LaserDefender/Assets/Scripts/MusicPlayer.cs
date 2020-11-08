using UnityEngine;
using System.Collections;

/// <summary>
/// Music player.
/// </summary>
public class MusicPlayer : MonoBehaviour
{
	#region field
	
	static MusicPlayer instance = null;

	#endregion
	
	#region states
	
	/// <summary>
	/// Awake this instance.
	/// </summary>
	void Awake ()
	{
		Debug.Log ("Music player Awake " + GetInstanceID ());
		
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad (gameObject);
		}
	}
	
	#endregion
}
