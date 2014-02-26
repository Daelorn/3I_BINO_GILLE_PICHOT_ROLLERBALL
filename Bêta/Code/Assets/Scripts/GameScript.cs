using UnityEngine;
using System.Collections;


public class GameScript : MonoBehaviour
{

    #region Fields
    private Vector3 _ballInitPosition;
    private int _team1;
    private int _team2;
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
        _team1 = 0;
        _team2 = 0;
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
        if (team == 1)
            _team1++;
        else
            _team2++;

        if (_team2 == MyResources.MAX_SCORE || _team1 == MyResources.MAX_SCORE)
            MyResources.GameEndEvent(team == 1);

        else
        {
            ball.position = _ballInitPosition;
            ball.gameObject.SetActive(true);
        }
    }
    #endregion
}
