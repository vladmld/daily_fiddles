using UnityEngine;
using System.Collections;

/// <summary>
/// Level manager.
/// </summary>
public class LevelManager : MonoBehaviour
{
	#region methods
	/// <summary>
	/// Loads the level.
	/// </summary>
	/// <param name="name">Level Name.</param>
	public void LoadLevel (string name)
	{
		Application.LoadLevel (name);
	}
	
	/// <summary>
	/// Loads the next level.
	/// </summary>
	public void LoadNextLevel ()
	{
		Application.LoadLevel (Application.loadedLevel + 1);
	}
	
	/// <summary>
	/// Quit the game.
	/// </summary>
	public void QuitRequest ()
	{
		Application.Quit ();
	}
	
	#endregion	
}
