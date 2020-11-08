using UnityEngine;
using System.Collections;

/// <summary>
/// Player controller.
/// </summary>
public class PlayerController : MonoBehaviour
{
	#region fields
	
	Vector3 playerPosition;
	float xmin;
	float xmax;
	
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
		
		
		xmin = leftMostPosition.x + 0.7f;
		xmax = rightMostPosition.x - 0.7f;
	}
	
	/// <summary>
	/// Update this instance.
	/// </summary>
	void Update ()
	{
		if (Input.GetKey (KeyCode.LeftArrow)) {
			
			transform.position += Vector3.left * ShipSpeed * Time.deltaTime;
			
		} else	if (Input.GetKey (KeyCode.RightArrow)) {
			
			transform.position += Vector3.right * ShipSpeed * Time.deltaTime;
		}
		
		if (Input.GetKeyDown (KeyCode.Space)) {
			InvokeRepeating ("Fire", 0.000001f, FireRate);
		}
		
		if (Input.GetKeyUp (KeyCode.Space)) {
			
			CancelInvoke ("Fire");
		}
		
		float newX = Mathf.Clamp (transform.position.x, xmin, xmax);
		transform.position = new Vector3 (newX, transform.position.y, transform.position.z);
	}
	
	#endregion
		
	#region events
	
	/// <summary>
	/// Raises the trigger enter2 d event.
	/// </summary>
	/// <param name="col">The collider.</param>
	void OnTriggerEnter2D (Collider2D col)
	{
		Debug.Log ("Player got hit.");
		
		Projectile projectile = col.gameObject.GetComponent<Projectile> ();
		
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
	
	public AudioClip FireSound ;
	public GameObject ProjectilePrefab;
	public float ShipSpeed = 10f;
	public float ProjectileSpeed = 10f;
	public float FireRate = 0.1f;
	public float Health = 500.0f;	

		
	#endregion
					
	#region methods
	
	/// <summary>
	/// Fire this instance.
	/// </summary>
	void Fire ()
	{
		Vector3 startPosition = transform.position + new Vector3 (0, 1, 0);
		GameObject projectile = Instantiate (ProjectilePrefab, startPosition, Quaternion.identity) as GameObject;	
		projectile.GetComponent<Rigidbody2D>().velocity = new Vector3 (0, ProjectileSpeed, 0);
		
		AudioSource.PlayClipAtPoint (FireSound, transform.position);
	}
	
	/// <summary>
	/// Kill this instance.
	/// </summary>
	void Kill ()
	{
		Destroy (gameObject);
		LevelManager man = GameObject.Find ("LevelManager").GetComponent<LevelManager> ();
		man.LoadLevel ("Win");
	}
	
	#endregion
}
