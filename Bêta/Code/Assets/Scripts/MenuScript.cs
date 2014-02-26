using UnityEngine;
using System.Collections;

public class MenuScript : MonoBehaviour
{
    #region Privayte Fields
    [SerializeField]
    TextMesh _myName;
    [SerializeField]
    ActionSceneType _actualAction;
    #endregion

    #region Properties
    public TextMesh MyName
    {
        get { return _myName; }
        set { _myName = value; }
    }
    public ActionSceneType ActualAction
    {
        get { return _actualAction; }
        set { _actualAction = value; }
    }
    #endregion

    void OnMouseEnter(){
		//change text color
		renderer.material.color=Color.red;
	}
	
	void OnMouseExit(){
		//change text color
		renderer.material.color=Color.white;
	}

	
	void OnMouseUp(){
        switch (_actualAction)
        {
            case ActionSceneType.Quit:
                {
                    Application.Quit();
                    break;
                }
            case ActionSceneType.Back:
                {
                    Application.LoadLevel("Menu");
                    break;
                }
            case ActionSceneType.LinkToServer:
                {
                    MyResources.IsServer = true;
                    Application.LoadLevel("PartyScene");
                    break;
                }
            case ActionSceneType.LinkToGame:
                {
                    string[] data = _myName.text.Split('-');
                    MyResources.ConnexionData.IP = data[0];
                    int port;
                    int.TryParse(data[1], out port);
                    MyResources.ConnexionData.Port = port;
                    MyResources.IsServer = false;
                    Application.LoadLevel("PartyScene");
                    break;
                }
            default:
                {
                    Application.LoadLevel("Lobby");
                    break;
                }
        }
	}
	// Update is called once per frame
	void Update(){
		//quit game if escape key is pressed
		if (Input.GetKey(KeyCode.Escape)) { Application.Quit();
		}
	}
	


}
