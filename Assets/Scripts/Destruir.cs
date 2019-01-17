using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destruir : MonoBehaviour {

    private void Start()
    {

    }
   

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
