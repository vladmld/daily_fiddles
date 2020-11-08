using UnityEngine;
using System.Collections;

/// <summary>
/// Enemy behaviour.
/// </summary>
public class EnemyBehaviour : MonoBehaviour
{
	#region fields
		
	private ScoreKeeper scoreKeeper;
	private Vector3 parentPosition;
	
	#endregion
	
	#region states

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{	
		scoreKeeper = GameObject.Find ("Score").GetComponent<ScoreKeeper> ();	
		
		parentPosition = gameObject.transform.parent.localPosition;
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update ()
	{
		float probability = ShotsPerSecond * Time.deltaTime;

        string test = string.Empty;

		if (Random.value < probability) {
			Fire ();
		}	
	}
	
	#endregion
	
	#region events
	
	/// <summary>
	/// Raises the trigger enter2d event.
	/// </summary>
	/// <param name="col">The collider</param>
	void OnTriggerEnter2D (Collider2D col)
	{
		Projectile projectile = col.gameObject.GetComponent<Projectile> ();
		
		if (projectile) {
			Health -= projectile.GetDamage ();
			
			//push the enemy ship in a direction
			if (Health <= 100) {
				gameObject.transform.parent.GetComponent<Rigidbody2D>().velocity += projectile.GetComponent<Rigidbody2D>().velocity / 10;
				gameObject.layer = (int)Types.Layers.DamagedEnemy;	
				gameObject.GetComponent<Collider2D>().isTrigger = false;
			}
			
			if (Health <= 0) {
				Kill ();
			}
			
			projectile.Hit ();
		}	
		
		EnemyBehaviour enemy = col.gameObject.GetComponent<EnemyBehaviour> ();
		
		if (enemy) {
			Kill ();
		}
	}
	
	void OnCollisionEnter2D (Collision2D collision)
	{
		EnemyBehaviour enemy = collision.gameObject.GetComponent<EnemyBehaviour> ();
		
		if (enemy) {
			Kill ();
		}
		
		Projectile projectile = collision.gameObject.GetComponent<Projectile> ();
		
		if (projectile) {
			Health -= projectile.GetDamage ();
					
			if (Health <= 0) {
				Kill ();
			}
			
			projectile.Hit ();
		}	
	}
	
	#endregion
	
	#region properties
	
	public GameObject ProjectilePrefab;
	public float Health = 200f;
	public float ProjectileSpeed = 10f;
	public float ShotsPerSecond = 0.5f;
	public int ScoreValue = 150;	
	public AudioClip FireSound;
	public AudioClip DeathSound;
	public GameObject Explosion;
	
	#endregion						
																				
	#region methods
	
	/// <summary>
	/// Kill this instance.
	/// </summary>
	public void Kill ()
	{
		Destroy (gameObject);
		Instantiate (Explosion, gameObject.transform.position, Quaternion.identity);
		
		//reset formation position
		gameObject.transform.parent.localPosition = parentPosition;
		gameObject.transform.parent.GetComponent<Rigidbody2D>().velocity = new Vector2 ();
		

		
		scoreKeeper.ScorePoints (ScoreValue);
		AudioSource.PlayClipAtPoint (DeathSound, transform.position);
	}
	
	/// <summary>
	/// Fire a projectile.
	/// </summary>
	void Fire ()
	{
		Vector3 startPosition = transform.position + new Vector3 (0, -1, 0);
		GameObject projectile = Instantiate (ProjectilePrefab, startPosition, Quaternion.identity) as GameObject;	
		projectile.GetComponent<Rigidbody2D>().velocity = new Vector3 (0, -ProjectileSpeed, 0);
		
		AudioSource.PlayClipAtPoint (FireSound, transform.position);
	}
	#endregion
}
