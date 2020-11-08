using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerControls : MonoBehaviour
{

    public float speed = 10.0f;
    public float boundY = 2.25f;
    private Rigidbody2D rb2d;
    private float ballPositionCoeff = 0.68268f;
    private float ballDirectionCoeff = 2.869196f;
    private float freeCoeff = 0.0579048f;
    private bool move = false;
    private float targetPosition = 0.0f;

    // Use this for initialization
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// MoveAI 
    /// </summary>
    void MoveAI(LearningData data)
    {
        float position = (float)(ballPositionCoeff * data.BallYPosition + ballDirectionCoeff * data.BallDirection + freeCoeff);

        if (position > 3.0f - 0.75f)
        {
            position = position - (position - 3.0f + 0.75f);
        }
        else if (position < -3.0f + 0.75f)
        {
            position = position + (-3.0f + 0.75f - position);
        }

        targetPosition = position;
        move = true;
    }


    // Update is called once per frame
    void Update()
    {
        var pos = transform.position;

        if (move)
        {
            // move until getting to the position
            var vel = rb2d.velocity;

            // needs to go up
            if (pos.y < targetPosition)
            {
                vel.y = speed;
                rb2d.velocity = vel;
            }
            else if (pos.y > targetPosition)
            {
                vel.y = -speed;
                rb2d.velocity = vel;

            }

            pos = transform.position;

            if (pos.y > boundY)
            {
                pos.y = boundY;
            }
            else if (pos.y < -boundY)
            {
                pos.y = -boundY;
            }

            transform.position = pos;
        }
    }
}
