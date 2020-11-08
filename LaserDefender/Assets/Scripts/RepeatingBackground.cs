using UnityEngine;
using System.Collections;

public class RepeatingBackground : MonoBehaviour
{
	private BoxCollider2D groundCollider;
	private float groundHorizontalLength;

	// Use this for initialization
	void Start ()
	{
		groundCollider = GetComponent<BoxCollider2D> ();
		groundHorizontalLength = groundCollider.size.y;
	}
	
	private void RepositionBackground ()
	{
		Vector2 groundOffset = new Vector2 (0, groundHorizontalLength * 2f);	
		transform.position = (Vector2)transform.position + groundOffset;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (transform.position.y < -groundHorizontalLength) {
			RepositionBackground ();
		}
	}
}
