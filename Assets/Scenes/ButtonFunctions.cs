using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonFunctions : MonoBehaviour
{
	public void loadLevel(){
		UnityEngine.SceneManagement.SceneManager.LoadScene ("SampleScene");
	}

	public void exitGame(){
		Application.Quit ();
	}
}
