using UnityEngine;
using System.Collections;

/// <summary>
/// Position.
/// </summary>
public class Position : MonoBehaviour
{
	#region events
	
	/// <summary>
	/// Raises the draw gizmos event.
	/// </summary>
	void OnDrawGizmos ()
	{
		Gizmos.DrawWireSphere (transform.position, 0.5f);	
	}
	
	#endregion
}
