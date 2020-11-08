using UnityEngine;
using System.Collections;

public class WallCollider : MonoBehaviour
{
	void OnTriggerEnter2D (Collider2D col)
	{
		EnemyBehaviour enemy = col.gameObject.GetComponent<EnemyBehaviour> ();
		enemy.Kill ();
	}
}
