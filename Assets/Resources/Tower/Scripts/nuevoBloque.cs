using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nuevoBloque : MonoBehaviour
{
    private GestorPartida gestorPartida;

    private bool bloqueListo = false;
    private bool bloqueLanzado = false;

    //Antigua variable "t"
    private float tiempo = 0.0f;

    private float radioActual; // Radio actual de la circunferencia
    private float radioMaximo; // radio maximo al que puede llegar

    // Sirve para aplanar la curva vertical del movimiento del cubo
    [SerializeField] private float decremento;

    //Sirven para calcular el angulo del bloque
    private float posY, posX, posZ, anguloX, anguloZ;

    private Vector3 aux, pos = Vector3.zero;

    private Transform heliCuerpo;

    void Start()
    {
        // Busca al Gestor Partida
        gestorPartida = GameObject.Find("GestorPartida").GetComponent<GestorPartida>();
        //Obtiene el radio maximo 
        radioMaximo = gestorPartida.getRadioMaximo();

        //SEtea el cubo para que no le agecte la greavedad no las colisiones
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;

        //Obtiene la posicion del helicoptero
        heliCuerpo = GameObject.Find("Helicoptero/Body").GetComponent<Transform>();
        //Pone esta posicion en la variable pos
        pos.y = heliCuerpo.transform.position.y;

        // SEtea la escala del bloque a 0
        transform.localScale = new Vector3(0.0f, 0.0f, 0.0f); // el bloque parte con una escala de 0, es decir no existe
        //GetComponent<Renderer>().material.SetColor("_Color", Random.ColorHSV());

    }


    void Update()
    {

        tiempo += Time.deltaTime * 0.5f;

        Debug.DrawLine(heliCuerpo.position, transform.position, Color.white, 0.0001f);

        // Escala el bloque mientras no este listo
        if (!bloqueListo)
        {
            aux.x = tiempo / 8;
            aux.y = tiempo / 32f;
            aux.z = tiempo / 8;
            transform.localScale = aux;
        }


        // Escalado del bloque al ser instanciado
        if (transform.localScale.y >= 0.05f && !bloqueListo)
        {
            transform.localScale = new Vector3(0.2f, 0.05f, 0.2f);
            gestorPartida.setListo();
            bloqueListo = true;

        }


        // Movimiento del bloque

        //Debug.DrawLine(heliCuerpo.position, transform.position, Color.white, 0.0001f);

        // Posicion en forma de rosa  de 8 petalos R(theta) = a * cos(2Thetha)
        radioActual = radioMaximo * Mathf.Cos((5) * tiempo + 24);
        pos.x = posX = Mathf.Cos(tiempo) * radioActual;
        pos.z = posZ = Mathf.Sin(tiempo) * radioActual;


        posY = decremento * Mathf.Sqrt(Mathf.Pow(radioMaximo, 2) - Mathf.Pow(radioActual, 2));
        //Debug.Log("pos Z: "+ posZ);
        pos.y = -posY - radioMaximo;

        transform.position = pos + heliCuerpo.position;

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
        bloqueListo = false;

        yield return new WaitForSeconds(2f);


        bool flag = true;

        if (gestorPartida.getNumeroBloques()  == 0)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            gestorPartida.agregarBloque(gameObject);
            flag = false;
        }       

        if(bloqueListo && flag)
        {
            gameObject.AddComponent<HingeJoint>();
            GetComponent<HingeJoint>().connectedBody = gestorPartida.getUltimoBloque().GetComponent<Rigidbody>();
            GetComponent<Rigidbody>().useGravity = false;
            gestorPartida.agregarBloque(gameObject);
        }
        else if(!bloqueListo && flag)
        {
            Destroy(gameObject);
        }

        gestorPartida.actualizarPosicionY();

    }

    private void OnCollisionEnter(Collision collision)
    {
        if(gestorPartida.getNumeroBloques() != 0)
        {
            if (GameObject.ReferenceEquals(collision.gameObject, gestorPartida.getUltimoBloque()))
            {
                bloqueListo = true;
            }
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if (gestorPartida.getNumeroBloques() != 0)
        {
            if (GameObject.ReferenceEquals(collision.gameObject, gestorPartida.getUltimoBloque()))
            {
                bloqueListo = false;
            }
        }
    }




}
