using UnityEngine;
using System.Collections;

public class VirusControl : MonoBehaviour {

	private bool hasVirus = false;
    private GameObject virusProgressBar;
    private GameObject virus;

	public Material regularVirus;
	public Material successfulVirus;

	public Camera mainCamera;
	public AudioClip failureBuzz;
	public AudioClip virusBuzz;

	public float virusTimer;

	// Use this for initialization
	void Start () {
        string monitorNum = gameObject.name.Substring(gameObject.name.Length - 2);//grab the number
        print("monitorNum for "+gameObject.name+" = " + monitorNum);

        virus = transform.Find("screen"+monitorNum+"/virus" + monitorNum).gameObject;
        virusProgressBar = virus.transform.Find("progresswell"+monitorNum+"/progressbar" + monitorNum).gameObject;

        //disable the virus and progress bar
        virus.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetVirus(string value) {
		if (value == "start") {
			hasVirus = true;
			StartCoroutine (RunVirus ());
		} 
		else if (value == "stop") {//we don't have the virus anymore, so hide the window and reset the progress bar
			hasVirus = false;
			virus.SetActive (false);
			virus.GetComponent<Renderer> ().material = regularVirus;
			virusProgressBar.transform.localScale = new Vector3 (0, 1, 1);
			//transform.FindChild("Virus").GetComponent<Renderer>().material = noVirus;//probably want to turn off the gameobject itself so we also hide the progress bar, etc.
		} 
		else if (value == "success") {//show the success virus on this screen because we lost
			virus.GetComponent<Renderer> ().material = successfulVirus;
			virus.SetActive(true);
		}
	}

	public bool GetVirus() {
		return hasVirus;
	}

	IEnumerator RunVirus() {
        //show the virus on screen
        virus.SetActive(true);

		//play the sound
		GetComponent<AudioSource>().clip = virusBuzz;
		GetComponent<AudioSource>().Play();

		//transform.FindChild ("Virus").GetComponent<Renderer> ().material = yesVirus;//probably want to just enable the gameobject itself so the progress bar is also enabled

		float t = 0;
		while(t < virusTimer && hasVirus)
		{
			t += Time.deltaTime;
            //expand the virus bar
            Vector3 progressBarScale = virusProgressBar.transform.localScale;
            progressBarScale.x = t / virusTimer;//range from 0 to 1, not 0 to virusTimer which is what t goes to
            virusProgressBar.transform.localScale = progressBarScale;
			yield return null;
		}
        //yield return new WaitForSeconds (virusTimer);

        if (hasVirus) {//if we still have the virus after waiting it out
            virusProgressBar.transform.localScale = new Vector3(1, 1, 1);//make sure it's exactly 1
            virus.GetComponent<Renderer> ().material = successfulVirus;
            
			//play the failure sound
			GetComponent<AudioSource>().clip = failureBuzz;
			GetComponent<AudioSource>().Play();

			//game over
			mainCamera.GetComponent<GameControl>().SetStillAlive(false, gameObject);
		}

		yield return null;
	}
}
