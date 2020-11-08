using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SideColliderWalls : MonoBehaviour
{
    GameObject theBall;

    /// <summary>
    /// Initializes
    /// </summary>
    void Start()
    {
        theBall = GameObject.FindGameObjectWithTag("Ball");
    }

    /// <summary>
    /// Triggers on collision of the ball with the side collison walls
    /// </summary>
    /// <param name="coll"></param>
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.name.Equals("Ball"))
        {
            if (coll.contacts.Length > 0)
            {
                theBall.SendMessage("GetPaddlePosition", coll.contacts.First().point);
            }
        }
    }
}
