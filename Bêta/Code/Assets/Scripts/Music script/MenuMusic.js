#pragma strict
    function Awake() {
    // see if we've got game music still playing
    var gameMusic : GameObject = GameObject.Find("GameMusic");
    if (gameMusic) {
    // kill game music
    Destroy(gameMusic);
    }
    // make sure we survive going to different scenes
    DontDestroyOnLoad(gameObject);
    }
function Start () {

}

function Update () {

}