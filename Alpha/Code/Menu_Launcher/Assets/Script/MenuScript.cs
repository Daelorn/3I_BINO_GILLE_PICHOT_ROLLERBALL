using UnityEngine;
using System.Collections;
using System;
using System.Diagnostics;

public class MenuScript : MonoBehaviour
{
    #region Fields
    [SerializeField]
    bool _isQuit = false;
    [SerializeField]
    bool _isBack = false;
    [SerializeField]
    bool _isServer = false;
    #endregion

    #region Private Methods
    void OnMouseEnter(){
		//change text color
		renderer.material.color=Color.red;
	}
	
	void OnMouseExit(){
		//change text color
		renderer.material.color=Color.white;
	}
	
	void OnMouseUp(){
		//is this quit
		if (_isQuit == true) {
			//quit the game
			Application.Quit ();
		} else if (_isBack == true) {
			//Back to menu
			Application.LoadLevel("Menu");
        }
        else if (_isServer)
        {
            String path = @"\Data\Server.exe";
            Process foo = new Process();
            foo.StartInfo.FileName = @"Data\Server.exe";
            foo.StartInfo.Arguments = path;
            foo.Start();
        }
        else
        {
            String path = @"\Data\Player.exe";
            Process foo = new Process();
            foo.StartInfo.FileName = @"Data\Player.exe";
            foo.StartInfo.Arguments = path;
            foo.Start();
            foo.Start();
        }
	}
	// Update is called once per frame
	void Update(){
		//quit game if escape key is pressed
		if (Input.GetKey(KeyCode.Escape)) { Application.Quit();
		}
    }
    #endregion


}
