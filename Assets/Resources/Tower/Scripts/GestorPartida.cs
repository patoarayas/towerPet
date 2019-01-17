using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestorPartida : MonoBehaviour {

    private AudioSource audioSrc;
    public AudioClip sonidoBien;
    public AudioClip sonidoMal;

    public GameManager gameManager;
    public Button botonSoltarBloque;
    public GameObject partidaTerminada;
    public GameObject vidasCanvas;
    public Transform torre;
    public Text record;
    public Text contadorBloquesText;

    private GameObject notificacionHelicoptero;
    // Contador de bloques 3D
    //private SimpleHelvetica contadorBloques3D;

    private int vidas;
    private Text vidas_text;

    private GameObject cuboPrefab;

    //Bloque actualmente colgado del helicoptero
    private GameObject nuevoBloque;

    //Transform del GestorPartida
    private Transform posicionLanzamiento;

    //Radio maximo de oscilacion del bloque
    private float radioMaximo;
    //Radio minimo de oscilacion del bloque
    private float separacion;

    //Avisa que el bloque esta listo para ser lanzado (Escalar)
    private bool bloqueListo = false;

    //Promedio posicion de la torre (punto medio)
    private float promX, promZ, alturaActual;

    //Arreglo de bloques
    private  LinkedList<GameObject> arrayBloques = new LinkedList<GameObject>();



    private void OnEnable()
    {
        iniciarPartida();

    }
    private void OnDisable()
    {
        // eliminar cubos
        for (int i = 0; i < arrayBloques.Count; i++)
        {
            Destroy(arrayBloques.First.Value);
        }
        arrayBloques.Clear();


    }
    private void Awake()
    {


        audioSrc = GetComponent<AudioSource>();
        vidas_text = vidasCanvas.GetComponent<Text>();
        posicionLanzamiento =  GetComponent<Transform>();

        // Cargamos el cubo
        cuboPrefab = Resources.Load("Tower/Cube") as GameObject;

        //Instanciamos y configuramos el contador de bloques 3D.
       // contadorBloques3D = GameObject.Find("Ground Plane Stage/Torre/marcador_bloques").GetComponent<SimpleHelvetica>();

        // Configuramos la alerta_helicoptero
        notificacionHelicoptero = GameObject.Find("Ground Plane Stage/Torre/alerta_helicoptero");

        //Obtenemos el boton: "Soltar bloque" y le agregamos el metodo, solo de esta manera funciona
        //GameObject.Find("Canvas/Torre/SoltarBloque").GetComponent<Button>().onClick.AddListener(this.lanzarBloque);
        //botonSoltarBloque.onClick.AddListener(lanzarBloque);


    }

    public void iniciarPartida()
    {
        partidaTerminada.SetActive(false);
        vidas = 3;
        vidas_text.text = vidas.ToString();
        //transform.position = new Vector3(0, 0.3f, 0);

        radioMaximo = 0.2f;
        separacion = 0.075f;


        //Se instancia el primer bloque y se inicia la partida.
        // TRANSFORM?
        nuevoBloque = Instantiate(cuboPrefab, posicionLanzamiento.position, Quaternion.identity, torre);

        record.text = PlayerPrefs.GetInt("valorLocal").ToString();

        //reiniciamos la Alerta del helicoptero
        notificacionHelicoptero.GetComponent<SimpleHelvetica>().Text = "Espere..";
        notificacionHelicoptero.GetComponent<SimpleHelvetica>().GenerateText();
        notificacionHelicoptero.GetComponent<MeshRenderer>().material.color = Color.red;
        notificacionHelicoptero.GetComponent<SimpleHelvetica>().ApplyMeshRenderer();
        // Reiniciamos al contador de bloques


        //contadorBloques3D.Text = "0";
        //contadorBloques3D.GenerateText();

        contadorBloquesText.text = "0";
        botonSoltarBloque.enabled = true;

    }




    // Update is called once per frame
    void Update () {

       // actualizarLanzamiento();

    }


    // Se calcula el promedio del centro de la torre y se mueve el gestor partida hacia ella
    private void actualizarLanzamiento()
    {

        if(arrayBloques.Count > 0)
        {
            foreach (GameObject bloque in arrayBloques)
            {
                promX += bloque.transform.position.x /arrayBloques.Count;
                promZ += bloque.transform.position.z / arrayBloques.Count;

            }


            alturaActual = arrayBloques.First.Value.transform.position.y + 0.1f + separacion + radioMaximo;
            posicionLanzamiento.position = new Vector3(promX, alturaActual, promZ);

            promZ = promX = 0;
        }     
        
    }

    private void actualizarAlerta()
    {
        if (bloqueListo)
        {
            notificacionHelicoptero.GetComponent<SimpleHelvetica>().Text = "Listo!";
            notificacionHelicoptero.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            notificacionHelicoptero.GetComponent<SimpleHelvetica>().Text = "Espere..";
            notificacionHelicoptero.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        notificacionHelicoptero.GetComponent<SimpleHelvetica>().GenerateText();
        notificacionHelicoptero.GetComponent<SimpleHelvetica>().ApplyMeshRenderer();
    }


    // Lanzar bloque a traves del boton.
    public void lanzarBloque()
    {
        if (bloqueListo)
        {
            Destroy(GameObject.Find("Ground Plane Stage/Torre/cuerda(Clone)"));
            //Se desactiva el update del bloque, y se llama a la funcion soltar

            nuevoBloque.GetComponent<nuevoBloque>().soltarBloque();
            nuevoBloque.GetComponent<nuevoBloque>().enabled = false;

            radioMaximo += 0.005f;
            separacion += 0.002f;
            bloqueListo = false;

            //Creamos un nuevo bloque para lanzar
            nuevoBloque = Instantiate(cuboPrefab, posicionLanzamiento.position, Quaternion.identity, GameObject.Find("Ground Plane Stage/Torre").transform);
            actualizarAlerta();
        }

        actualizarLanzamiento();

    }
    




    // Metodos de encapsulamiento
    public void setListo()
    {
        bloqueListo = true;
        actualizarAlerta();
    }

    public Transform getLanzamiento()
    {
        return this.posicionLanzamiento;
    }

    public float getRadioMaximo()
    {
        return radioMaximo;
    }

    public int getNumeroBloques()
    {
        return arrayBloques.Count;
    }

    public GameObject getUltimoBloque()
    {
        return arrayBloques.First.Value;
    }

    public void restarVida()
    {

        audioSrc.PlayOneShot(sonidoMal);
        if(vidas == 1) // Si perdemos todas las vidas
        {
            vidas--;
            vidas_text.text = vidas.ToString();
            botonSoltarBloque.enabled = false;
            partidaTerminada.SetActive(true);
            Text texto = partidaTerminada.GetComponentInChildren<Text>();
            texto.text = "Haz perdido \n Tu puntuación: \n "+ arrayBloques.Count.ToString();

            gameManager.terminada = true;
            gameManager.ultimoScore = arrayBloques.Count; // Agregamos el ultimo Score al local


            // eliminar cubos
            for (int i = 0; i < arrayBloques.Count; i++)
            {
                Destroy(arrayBloques.First.Value);
            }
            arrayBloques.Clear();


        }
        else
        {
            vidas--;
            vidas_text.text = vidas.ToString();
        }
      
    }
    //Agrega el bloque lanzado a el arreglo, si es que se unió correctamente.
    public void agregarBloque(GameObject nuevoBloque)
    {
        audioSrc.PlayOneShot(sonidoBien);
        arrayBloques.AddFirst(nuevoBloque);
        // Actualizamos el contador 3D.
        //contadorBloques3D.Text = arrayBloques.Count.ToString();
        //contadorBloques3D.GenerateText();
        contadorBloquesText.text = arrayBloques.Count.ToString();



    }

    
}
