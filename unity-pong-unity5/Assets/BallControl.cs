using Assets;
using Assets.Structs;
using System.Collections;
using UnityEngine;

public class BallControl : MonoBehaviour
{
    #region fields

    private Rigidbody2D rb2d;
    private Vector2 vel;
    private LearningData gatheredData;
    private GameObject learningDataManager;
    private GameObject theAIPlayer;
    private bool gateringData = false;
    #endregion

    #region start // update

    /// <summary>
    /// The start method.
    /// </summary>
    void Start()
    {
        learningDataManager = GameObject.FindGameObjectWithTag("LearningDataManager");
        theAIPlayer = GameObject.FindWithTag("PlayerAI");
        rb2d = GetComponent<Rigidbody2D>();

        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startWidth = 0.1F;
        lineRenderer.endWidth = 0.1F;
    }

    private void Update()
    {

    }

    #endregion

    #region methods

    /// <summary>
    /// Start the ball movement
    /// </summary>
    void GoBall()
    {
        float rand = Random.Range(0, 2);
        if (rand < 1)
        {
            rb2d.AddForce(new Vector2(20, -7.05f));
        }
        else
        {
            rb2d.AddForce(new Vector2(-20, +7.05f));
        }

        SetBallPoints();
    }

    /// <summary>
    /// Send the ball in a random direction
    /// </summary>
    /// <param name="rigidBody">The rigid body of the ball</param>
    private void Throw(Rigidbody2D rigidBody)
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Random.Range(0, 360), transform.eulerAngles.z);
        float speed = 600;
        rigidBody.isKinematic = false;
        Vector3 force = transform.forward;
        force = new Vector3(force.x, 1, force.z);
        rigidBody.AddForce(force * speed);
    }

    /// <summary>
    /// Set the points of the ball
    /// </summary>
    void SetBallPoints()
    {
        gatheredData = new LearningData();
        StartCoroutine(GatherPoints(gatheredData));
    }

    /// <summary>
    /// Gather the points
    /// </summary>
    /// <param name="gatheredData">The learning data object.</param>
    /// <returns></returns>
    IEnumerator GatherPoints(LearningData gatheredData)
    {
        CustomVector3 firstPosition = new CustomVector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        gatheredData.BallYPosition = firstPosition.Y;

        yield return new WaitForSeconds(Time.deltaTime * 7);

        CustomVector3 secondPosition = new CustomVector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
        gatheredData.BallDirection = (secondPosition.Y - firstPosition.Y) / (secondPosition.X - firstPosition.X);

        LineRenderer lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(0, new Vector3(firstPosition.X, firstPosition.Y, firstPosition.Z));
        lineRenderer.SetPosition(1, new Vector3(secondPosition.X, secondPosition.Y, secondPosition.Z));

        // move only when the ball is going to the right
        if (!gateringData && secondPosition.X > 0)
        {
            // send message to move the paddle
            theAIPlayer.SendMessage("MoveAI", gatheredData);
        }
    }

    /// <summary>
    /// Get the position of the paddle
    /// </summary>
    /// <param name="paddlePosition"></param>
    void GetPaddlePosition(Vector2 contactPoint)
    {
        LineRenderer lineRenderer = GetComponent<LineRenderer>();

        if (contactPoint.y > 3.0f - 0.75f)
        {
            contactPoint.y = contactPoint.y - (contactPoint.y - 3.0f + 0.75f);
        }
        else if (contactPoint.y < -3.0f + 0.75f)
        {
            contactPoint.y = contactPoint.y + (-3.0f + 0.75f - contactPoint.y);
        }

        gatheredData.PaddleYPosition = contactPoint.y;

        lineRenderer.SetPosition(0, new Vector3(contactPoint.x, contactPoint.y + 0.75f, 0));
        lineRenderer.SetPosition(1, new Vector3(contactPoint.x, contactPoint.y - 0.75f, 0));

        if (contactPoint.x > 0 && gatheredData.BallYPosition != 0 && gatheredData.BallDirection != float.NaN)
        {
            learningDataManager.SendMessage("AddData", gatheredData);
        }
    }

    /// <summary>
    /// Reset ball position
    /// </summary>
    void ResetBall()
    {
        vel = new Vector2(0, 0);
        rb2d.velocity = vel;
        this.transform.position = Vector2.zero;
    }

    /// <summary>
    /// Restart the game
    /// </summary>
    void RestartGame()
    {
        ResetBall();
        Invoke("GoBall", 3);
    }

    /// <summary>
    /// Restart the game
    /// </summary>
    void StartGame(bool isLearning)
    {
        ResetBall();
        gateringData = isLearning;

        if (isLearning)
        {
            learningDataManager.SendMessage("LoadGatheredData", SendMessageOptions.RequireReceiver);
            Invoke("GoBall", 0);
        }
        else
        {
            Invoke("GoBall", 3);
        }
    }

    /// <summary>
    /// Restart the game
    /// </summary>
    void FinishLearning()
    {
        ResetBall();
        gateringData = false;
        learningDataManager.SendMessage("SaveGatheredData", SendMessageOptions.RequireReceiver);
    }

    #endregion

    #region events

    /// <summary>
    /// On collision enter event.
    /// </summary>
    /// <param name="coll"></param>
    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.collider.CompareTag("Player"))
        {
            //vel.x = rb2d.velocity.x;
            //vel.y = (rb2d.velocity.y / 2.0f) + (coll.collider.attachedRigidbody.velocity.y / 3.0f);
            //rb2d.velocity = vel;
        }
    }

    #endregion

}
