using UnityEngine;
using System.Collections;

/// <summary>
/// Enemy spawner.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
	#region fields
	
	private float xmin;
	private float xmax;
	private bool movingLeft = true;
	
	#endregion
	
	#region states
	
	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		//calculate xmin and xmax from the camera
		float distance = transform.position.z - Camera.main.transform.position.z;
		Vector3 leftMostPosition = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, distance));
		Vector3 rightMostPosition = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, distance));
		
		xmin = leftMostPosition.x;
		xmax = rightMostPosition.x;
		
		SpawnUntilFull ();
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update ()
	{	
		float rightEdgeOfFormation = transform.position.x + (0.5f * Width);
		float leftEdgeOfFormation = transform.position.x - (0.5f * Width);
		
		if (leftEdgeOfFormation < xmin) {
			movingLeft = false;
		} else if (rightEdgeOfFormation > xmax) {
			movingLeft = true;
		}
		
		if (movingLeft) {
			transform.position += Vector3.left * Speed * Time.deltaTime;
		} else {
			transform.position += Vector3.right * Speed * Time.deltaTime;
		}
		
		if (AllMembersDead ()) {
			SpawnUntilFull ();
		}
	}
	
	#endregion
	
	#region events
	
	/// <summary>
	/// Raises the draw gizmos event and draws a cube in the enemy position.
	/// </summary>
	void OnDrawGizmos ()
	{
		Gizmos.DrawWireCube (transform.position, new Vector3 (Width, Height));
	}
	
	#endregion
	
	#region properties
	
	public GameObject EnemyPrefab;
	public float Width = 10f;
	public float Height = 5f;
	public float SpawnDelay = 0.5f;
	public float Speed = 2f;
	
	#endregion	
	
	#region methods
	
	/// <summary>
	/// Spawns the enemies.
	/// </summary>
	void SpawnEnemies ()
	{
		foreach (Transform child in transform) {
			GameObject enemy = Instantiate (EnemyPrefab, child.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = child;
		}	
	}
	
	/// <summary>
	/// Spawns enemies the until the formation is full.
	/// </summary>
	void SpawnUntilFull ()
	{
		Transform freePositon = NextFreePosition ();
		if (freePositon) {
			GameObject enemy = Instantiate (EnemyPrefab, freePositon.position, Quaternion.identity) as GameObject;
			enemy.transform.parent = freePositon;
		}
		
		if (NextFreePosition ()) {
			Invoke ("SpawnUntilFull", SpawnDelay);
		}
	}
	
	/// <summary>
	/// Checks if all enemies are dead
	/// </summary>
	/// <returns><c>true</c> if all members dead <c>false</c> otherwise.</returns>
	bool AllMembersDead ()
	{
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount > 0) {
				return false;
			}
		}
		
		return true;
	}
	
	/// <summary>
	/// Gets the next free position
	/// </summary>
	/// <returns>The free position.</returns>
	Transform NextFreePosition ()
	{
		foreach (Transform childPositionGameObject in transform) {
			if (childPositionGameObject.childCount == 0) {
				return childPositionGameObject;
			}
		}
		
		return null;
	}
	
	#endregion
	
}




















