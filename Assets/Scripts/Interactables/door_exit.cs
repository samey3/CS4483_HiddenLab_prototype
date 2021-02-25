using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door_exit : interactable_object
{
	public override void interact(GameObject playerObject = null){
		// Exit the level
		UnityEngine.SceneManagement.SceneManager.LoadScene ("titleScene");
	}
}
