using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Velocidad que afecta al avance
    [SerializeField] float velocidadAvance = 0f;
    //Velocidad que afecta al giro
    [SerializeField] float velocidadGiro = 0f;
    [SerializeField] float velocidadMultiplicadorGiro = 1f;
    private Vector3 miVectorAvance;
    private Vector3 miVectorGlobalAvance;
    private Vector3 miVectorGlobalGiro;
    private Vector3 miVectorGiro;
    CharacterController miObjectoController;


    private void Awake()
    {
        //Inicializo el componente para utilizar los metodos
        miObjectoController = gameObject.GetComponent<CharacterController>();

    }


    // Update is called once per frame
    void Update()
    {

        //Funcion encargada de realizar el movimiento
        mover();

    }

    void mover()
    {
        //hacia adelante y hacia atras
        float directionVertical = this.directionVertical();
        //Giro izquierda y derecha
        float directionHorizontal = this.directionHorizontal();
        

        if(directionVertical != 0f)
        {
            
           // miVectorAvance.Set(0,0,directionVertical);
            miVectorAvance = new Vector3(0,0,directionVertical);
            //Transformo de local a global el espacio
            miVectorGlobalAvance = transform.TransformDirection(miVectorAvance);
            //Multiplico la velocidad indicada por el avance del vector
            miObjectoController.Move(miVectorGlobalAvance * velocidadAvance * Time.deltaTime);

            //Esto para arreglar el hecho de que cuando empieza a moverse, el eje Y cambia su valor a 0.58
            transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
        

            
        }
        if (directionHorizontal != 0f)
        {

            miVectorGiro = new Vector3(0,directionHorizontal,0);
            //Transformo de local a global el espacio
            miVectorGlobalGiro = transform.TransformDirection(miVectorGiro);
            //Multiplico la velocidad indicada por el giro del vector
            this.transform.Rotate(miVectorGlobalGiro * velocidadGiro * Time.deltaTime * velocidadMultiplicadorGiro);

        }
    }

    float directionVertical()
    {
        return Input.GetAxis("Vertical");
    }

    float directionHorizontal()
    {
        return Input.GetAxis("Horizontal");
    }
}
