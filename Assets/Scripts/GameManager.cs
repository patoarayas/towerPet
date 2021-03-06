﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

    //Animador de la mascota, TODO: llevar este metodo a la mascota
    public Animator mascotaAnimator;
    public ControladorMascota controlMascota;
    // Contador global de monedas
    public GameObject uIMonedas;
    public Text textoMonedas;
    public int monedas;

    //Permite acceder y controlar el menu (UI), notar que la
    //variable es un GameObject, no el canvas
    public GameObject menu;
    //Permite acceder y controlar la mascota
    public GameObject mascota;

    public GameObject torre;
    //Permite acceder y controlar el canvas de la mascota, notar que
    //la variable es el GameObject, y no es un Canvas
    public GameObject mascotaCanvas;
    // corresponde al gameObject de la torre y todos sus elementos: Torre, helicoptero, textos en 3D, bloques...

    // Puntaje de bloques o algo asi supongo
    public int ultimoScore;
    // Si el usuario finalizo la partida correctamente sera true y las monedas se sumaran.
    public bool terminada = false;

    // TODO: Falta ver esto
    // Emisor de sonidos
    private AudioSource audioSrc;
    // Audios del sonido mp3
    public AudioClip sonidoComer;
    public AudioClip sonidoMinijuego;
    public AudioClip sonidoInicio;
    public AudioClip sonidoBoton;
    public AudioClip sonidoNo;
    public AudioClip sonidoAparecer;

    //Permite acceder y controlar el canvas de la torre, notar que
    //la variable es el GameObject, y no es un Canvas
    public GameObject torreCanvas;
    //Permite acceder al PlaneFinder
    public GameObject planeFinder;

    private void Awake()
    {
        audioSrc = GetComponent<AudioSource>();
        // Oculta los elementos del juego, dejando solo el menu principal
        torre.SetActive(false);
        torreCanvas.SetActive(false);
        mascota.SetActive(false);
        mascotaCanvas.SetActive(false);
        uIMonedas.SetActive(false);
        planeFinder.SetActive(false);
        menu.SetActive(true);
        


        // Carga la partida guardada, si no existe la crea
        if (!PlayerPrefs.HasKey("valorMonedas"))
        {
            PlayerPrefs.SetInt("valorMonedas", 100);
        }

        monedas = PlayerPrefs.GetInt("valorMonedas");
        
 




    }
	
	// Update is called once per frame
	void Update () {

        // Muestra la cantidad de monedas en pantalla
        textoMonedas.text = monedas.ToString();
        if(monedas == 0)
        {
            Color col = new Color(255, 0, 0);
            textoMonedas.color = col;
        }
        else
        {
            Color col = new Color(0, 0, 0);
            textoMonedas.color = col;
        }

    }

    public void iniciarJuego()
    {
        audioSrc.PlayOneShot(sonidoInicio);
        menu.SetActive(false);
        planeFinder.SetActive(true);
        mascota.SetActive(true);
        mascotaCanvas.SetActive(true);
        uIMonedas.SetActive(true);

        controlMascota.realinear();



    }

    public void iniciarTorre()
    {
        audioSrc.PlayOneShot(sonidoMinijuego);
        //Pausamos la mascota y su canvas(se esconden)
        mascota.SetActive(false);
        mascotaCanvas.SetActive(false);
        torreCanvas.SetActive(true);
        // Activamos la torre
        torre.SetActive(true);
        //GameObject.Find("Ground Plane Stage/Torre/GestorPartida").GetComponent<GestorPartida>().iniciarPartida();
        //GameObject.Find("Canvas/Torre/partida_terminada").SetActive(false);
        //torre.GetComponentInChildren<GestorPartida>().iniciarPartida();
    }

    public void iniciarMascota()
    {
        // Eliminamos la instancia "Torre".
        //GameObject.Find("Canvas/Torre/soltarBloque_Boton").SetActive(true);
        audioSrc.PlayOneShot(sonidoBoton);
        // desactivamos y activamos los canvas
        torreCanvas.SetActive(false);
        mascota.SetActive(true);
        mascotaCanvas.SetActive(true);
        torre.SetActive(false);

        finPartida();
        controlMascota.realinear();
    }

    public void darComida(int precio)
    {
        if(monedas >= precio)
        {
            audioSrc.PlayOneShot(sonidoComer);
            mascotaAnimator.SetTrigger("Comida");
            monedas -= precio;
            PlayerPrefs.SetInt("valorMonedas", monedas);
        }
        else
        {
            audioSrc.PlayOneShot(sonidoNo);
            mascotaAnimator.SetTrigger("No");
            Debug.Log("Sin saldo para comida");
        }
    }

    //TODO: Revisar si esto es necesario
    private void finPartida()
    {
        // TODO: Comprobar que este terminada podria ser innecesario,
        // de todas maneras a volver a la mascota se deben ejecutar las mismas acciones,
        // halla terminado la partida o no
       /* if (!terminada)
        {
            return;
        }
        */

        terminada = false;

        //Sumamos las monedas

        monedas += ultimoScore * 10;
        PlayerPrefs.SetInt("valorMonedas", monedas);

        //Guardamos de manera local
        if (!PlayerPrefs.HasKey("valorLocal"))
        {
            PlayerPrefs.SetInt("valorLocal", ultimoScore);
        }
        else
        {
            if (PlayerPrefs.GetInt("valoLocal") < ultimoScore)
                PlayerPrefs.SetInt("valorLocal",ultimoScore);
        }




            
    }

    public void setPlaneFinder(bool set)
    {
        planeFinder.SetActive(set);
        audioSrc.PlayOneShot(sonidoAparecer);
        controlMascota.realinear();
    }

}
