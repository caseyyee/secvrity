using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameControl : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject endGameMenu;
	public GameObject[] monitors;
	public GameObject scoreText;
    public GameObject cameraContainer;
    public GameObject cameraDot;
    public GameObject restartButton;
    public GameObject VRButton;
	public Material mainMenuMat;
	public Material gameOverMat;
	public Material restartMat;
	public Material mouseMat;
	public Material VRMat;
	public Material mainMenuOptionMat;
	public Material highScoreMat;
	public Material finalScoreMat;
	public Material instructionsMat;
	public Material regularMonitorMat;
	public GameObject highScoreText;
	private bool stillAlive;
	private float spawnGap;
	private float spawnGapUpdate;
	private bool mouseControl;
	private int currentScore;
    private bool ranFirstSpawnGap;
    private bool atAMenu;
	private bool showingMainMenu;
	private bool showingEndGameMenu;
	private bool gameIsOver;

	// Use this for initialization
	void Start () {
		stillAlive = false;
		spawnGap = 3.5f;
		spawnGapUpdate = 3.0f;
		mouseControl = false;
		currentScore = 0;
        ranFirstSpawnGap = false;
        atAMenu = true;
		showingMainMenu = true;
		showingEndGameMenu = false;
		gameIsOver = false;

        //set the high score variable
        if (!PlayerPrefs.HasKey("highScore"))
            PlayerPrefs.SetInt("highScore", 0);

		highScoreText.GetComponent<TextMesh>().text = PlayerPrefs.GetInt("highScore").ToString();

		ShowMainMenu ();//show the main menu

        //ActivateNewVirus ();

        //StartCoroutine(VirusSpawner());
        //StartCoroutine (SpawnGapController ());
    }

	// Update is called once per frame
	void Update () {

        //		if (atAMenu && Input.GetMouseButtonDown (0)) {//if we're at a menu and we click, select the currently selected menu item
        //			UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.
        //		}

//        if (UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject == null)
//        {
//            Debug.Log("Reselecting first input");
//            UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(UnityEngine.EventSystems.EventSystem.current.firstSelectedGameObject);
//        }

    }

	public void ShowMainMenu() {//populate the monitors with menu screens
		for (int i = 0; i < monitors.Length; i++) 
		{
			if (monitors [i].name == "monitor05") {//center screen
				monitors [i].transform.Find ("screen05").gameObject.GetComponent<Renderer> ().material = mainMenuMat;
				monitors [i].transform.Find ("screen05").gameObject.GetComponent<CodeOnScreenControl> ().StopUpdatingCode ();
			} else if (monitors [i].name == "monitor03") {//VR screen
				monitors [i].transform.Find ("screen03").gameObject.GetComponent<Renderer> ().material = VRMat;
				monitors [i].transform.Find ("screen03").gameObject.GetComponent<CodeOnScreenControl> ().StopUpdatingCode ();
			} else if (monitors [i].name == "monitor07") {//mouse screen
				monitors [i].transform.Find ("screen07").gameObject.GetComponent<Renderer> ().material = mouseMat;
				monitors [i].transform.Find ("screen07").gameObject.GetComponent<CodeOnScreenControl> ().StopUpdatingCode ();
			} else if (monitors [i].name == "monitor04") {//high score screen
				monitors [i].transform.Find ("screen04").gameObject.GetComponent<Renderer> ().material = highScoreMat;
				monitors [i].transform.Find ("screen04").gameObject.GetComponent<CodeOnScreenControl> ().StopUpdatingCode ();
			} else if (monitors [i].name == "monitor06") {//instructions screen
				monitors [i].transform.Find ("screen06").gameObject.GetComponent<Renderer> ().material = instructionsMat;
				monitors [i].transform.Find ("screen06").gameObject.GetComponent<CodeOnScreenControl> ().StopUpdatingCode ();
			} else {//non-menu monitors
				string monitorNum = monitors[i].name.Substring(monitors[i].name.Length - 2);//grab the number
				monitors[i].transform.Find("screen"+monitorNum).gameObject.GetComponent<Renderer>().material = regularMonitorMat;
				monitors[i].transform.Find("screen"+monitorNum).gameObject.GetComponent<CodeOnScreenControl>().StartUpdatingCode();
			}
		}

		//show high score text
		highScoreText.SetActive(true);
	}

	public void ShowEndGameMenu() {
		//show proper screens on monitors
		for (int i = 0; i < monitors.Length; i++) 
		{
			if (monitors [i].name == "monitor05") {//center screen
				monitors [i].transform.Find ("screen05").gameObject.GetComponent<Renderer> ().material = gameOverMat;
				monitors [i].transform.Find ("screen05").gameObject.GetComponent<CodeOnScreenControl> ().StopUpdatingCode ();
			} else if (monitors [i].name == "monitor03") {//VR screen
				monitors [i].transform.Find ("screen03").gameObject.GetComponent<Renderer> ().material = restartMat;
				monitors [i].transform.Find ("screen03").gameObject.GetComponent<CodeOnScreenControl> ().StopUpdatingCode ();
			} else if (monitors [i].name == "monitor07") {//mouse screen
				monitors [i].transform.Find ("screen07").gameObject.GetComponent<Renderer> ().material = mainMenuOptionMat;
				monitors [i].transform.Find ("screen07").gameObject.GetComponent<CodeOnScreenControl> ().StopUpdatingCode ();
			} else if (monitors [i].name == "monitor04") {//high score screen
				monitors [i].transform.Find ("screen04").gameObject.GetComponent<Renderer> ().material = highScoreMat;
				monitors [i].transform.Find ("screen04").gameObject.GetComponent<CodeOnScreenControl> ().StopUpdatingCode ();
			} else if(monitors[i].name == "monitor06") {//final score screen
				monitors [i].transform.Find ("screen06").gameObject.GetComponent<Renderer> ().material = finalScoreMat;
				monitors [i].transform.Find ("screen06").gameObject.GetComponent<CodeOnScreenControl> ().StopUpdatingCode ();
			} else {//non-menu monitors
				string monitorNum = monitors[i].name.Substring(monitors[i].name.Length - 2);//grab the number
				monitors[i].transform.Find("screen"+monitorNum).gameObject.GetComponent<Renderer>().material = regularMonitorMat;
				monitors[i].transform.Find("screen"+monitorNum).gameObject.GetComponent<CodeOnScreenControl>().StartUpdatingCode();
			}
		}

		//show high score text
		highScoreText.SetActive(true);
		scoreText.SetActive (true);
	}

	public void HideMenus() {
		gameIsOver = false;
		for (int i = 0; i < monitors.Length; i++) {
			string monitorNum = monitors[i].name.Substring(monitors[i].name.Length - 2);//grab the number
			monitors[i].transform.Find("screen"+monitorNum).gameObject.GetComponent<Renderer>().material = regularMonitorMat;
			monitors[i].transform.Find("screen"+monitorNum).gameObject.GetComponent<CodeOnScreenControl>().StartUpdatingCode();
		}

		//hide high score text
		highScoreText.SetActive(false);
		scoreText.SetActive (false);
	}

	public void SelectedMouseControl() {
		//hide the menu
		//mainMenu.SetActive (false);
        atAMenu = false;
        stillAlive = true;

		//hide the menu options
		HideMenus();

		//let this script know we're using mouse control
		mouseControl = true;

		//tell playerControl what control scheme we chose
		gameObject.GetComponent<PlayerControl>().SelectedMouseControl();

		StartCoroutine(VirusSpawner());
		StartCoroutine (SpawnGapController ());
	}

	public void SelectedVRControl() {
		//hide the menu
		//mainMenu.SetActive (false);
        atAMenu = false;
        stillAlive = true;

		//hide the menu options
		HideMenus();

		mouseControl = false;

		//tell playerControl what control scheme we chose
		gameObject.GetComponent<PlayerControl>().SelectedVRControl();

        //report to our code to switch to VR mode
        //cameraContainer.GetComponent<StereoCamera>().CallVRCode(true);

        //start the game
        StartCoroutine(VirusSpawner());
		StartCoroutine (SpawnGapController ());
	}

	public void SelectedRestart() {
		//hide the endgame menu
		//endGameMenu.SetActive (false);
        atAMenu = false;

		//reset the score counter for this new round
		gameObject.GetComponent<GameControl> ().SetCurrentScore (false);

		//hide the menu options
		HideMenus();

		//reset variables
		stillAlive = true;
		spawnGap = 3.0f;
		spawnGapUpdate = 3.0f;
        ranFirstSpawnGap = false;

		//reset all monitors
		for (int i = 0; i < monitors.Length; i++) {
			monitors [i].GetComponent<VirusControl> ().SetVirus ("stop");
		}

		if(mouseControl)//tell playerControl what control scheme we chose
			gameObject.GetComponent<PlayerControl>().SelectedMouseControl();
		else//tell playerControl what control scheme we chose
			gameObject.GetComponent<PlayerControl>().SelectedVRControl();
			

		//start the game
		StartCoroutine(VirusSpawner());
		StartCoroutine (SpawnGapController ());
	}

	public void SelectedMainMenu() {
		//hide the endgame menu
		//endGameMenu.SetActive (false);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(VRButton);
        UnityEngine.EventSystems.EventSystem.current.firstSelectedGameObject = VRButton;//set first selected in case we lose focus
        atAMenu = true;

		//reset variables
		//stillAlive = true;
		spawnGap = 3.0f;
		spawnGapUpdate = 3.0f;
        ranFirstSpawnGap = false;

		//show the main menu options
		HideMenus();
		ShowMainMenu();

		//reset all monitors
		for (int i = 0; i < monitors.Length; i++) {
			monitors [i].GetComponent<VirusControl> ().SetVirus ("stop");
		}

		//let our playercontrol script know we're back at the main menu
		gameObject.GetComponent<PlayerControl>().SelectedMainMenu();

        //report to our code to switch to normal mode and leave VR
        //if(!mouseControl)
            //cameraContainer.GetComponent<StereoCamera>().CallVRCode(false);

        //show the main menu
        //mainMenu.SetActive(true);
	}

	public void SetCurrentScore(bool increasing) {
		if (increasing)
			currentScore++;
		else//if we're not increasing, we're resetting
			currentScore = 0;
	}

	public int GetCurrentScore() {
		return currentScore;
	}

	//this will constantly look to spawn a new virus every so often, and the gap between spawns slowly decreases
	IEnumerator VirusSpawner() {
		while (stillAlive) {
			//print ("looping, spawnGap = " + spawnGap);
			yield return new WaitForSeconds (spawnGap);
            if (!ranFirstSpawnGap)
            {
                yield return new WaitForSeconds(spawnGap);//on the first time in every level, give a little extra time for people to look around
                ranFirstSpawnGap = true;
            }
			StartCoroutine(ActivateNewVirus ());
		}
	}

	IEnumerator SpawnGapController() {
		while (stillAlive) {
			yield return new WaitForSeconds (spawnGapUpdate);//every X seconds
			if (spawnGap > 0.46f) {//the smallest gap between spawns is 0.4 seconds
				spawnGap -= 0.1f;
				//spawnGapUpdate += 0.05f;
			}
		}
	}

	public void SetStillAlive(bool value, GameObject monitor) {
		stillAlive = value;
		if (!stillAlive && !gameIsOver)
			GameOverCaller (monitor);
			//StartCoroutine (GameOver (monitor));
	}

	public bool GetStillAlive() {
		return stillAlive;
	}

	IEnumerator ActivateNewVirus() {
		bool done = false;
		int rand = 0;
		while (!done) {
			rand = Random.Range (0, monitors.Length);
			if (!monitors [rand].GetComponent<VirusControl> ().GetVirus ())
				done = true;
			yield return null;
		}
		monitors [rand].GetComponent<VirusControl> ().SetVirus ("start");//when we find an open monitor, spawn a virus
		yield return null;
	}

	void GameOverCaller(GameObject monitor) {
		gameIsOver = true;

		//stop the spawners and timers
		StopAllCoroutines();

		StartCoroutine (GameOver (monitor));
	}

	IEnumerator GameOver(GameObject monitor) {

		//update the end game score number
		scoreText.GetComponent<TextMesh>().text = currentScore.ToString();

        //check to see if this is higher than the high score
        if (PlayerPrefs.GetInt("highScore") < currentScore)
        {
            PlayerPrefs.SetInt("highScore", currentScore);
            highScoreText.GetComponent<TextMesh>().text = PlayerPrefs.GetInt("highScore").ToString();
        }

		//show viruses on every screen
		for (int i = 0; i < monitors.Length; i++) {
			monitors [i].GetComponent<VirusControl> ().SetVirus ("success");
		}
		//wait for any still-running viruses to finish
		yield return new WaitForSeconds (3.5f);

		//hide all viruses
		for (int j = 0; j < monitors.Length; j++) {
			monitors [j].GetComponent<VirusControl> ().SetVirus ("stop");
		}

            //show the end game menu
            //endGameMenu.SetActive (true);
        //UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(restartButton);
        //UnityEngine.EventSystems.EventSystem.current.firstSelectedGameObject = restartButton;//set the first selected in case our menu loses focus
        atAMenu = true;

		//show the game over menu
		ShowEndGameMenu();

		//stop letting control happen in-game (switch to menu control)
		gameObject.GetComponent<PlayerControl> ().GameOver ();

		//print ("pos = " + monitor.transform.position);
		//if we're using the mouse, rotate back to face center
		//if (mouseControl)
			//iTween.LookTo (gameObject, monitor.transform.position, 0.5f);

		yield return null;
	}
}
