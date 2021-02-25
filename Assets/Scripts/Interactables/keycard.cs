using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keycard : interactable_object
{
	public override void interact(GameObject playerObject){
		Destroy(gameObject);
	}
}
