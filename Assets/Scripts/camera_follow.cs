using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_follow : MonoBehaviour
{

	public GameObject playerObj;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		transform.position = new Vector3(playerObj.transform.position.x, 20, playerObj.transform.position.z);
    }
}
