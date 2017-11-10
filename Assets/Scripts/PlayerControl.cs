using UnityEngine;
using System.Collections;

/// MouseLook rotates the transform based on the mouse delta.
/// Minimum and Maximum values can be used to constrain the possible rotation

/// To make an FPS style character:
/// - Create a capsule.
/// - Add a rigid body to the capsule
/// - Add the MouseLook script to the capsule.
///   -> Set the mouse look to use LookX. (You want to only turn character but not tilt it)
/// - Add FPSWalker script to the capsule

/// - Create a camera. Make the camera a child of the capsule. Reset it's transform.
/// - Add a MouseLook script to the camera.
///   -> Set the mouse look to use LookY. (You want the camera to tilt up and down like a head. The character already turns.)
[AddComponentMenu("Camera-Control/Mouse Look")]
public class PlayerControl : MonoBehaviour {

	public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
	public RotationAxes axes = RotationAxes.MouseXAndY;
	public float sensitivityX;
	public float sensitivityY;

	public float minimumX = -360F;
	public float maximumX = 360F;

	public float minimumY = -90F;
	public float maximumY = 90F;

	public Material regularMonitor;
	public Material highlightedMonitor;

	public Camera mainCamera;

	public GameObject[] monitors;
	public GameObject guideDot;

	public GameObject leftHandObj;
	public GameObject rightHandObj;

	float rotationX = 0F;
	float rotationY = 0F;

	bool controllingGame;
	private GameObject currentMonitor;

	private bool mouseControl = false;
	private bool atMainMenu;
	private bool atGameOverMenu;
	private string highlightedOption;

	Quaternion originalRotation;

	WebVRCamera webVRCamera;
	private bool handControls = false;

	void Start ()
	{
		// Make the rigid body not change rotation
		if (GetComponent<Rigidbody>())
			GetComponent<Rigidbody>().freezeRotation = true;
		originalRotation = transform.localRotation;

		// todo: disable cursor hide and lock until we bring back mouse gaze support.
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
        mouseControl = false;
		atMainMenu = true;
		atGameOverMenu = false;
		highlightedOption = "";

        //		Cursor.lockState = CursorLockMode.Locked;
        //		Cursor.visible = false;

        currentMonitor = null;

        //controllingGame = false;
        controllingGame = true;

		if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) {
			sensitivityX = 5;
			sensitivityY = 5;
		}

		webVRCamera = GameObject.Find ("WebVRCameraSet").GetComponent<WebVRCamera>();
	}

    public void UsingVR()//we call this if StereoCamera is getting values
    {
        mouseControl = false;
    }

	public void SelectedMainMenu() {
		atMainMenu = true;
		atGameOverMenu = false;
	}

	public void SelectedMouseControl() {
		//hide the mouse
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
		//controllingGame = true;

		//control the camera movement in game with the mouse
		mouseControl = true;
		atMainMenu = false;
		atGameOverMenu = false;
	}

	public void SelectedVRControl() {
		//hide the mouse
//		Cursor.lockState = CursorLockMode.Locked;
//		Cursor.visible = false;

		//control the camera movement in game with VR
		mouseControl = false;
		atMainMenu = false;
		atGameOverMenu = false;
		//controllingGame = true;
	}

	public void GameOver() {
		//controllingGame = false;

		//if we're locked in using mouse control, unlock the mouse for menu selection
		//Cursor.lockState = CursorLockMode.None;
		//Cursor.visible = true;

		for (int i = 0; i < monitors.Length; i++) {//unhighlight all monitors
			monitors[i].GetComponent<Renderer>().material = regularMonitor;
		}

		atGameOverMenu = true;
	}

	void Update ()
	{
		// handle 6dof controls
		if (webVRCamera.handControllers) {
			bool hitMonitor = false;
			for (int i = 0; i < monitors.Length; i++) {
				bool leftHit = leftHandObj.GetComponent<Renderer> ().bounds.Intersects (monitors [i].GetComponent<Renderer> ().bounds);
				bool rightHit = rightHandObj.GetComponent<Renderer> ().bounds.Intersects (monitors [i].GetComponent<Renderer> ().bounds);

				if (leftHit || rightHit) {
					currentMonitor = monitors [i];
					hitMonitor = true;
					if (currentMonitor.GetComponent<Renderer> ().material != highlightedMonitor)
						currentMonitor.GetComponent<Renderer> ().material = highlightedMonitor;
				} else {
					if (monitors [i].GetComponent<Renderer> ().material != regularMonitor)
						monitors [i].GetComponent<Renderer> ().material = regularMonitor;// turn off all monitors
				}
			}

			if (!hitMonitor) {
				currentMonitor = null;
			}
		}

		// main menu
		if (atMainMenu && currentMonitor != null && currentMonitor.name == "monitor03" && webVRCamera.buttonPressed) {
			highlightedOption = "VR";
			gameObject.GetComponent<GameControl> ().SelectedVRControl ();
		}

		// game over menu
		if (atGameOverMenu && currentMonitor != null && currentMonitor.name == "monitor03" && webVRCamera.buttonPressed) {
			highlightedOption = "restart";
			gameObject.GetComponent<GameControl> ().SelectedRestart ();
		}
	
		if (atGameOverMenu && currentMonitor != null && currentMonitor.name == "monitor07" && webVRCamera.buttonPressed) {
			highlightedOption = "mainmenu";
			gameObject.GetComponent<GameControl> ().SelectedMainMenu ();
		}


		//handle menu keyboard input
		if (atMainMenu) {
			if (Input.GetKeyDown ("left") || Input.GetAxis("Horizontal") < -0.2f) {//highlight the left monitor
				highlightedOption = "VR";
				for (int i = 0; i < monitors.Length; i++) {
					if (monitors [i].name == "monitor03")
						monitors [i].GetComponent<Renderer> ().material = highlightedMonitor;
					else
						monitors [i].GetComponent<Renderer> ().material = regularMonitor;
				}
			} else if (Input.GetKeyDown ("right") || Input.GetAxis("Horizontal") > 0.2f) {//highlight the right monitor
				highlightedOption = "mouse";
				for (int i = 0; i < monitors.Length; i++) {
					if (monitors [i].name == "monitor07")
						monitors [i].GetComponent<Renderer> ().material = highlightedMonitor;
					else
						monitors [i].GetComponent<Renderer> ().material = regularMonitor;
				}
			} else if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown ("space") || Input.GetButtonDown("Submit")) {
				if (highlightedOption == "VR") {
					gameObject.GetComponent<GameControl> ().SelectedVRControl ();
				} else if (highlightedOption == "mouse") {
					gameObject.GetComponent<GameControl> ().SelectedMouseControl ();
				}
				highlightedOption = "";
				for(int i = 0; i < monitors.Length; i++)//reset the material for all the monitors
					monitors [i].GetComponent<Renderer> ().material = regularMonitor;
			}
		}
		else if (atGameOverMenu) {
			if (Input.GetKeyDown ("left") || Input.GetAxis("Horizontal") < -0.2f) {//highlight the left monitor
				highlightedOption = "restart";
				for (int i = 0; i < monitors.Length; i++) {
					if (monitors [i].name == "monitor03")
						monitors [i].GetComponent<Renderer> ().material = highlightedMonitor;
					else
						monitors [i].GetComponent<Renderer> ().material = regularMonitor;
				}
			} else if (Input.GetKeyDown ("right") || Input.GetAxis("Horizontal") > 0.2f) {//highlight the right monitor
				highlightedOption = "mainmenu";
				for (int i = 0; i < monitors.Length; i++) {
					if (monitors [i].name == "monitor07")
						monitors [i].GetComponent<Renderer> ().material = highlightedMonitor;
					else
						monitors [i].GetComponent<Renderer> ().material = regularMonitor;
				}
			} else if (Input.GetKeyDown (KeyCode.Return) || Input.GetKeyDown ("space") || Input.GetButtonDown("Submit")) {//select what the user has chosen
				if (highlightedOption == "restart") {
					gameObject.GetComponent<GameControl> ().SelectedRestart ();
				} else if (highlightedOption == "mainmenu") {
					gameObject.GetComponent<GameControl> ().SelectedMainMenu ();
				}
				highlightedOption = "";
				for(int i = 0; i < monitors.Length; i++)//reset the material for all the monitors
					monitors [i].GetComponent<Renderer> ().material = regularMonitor;
			}
		}

		//control whether we're playing or trying to get our mouse back
//		if (Input.GetKeyDown ("escape")) 
//		{
//			if (controllingGame && mouseControl) {
//				Cursor.visible = true;
//				Cursor.lockState = CursorLockMode.None;
//				controllingGame = false;
//			} 
//			else if(!controllingGame && mouseControl) {
//				Cursor.lockState = CursorLockMode.Locked;
//				Cursor.visible = false;
//				controllingGame = true;
//			}
//		}

		//raycast from center of camera to figure out which monitor we're looking at
		if (controllingGame && mainCamera.GetComponent<GameControl>().GetStillAlive() && !webVRCamera.handControllers) 
		{
			RaycastHit hit;
			Ray ray = mainCamera.ViewportPointToRay (new Vector3 (0.5f, 0.5f, 0f));

			if (Physics.Raycast (ray, out hit)) {
				GameObject objectHit = hit.transform.gameObject;
				//print ("objectHit = " + objectHit);
				bool hitMonitor = false;
				for (int i = 0; i < monitors.Length; i++) {
					if (objectHit == monitors [i]) {
						hitMonitor = true;
						currentMonitor = objectHit;
						if (objectHit.GetComponent<Renderer> ().material != highlightedMonitor)
							objectHit.GetComponent<Renderer> ().material = highlightedMonitor;
					} else {
						if (monitors [i].GetComponent<Renderer> ().material != regularMonitor)
							monitors [i].GetComponent<Renderer> ().material = regularMonitor;
					}
				}
				if (!hitMonitor) {//if we didn't hit a monitor, turn them all off
					currentMonitor = null;
					for (int i = 0; i < monitors.Length; i++) {
						if (monitors [i].GetComponent<Renderer> ().material != regularMonitor)
							monitors [i].GetComponent<Renderer> ().material = regularMonitor;
					}
				}
			} else {//if we didn't hit anything, turn them all off
				currentMonitor = null;
				for (int i = 0; i < monitors.Length; i++) {
					if (monitors [i].GetComponent<Renderer> ().material != regularMonitor)
						monitors [i].GetComponent<Renderer> ().material = regularMonitor;
				}
			}
		}

		//mouse input control
		if (controllingGame) 
		{
			
			//mouse click
			//if we're hovering over a monitor and click, check to see if that monitor has a virus we should disable
			if ((webVRCamera.buttonPressed || Input.GetMouseButtonDown (0) || Input.GetButtonDown("Submit")) && currentMonitor != null && mainCamera.GetComponent<GameControl>().GetStillAlive()) {
				//print ("clicking " + currentMonitor + ", getVirus = " + currentMonitor.GetComponent<VirusControl> ().GetVirus ());
				//if the monitor has a virus, disable it
				if (currentMonitor.GetComponent<VirusControl> ().GetVirus ()) {
					
					//play the click sound
					guideDot.GetComponent<AudioSource>().Play();

					//increase our score
					gameObject.GetComponent<GameControl>().SetCurrentScore(true);

					currentMonitor.GetComponent<VirusControl> ().SetVirus ("stop");
					//mainCamera.GetComponent<GameControl> ().ActivateNewVirus ();//temporary
				}
			}
			//mouse movement
			if (mouseControl) {
				if (axes == RotationAxes.MouseXAndY) {
					// Read the mouse input axis
					rotationX += Input.GetAxis ("Mouse X") * sensitivityX;
					rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;

					rotationX = ClampAngle (rotationX, minimumX, maximumX);
					rotationY = ClampAngle (rotationY, minimumY, maximumY);

					Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
					Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, Vector3.left);

					transform.localRotation = originalRotation * xQuaternion * yQuaternion;
				} else if (axes == RotationAxes.MouseX) {
					rotationX += Input.GetAxis ("Mouse X") * sensitivityX;
					rotationX = ClampAngle (rotationX, minimumX, maximumX);

					Quaternion xQuaternion = Quaternion.AngleAxis (rotationX, Vector3.up);
					transform.localRotation = originalRotation * xQuaternion;
				} else {
					rotationY += Input.GetAxis ("Mouse Y") * sensitivityY;
					rotationY = ClampAngle (rotationY, minimumY, maximumY);

					Quaternion yQuaternion = Quaternion.AngleAxis (rotationY, Vector3.left);
					transform.localRotation = originalRotation * yQuaternion;
				}
			}
		}
	}

	public static float ClampAngle (float angle, float min, float max)
	{
		if (angle < -360F)
			angle += 360F;
		if (angle > 360F)
			angle -= 360F;
		return Mathf.Clamp (angle, min, max);
	}
}