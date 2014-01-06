using UnityEngine;
using System.Collections;
using System;


public class MinionScript : MonoBehaviour
{

    #region Fields
    private bool _isGotBall;
    private bool _isGameRun;
    private Vector3 _actualGoal;
    private float _waitingTime;
    private Vector3 _initPosition;
    private int _team;
    private Transform _teamGoal;
    private Transform _enemyGoal;
    private Transform _ball;
    private float _walkSpeed;

    [SerializeField]
    private Transform _myBall;
    [SerializeField]
    private Transform _myMinion;
    #endregion

    #region Properties

    public bool IsGotBall
    {
        get { return _isGotBall; }
        set { _isGotBall = value; }
    }

    /// <summary>
    /// Gets / sets teamNumber
    /// </summary>
    public int Team
    {
        get { return _team; }
        set { _team = value; }
    }

    /// <summary>
    /// Gets / sets the ball object of the minion
    /// </summary>
    public Transform MyBall
    {
        get { return _myBall; }
        set { _myBall = value; }
    }

    /// <summary>
    /// Gets / sets the ball object of the minion
    /// </summary>
    public Transform Ball
    {
        get { return _ball; }
        set { _ball = value; }
    }

    /// <summary>
    /// Gets / sets the enemy goal object
    /// </summary>
    public Transform EnemyGoal
    {
        get { return _enemyGoal; }
        set { _enemyGoal = value; }
    }

    /// <summary>
    /// Gets / sets the minion object
    /// </summary>
    public Transform MyMinion
    {
        get { return _myMinion; }
        set { _myMinion = value; }
    }

    /// <summary>
    /// Gets / sets the ally goal object
    /// </summary>
    public Transform TeamGoal
    {
        get { return _teamGoal; }
        set { _teamGoal = value; }
    }
    #endregion

    #region Private Methods
    void Start()
    {
        _isGotBall = false;
        _myBall.gameObject.SetActive(false);
        _initPosition = _myMinion.position;
        _actualGoal = _myMinion.position;
        _walkSpeed = MyResources.MINION_SPEED;
        _isGameRun = false;
        MyResources.GotTheBall += new MyResources.BallHasMovedDelegate(AnotherGotTheBall);
        MyResources.MoveWithBall += new MyResources.BallHasMovedDelegate(BallMove);
        MyResources.Scored += new MyResources.BallHasMovedDelegate(Scored);
        MyResources.GameStart += new MyResources.GameEventDelegate(GameStart);
        MyResources.GameEnd += new MyResources.GameEventDelegate(EndGame);
	}

    void GameStart(bool value)
    {
        _isGameRun = true;
        _actualGoal = _ball.position;
        _waitingTime = MyResources.START_GAME_WAITING_TIME;
    }

    void Scored(Transform ball, int team)
    {
        _isGotBall = false;
        _myBall.gameObject.SetActive(false);
        _myMinion.position = _initPosition;
        _actualGoal = ball.position;
        if (team == _team)
            _waitingTime = MyResources.GOAL_WIN_WAITING_TIME;
        else
            _waitingTime = MyResources.GOAL_LOST_WAITING_TIME;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.Equals(Ball))
        {
            _myBall.gameObject.SetActive(true);
            _isGotBall = true;
            MyResources.GotTheBallEvent(Ball, _team);
        }

        if (collision.transform.name.Equals("Minion"))
        {
            MinionScript coll = (MinionScript) collision.gameObject.GetComponent("MinionScript");
            if (coll.Team != this.Team && _waitingTime <= 0)
            {
                if (_isGotBall)
                    _waitingTime = MyResources.CAUGHT_WAITING_TIME;
                else if (coll.IsGotBall)
                    _waitingTime =  MyResources.CATCHING_WAITING_TIME;

            }
        }

        if (collision.transform.name.Equals("Player"))
        {
            AvatarScript coll = (AvatarScript) collision.gameObject.GetComponent("AvatarScript");
            if (_isGotBall && coll.Team == _team)
            {
                _myBall.gameObject.SetActive(false);
                _isGotBall = false;
            }
        }

        if (collision.transform.Equals(_enemyGoal))
        {
            if(_isGotBall)
                MyResources.ScoredEvent(Ball, _team);
        }
    }

    void EndGame(bool value)
    {
        print("Pouet");
        _isGotBall = false;
        _isGameRun = false;
        _myBall.gameObject.SetActive(false);
        _myMinion.position = _initPosition;
        _actualGoal = _myMinion.position;
        _myMinion = null;
    }

    void AnotherGotTheBall(Transform ball, int team)
    {
        Vector3 actualPos = ball.position;
        if (team != 0)
        {
            if (_isGotBall)
            {
                _actualGoal = _enemyGoal.position;
            }
            else
            {
                _actualGoal = actualPos;
            }
        }
        else
        {
            _actualGoal = actualPos;
        }
    }

    void BallMove(Transform ball, int team)
    {
        Vector3 actualPos = ball.position;
        if (!_isGotBall)
            _actualGoal = actualPos;
    }

    void MoveMinion()
    {
        if (_waitingTime <= 0)
        {
            _myMinion.position = Vector3.MoveTowards(_myMinion.position, _actualGoal, _walkSpeed * Time.deltaTime);

            if (_isGotBall)
                MyResources.MoveWithBallEvent(_myMinion, _team);
        }
    }

    void FixedUpdate()
    {
        _waitingTime -= Time.fixedDeltaTime;
        MoveMinion();
    }
    #endregion

}
