// FadeInOut (downloaded from the webz)
//
//--------------------------------------------------------------------
//                        Public parameters
//--------------------------------------------------------------------

public var fadeOutTexture : Texture2D;
public var fadeSpeed = 0.3;

var drawDepth = -1000;

//--------------------------------------------------------------------
//                       Private variables
//--------------------------------------------------------------------

private var alpha = 1.0; 

private var fadeDir = -1;

//--------------------------------------------------------------------
//                       Runtime functions
//--------------------------------------------------------------------

//--------------------------------------------------------------------

function OnGUI(){
    
    alpha += fadeDir * fadeSpeed * Time.deltaTime;  
    alpha = Mathf.Clamp01(alpha);   
    
    GUI.color.a = alpha;
    
    GUI.depth = drawDepth;
    
    GUI.DrawTexture(Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
}

function blackOut() {
	fadeDir = 1;
	fadeSpeed = 1000;
}

function clearOut() {
	fadeDir = -1;
	fadeSpeed = 1000;
}

//--------------------------------------------------------------------

function fadeIn(){
    fadeDir = -1;   
    fadeSpeed = 0.3;
}

//--------------------------------------------------------------------

function fadeOut(){
    fadeDir = 1;    
    fadeSpeed = 0.3;
}

function fadeInFast(){
    fadeDir = -1;   
    fadeSpeed = 2;
}

//--------------------------------------------------------------------

function fadeOutFast(){
    fadeDir = 1;    
    fadeSpeed = 2;
}

function Start(){
    alpha=1;
    //fadeInFast();
}