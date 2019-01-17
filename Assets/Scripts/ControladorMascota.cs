using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControladorMascota : MonoBehaviour {

    public Transform camara;
    private Transform mascota;
    bool flag = false;
	// Use this for initialization
	void Start () {

        mascota = GetComponent<Transform>();
        
        

       
	}

    public void realinear()
    {
        mascota.LookAt(camara);
    }







}
