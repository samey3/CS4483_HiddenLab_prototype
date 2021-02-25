using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hideable_object : interactable_object
{

	public override void interact(GameObject playerObject){
		playerObject.GetComponent<Player_controls>().toggleHidden();
	}
}
