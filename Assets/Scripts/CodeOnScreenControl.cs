using UnityEngine;
using System.Collections;

public class CodeOnScreenControl : MonoBehaviour {

	public Texture[] codeTextures;
	private bool done;

	// Use this for initialization
	void Start () {
		//pick a random code texture for this screen
		//GetComponent<Renderer> ().material.mainTexture = codeTextures [Random.Range (0, codeTextures.Length)];

		done = true;//not running by default
		//StartCoroutine (UpdateScreen ());
	}
	
	// Update is called once per frame
	void Update () {
//		if(Input.GetKeyDown("space"))
//			GetComponent<Renderer> ().material.mainTexture = codeTextures [Random.Range (0, codeTextures.Length)];
	
	}

	public void StopUpdatingCode() {
		done = true;
	}

	public void StartUpdatingCode() {
		if(done)//if we're not currently running, run the code
			StartCoroutine (UpdateScreen ());
	}

	//wait between 1-3 seconds and change the code on the screen to another random code snippet
	IEnumerator UpdateScreen() {
		done = false;
		while (!done) {
			float rand = Random.Range (1.0f, 3.0f);
			GetComponent<Renderer> ().material.mainTexture = codeTextures [Random.Range (0, codeTextures.Length)];
			yield return new WaitForSeconds (rand);
		}
	}
}
