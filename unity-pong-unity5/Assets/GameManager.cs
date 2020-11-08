using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region fields

    GameObject learningDataManager;
    GameObject theBall;
    GameObject theAIPlayer;
    GameObject thePlayer;
    GameObject[] theSideWallTrigger;
    GameObject[] theSideWallCollider;

    #endregion

    #region properties

    public static int PlayerScore1 = 0;
    public static int PlayerScore2 = 0;
    public GUISkin layout;

    #endregion

    #region start // update

    void Start()
    {
        learningDataManager = GameObject.FindGameObjectWithTag("LearningDataManager");
        theBall = GameObject.FindGameObjectWithTag("Ball");
        theAIPlayer = GameObject.FindWithTag("PlayerAI");
        thePlayer = GameObject.FindWithTag("Player");
        theSideWallTrigger = GameObject.FindGameObjectsWithTag("SideWallTrigger");
        theSideWallCollider = GameObject.FindGameObjectsWithTag("SideWallCollider");

        foreach (var obj in theSideWallCollider)
        {
            obj.SetActive(false);
        }

    }

    #endregion

    #region methods

    public static void Score(string wallID)
    {
        if (wallID == "RightWall")
        {
            PlayerScore1++;
        }
        else
        {
            PlayerScore2++;
        }
    }

    void OnGUI()
    {
        GUI.skin = layout;
        GUI.Label(new Rect(100, 20, 100, 100), "" + PlayerScore1);
        GUI.Label(new Rect(Screen.width - 100, 20, 100, 100), "" + PlayerScore2);

        if (GUI.Button(new Rect(Screen.width / 2 - 470, 10, 180, 53), "NORMAL GAME"))
        {
            PlayerScore1 = 0;
            PlayerScore2 = 0;
            theAIPlayer.SetActive(true);
            thePlayer.SetActive(true);
            foreach (var obj in theSideWallTrigger)
            {
                obj.SetActive(true);
            }
            foreach (var obj in theSideWallCollider)
            {
                obj.SetActive(false);
            }
            theBall.SendMessage("StartGame", false, SendMessageOptions.RequireReceiver);
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 270, 10, 220, 53), "START GATHERING"))
        {
            PlayerScore1 = 0;
            PlayerScore2 = 0;
            theAIPlayer.SetActive(false);
            thePlayer.SetActive(false);
            foreach (var obj in theSideWallTrigger)
            {
                obj.SetActive(false);
            }
            foreach (var obj in theSideWallCollider)
            {
                obj.SetActive(true);
            }
            theBall.SendMessage("StartGame", true, SendMessageOptions.RequireReceiver);
        }

        if (GUI.Button(new Rect(Screen.width / 2 - 30, 10, 220, 53), "FINISH GATHERING"))
        {
            PlayerScore1 = 0;
            PlayerScore2 = 0;
            theAIPlayer.SetActive(true);
            thePlayer.SetActive(true);
            foreach (var obj in theSideWallTrigger)
            {
                obj.SetActive(true);
            }
            foreach (var obj in theSideWallCollider)
            {
                obj.SetActive(false);
            }
            theBall.SendMessage("FinishLearning", 0.5f, SendMessageOptions.RequireReceiver);
        }

        if (GUI.Button(new Rect(Screen.width / 2 + 210, 10, 120, 53), "STOP"))
        {
            PlayerScore1 = 0;
            PlayerScore2 = 0;
            foreach (var obj in theSideWallTrigger)
            {
                obj.SetActive(true);
            }
            foreach (var obj in theSideWallCollider)
            {
                obj.SetActive(false);
            }
            theBall.SendMessage("ResetBall", SendMessageOptions.RequireReceiver);
        }

        if (GUI.Button(new Rect(Screen.width / 2 + 350, 10, 120, 53), "RESTART"))
        {
            PlayerScore1 = 0;
            PlayerScore2 = 0;
            foreach (var obj in theSideWallTrigger)
            {
                obj.SetActive(true);
            }
            foreach (var obj in theSideWallCollider)
            {
                obj.SetActive(false);
            }
            theBall.SendMessage("RestartGame", SendMessageOptions.RequireReceiver);
        }

        //if (PlayerScore1 == 10)
        //{
        //    GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER ONE WINS");
        //    theBall.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        //}
        //else if (PlayerScore2 == 10)
        //{
        //    GUI.Label(new Rect(Screen.width / 2 - 150, 200, 2000, 1000), "PLAYER TWO WINS");
        //    theBall.SendMessage("ResetBall", null, SendMessageOptions.RequireReceiver);
        //}
    }

    #endregion
}
