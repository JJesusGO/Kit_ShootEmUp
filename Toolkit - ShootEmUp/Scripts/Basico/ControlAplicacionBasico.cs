using UnityEngine;
using System.Collections;

namespace SEUP{

    public class ControlAplicacionBasico : ManagerAplicacion{
    
        private static ControlAplicacionBasico instancia = null;    

        protected override void Awake(){
            base.Awake();
            instancia = this;
        }
            
        public void EventoLog(string mensaje){
            Debug.Log(mensaje);
        }

        public static ControlAplicacionBasico GetInstancia(){
            if (instancia == null)
                instancia = GameObject.FindObjectOfType<ControlAplicacionBasico>();
            return instancia;
        }

    }

}
