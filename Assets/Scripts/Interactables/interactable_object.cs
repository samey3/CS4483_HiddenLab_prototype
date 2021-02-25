using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class interactable_object : MonoBehaviour
{
	// Variables
	public string objectName;
	public string objectDescription;

	// Functions
	public abstract void interact(GameObject playerObject = null);
}
