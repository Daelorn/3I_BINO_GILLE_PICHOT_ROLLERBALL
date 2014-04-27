using UnityEngine;
using System.Collections;

public class MyResources : MonoBehaviour
{

    #region MinionStats
    public const float CATCHING_WAITING_TIME = 1;
    public const float CAUGHT_WAITING_TIME = (float) 1.5;
    public const float START_GAME_WAITING_TIME = 5;
    public const float GOAL_LOST_WAITING_TIME = 5;
    public const float GOAL_WIN_WAITING_TIME = (float) 5.1;

    public const float MINION_SPEED = 5;
    #endregion

    #region PlayerStats
    public const float PLAYER_ROLLER_SPEED = 7;
    public const float PLAYER_MOTOR_SPEED = 15;
    public const float PLAYER_BACKWARD_SPEED = 4;
    public const float PLAYER_ROTATION_SPEED = 60;
    #endregion

    #region GameResources
    public const float MAX_SCORE = 3;
    public const float TOP_AREA_HEIGHT = 15;
    public const string URL_SERVER_LIST = @"http://localhost/RollerBowlWebSite/Home/ListServer";
    public static ConnectionData ConnexionData = new ConnectionData(6600, "127.0.0.1", 3);
    public static bool IsServer = true;
    public static bool IsPausedGame = false;

    public static KeyCode goForward = KeyCode.UpArrow;
    public static KeyCode goLeft = KeyCode.LeftArrow;
    public static KeyCode goRight = KeyCode.RightArrow;
    public static KeyCode goPause = KeyCode.Space;
    public static KeyCode goAction = KeyCode.Mouse0;

    #endregion

    #region EventResources
    #region Delegates
    public delegate void BallHasMovedDelegate(Transform ball, int team);
    public delegate void GameEventDelegate(bool value);
    public delegate void TPDelegate(Transform player, Transform tp);
    public delegate void MoveDelegate(Transform player, Vector3 direction);
    public delegate void TypeDelegate(Transform player, PlayerType tplayer);
    #endregion

    #region Events
    public static event BallHasMovedDelegate GotTheBall;
    public static event MoveDelegate DropTheBall;
    public static event BallHasMovedDelegate MoveWithBall;
    public static event BallHasMovedDelegate Scored;

    public static event GameEventDelegate GameStart;
    public static event GameEventDelegate GameEnd;

    public static event TPDelegate PlayerWantToTP;
    public static event TPDelegate PlayerWasTP;

    public static event MoveDelegate PlayerWantToMove;
    public static event MoveDelegate PlayerWantToAction;

    public static event TypeDelegate PlayerSelectedType;
    #endregion

    #region CallingEventMethods
    public static void GotTheBallEvent(Transform ball, int team)
    {
        GotTheBall(ball, team);
    }
    public static void DropTheBallEvent(Transform thrower, Vector3 ballPos)
    {
        DropTheBall(thrower, ballPos);
    }
    public static void MoveWithBallEvent(Transform ball, int team)
    {
        MoveWithBall(ball, team);
    }
    public static void ScoredEvent(Transform ball, int team)
    {
        Scored(ball, team);
    }

    public static void GameStartEvent(bool value)
    {
        GameStart(value);
    }
    public static void GameEndEvent(bool isFirstTeamWon)
    {
        GameEnd(isFirstTeamWon);
    }

    public static void PlayerWantToTPEvent(Transform player, Transform tp)
    {
        PlayerWantToTP(player, tp);
    }
    public static void PlayerWasTPEvent(Transform player, Transform tp)
    {
        PlayerWasTP(player, tp);
    }

    public static void PlayerWantToMoveEvent(Transform player, Vector3 direction)
    {
        PlayerWantToMove(player, direction);
    }
    public static void PlayerWantToActionEvent(Transform player, Vector3 mousePosition)
    {
        PlayerWantToAction(player, mousePosition);
    }

    public static void PlayerSelectedTypeEvent(Transform player, PlayerType tplayer)
    {
        PlayerSelectedType(player, tplayer);
    }
    #endregion
    #endregion
}

public enum PlayerType
{
    Moto = 1,
    Roller = 2
}

public enum ActionSceneType
{
    Quit = 0,
    Back = 1,
    LinkToGame = 2,
    LinkToServer = 3,
    LinkToListServer = 4,
    AddLinkToServer = 5,
    LinkToOption = 6,
    ChangeTouch = 7
}

public class ConnectionData
{

    #region Fields
    private int _port;
    private string _IP;
    private int _maxConnexion;
    private int _actualConnexions;
    #endregion

    #region Properties
    public int ActualConnexions
    {
        get { return _actualConnexions; }
        set { _actualConnexions = value; }
    }
    public int Port
    {
        get { return _port; }
        set { _port = value; }
    }
    public string IP
    {
        get { return _IP; }
        set { _IP = value; }
    }
    public int MaxConnexion
    {
        get { return _maxConnexion; }
        set { _maxConnexion = value; }
    }
    #endregion

    #region Public Methods
    public ConnectionData(int port, string ip, int maxConnexion)
    {
        _port = port;
        _IP = ip;
        _maxConnexion = maxConnexion;
    }
    public ConnectionData()
    {
        _port = 0;
        _IP = "";
        _maxConnexion = 0;
    }
    public bool AddPlayer()
    {
        if (_maxConnexion > _actualConnexions)
        {
            _actualConnexions++;
            return true;
        }
        else return false;
    }
    #endregion
}