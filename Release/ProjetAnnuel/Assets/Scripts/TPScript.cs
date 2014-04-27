using UnityEngine;
using System.Collections;

public class TPScript : MonoBehaviour
{
    #region Fields
    [SerializeField]
    Transform _myTransform;
    [SerializeField]
    Transform _myTarget;
    Vector3 _targetPos;

    #endregion

    #region Properties
    public Transform MyTransform
    {
        get { return _myTransform; }
        set { _myTransform = value; }
    }

    public Transform MyTarget
    {
        get { return _myTarget; }
        set { _myTarget = value; }
    }
    #endregion

    #region Private Methods
    void Start()
    {
        _targetPos = _myTarget.position;
        _targetPos.y += 1;
        MyResources.PlayerWantToTP += new MyResources.TPDelegate(MyResources_PlayerWantToTP);
    }

    void MyResources_PlayerWantToTP(Transform player, Transform tp)
    {
        if(tp.Equals(_myTransform))
        {
            player.position = _targetPos;
            MyResources.PlayerWasTPEvent(player, null);
        }

    }
    #endregion
}
