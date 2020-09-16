using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using SEUP;

public class DetectorBasico : MonoBehaviour{

    [SerializeField]
    private DeteccionObjetivo objetivo = DeteccionObjetivo.TODOS;
    [SerializeField]
    private UnityEvent eventodeteccion = new UnityEvent();

    private Entidad entidad = null;

    private void Awake(){

        Colision[] colisiones = GetComponentsInChildren<Colision>();
        if (colisiones != null)
            for (int i = 0; i < colisiones.Length; i++)
                colisiones[i].AddColisionEvento(EventoColision);

    }

    private void EventoColision(ColisionInformacion info){

        Entidad entidad = info.GetEntidadImpacto();
        if (entidad == null || info.GetColisionEstado() != ColisionEstado.ENTER)
            return;

        switch(objetivo){
            case DeteccionObjetivo.ALIADO:

                if (entidad.GetTipo() == EntidadTipo.ALIADO){
                    this.entidad = entidad;
                    eventodeteccion.Invoke();
                }
                   

                break;
            case DeteccionObjetivo.ENEMIGO:

                if (entidad.GetTipo() == EntidadTipo.ENEMIGO){
                    this.entidad = entidad;
                    eventodeteccion.Invoke();
                }


                break;
            case DeteccionObjetivo.AMBOS:

                if (entidad.GetTipo() != EntidadTipo.DESCONOCIDO){
                    this.entidad = entidad;
                    eventodeteccion.Invoke();
                }

                break;
            case DeteccionObjetivo.TODOS:

                this.entidad = entidad;
                eventodeteccion.Invoke();

                break;
        }

    }    


    public void EventoMatarEntidad(){
        if (entidad == null)
            return;
        entidad.Muerte();
    }
    public void EventoDestruirEntidad(){
        if (entidad == null)
            return;
        entidad.Destruir();
    }
    public void EventoActivarTrigger(string trigger){
        if (entidad == null)
            return;
        if(entidad.GetAnimador()!=null)
            entidad.GetAnimador().ActivarTrigger(trigger);
    }

}

