using UnityEngine;
using System.Collections;

public class AvatarScript : MonoBehaviour
{

    #region Fields
    private bool _isGotBall;
    private Vector3 _initPosition;
    private int _team;
    private Transform _ball;
    private Transform _teamGoal;
    private Transform _enemyGoal;
    [SerializeField]
    private Transform _myPlayer;
    [SerializeField]
    private Transform _myBall;
    #endregion

    #region Properties
    public Transform MyBall
    {
        get { return _myBall; }
        set { _myBall = value; }
    }
    public Transform MyPlayer
    {
        get { return _myPlayer; }
        set { _myPlayer = value; }
    }
    public bool IsGotBall
    {
        get { return _isGotBall; }
        set { _isGotBall = value; }
    }
    public int Team
    {
        get { return _team; }
        set { _team = value; }
    }
    public Transform Ball
    {
        get { return _ball; }
        set { _ball = value; }
    }
    public Transform EnemyGoal
    {
        get { return _enemyGoal; }
        set { _enemyGoal = value; }
    }
    #endregion

    #region Private Methods
    void Start()
    {
        _isGotBall = false;
        _myBall.gameObject.SetActive(false);
        _initPosition = _myPlayer.position;
        MyResources.Scored += new MyResources.BallHasMovedDelegate(Scored);
        MyResources.GameEnd += new MyResources.GameEventDelegate(EndGame);
    }

    void EndGame(bool isFirstTeamWon)
    {
        _isGotBall = false;
        _myBall.gameObject.SetActive(false);
        _myPlayer.position = _initPosition;
    }

    void Scored(Transform ball, int team)
    {
        _isGotBall = false;
        _myBall.gameObject.SetActive(false);
        _myPlayer.position = _initPosition;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.Equals(Ball))
        {
            _myBall.gameObject.SetActive(true);
            _isGotBall = true;
            MyResources.GotTheBallEvent(Ball, _team);
        }
        if (collision.transform.Equals(_enemyGoal))
        {
            if (_isGotBall)
                MyResources.ScoredEvent(Ball, _team);
        }
        if (collision.transform.name.Equals("Minion"))
        {
            MinionScript coll = (MinionScript) collision.gameObject.GetComponent("MinionScript");
            if (coll.IsGotBall && coll.Team == _team)
            {
                _myBall.gameObject.SetActive(true);
                _isGotBall = true;
                MyResources.GotTheBallEvent(Ball, _team);
            }
        }
    }

    void Update()
    {
        if (_isGotBall)
            MyResources.MoveWithBallEvent(_myPlayer, _team);

    }
    #endregion
}
