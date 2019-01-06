using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestorPartida : MonoBehaviour {

    private GameManager gameManager;

    private GameObject alerta_helicopter_3D;
    // Contador de bloques 3D
    private SimpleHelvetica contador_bloques_3D;

    private int vidas;
    private Text vidas_text;

    private GameObject cuboPrefab;

    //Bloque actualmente colgado del helicoptero
    private GameObject nuevoBloque;

    //Transform del GestorPartida
    private Transform posicionLanzamiento;

    //Radio maximo de oscilacion del bloque
    private float _radioMaximo;
    //Radio minimo de oscilacion del bloque
    private float _separacion;

    //Avisa que el bloque esta listo para ser lanzado (Escalar)
    private bool _bloqueListo = false;

    //Promedio posicion de la torre (punto medio)
    private float promX, promZ, alturaActual;

    //Arreglo de bloques
    private  LinkedList<GameObject> arrayBloques = new LinkedList<GameObject>();




    private void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        vidas_text = GameObject.Find("Canvas/Torre/marcador_vidas").GetComponent<Text>();
        posicionLanzamiento =  GetComponent<Transform>();

        // Cargamos el cubo
        cuboPrefab = Resources.Load("Tower/Cube") as GameObject;

        //Instanciamos y configuramos el contador de bloques 3D.
        contador_bloques_3D = GameObject.Find("Ground Plane Stage/Torre/marcador_bloques").GetComponent<SimpleHelvetica>();

        // Configuramos la alerta_helicoptero
        alerta_helicopter_3D = GameObject.Find("Ground Plane Stage/Torre/alerta_helicoptero");

        //Obtenemos el boton: "Soltar bloque" y le agregamos el metodo, solo de esta manera funciona
        GameObject.Find("Canvas/Torre/soltarBloque_Boton").GetComponent<Button>().onClick.AddListener(this.lanzarBloque);

    }

    public void iniciarPartida()
    {
        vidas = 3;
        vidas_text.text = "Vidas x " + vidas.ToString();
        transform.position = new Vector3(0, 0.3f, 0);

        _radioMaximo = 0.2f;
        _separacion = 0.075f;


        //Se instancia el primer bloque y se inicia la partida.
        nuevoBloque = Instantiate(cuboPrefab, posicionLanzamiento.position, Quaternion.identity, GameObject.Find("Ground Plane Stage/Torre").transform);

        GameObject.Find("Canvas/Torre/record_local/Text").GetComponent<Text>().text = PlayerPrefs.GetInt("valorLocal").ToString();

        //reiniciamos la Alerta del helicoptero
        alerta_helicopter_3D.GetComponent<SimpleHelvetica>().Text = "Espere..";
        alerta_helicopter_3D.GetComponent<SimpleHelvetica>().GenerateText();
        alerta_helicopter_3D.GetComponent<MeshRenderer>().material.color = Color.red;
        alerta_helicopter_3D.GetComponent<SimpleHelvetica>().ApplyMeshRenderer();
        // Reiniciamos al contador de bloques
        contador_bloques_3D.Text = "0";
        contador_bloques_3D.GenerateText();

    }




    // Update is called once per frame
    void Update () {

        actualizarLanzamiento();

    }


    // Se calcula el promedio del centro de la torre y se mueve el gestor partida hacia ella
    private void actualizarLanzamiento()
    {

        if(arrayBloques.Count > 0)
        {
            foreach (GameObject bloque in arrayBloques)
            {
                promX = +bloque.transform.position.x / arrayBloques.Count;
                promZ = +bloque.transform.position.z / arrayBloques.Count;

            }
            alturaActual = arrayBloques.First.Value.transform.position.y + 0.1f + _separacion + _radioMaximo;
            posicionLanzamiento.position = new Vector3(promX, alturaActual, promZ);

            promZ = promX = 0;
        }     
        
    }

    private void actualizarAlerta()
    {
        if (_bloqueListo)
        {
            alerta_helicopter_3D.GetComponent<SimpleHelvetica>().Text = "Listo!";
            alerta_helicopter_3D.GetComponent<MeshRenderer>().material.color = Color.green;
        }
        else
        {
            alerta_helicopter_3D.GetComponent<SimpleHelvetica>().Text = "Espere..";
            alerta_helicopter_3D.GetComponent<MeshRenderer>().material.color = Color.red;
        }
        alerta_helicopter_3D.GetComponent<SimpleHelvetica>().GenerateText();
        alerta_helicopter_3D.GetComponent<SimpleHelvetica>().ApplyMeshRenderer();
    }


    // Lanzar bloque a traves del boton.
    public void lanzarBloque()
    {
        if (_bloqueListo)
        {
            Destroy(GameObject.Find("Ground Plane Stage/Torre/cuerda(Clone)"));
            //Se desactiva el update del bloque, y se llama a la funcion soltar
            nuevoBloque.GetComponent<nuevoBloque>().enabled = false;
            nuevoBloque.GetComponent<nuevoBloque>().soltarBloque();

            _radioMaximo += 0.005f;
            _separacion += 0.002f;
            _bloqueListo = false;

            //Creamos un nuevo bloque para lanzar
            nuevoBloque = Instantiate(cuboPrefab, posicionLanzamiento.position, Quaternion.identity, GameObject.Find("Ground Plane Stage/Torre").transform);
            actualizarAlerta();
        }

    }
    




    // Metodos de encapsulamiento
    public void setListo()
    {
        _bloqueListo = true;
        actualizarAlerta();
    }

    public Transform getLanzamiento()
    {
        return this.posicionLanzamiento;
    }

    public float getRadioMaximo()
    {
        return _radioMaximo;
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
        if(vidas == 1) // Si perdemos todas las vidas
        {
            // desactivamos el boton 
            GameObject.Find("Canvas/Torre/soltarBloque_Boton").SetActive(false);
            GameObject.Find("Canvas/Torre/partida_terminada").SetActive(true);
            Text texto = GameObject.Find("Canvas/Torre/partida_terminada/Text").GetComponent<Text>();
            texto.text = "Haz perdido \n puntuacion maxima: \n " + arrayBloques.Count.ToString();
            gameManager.terminada = true;
            gameManager.ultimoScore = arrayBloques.Count; // Agregamos el ultimo Score al local
            return;
        }
        vidas -= 1;
        vidas_text.text = "Vidas x " + vidas.ToString();
    }
    //Agrega el bloque lanzado a el arreglo, si es que se unio correctamente.
    public void agregarBloque(GameObject nuevoBloque)
    {
        this.arrayBloques.AddFirst(nuevoBloque);
        // Actualizamos el contador 3D.
        contador_bloques_3D.Text = arrayBloques.Count.ToString();
        contador_bloques_3D.GenerateText();

    }

    
}
