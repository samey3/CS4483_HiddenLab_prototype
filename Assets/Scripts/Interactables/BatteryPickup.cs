using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryPickup : interactable_object
{

	public float batteryAmount;

	public override void interact(GameObject playerObject){
		playerObject.GetComponent<Player_controls>().addPower(batteryAmount);
		Destroy(gameObject);
	}
}
