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
    // corresponde al gameObject de la torre y todos sus elementos: Torre, helicoptero, textos en 3D, bloques...
    private GameObject instanciaTorre;
    public int ultimoScore;
    public bool terminada = false;
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
        instanciaTorre = Instantiate(Resources.Load("Prefabs/Torre") as GameObject, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("Ground Plane Stage").transform);
        instanciaTorre.name = "Torre"; // le quitamos el "(Clone)".

        // activamos el canvas de la torre.
        torreCanvas.SetActive(true);
        GameObject.Find("Canvas/Torre/partida_terminada").SetActive(false);
    }

    public void iniciarMascota()
    {
        // Eliminamos la instancia "Torre".
        GameObject.Find("Canvas/Torre/soltarBloque_Boton").SetActive(true);
        Destroy(instanciaTorre);
        // desactivamos y activamos los canvas
        torreCanvas.SetActive(false);
        mascota.SetActive(true);
        mascotaCanvas.SetActive(true);

        if (terminada)
        {
            terminada = false;
            if (ultimoScore < 9)
                monedas += ultimoScore;
            else
                monedas += ultimoScore * 10;
        }
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
