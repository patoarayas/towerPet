using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nuevoBloque : MonoBehaviour
{
    private GestorPartida gestorPartida;

    private bool bloqueListo = false;
    private bool bloqueLanzado = false;

    private GameObject cuerda;
    //Antigua variable "t"
    private float tiempo = 0.0f;

    private float radioActual; // Radio actual de la circunferencia
    private float radioMaximo; // radio maximo al que puede llegar

    // Sirve para aplanar la curva vertical del movimiento del cubo
    [SerializeField] private float decremento;

    //Sirven para calcular el angulo del bloque
    private float posY, posX, posZ, anguloX, anguloZ;

    private Vector3 aux, pos = Vector3.zero;

    private Transform helicoptero;

    void Start()
    {
        // Busca al Gestor Partida
        gestorPartida = GameObject.Find("Ground Plane Stage/Torre/GestorPartida").GetComponent<GestorPartida>();
        // Obtenemos la posicion del helicoptero
        helicoptero = GameObject.Find("Ground Plane Stage/Torre/Helicoptero/Body").transform;
        //Obtiene el radio maximo
        radioMaximo = gestorPartida.getRadioMaximo();

        //Setea el cubo para que no le afecte la greavedad ni las colisiones
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;

        //Pone esta posicion en la variable pos
        pos.y = helicoptero.transform.position.y;

        // Setea la escala del bloque a 0
        transform.localScale = new Vector3(0.0f, 0.0f, 0.0f); // el bloque parte con una escala de 0, es decir no existe
        GetComponent<Renderer>().material.SetColor("_Color", Random.ColorHSV());

        cuerda = Instantiate(Resources.Load("Prefabs/cuerda") as GameObject, new Vector3(0, 0, 0), Quaternion.identity, GameObject.Find("Ground Plane Stage/Torre").transform);
        cuerda.GetComponent<Cuerda>().iniciarCuerda(GetComponent<Transform>(), helicoptero);

    }


    void Update()
    {

        tiempo += Time.deltaTime * 0.5f;

        // Escala el bloque mientras no este listo
        if (!bloqueListo)
        {
            aux.x = tiempo / 8;
            aux.y = tiempo / 32f;
            aux.z = tiempo / 8;
            transform.localScale = aux;
        }


        // Escalado del bloque al ser instanciado
        if (transform.localScale.y >= 0.025f && !bloqueListo)
        {
            transform.localScale = new Vector3(0.1f, 0.025f, 0.1f);
            gestorPartida.setListo();
            bloqueListo = true;
        }


        // Movimiento del bloque


        // Posicion en forma de rosa  de 8 petalos R(theta) = a * cos(2Thetha)
        radioActual = radioMaximo * Mathf.Cos((5) * tiempo + 24);
        pos.x = posX = Mathf.Cos(tiempo) * radioActual;
        pos.z = posZ = Mathf.Sin(tiempo) * radioActual;


        posY = decremento * Mathf.Sqrt(Mathf.Pow(radioMaximo, 2) - Mathf.Pow(radioActual, 2));
        //Debug.Log("pos Z: "+ posZ);
        pos.y = -posY - radioMaximo;

        transform.position = pos + helicoptero.position;

        anguloX = decremento * Mathf.Abs(Mathf.Atan(posX / posY) * Mathf.Rad2Deg);
        anguloZ = decremento * Mathf.Abs(Mathf.Atan(posZ / posY) * Mathf.Rad2Deg);

        if (posX < 0)
            anguloZ = -anguloZ;
        if (posZ > 0)
            anguloX = -anguloX;

        transform.eulerAngles = new Vector3(anguloX, 0, anguloZ);
        //Debug.Log(anguloZ);





    }


    // Metodos de 
    public void soltarBloque()
    {

        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().useGravity = true;
        StartCoroutine(iniciarComprobacion());

    }

    private IEnumerator iniciarComprobacion()
    {
        yield return new WaitForSeconds(2f);


        if (gestorPartida.getNumeroBloques()  == 0)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            gestorPartida.agregarBloque(gameObject);
        }
        else
        {
            if (Vector3.Distance(transform.position, gestorPartida.getUltimoBloque().transform.position) >= 0.0559f)
            {
                gestorPartida.restarVida();
                Destroy(gameObject);
            }
            else
            {
                gameObject.AddComponent<HingeJoint>();
                GetComponent<HingeJoint>().connectedBody = gestorPartida.getUltimoBloque().GetComponent<Rigidbody>();
                GetComponent<Rigidbody>().useGravity = false;
                gestorPartida.agregarBloque(gameObject);
            }
        }



    }






}
