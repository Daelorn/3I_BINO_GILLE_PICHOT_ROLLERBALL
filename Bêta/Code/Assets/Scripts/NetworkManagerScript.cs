using UnityEngine;
using System.Collections;

public class NetworkManagerScript : MonoBehaviour
{

    #region Fields
    [SerializeField]
    private bool _isServer = true;
    #endregion

    #region Properties
    public bool IsServer
    {
        get { return _isServer; }
        set { _isServer = value; }
    }
    public Camera actualView;
    #endregion

    #region Private Methods

    // Use this for initialization
    void Start()
    {
        Application.runInBackground = true;

        if (MyResources.IsServer)
        {

            Network.InitializeSecurity();

            //TODO : AurélienGILLE 2014-02-24 -> Chercher IP actuelle
            //TODO : AurélienGILLE 2014-02-24 -> Envoyer Donnée au site
            Network.InitializeServer(3, 6600, true);
        }
        else
        {
            Network.Connect(MyResources.ConnexionData.IP, MyResources.ConnexionData.Port);
        }
    }
    #endregion
}
