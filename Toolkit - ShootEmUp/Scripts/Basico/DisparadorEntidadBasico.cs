using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using SEUP;

public class DisparadorEntidadBasico : MonoBehaviour{

    [SerializeField]
    private Entidad entidadprefab = null;
    [SerializeField]
    private Transform carpeta = null;
    [Header("Disparador - Eventos")]
    [SerializeField]
    private UnityEvent eventodisparo = new UnityEvent();

    private Entidad entidad = null;

    private void Awake(){
        Transform padre = transform.parent;
        while (padre != null)
        {
            entidad = padre.gameObject.GetComponent<Entidad>();
            if (entidad != null)
                break;
            padre = padre.transform.parent;
        }            
    }
    public void Disparar(){  
         
        if (entidadprefab == null){
            Debug.LogError("No contiene una entidad valida.");
            return;
        }
        if (carpeta != null){            
            if (entidad != null)
                entidadprefab.Create(carpeta, transform.position, entidad);
            else
                entidadprefab.Create(carpeta, transform.position, null);
        }
        else if (entidad != null)
            entidadprefab.Create(entidad.transform.parent, transform.position, entidad);        
        else 
            entidadprefab.Create(carpeta, transform.position, null);
        
        eventodisparo.Invoke();        

    }

    public void EventoDisparar(){
        Disparar();
    }
  
}

