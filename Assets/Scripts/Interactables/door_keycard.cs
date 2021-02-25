using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_keycard : interactable_object
{
	public GameObject gateObject;
	public GameObject keyCard;

	public override void interact(GameObject playerObject = null){
		// Toggle the gate object of the door if the keycard object does not exist anymore (was destroyed)
		if(keyCard == null){
			gateObject.active = !gateObject.active;
		}
	}
}
