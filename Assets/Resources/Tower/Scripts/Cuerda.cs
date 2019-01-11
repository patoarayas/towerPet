using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuerda : MonoBehaviour {

    private LineRenderer lineRenderer;
    private Transform origen;
    private Transform destino;

	public void iniciarCuerda(Transform origen, Transform destino)
    {
        lineRenderer = GetComponent<LineRenderer>();
        this.origen = origen;
        this.destino = destino;
        GetComponent<Cuerda>().enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
        lineRenderer.SetPosition(0, origen.position);
        lineRenderer.SetPosition(1, destino.position);
    }

    private void OnDisable()
    {
        Destroy(gameObject);
    }
}
