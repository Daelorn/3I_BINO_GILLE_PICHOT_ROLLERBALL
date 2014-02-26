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
        public bool _wantToPause = false;
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
    private int _pauser;
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
        _pauser = 0;
        PlayersIntents = new Dictionary<NetworkPlayer, sPlayerIntents>();
        _myNetworkView = this.gameObject.GetComponent<NetworkView>();
        _player1 = _team1.FindChild("Player");
        _player2 = _team2.FindChild("Player");
        _endGameText.guiText.enabled = false;
        _isGameRunning = false;
        MyResources.GameEnd += new MyResources.GameEventDelegate(GameEnd);
        MyResources.GameStart += new MyResources.GameEventDelegate(GameStart);
        MyResources.PlayerWasTP += new MyResources.TPDelegate(MyResources_PlayerWasTP);
    }

    void MyResources_PlayerWasTP(Transform player, Transform tp)
    {
        if (Network.isServer)
        {
            NetworkPlayer p = new NetworkPlayer();

            bool isplayer1 = player == _player1;
            int cpt = 0;
            foreach (NetworkPlayer i in _playersIntents.Keys)
            {
                if (isplayer1 && cpt == 0)
                    p = i;
                if (!isplayer1 && cpt == 1)
                    p = i;
                cpt++;
            }

            if (player.position.y > MyResources.TOP_AREA_HEIGHT)
                _myNetworkView.RPC("OnTopPlatform", RPCMode.Others, p, isplayer1);
            else
                _myNetworkView.RPC("OnArena", RPCMode.Others, p, isplayer1);
        }
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
            int winner = value ? 1 : 2;

            _myNetworkView.RPC("PlayerEndParty", RPCMode.All, winner);
        }
    }

    void OnPlayerConnected(NetworkPlayer p)
    {
        PlayersIntents.Add(p, new sPlayerIntents());
        _myNetworkView.RPC("NewPlayerConnected", RPCMode.OthersBuffered, p);

        if (Network.connections.Length == 2)
        {
            MyResources.IsPausedGame = false;
            int player = 1;
            foreach (NetworkPlayer np in _playersIntents.Keys)
            {

                _myNetworkView.RPC("PlayerGetCamera", RPCMode.All, np, player);
                player++;
            }
            MyResources.GameStartEvent(true);
            _myNetworkView.RPC("RPCGameStart", RPCMode.Others, true);
        }
    }

    [RPC]
    void RPCGameStart(bool isGameStart)
    {
        if(Network.isClient)
            MyResources.GameStartEvent(isGameStart);
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
            if (Input.GetKeyUp(KeyCode.Space))
                _myNetworkView.RPC("PlayerWantToPause", RPCMode.Server, Network.player);

        }
    }

    void FixedUpdate()
    {
            int i = 1;
            foreach (var p in _playersIntents)
            {
                if (p.Value._wantToPause)
                {
                    if (i == _pauser || !MyResources.IsPausedGame)
                    {
                        _pauser = i;
                        _myNetworkView.RPC("PartyPause", RPCMode.All, i);
                    }
                    p.Value._wantToPause = false;
                }
                if (!MyResources.IsPausedGame)
                {
                    if (i == 1 && p.Value._wantToGoForward)
                        MyResources.PlayerWantToMoveEvent(_player1, Vector3.left);

                    if (i == 1 && p.Value._wantToGoBackward)
                        MyResources.PlayerWantToMoveEvent(_player1, Vector3.right);

                    if (i == 1 && p.Value._wantToTurnLeft)
                        _player1.Rotate(Vector3.up * -MyResources.PLAYER_ROTATION_SPEED * Time.deltaTime);

                    if (i == 1 && p.Value._wantToTurnRight)
                        _player1.Rotate(Vector3.up * MyResources.PLAYER_ROTATION_SPEED * Time.deltaTime);

                    if (i == 2 && p.Value._wantToGoForward)
                        MyResources.PlayerWantToMoveEvent(_player2, Vector3.right);

                    if (i == 2 && p.Value._wantToGoBackward)
                    {
                        MyResources.PlayerWantToMoveEvent(_player2, Vector3.left);
                    }

                    if (i == 2 && p.Value._wantToTurnLeft)
                        _player2.Rotate(Vector3.up * -MyResources.PLAYER_ROTATION_SPEED * Time.deltaTime);

                    if (i == 2 && p.Value._wantToTurnRight)
                        _player2.Rotate(Vector3.up * MyResources.PLAYER_ROTATION_SPEED * Time.deltaTime);

                }
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
    void PartyPause(int i)
    {
        MyResources.IsPausedGame = !MyResources.IsPausedGame;

        _endGameText.guiText.text = "Pause par joueur : " + i;
        _endGameText.guiText.enabled = MyResources.IsPausedGame;

        if (!MyResources.IsPausedGame && Network.isServer)
            _pauser = 0;
    }

    [RPC]
    void PlayerWantToPause(NetworkPlayer p)
    {
        PlayersIntents[p]._wantToPause = true;
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
    void PlayerEndParty(int winner)
    {
        if (Network.isClient)
        {
            Camera cam = (Camera)_player1.FindChild("PlayerCamera").camera;
            cam.enabled = false;

            cam = (Camera)_player2.FindChild("PlayerCamera").camera;
            cam.enabled = false;

            
            int cpt = 1 ;
            foreach (NetworkPlayer pi in this._playersIntents.Keys)
            {
                if (pi == Network.player)
                {
                    if (winner == cpt)
                        _endGameText.guiText.text = "Victoire !";
                    else
                        _endGameText.guiText.text = "Defaite !";
                }
                cpt++;
            }
            _endGameText.guiText.enabled = true;
        }
        _player1 = null;
        _player2 = null;
    }

    [RPC]
    void PartyStart()
    {
        _isGameRunning = true;
    }

    [RPC]
    void OnTopPlatform(NetworkPlayer p, bool isPlayerOne)
    {
        if (Network.isClient)
        {
            if (Network.player == p)
            {
                Camera tmpcam = isPlayerOne ? _player1.FindChild("PlayerCamera").camera : _player2.FindChild("PlayerCamera").camera;
                tmpcam.enabled = false;
            }
        }
    }

    [RPC]
    void OnArena(NetworkPlayer p, bool isPlayerOne)
    {
        if (Network.isClient)
        {
            if (Network.player == p)
            {
                Camera tmpcam = isPlayerOne ? _player1.FindChild("PlayerCamera").camera : _player2.FindChild("PlayerCamera").camera;
                tmpcam.enabled = true;
            }
        }
    }



    #endregion
}
