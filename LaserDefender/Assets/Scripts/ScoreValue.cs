using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Score value.
/// </summary>
public class ScoreValue : MonoBehaviour
{
	#region states
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		Text myText = GetComponent<Text> ();
		myText.text = ScoreKeeper.Score.ToString ();
		ScoreKeeper.Reset ();
	}
	
	#endregion
}
