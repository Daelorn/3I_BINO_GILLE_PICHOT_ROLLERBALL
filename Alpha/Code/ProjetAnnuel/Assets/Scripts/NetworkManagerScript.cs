using UnityEngine;
using System.Collections;

public class NetworkManagerScript : MonoBehaviour {

    [SerializeField]
    private bool _isServer = true;
    public bool IsServer
    {
        get { return _isServer; }
        set { _isServer = value; }
    }
    public Camera actualView;

	// Use this for initialization
    void Start()
    {
        Application.runInBackground = true;

        if (IsServer)
        {
            Network.InitializeSecurity();
            Network.InitializeServer(3, 6600, true);
        }
        else
        {
            Network.Connect("127.0.0.1", 6600);
        }
	}
	// Update is called once per frame
	void Update () {
	
	}
}
