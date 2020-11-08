using UnityEngine;
using System.Collections;

public class ScrollingObject : MonoBehaviour
{
	
	private Rigidbody2D rb2d;

	/// <summary>
	/// Start this instance.
	/// </summary>
	void Start ()
	{
		rb2d = GetComponent<Rigidbody2D> ();
		rb2d.velocity = new Vector2 (0, -0.5f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		//if (gameover) {
		//	rb2d.velocity = Vector2.zero;
		//}
	}
}
