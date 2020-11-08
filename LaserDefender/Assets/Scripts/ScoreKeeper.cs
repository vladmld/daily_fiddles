using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Score keeper.
/// </summary>
public class ScoreKeeper : MonoBehaviour
{
	#region fields
	
	private Text myScore;
	
	#endregion
	
	#region states
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		myScore = GetComponent<Text> ();
		myScore.text = Score.ToString ();
	}
	
	#endregion
	
	#region properties
	
	public static int Score = 0;
	
	#endregion
	
	#region methods
	
	/// <summary>
	/// Score the specified points.
	/// </summary>
	/// <param name="points">Points.</param>
	public void ScorePoints (int points)
	{		
		Score += points;
		myScore.text = Score.ToString ();
	}	
	
	/// <summary>
	/// Reset the score.
	/// </summary>
	public static void Reset ()
	{
		Score = 0;
	}
	
	#endregion
}
