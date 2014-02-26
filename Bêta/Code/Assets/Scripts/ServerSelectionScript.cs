using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;

public class ServerSelectionScript : MonoBehaviour
{
    #region Fields
    [SerializeField]
    private Transform _listServers;
    [SerializeField]
    private Transform _serverLine;
    private float _acutalPos = 0;
    private List<ConnectionData> _connexionLines;
    #endregion

    #region Properties

    public Transform ListServers
    {
        get { return _listServers; }
        set { _listServers = value; }
    }

    public Transform ServerLine
    {
        get { return _serverLine; }
        set { _serverLine = value; }
    }
    
    #endregion

    #region Private Methods
    private void Start()
    {
        _connexionLines = new List<ConnectionData>();
        #region TODO A supprimer (pour Beta)
        _connexionLines.Add(new ConnectionData(6600, "127.0.0.1", 3));
        #endregion
        #region TODO CodeFinal
        //GetListOfConnexionData();
        CreateList();
        #endregion
    }

    private void launchGame()
    {
        MyResources.IsServer = false;
        Application.LoadLevel("PartyScene");
    }

    private void CreateList()
    {
        foreach (ConnectionData cd in _connexionLines)
        {
            Transform tmp = ServerLine.FindChild("Text_CurrentServerName");
            tmp.GetComponent<TextMesh>().text = cd.IP + "-" + cd.Port;

            tmp = ServerLine.FindChild("Text_MaxSlot");
            tmp.GetComponent<TextMesh>().text = "  /" + cd.MaxConnexion;

            tmp = ServerLine.FindChild("Text_CurrentSlot");
            tmp.GetComponent<TextMesh>().text = cd.ActualConnexions.ToString();

            Transform tmpT = (Transform)Instantiate(_serverLine);
            tmpT.parent = _listServers;
            tmpT.position = new Vector3(-13, ((float)(9.5 - _acutalPos)), -42);
            _acutalPos += 3/2;
        }
    }

    private void GetListOfConnexionData()
    {
        string tmpString = "";
        List<ConnectionData> tmp = new List<ConnectionData>();
        HttpWebRequest request = WebRequest.Create(MyResources.URL_SERVER_LIST) as HttpWebRequest;
        using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
        {

            StreamReader reader = new StreamReader(response.GetResponseStream());
            tmpString = reader.ReadToEnd();



            var stringReader = new System.IO.StringReader(tmpString);
            var serializer = new XmlSerializer(typeof(List<ConnectionData>));
            tmp = serializer.Deserialize(stringReader) as List<ConnectionData>;
        }

        foreach (ConnectionData cd in tmp)
            _connexionLines.Add(cd);
    }
    #endregion
}
