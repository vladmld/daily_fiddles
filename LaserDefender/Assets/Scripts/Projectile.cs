using UnityEngine;
using System.Collections;

/// <summary>
/// Projectile.
/// </summary>
public class Projectile : MonoBehaviour
{
	#region properties
	
	public float Damage = 50f;
	
	#endregion
	
	#region methods
	
	/// <summary>
	/// Gets the damage.
	/// </summary>
	/// <returns>The damage.</returns>
	public float GetDamage ()
	{
		return Damage;
	}
	
	/// <summary>
	/// Hit this instance.
	/// </summary>
	public void Hit ()
	{
		Destroy (gameObject);	
	}
	
	#endregion
}
