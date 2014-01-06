using UnityEngine;
using System.Collections;


public class GameScript : MonoBehaviour
{

    #region Fields
    private Vector3 _ballInitPosition;
    [SerializeField]
    private Transform _ball;
    #endregion

    #region Properties
    public Transform Ball
    {
        get { return _ball; }
        set { _ball = value; }
    }
    #endregion

    #region Private Methods
    void Start()
    {
        _ballInitPosition = Ball.position;
        int cptTeam = 0;
        TeamScript tmpTeam = null;
        TeamScript firstTeam = null;
        MyResources.Scored += new MyResources.BallHasMovedDelegate(MinionScript_Scored);
        MyResources.DropTheBall += new MyResources.BallHasMovedDelegate(MinionScript_DropTheBall);
        MyResources.GotTheBall += new MyResources.BallHasMovedDelegate(GotTheBall);
        foreach (Transform child in transform)
        {
            if (child.name.Equals("Team"))
            {
                cptTeam++;
                TeamScript team = (TeamScript)child.gameObject.GetComponent("TeamScript");
                team.NbTeam = cptTeam;
                team.Ball = _ball;

                if (cptTeam < 2)
                    firstTeam = team;
                else
                    team.EnemyGoal = tmpTeam.TeamGoal;
                
                tmpTeam = team;
            }
        }
        firstTeam.EnemyGoal = tmpTeam.TeamGoal;

	}

    void GotTheBall(Transform ball, int team)
    {
        ball.gameObject.SetActive(false);
    }

    void EndGame()
    {
        Ball.gameObject.SetActive(false);
    }

    void MinionScript_DropTheBall(Transform ball, int team)
    {
        ball.gameObject.SetActive(true);
    }

    void MinionScript_Scored(Transform ball, int team)
    {
        ball.position = _ballInitPosition;
        ball.gameObject.SetActive(true);
        foreach (Transform child in transform)
        {
            if (child.name.Equals("Team"))
            {
                TeamScript tmpTeam = (TeamScript)child.gameObject.GetComponent("TeamScript");
                if (tmpTeam.NbTeam == team && tmpTeam.Point == MyResources.MAX_SCORE)
                {
                    MyResources.GameEndEvent(true);
                }
            }
        }
    }
    #endregion
}
