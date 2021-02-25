using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : interactable_object
{
	public GameObject gateObject;
    
	public override void interact(GameObject playerObject = null){
		// Toggle the gate object of the door
		gateObject.active = !gateObject.active;
	}
}
