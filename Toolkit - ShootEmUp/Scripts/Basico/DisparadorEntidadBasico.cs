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

    public void Disparar(){      
        if (entidadprefab == null){
            Debug.LogError("No contiene una entidad valida.");
            return;
        }

        Entidad p = entidadprefab.Create(carpeta, transform.position, null);
        eventodisparo.Invoke();
        
    }

    public void EventoDisparar(){
        Disparar();
    }
  
}

