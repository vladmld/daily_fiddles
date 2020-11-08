using UnityEngine;

public class MiddleFieldControl : MonoBehaviour
{
    GameObject theBall;

    // Use this for initialization
    void Start()
    {
        theBall = GameObject.FindGameObjectWithTag("Ball");
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.name == "Ball")
        {
            theBall.SendMessage("SetBallPoints");
        }
    }

}
