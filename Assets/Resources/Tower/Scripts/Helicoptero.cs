using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicoptero : MonoBehaviour {

    //Partes en movimiento del helicoptero
    public GameObject rotor;
    public GameObject rotorTrasero;

    private Rigidbody helicopteroRigidBody;

    //Transform del GameObject gestorPartida
    public Transform posicionLanzamiento;
    

    void Start () {

        helicopteroRigidBody = GetComponent<Rigidbody>();
        posicionLanzamiento = GameObject.Find("Ground Plane Stage/Torre/GestorPartida(Clone)").transform;
    }
	
	void Update () {

        rotor.transform.Rotate(Vector3.up * Time.deltaTime * 600, Space.World);
        rotorTrasero.transform.Rotate(Vector3.right * Time.deltaTime * 600, Space.World);

        helicopteroRigidBody.velocity =  posicionLanzamiento.position - transform.position;

    }




}
