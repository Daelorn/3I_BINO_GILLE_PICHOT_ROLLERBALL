using UnityEngine;
using System.Collections;

public class TeamScript : MonoBehaviour
{

    #region Fields
    [SerializeField]
    private Transform _myTeam;
    [SerializeField]
    private Transform _teamGoal;

    private Transform _ball;
    private Transform _enemyGoal;
    private int _nbTeam;
    private int _point;

    #endregion

    #region Properties
    public int Point
    {
        get { return _point; }
        set { _point = value; }
    }
    public int NbTeam
    {
        get { return _nbTeam; }
        set { _nbTeam = value; }
    }
    public Transform EnemyGoal
    {
        get { return _enemyGoal; }
        set { _enemyGoal = value; }
    }
    public Transform Ball
    {
        get { return _ball; }
        set { _ball = value; }
    }
    public Transform MyTeam
    {
        get { return _myTeam; }
        set { _myTeam = value; }
    }
    public Transform TeamGoal
    {
        get { return _teamGoal; }
        set { _teamGoal = value; }
    }
    #endregion

    #region Private Methods
    // Use this for initialization
    void Start()
    {
        _point = 0;
        foreach(Transform child in transform)
        {
            if (child.name.Equals("Minion"))
            {
                MinionScript minion = (MinionScript)child.gameObject.GetComponent("MinionScript");
                minion.Team = this._nbTeam;
                minion.Ball = this._ball;
                minion.TeamGoal = this.TeamGoal;
                minion.EnemyGoal = this._enemyGoal;
            }
            if (child.name.Equals("Player"))
            {
                AvatarScript avatar = (AvatarScript)child.gameObject.GetComponent("AvatarScript");
                avatar.Team = this._nbTeam;
                avatar.Ball = this._ball;
                avatar.EnemyGoal = this._enemyGoal;
            }
            if (child.name.Equals("Goal"))
            {
                GoalScript goal = (GoalScript)child.gameObject.GetComponent("GoalScript");
                goal.Team = this._nbTeam == 1 ? 0 : 1;
                goal.Ball = this._ball;
            }
        }
        MyResources.Scored += new MyResources.BallHasMovedDelegate(MinionScript_Scored);
	}

    void MinionScript_Scored(Transform ball, int team)
    {
        this._point++;
    }
    #endregion
}


