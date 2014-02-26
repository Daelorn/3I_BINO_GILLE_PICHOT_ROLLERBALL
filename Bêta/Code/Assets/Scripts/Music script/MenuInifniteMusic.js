#pragma strict
    static var AudioBegin : boolean = false;
     
    function Awake()
    {
    if (!AudioBegin)
    {
    audio.Play();
    DontDestroyOnLoad(gameObject);
    AudioBegin = true;
    }
    }
function Start () {

}

function Update () {

}