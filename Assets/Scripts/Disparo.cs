using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Disparo : MonoBehaviour
{
    [SerializeField] private float longitud_rayo = 8f;

    //Elegir con los elementos que queremos que choque el rayo
    public LayerMask LayerToHitRaycast;
    //Posicion de la pistola
    [SerializeField] Transform pistola;
    private float altura = 5f;

    PlayerInput playerInput;

    private GameObject pared;

    Transform basePlayer;

    Vector3 direction;

    private float duracionTexto = 2f;
    [SerializeField] private TextMeshProUGUI texto;

    //Accedo al elemento hijo para obtener su posicion para la orientacion del rayo
    private void Awake()
    {
        basePlayer = gameObject.GetComponentsInChildren<Transform>().FirstOrDefault(hijo => hijo.name == "Pistola");
    }

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actions["Dispara"].started += EmpezarDisparo;

        
        texto.enabled = false;
        

        
    }

    //Metodo encargado de lanzar el rayo que abre las puertas, estas se van hacia arriba x posiciones establecidas
    IEnumerator Raycast()
    {
        //Coge la orientacion del objeto de forma local
        direction = basePlayer.transform.forward;
        
        string tagPuerta = "";


        //longitud del disparo
        Vector3 disparo = (pistola.position + (direction * longitud_rayo));

        //Para almacenar la informacion con los collider que colisiona el rayo
        RaycastHit hit;

        if (Physics.Raycast(pistola.position, direction, out hit, longitud_rayo, LayerToHitRaycast))
        {
            tagPuerta = hit.collider.tag.Substring("Interruptor".Length);

            pared = GameObject.FindGameObjectWithTag(tagPuerta);
            if (pared != null)
            {

                float alturaDeseada = pared.transform.position.y + altura;
                for (float i = pared.transform.position.y; i < alturaDeseada; i++)
                {
                    pared.transform.position = new Vector3(pared.transform.position.x, i, pared.transform.position.z);
                    yield return new WaitForSeconds(0.05f);
                }

                MostrarTexto();
            }
        }

    }

    //Llama a corutina al pulsar el boton izquierdo del raton
    private void EmpezarDisparo(InputAction.CallbackContext context)
    {
        StartCoroutine(Raycast());
    }

    //Cuando se destruya el objeto, se desuscriba del eventos
    private void OnDisable()
    {
        if (playerInput != null)
        {
            playerInput.actions["Dispara"].started -= EmpezarDisparo;
        }
    }

    //Muestra el texto con el mensaje
    void MostrarTexto()
    {
        if (!texto.enabled)
        {
            texto.enabled = true;
        }
        StartCoroutine(DesaparecerDespuesDeTiempo());
    }

    //Metodo para hacer desaparecer el texto del feedback a los segundos establecidos
    IEnumerator DesaparecerDespuesDeTiempo()
    {
        // Espera durante el tiempo especificado
        yield return new WaitForSeconds(duracionTexto);

        // Desactiva el objeto o el componente TextMeshPro
        if (texto.enabled)
        { 
            texto.enabled = false;
        }
    }
}
