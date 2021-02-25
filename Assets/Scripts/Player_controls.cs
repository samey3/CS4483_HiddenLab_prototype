using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_controls : MonoBehaviour
{


	/*---------------------------------------------------------------------*\
	|								Variables								|
	\* --------------------------------------------------------------------*/


	// Flashlight
	public GameObject flashlight;
	float powerLevel = 1.0F;			// Maximum of 1.0
	float maxIntensity = 3.0F;
	float consumptionRate = 0.01F;		// 1% per second

	// Movement
	Rigidbody rb;
	float verticalIn, horizontalIn;
	float minThresh = 0.1F;
	float speed = 3.0f;						// Movement speed
	Vector3 lastPosition = Vector3.forward;		// Last postion of the player
	Vector3 dirVec = new Vector3(0,0,0);			// Movement direction of the player

	// Object interaction
	float detectionRadius = 0.5F; // 1.0F
	KeyCode interactKey = KeyCode.F;
	string shaderPath = ("highlight");
	bool isActive;
	bool isDisplayed;
	GameObject closestObject;
	interactable_object interactableObject;

	// Hiding
	bool isHidden = false;


	/*---------------------------------------------------------------------*\
	|									Start								|
	\* --------------------------------------------------------------------*/


    // Start is called before the first frame update
    void Start()
    {
		rb = GetComponent<Rigidbody>();
		lastPosition = transform.localPosition;
    }


	/*---------------------------------------------------------------------*\
	|									Update								|
	\* --------------------------------------------------------------------*/


    // Update is called once per frame
    void Update()
    {

		/*---------------------------------------------------------------*\
		/*Player movement------------------------------------------------*/

		// Get input
		verticalIn = (Mathf.Abs(Input.GetAxis("Vertical")) > minThresh ? Input.GetAxis("Vertical") : 0);
		horizontalIn = (Mathf.Abs(Input.GetAxis("Horizontal")) > minThresh ? Input.GetAxis("Horizontal") : 0);
		if (!isHidden) {
			rb.MovePosition (transform.localPosition + (verticalIn * Vector3.forward + horizontalIn * Vector3.right) * speed * Time.deltaTime);
		}

		// If the player falls through the plane, bring them back up
		if (transform.position.y < -1)
			transform.position = new Vector3 (transform.position.x, 1, transform.position.z);

		// Manage rotation
		if(transform.localPosition != lastPosition){
			dirVec = Vector3.ProjectOnPlane ((transform.localPosition - lastPosition), Vector3.up);
			lastPosition = transform.localPosition;
			if(dirVec.magnitude > 0.01) transform.rotation = Quaternion.LookRotation(dirVec, Vector3.up);
		}


		/*---------------------------------------------------------------*\
		/*Flashlight-----------------------------------------------------*/

		if (flashlight != null) {
			if (!isHidden) {
				powerLevel = Mathf.Clamp (powerLevel - consumptionRate * Time.deltaTime, 0, 1.0F);	// Decrease the power level by the consumption rate
				flashlight.GetComponent<Light> ().intensity = maxIntensity * powerLevel;
			}
			else {
				flashlight.GetComponent<Light>().intensity = 0; // Disable the flashlight while hiding
			}
		}


		/*---------------------------------------------------------------*\
		/*Interactable objects-------------------------------------------*/

		// Find nearby colliders of objects in the "Interactable" layer
		Collider[] nearbyColliders = Physics.OverlapSphere(transform.position, detectionRadius, 1<<LayerMask.NameToLayer("Interactable"));

		// If there is no nearest object, make sure these are set to false (e.g. they were destroyed by another script)
		if (closestObject == null) isActive = false;

		// If near an object, set the shader
		if(nearbyColliders.Length != 0 && !isActive){
			// Get the object, set the shader
			closestObject = nearbyColliders[0].gameObject;
			closestObject.GetComponent<Renderer>().material.shader = Shader.Find(shaderPath);

			// Display the info
			interactableObject = closestObject.GetComponent<interactable_object>();
			isDisplayed = true;

			isActive = true;
			Debug.Log ("switch, display info");
		}
		// Else return it to default
		else if(nearbyColliders.Length == 0 && closestObject != null && isActive)
		{  
			closestObject.GetComponent<Renderer>().material.shader = Shader.Find("Diffuse");
			isDisplayed = false;
			isActive = false;
			closestObject = null;
		}
		// If key press, interact with it
		if(isActive && Input.GetKeyDown(interactKey))
		{
			interactableObject = closestObject.GetComponent<interactable_object>();
			interactableObject.interact(gameObject);
		}
    }


	/*---------------------------------------------------------------------*\
	|								Object collisions						|
	\* --------------------------------------------------------------------*/


	private void OnTriggerEnter(Collider colObj){
		// Battery
		/*if (colObj.tag == "Battery") {
			// Ensure it has the component
			if (colObj.gameObject.GetComponent<BatteryPickup> () != null) {
				Debug.Log ("Before: " + powerLevel);
				//powerLevel = Mathf.Clamp(powerLevel + colObj.gameObject.GetComponent<BatteryPickup>().getAmount (), 0, 1.0F);
				//Destroy(colObj.gameObject);
				Debug.Log ("Now have: " + powerLevel);
			}
		}*/

		// Monster
		if (colObj.tag == "Enemy" && !isHidden) {
			UnityEngine.SceneManagement.SceneManager.LoadScene ("titleScene");
		}
	}


	/*---------------------------------------------------------------------*\
	|								Access methods							|
	\* --------------------------------------------------------------------*/


	public void addPower(float pow){
		powerLevel = Mathf.Clamp(powerLevel + pow, 0, 1.0F);
	}

	public void toggleHidden(){
		isHidden = !isHidden;
		GetComponent<Renderer>().enabled = !isHidden;
		rb.isKinematic = isHidden;
	}

	public bool isPlayerHidden(){
		return isHidden;
	}


	/*---------------------------------------------------------------------*\
	|									GUI									|
	\* --------------------------------------------------------------------*/


	void OnGUI(){
		GUILayout.BeginArea(new Rect(10f, 10f, Screen.width, Screen.height));
		GUILayout.Label("Power level = " + string.Format("{0:0.00}", powerLevel));
		GUILayout.EndArea();

		if(isDisplayed && closestObject != null){
			Vector3 objectScreenPosition = Camera.main.WorldToScreenPoint(closestObject.transform.position);
			GUI.Label(new Rect(objectScreenPosition.x, objectScreenPosition.y, 300, 100), "Press F to interact with " + interactableObject.objectName + ".\r\n<i>" + interactableObject.objectDescription + "</i>");
		}
	}
}
