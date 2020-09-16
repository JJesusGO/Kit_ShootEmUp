using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using SEUP;

public class DisparadorProyectilBasico : MonoBehaviour{
    [Header("Disparador - General")]
    [SerializeField]
    private float      tiempo = 0.5f;
    [SerializeField]
    private Proyectil  disparoprefab = null;
    [Header("Disparador - Eventos")]
    [SerializeField]
    private UnityEvent eventodisparo = new UnityEvent();

    private Temporizador temporizador = null;

    public void Disparar(Entidad entidad){      
        if (disparoprefab == null){
            Debug.LogError("No contiene un proyectil valido.");
            return;
        }
        if (IsActivo()){

            Proyectil p = (Proyectil)disparoprefab.Create(transform.parent, transform.position, entidad);
            p.Disparar(transform.forward);
            eventodisparo.Invoke();

            temporizador.Start();
        }
    }
    private void Awake(){
        temporizador = new Temporizador(tiempo);
    }
    private void Update(){
        temporizador.Update();


    }

    public bool IsActivo(){
        return temporizador.IsActivo();      
    }
}

