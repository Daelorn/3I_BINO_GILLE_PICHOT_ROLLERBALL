using UnityEngine;
using System.Collections;

public class GoalScript : MonoBehaviour
{
    #region Fields
    private int _team;
    private Transform _ball;
    #endregion
    #region Properties
    public int Team { get { return _team; } set { _team = value; } }
    public Transform Ball { get { return _ball; } set { _ball = value; } }
    #endregion
    #region Priate Methods

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.Equals(Ball))
        {
            MyResources.ScoredEvent(_ball, _team);
        }
    }

    #endregion
}
