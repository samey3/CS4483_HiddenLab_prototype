using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_switch : interactable_object
{

	public List<GameObject> doorList;

	public override void interact(GameObject playerObject = null){
		// Toggle each object in the list
		foreach(GameObject door in doorList){
			door.GetComponent<interactable_object>().interact();
		}
	}
}
