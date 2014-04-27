#pragma strict
    function Awake() {
    // see if we've got menu music still playing
    var menuMusic : GameObject = GameObject.Find("MenuMusic");
    if (menuMusic) {
    // kill menu music
    Destroy(menuMusic);
    }
    // make sure we survive going to different scenes
    DontDestroyOnLoad(gameObject);
    }
function Start () {

}

function Update () {

}