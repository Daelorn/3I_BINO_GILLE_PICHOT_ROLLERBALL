using UnityEngine;
using System.Collections;

public class OptionScript : MonoBehaviour {
	public string[] items;
	public string slectedItem = "None";	
	private bool editing = false;
	private bool toggleFullScreen = true;
    public string buttonCommand = "";
    [SerializeField]
    private Transform TouchUp;
    [SerializeField]
    private Transform TouchLeft;
    [SerializeField]
    private Transform TouchRight;
    [SerializeField]
    private Transform TouchAction;
    private bool newOne = false;
	/*KeyCode inputForward = KeyCode.UpArrow;
	KeyCode inputAction = KeyCode.LeftShift;
	KeyCode inputLeft = KeyCode.LeftArrow;
	KeyCode inputRight = KeyCode.RightArrow;*/
	KeyCode current = KeyCode.None;

	KeyCode FetchKey()
	{
		int e = System.Enum.GetNames(typeof(KeyCode)).Length;
		for(int i = 0; i < e; i++)
		{
			if(Input.GetKey((KeyCode)i))
			{
                newOne = true;
				return (KeyCode)i;
			}
		}
		
		return KeyCode.None;
	}
    void Start()
    {
        TextMesh tm =  (TextMesh)TouchAction.GetComponent("TextMesh");
        tm.text = MyResources.goAction.ToString();
        tm = (TextMesh)TouchUp.GetComponent("TextMesh");
        tm.text = MyResources.goForward.ToString();
        tm = (TextMesh)TouchLeft.GetComponent("TextMesh");
        tm.text = MyResources.goLeft.ToString();
        tm = (TextMesh)TouchRight.GetComponent("TextMesh");
        tm.text = MyResources.goRight.ToString();

    }

	private void OnGUI()
	{
		#region resolution controller
		toggleFullScreen = GUI.Toggle(new Rect(Screen.width/3+160, Screen.width/7, 200,45), toggleFullScreen, "Toggle full screen ?");
		if (GUI.Button(new Rect(Screen.width/3-20, Screen.width/8+25, 150, 45), slectedItem))
		{
			editing = true;
		}
		
		if (editing)
		{
			for (int x = 0; x < items.Length; x++)
			{
				if (GUI.Button(new Rect(Screen.width/3-20, (45 * x) + Screen.width/8+25 + 45, 150, 45), items[x]))
				{
					slectedItem = items[x];
					editing = false;
				}
			}
		}
		#endregion

        #region Kamoulox
        //if(GUI.Button(new Rect(Screen.width/3-20, Screen.width/4-15, 190, 45),"Change move forward action"))
        //{
        //    if(Input.anyKeyDown){
        //        KeyCode inputForward = current;
        //        MyResources.goForward = inputForward;
        //    }
        //}
        //if(GUI.Button(new Rect(Screen.width/3-20, Screen.width/3-75, 190, 45),"Change action input"))
        //{
        //    KeyCode inputaction = current;
        //    MyResources.goAction = inputAction;
        //}
        //if(GUI.Button(new Rect(Screen.width/3-20, Screen.width/3+35, 190, 45),"Change turn left action"))
        //{
        //    KeyCode inputleft = current;
        //    MyResources.goLeft = inputLeft;
        //}
        //if(GUI.Button(new Rect(Screen.width/3-20, Screen.width/3+135, 190, 45),"Change turn right action"))
        //{
        //    KeyCode inputright = current;
        //    MyResources.goRight = inputRight;
        //}
		#endregion

	}
	void OnMouseUp(){
		#region Change resolution
		if (slectedItem == "800x600"){
			Screen.SetResolution(800,600,true,0);
		}
		else if(slectedItem == "1024x768"){
			Screen.SetResolution(1024,768,true);
		}
		else if(slectedItem == "1280x800"){
			Screen.SetResolution(1280,800,true);
		}
		else if(slectedItem == "1280x1024"){
			Screen.SetResolution(1280,1024,true);
		}
		else if(slectedItem == "1366x768"){
			Screen.SetResolution(1366,768,true);
		}
		else if(slectedItem == "1920x1080"){
			Screen.SetResolution(1920,1080,true);
		}
		if (toggleFullScreen == true){
			Screen.fullScreen = Screen.fullScreen;
		}
		else{
			Screen.fullScreen = !Screen.fullScreen;
		}
		#endregion

	}
	
	// Update is called once per frame
	void Update () {
	}

    public void ChangeTouch(Transform touch)
    {
        switch (touch.name)
        {
            case "Text_Action_Touch":
                {
                    MyResources.goAction = current;
                    TextMesh tm =  (TextMesh)TouchAction.GetComponent("TextMesh");
                    tm.text = current.ToString();
                    break;
                }
            case "Text_Forward_Touch": { MyResources.goAction = current; break; }
            case "Text_Left_Touch": { MyResources.goAction = current; break; }
            case "Text_Right_Touch": { MyResources.goAction = current; break; }
        }
    }
}
