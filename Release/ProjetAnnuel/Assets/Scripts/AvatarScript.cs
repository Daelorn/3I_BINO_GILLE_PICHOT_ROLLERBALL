using UnityEngine;
using System.Collections;

public class AvatarScript : MonoBehaviour
{
     
    #region Fields
    private bool _isGotBall;
    private Vector3 _initPosition;
    private int _team;
    private float _speed;
    private Transform _actualTeleport;
    private Transform _ball;
    private Transform _teamGoal;
    private Transform _enemyGoal;
    [SerializeField]
    private Transform _myPlayer;
    [SerializeField]
    private Transform _myBall;
    private PlayerType _myType;
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
        _actualTeleport = null;
        _isGotBall = false;
        _myBall.gameObject.SetActive(false);
        _initPosition = _myPlayer.position;
        MyResources.Scored += new MyResources.BallHasMovedDelegate(Scored);
        MyResources.GameEnd += new MyResources.GameEventDelegate(EndGame);
        MyResources.PlayerWantToMove += new MyResources.MoveDelegate(MyResources_PlayerWantToMove);
        MyResources.PlayerSelectedType += new MyResources.TypeDelegate(MyResources_PlayerSelectedType);
        MyResources.PlayerWantToAction += new MyResources.MoveDelegate(MyResources_PlayerWantToAction);

    }

    void MyResources_PlayerWantToAction(Transform player, Vector3 targetPosition)
    {
        if (player.Equals(_myPlayer))
        {
            if (this._isGotBall)
            {
                this._isGotBall = false;
                this.MyBall.gameObject.SetActive(false);
                Vector3 direction = (targetPosition - _myPlayer.position);
                MyResources.DropTheBallEvent(_myPlayer, direction);
            }
            else
            {
                Collider[] nears = Physics.OverlapSphere(_myPlayer.position, 5f);
                foreach (Collider anear in nears)
                {
                    try
                    {
                        Transform near = anear.transform.parent;
                        if(near.name.Equals("Minion"))
                        {
                            print("minion");
                            MinionScript coll = (MinionScript)near.gameObject.GetComponent("MinionScript");
                            print("minion pass");
                            if (coll.IsGotBall)
                            {
                                coll.LostTheBall();
                                _myBall.gameObject.SetActive(true);
                                _isGotBall = true;
                                MyResources.GotTheBallEvent(Ball, _team);
                            }
                        }
                        if(near.name.Equals("Player"))
                        {
                            if (!near.Equals(_myPlayer))
                            {
                                AvatarScript coll = (AvatarScript)near.gameObject.GetComponent("AvatarScript");
                                if (coll._isGotBall)
                                {
                                    _myPlayer.audio.Play();
                                    coll.LostTheBall();
                                    _myBall.gameObject.SetActive(true);
                                    _isGotBall = true;
                                    MyResources.GotTheBallEvent(Ball, _team);
                                }
                            }
                        }
                    }
                    catch { print("pas de parent"); }
                }
            }
        }
    }

    void MyResources_PlayerSelectedType(Transform player, PlayerType tplayer)
    {
        if (player.Equals(_myPlayer))
        {
            _myType = tplayer;
            Transform tmpTrans = _myPlayer.FindChild("PlayerFormRoller");
            tmpTrans.gameObject.SetActive(false);
            tmpTrans = _myPlayer.FindChild("PlayerFormMoto");
            tmpTrans.gameObject.SetActive(false);

            if (_myType == PlayerType.Roller)
            {
                _speed = MyResources.PLAYER_ROLLER_SPEED;
                tmpTrans = _myPlayer.FindChild("PlayerFormRoller");
                tmpTrans.gameObject.SetActive(true);
                tmpTrans.audio.loop = true;
                tmpTrans.audio.Play();
            }
            else
            {
                _speed = MyResources.PLAYER_MOTOR_SPEED;
                tmpTrans = _myPlayer.FindChild("PlayerFormMoto");
                tmpTrans.gameObject.SetActive(true);
                tmpTrans.audio.loop = true;
                tmpTrans.audio.Play();
            }
        }
    }

    void MyResources_PlayerWantToMove(Transform player, Vector3 direction)
    {
        if (player.Equals(_myPlayer))
        {
            Vector3 tmp = direction * _speed * Time.deltaTime;
            player.Translate(tmp);
        }
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
        string ColName = collision.transform.name;
        if (!ColName.Equals("Terrain"))
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
                MinionScript coll = (MinionScript)collision.gameObject.GetComponent("MinionScript");
                if (coll.IsGotBall && coll.Team == _team)
                {
                    _myBall.gameObject.SetActive(true);
                    _isGotBall = true;
                    MyResources.GotTheBallEvent(Ball, _team);
                }
            }
        }
    }

    //void OnCollisionExit(Collision collision)
    //{
    //    if (collision.transform.name.Contains("TP") && _actualTeleport != collision.transform)
    //    {
    //        _actualTeleport = null;
    //    }
    //}

    void Update()
    {
        if (_isGotBall)
            MyResources.MoveWithBallEvent(_myPlayer, _team);

    }

    void OnTriggerEnter(Collider other)
    {
        if (this._myType == PlayerType.Roller && _actualTeleport == null)
        {
            _actualTeleport = other.transform;
            MyResources.PlayerWantToTPEvent(this._myPlayer, other.transform);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.name.Contains("TP") && _actualTeleport != other.transform)
        {
            _actualTeleport = null;
        }
    }
    #endregion

    #region Public Methods
    /// <summary>
    /// Delete the ball if minion have it
    /// </summary>
    public void LostTheBall()
    {
        _myBall.gameObject.SetActive(false);
        _isGotBall = false;
    }
    #endregion
}
