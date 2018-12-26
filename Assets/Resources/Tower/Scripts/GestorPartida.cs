using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GestorPartida : MonoBehaviour {

    //UI
    //TODO: Agregar contador de bloques al canvas de la torre
    public Text contBloquesUI;


   //private Helicoptero helicoptero;

    private GameObject cuboPrefab;

    //Bloque actualmente colgado del helicoptero
    private GameObject nuevoBloque;

    //Transform del GestorPartida
    private Transform posicionLanzamiento;

    //Radio maximo de oscilacion del bloque
    private float _radioMaximo = 0.25f;
    //Radio minimo de oscilacion del bloque
    private float _separacion = 0.3f;

    //Avisa que el bloque esta listo para ser lanzado (Escalar)
    private bool _bloqueListo = false;

    //Promedio posicion de la torre (punto medio)
    private float promX, promZ;

    //Arreglo de bloques
    private  LinkedList<GameObject> arrayBloques = new LinkedList<GameObject>();

    // Arreglo de materiales para los bloques
    //private Material[] arrayMaterial = new Material[7];



    private void Start()
    {
        posicionLanzamiento =  GetComponent<Transform>();
        // Cargamos el cubo
        cuboPrefab = Resources.Load("Tower/Cube") as GameObject;
        //Instanciamos el helicoptero en una posicion y lo ponemos como hijo de la torre.
        Instantiate(Resources.Load("Prefabs/Helicoptero", typeof(GameObject)), new Vector3(0, 1f, 0), Quaternion.identity, GameObject.Find("Ground Plane Stage/Torre").transform);

        //Obtenemos el boton: "Soltar bloque" y le agregamos el metodo, solo de esta manera funciona
        GameObject.Find("Canvas/Torre/soltarBloque_Boton").GetComponent<Button>().onClick.AddListener(this.lanzarBloque);
        //Se instancia el primer bloque 
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

                //Debug.Log("promX: " + promX + " // promZ: " + promZ);
                Debug.Log("cant bloques: " + arrayBloques.Count);
            }
            posicionLanzamiento.position = new Vector3(promX, posicionLanzamiento.position.y, promZ);

            promZ = promX = 0;
        }     
        
    }

    public void lanzarBloque()
    {
        Debug.Log("Boton: gestor partida");
        if (_bloqueListo)
        {
            Debug.Log("Lanzado!!");
            //Se desactiva el update del bloque, y se llama a la funcion soltar
            nuevoBloque.GetComponent<nuevoBloque>().enabled = false;
            nuevoBloque.GetComponent<nuevoBloque>().soltarBloque();



            _radioMaximo += 0.01f;
            _bloqueListo = false;

            //Creamos un nuevo bloque para lanzar
            nuevoBloque = Instantiate(cuboPrefab, posicionLanzamiento.position, Quaternion.identity, GameObject.Find("Ground Plane Stage/Torre").transform);
        }

    }
    




    // Metodos de encapsulamiento
    public void setListo()
    {
        Debug.Log("setlisto en gestorpartida");
        _bloqueListo = true;
        Debug.Log(_bloqueListo);
    }

    public Transform getLanzamiento()
    {
        return this.posicionLanzamiento;
    }

    public float getRadioMaximo()
    {
        if (_radioMaximo >= _separacion)
            return _radioMaximo;
        else
            return _separacion;
    }

    public int getNumeroBloques()
    {
        return arrayBloques.Count;
    }

    public GameObject getUltimoBloque()
    {
        return arrayBloques.First.Value;
    }


    //Agrega el bloque lanzado a el arreglo, si es que se unio correctamente
    public void agregarBloque(GameObject nuevoBloque)
    {
        this.arrayBloques.AddFirst(nuevoBloque);
        //contBloquesUI.text = "Bloques: " + arrayBloques.Count.ToString();
    }

    // Actualiza la posicion Y del gestor partida
    public void actualizarPosicionY()
    {
        if(arrayBloques.Count == 0)
            posicionLanzamiento.position = new Vector3(0, 0.5f, 0);
        else
        {
            float aux;
            if (_radioMaximo >= _separacion)
                aux = _radioMaximo;
            else
                aux = _separacion;
            posicionLanzamiento.position = new Vector3(0, arrayBloques.First.Value.transform.position.y + aux + 0.4f, 0);
        }

    }
}
