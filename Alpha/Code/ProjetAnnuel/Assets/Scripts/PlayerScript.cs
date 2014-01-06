using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerScript : MonoBehaviour
{

    #region Fields
    class sPlayerIntents
    {
        public bool _wantToTurnLeft = false;
        public bool _wantToTurnRight = false;
        public bool _wantToGoForward = false;
        public bool _wantToGoBackward = false;
    }
    private Dictionary<NetworkPlayer, sPlayerIntents> _playersIntents;
    private NetworkView _myNetworkView = null;
    private Transform _player1;
    private Transform _player2;
    private bool _isGameRunning;
    private Dictionary<NetworkPlayer, sPlayerIntents> PlayersIntents
    {
        get { return _playersIntents; }
        set { _playersIntents = value; }
    }
    [SerializeField]
    private Transform _team1;
    [SerializeField]
    private Transform _team2;
    [SerializeField]
    private Transform _endGameText;
    #endregion

    #region Properties
    public Transform Team1
    {
        get { return _team1; }
        set { _team1 = value; }
    }

    public Transform Team2
    {
        get { return _team2; }
        set { _team2 = value; }
    }

    public Transform EndGameText
    {
        get { return _endGameText; }
        set { _endGameText = value; }
    }
    #endregion

    #region Private Methods
    void Start()
    {
        PlayersIntents = new Dictionary<NetworkPlayer, sPlayerIntents>();
        _myNetworkView = this.gameObject.GetComponent<NetworkView>();
        _player1 = _team1.FindChild("Player");
        _player2 = _team2.FindChild("Player");
        _endGameText.guiText.enabled = false;
        _isGameRunning = false;
        MyResources.GameEnd += new MyResources.GameEventDelegate(GameEnd);
        MyResources.GameStart += new MyResources.GameEventDelegate(GameStart);
    }

    void GameStart(bool value)
    {
        if(Network.isServer)
            _myNetworkView.RPC("PartyStart", RPCMode.All);
    }

    void GameEnd(bool value)
    {
        _isGameRunning = false;
        if (Network.isServer)
        {
            int winner = value ? 1 : 0;
            int player = 1;
            foreach (NetworkPlayer np in _playersIntents.Keys)
            {

                _myNetworkView.RPC("PlayerEndParty", RPCMode.All, np, player, winner);
                player++;
            }
        }
    }

    void OnPlayerConnected(NetworkPlayer p)
    {
        PlayersIntents.Add(p, new sPlayerIntents());
        _myNetworkView.RPC("NewPlayerConnected", RPCMode.OthersBuffered, p);

        if (Network.connections.Length == 2)
        {
            int player = 1;
            foreach (NetworkPlayer np in _playersIntents.Keys)
            {

                _myNetworkView.RPC("PlayerGetCamera", RPCMode.All, np, player);
                player++;
            }
            MyResources.GameStartEvent(true);
        }
    }

    [RPC]
    void NewPlayerConnected(NetworkPlayer p)
    {
        PlayersIntents.Add(p, new sPlayerIntents());

    }

    void Update()
    {
        if (Network.isClient && _isGameRunning)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                _myNetworkView.RPC("PlayerWantToGoForward", RPCMode.Server, Network.player, true);
            if (Input.GetKeyUp(KeyCode.UpArrow))
                _myNetworkView.RPC("PlayerWantToGoForward", RPCMode.Server, Network.player, false);
            if (Input.GetKeyDown(KeyCode.LeftArrow))
                _myNetworkView.RPC("PlayerWantToTurnLeft", RPCMode.Server, Network.player, true);
            if (Input.GetKeyUp(KeyCode.LeftArrow))
                _myNetworkView.RPC("PlayerWantToTurnLeft", RPCMode.Server, Network.player, false);
            if (Input.GetKeyDown(KeyCode.RightArrow))
                _myNetworkView.RPC("PlayerWantToTurnRight", RPCMode.Server, Network.player, true);
            if (Input.GetKeyUp(KeyCode.RightArrow))
                _myNetworkView.RPC("PlayerWantToTurnRight", RPCMode.Server, Network.player, false);

        }
    }

    void FixedUpdate()
    {
            int i = 0;
            foreach (var p in _playersIntents)
            {
                if (i == 0 && p.Value._wantToGoForward)
                    _player1.Translate(Vector3.left * MyResources.PLAYER_SPEED * Time.deltaTime);

                if (i == 0 && p.Value._wantToGoBackward)
                    _player1.Translate(Vector3.right * MyResources.PLAYER_BACKWARD_SPEED * Time.deltaTime);

                if (i == 0 && p.Value._wantToTurnLeft)
                    _player1.Rotate(Vector3.up * -MyResources.PLAYER_ROTATION_SPEED * Time.deltaTime);

                if (i == 0 && p.Value._wantToTurnRight)
                    _player1.Rotate(Vector3.up * MyResources.PLAYER_ROTATION_SPEED * Time.deltaTime);

                if (i == 1 && p.Value._wantToGoForward)
                    _player2.Translate(Vector3.right * MyResources.PLAYER_SPEED * Time.deltaTime);

                if (i == 1 && p.Value._wantToGoForward)
                    _player2.Translate(Vector3.left * MyResources.PLAYER_BACKWARD_SPEED * Time.deltaTime);

                if (i == 1 && p.Value._wantToTurnLeft)
                    _player2.Rotate(Vector3.up * -MyResources.PLAYER_ROTATION_SPEED * Time.deltaTime);

                if (i == 1 && p.Value._wantToTurnRight)
                    _player2.Rotate(Vector3.up * MyResources.PLAYER_ROTATION_SPEED * Time.deltaTime);
                i++;
            }
    }

    [RPC]
    void PlayerWantToGoForward(NetworkPlayer p, bool b)
    {
        PlayersIntents[p]._wantToGoForward = b;
        if (Network.isServer)
        {
            _myNetworkView.RPC("PlayerWantToGoForward", RPCMode.Others, p, b);
        }
    }

    [RPC]
    void PlayerWantToTurnLeft(NetworkPlayer p, bool b)
    {
        PlayersIntents[p]._wantToTurnLeft = b;
        if (Network.isServer)
        {
            _myNetworkView.RPC("PlayerWantToTurnLeft", RPCMode.Others, p, b);
        }
    }

    [RPC]
    void PlayerWantToTurnRight(NetworkPlayer p, bool b)
    {
        PlayersIntents[p]._wantToTurnRight = b;
        if (Network.isServer)
        {
            _myNetworkView.RPC("PlayerWantToTurnRight", RPCMode.Others, p, b);
        }
    }

    [RPC]
    void PlayerWantToGoBackward(NetworkPlayer p, bool b)
    {
        PlayersIntents[p]._wantToGoBackward = b;
        if (Network.isServer)
        {
            _myNetworkView.RPC("PlayerWantToGoBackward", RPCMode.Others, p, b);
        }
    }

    [RPC]
    void PlayerGetCamera(NetworkPlayer p, int pl)
    {
        if (Network.isClient)
        {
            if (Network.player == p)
            {
                Transform tmp = null;
                if (pl == 1)
                    tmp = _player1;
                if (pl == 2)
                    tmp = _player2;
                tmp = tmp.FindChild("PlayerCamera");
                if (tmp)
                {
                    Camera cam = (Camera)tmp.camera;
                    cam.enabled = true;
                }
                GUIText txt = (GUIText) _player1.FindChild("PlayerInterface").FindChild("EndGameText").guiText;
                txt.enabled = false;
                txt = (GUIText)_player2.FindChild("PlayerInterface").FindChild("EndGameText").guiText;
                txt.enabled = false;
            }
        }
    }

    [RPC]
    void PlayerEndParty(NetworkPlayer p, int pl, int winner)
    {
        if (Network.isClient)
        {
            _isGameRunning = false;
            if (Network.player == p)
            {
                Transform tmpCam = null;
                if (pl == 1)
                {
                    tmpCam = _player1.FindChild("PlayerCamera");
                    if (winner == 1)
                        _endGameText.guiText.text = "Victoire !";
                    else
                        _endGameText.guiText.text = "Defaite !";
                }
                if (pl == 2)
                {
                    tmpCam = _player2.FindChild("PlayerCamera");
                    if (winner == 2)
                        _endGameText.guiText.text = "Victoire !";
                    else
                        _endGameText.guiText.text = "Defaite !";
                }

                _endGameText.guiText.enabled = true;

                if (tmpCam)
                {
                    Camera cam = (Camera)tmpCam.camera;
                    cam.enabled = false;
                }
            }
        }
        _player1 = null;
        _player2 = null;
    }

    [RPC]
    void PartyStart()
    {
        _isGameRunning = true;
    }
    #endregion
}
