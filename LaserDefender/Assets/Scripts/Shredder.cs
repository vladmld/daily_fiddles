using UnityEngine;
using System.Collections;

/// <summary>
/// Shredder.
/// </summary>
public class Shredder : MonoBehaviour
{
	#region events

	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="col">Col.</param>
	void OnTriggerEnter2D (Collider2D col)
	{
		Destroy (col.gameObject);
	}
	
	#endregion
}
