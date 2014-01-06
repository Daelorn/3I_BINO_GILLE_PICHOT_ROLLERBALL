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
    public const float PLAYER_SPEED = 7;
    public const float PLAYER_BACKWARD_SPEED = 4;
    public const float PLAYER_ROTATION_SPEED = 60;
    #endregion

    #region GameResources
    public const float MAX_SCORE = 3;
    #endregion

    #region EventResources
    #region Delegates
    public delegate void BallHasMovedDelegate(Transform ball, int team);
    public delegate void GameEventDelegate(bool value);
    #endregion

    #region Events
    public static event BallHasMovedDelegate GotTheBall;
    public static event BallHasMovedDelegate DropTheBall;
    public static event BallHasMovedDelegate MoveWithBall;
    public static event BallHasMovedDelegate Scored;

    public static event GameEventDelegate GameStart;
    public static event GameEventDelegate GameEnd;
    #endregion

    #region CallingEventMethods
    public static void GotTheBallEvent(Transform ball, int team)
    {
        GotTheBall(ball, team);
    }
    public static void DropTheBallEvent(Transform ball, int team)
    {
        DropTheBall(ball, team);
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

    #endregion
    #endregion
}
