using UnityEngine;
using System.Collections;

public class GameRoomScript : MonoBehaviour {
	[SerializeField]
	bool isHeavy=false;
	[SerializeField]
	bool isLight=false;
	[SerializeField]
	bool isReady = false;
	
	void OnMouseUp(){
		//Button light
		if (isLight == true) {
			//Set light to picked
			this.renderer.material.color=Color.red;
		} else if (isHeavy == true) {
			//Set heavy to picked
			this.renderer.material.color=Color.red;
		} else if (isReady == true && isLight == true || isHeavy == true) {
			//Set to ready
			this.renderer.material.color=Color.blue;
		}

	}
	// Update is called once per frame
	void Update(){
		//quit game if escape key is pressed
		if (Input.GetKey(KeyCode.Escape)) { Application.LoadLevel("Menu");
		}
	}
	
	
	
}
