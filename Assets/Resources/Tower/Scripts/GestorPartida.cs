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
    private float _radioMaximo = 0.2f;
    //Radio minimo de oscilacion del bloque
    private float _separacion = 0.075f;

    //Avisa que el bloque esta listo para ser lanzado (Escalar)
    private bool _bloqueListo = false;

    //Promedio posicion de la torre (punto medio)
    private float promX, promZ, alturaActual;

    //Arreglo de bloques
    private  LinkedList<GameObject> arrayBloques = new LinkedList<GameObject>();




    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        vidas = 3;
        vidas_text = GameObject.Find("Canvas/Torre/marcador_vidas").GetComponent<Text>();
        vidas_text.text = "Vidas x " + vidas.ToString();
        posicionLanzamiento =  GetComponent<Transform>();
        transform.position = new Vector3(0, 0.275f, 0);

        // Cargamos el cubo
        cuboPrefab = Resources.Load("Tower/Cube") as GameObject;


        // Cargamos e instanciamos el plano y lo dejamos como hijo de la torre.
        Instantiate(Resources.Load("Prefabs/PlanoInvicible") as GameObject, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("Ground Plane Stage/Torre").transform);

        //Instanciamos y configuramos el contador de bloques 3D.
        GameObject aux_contador = Instantiate(Resources.Load("Prefabs/marcador_bloques") as GameObject, new Vector3(-0.31f, 0, -0.372f), Quaternion.identity, GameObject.Find("Ground Plane Stage/Torre").transform);
        aux_contador.transform.eulerAngles = new Vector3(0, -45, 0);
        contador_bloques_3D = aux_contador.GetComponent<SimpleHelvetica>();

        // Instanciamos y configuramos la alerta_helicoptero
        GameObject aux_alerta = Instantiate(Resources.Load("Prefabs/alerta_helicoptero") as GameObject, new Vector3(-0.31f, 0, -0.372f), Quaternion.identity, GameObject.Find("Ground Plane Stage/Torre").transform);
        alerta_helicopter_3D = aux_alerta;
        alerta_helicopter_3D.GetComponent<SimpleHelvetica>().Text = "Espere..";
        alerta_helicopter_3D.GetComponent<SimpleHelvetica>().GenerateText();
        alerta_helicopter_3D.GetComponent<MeshRenderer>().material.color = Color.red;
        alerta_helicopter_3D.GetComponent<SimpleHelvetica>().ApplyMeshRenderer();


        // Cargamos e instanciamos el helicoptero en una posicion y lo ponemos como hijo de la torre.
        GameObject aux_heli = Instantiate(Resources.Load("Prefabs/Helicoptero") as GameObject, new Vector3(0, 1f, 0), Quaternion.identity, GameObject.Find("Ground Plane Stage/Torre").transform);
        // Le sacamos el "(Clone)"
        aux_heli.name = "Helicoptero";


        //Obtenemos el boton: "Soltar bloque" y le agregamos el metodo, solo de esta manera funciona
        GameObject.Find("Canvas/Torre/soltarBloque_Boton").GetComponent<Button>().onClick.AddListener(this.lanzarBloque);
        //Se instancia el primer bloque y se inicia la partida.
        nuevoBloque = Instantiate(cuboPrefab, posicionLanzamiento.position, Quaternion.identity, GameObject.Find("Ground Plane Stage/Torre").transform);

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
        if(vidas == 1)
        {
            GameObject.Find("Canvas/Torre/soltarBloque_Boton").SetActive(false);
            GameObject.Find("Canvas/Torre/partida_terminada").SetActive(true);
            Text texto = GameObject.Find("Canvas/Torre/partida_terminada/Text").GetComponent<Text>();
            texto.text = "Haz perdido \n puntuacion maxima: \n " + arrayBloques.Count.ToString();
            gameManager.terminada = true;
            gameManager.ultimoScore = arrayBloques.Count;
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
