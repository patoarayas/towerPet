using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camara : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey(KeyCode.W))
        {
            transform.position = new Vector3(0,transform.position.y+2,-20);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position = new Vector3(0, transform.position.y - 2, -20);
        }
    }
}
