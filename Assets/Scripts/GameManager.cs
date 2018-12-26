using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {

    //Animador de la mascota, TODO: llevar este metodo a la mascota
    public Animator mascotaAnimator;

    // Contador global de monedas
    public GameObject uIMonedas;
    public Text textoMonedas;    
    public int monedas = 100;

    //Permite acceder y controlar el menu (UI), notar que la
    //variable es un GameObject, no el canvas
    public GameObject menu;
    //Permite acceder y controlar la mascota
    public GameObject mascota;
    //Permite acceder y controlar el canvas de la mascota, notar que
    //la variable es el GameObject, y no es un Canvas
    public GameObject mascotaCanvas;
    // corresponde al gameObject de la torre, contiene el plano,gestorpartida y helicoptero
    public GameObject instanciaTorre;
    //Permite acceder y controlar el canvas de la torre, notar que
    //la variable es el GameObject, y no es un Canvas
    public GameObject torreCanvas;
    //Permite acceder al PlaneFinder
    public GameObject planeFinder;

    private void Awake()
    {
        //Se desactivan los elementos del juego
        //torre.SetActive(false);
        torreCanvas.SetActive(false);
        mascota.SetActive(false);
        mascotaCanvas.SetActive(false);
        uIMonedas.SetActive(false);
        planeFinder.SetActive(false);
       



    }
    // Use this for initialization
    void Start () {
		
        
	}
	
	// Update is called once per frame
	void Update () {

        textoMonedas.text = "Monedas: " + monedas;
        if(monedas == 0)
        {
            Color col = new Color(255, 0, 0);
            textoMonedas.color = col;
        }

    }
    public void iniciarJuego()
    {
        menu.SetActive(false);
        planeFinder.SetActive(true);
        mascota.SetActive(true);
        mascotaCanvas.SetActive(true);
        uIMonedas.SetActive(true);


    }

    public void iniciarTorre()
    {
        //Pausamos la mascota y su canvas(se esconden)
        mascota.SetActive(false);
        mascotaCanvas.SetActive(false);

        // Creamos una NUEVA torre con los prefab
        Instantiate(Resources.Load("Prefabs/Gestorpartida", typeof(GameObject)), new Vector3(0, 0.5f, 0), Quaternion.identity, instanciaTorre.transform);

        // activamos el canvas de la torre
        torreCanvas.SetActive(true);
    }

    public void iniciarMascota()
    {
        //torre.SetActive(false);
        torreCanvas.SetActive(false);
        mascota.SetActive(true);
        mascotaCanvas.SetActive(true);
    }

    public void darComida()
    {
        if(monedas >= 10)
        {
            mascotaAnimator.SetTrigger("Comida");
            monedas -= 10;
        }
        else
        {
            mascotaAnimator.SetTrigger("No");
            Debug.Log("Sin saldo para comida");
        }
    }
}
