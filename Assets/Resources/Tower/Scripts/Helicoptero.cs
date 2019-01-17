using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helicoptero : MonoBehaviour {

    //Partes en movimiento del helicoptero
    public GameObject rotor;
    public GameObject rotorTrasero;

    private Rigidbody helicopteroRigidBody;

    //Transform del GameObject gestorPartida
    private Transform posicionLanzamiento;
    // Tranform del alerta_helicoptero
    private Transform alerta_helicoptero;

    private AudioSource sonidoHelicoptero;

    void Awake () {

        sonidoHelicoptero = GetComponent<AudioSource>();
        helicopteroRigidBody = GetComponent<Rigidbody>();
        posicionLanzamiento = GameObject.Find("Ground Plane Stage/Torre/GestorPartida").transform;
        alerta_helicoptero = GameObject.Find("Ground Plane Stage/Torre/alerta_helicoptero").transform;
        transform.eulerAngles = new Vector3(0, 180, 0);
    }
	
	void Update () {

        rotor.transform.Rotate(Vector3.up * Time.deltaTime * 600, Space.World);
        rotorTrasero.transform.Rotate(Vector3.right * Time.deltaTime * 600, Space.World);

        helicopteroRigidBody.velocity =  posicionLanzamiento.position - transform.position;
        alerta_helicoptero.position = transform.position + new Vector3(-0.1f, 0.14f, 0);

    }

    private void OnEnable()
    {
        sonidoHelicoptero.Play();
    }
    private void OnDisable()
    {
        sonidoHelicoptero.Stop();
    }




}
