using UnityEngine;
using System.Collections;

namespace SEUP{

    public enum MovimientoTipo{
        BASICO, SEGUIMIENTO
    }

    [System.Serializable]
    public class ModuloMovimiento : EntidadModulo{

        [Header("General")]
        [SerializeField]
        private MovimientoTipo tipomovimiento = MovimientoTipo.BASICO;
        [SerializeField]
        private bool relativo = true;
        [SerializeField]
        private float velocidad = 0;
        [Header("Movimiento - Basico")]
        [SerializeField]
        private Vector3 direccionbasica = Vector3.forward;
        [Header("Movimiento - Seguimiento")]       
        [SerializeField]
        private float  ajustevelocidad      = 1;
        [SerializeField]
        private Vector3 posiciondeseada    = Vector3.zero;

        private ManagerGameplay gameplay = null;
        private float distanciaactivacion = 1.0f; 

        public override void Start(){
            gameplay = ManagerGameplay.GetInstanciaBase();

            SetVelocidad(velocidad);
            SetDireccionBasica(direccionbasica);
        }
        public void ActualizarVelocidad(){
            if (!IsEnable())
                return;


            Rigidbody r = GetEntidad().GetRigidbody();
            if (r == null)
                return;

            if (!gameplay.IsEstado(GameplayEstado.JUGANDO))
                r.velocity = Vector3.zero;
            else {

                Vector3 vbase = Vector3.zero;
                if (relativo && gameplay != null)
                    vbase = gameplay.GetVelocidad() * Vector3.forward;

                switch (tipomovimiento)
                {
                    case MovimientoTipo.BASICO:                                        
                        r.velocity = (velocidad * direccionbasica) - vbase;
                        break;
                    case MovimientoTipo.SEGUIMIENTO:

                        float distancia = (posiciondeseada - r.position).magnitude; 
                        Vector3 direccionajuste = (posiciondeseada - r.position).normalized; 

                        float k = 1.0f;
                        if (distancia < distanciaactivacion)
                            k = distancia / distanciaactivacion * ajustevelocidad;

                        r.velocity = ((k * velocidad) * direccionajuste) - vbase;

                        break;
                }
   
            }
        
        }
        public override void Update(){
            base.Update();
            ActualizarVelocidad();
        }
     
        public void SetVelocidad(float velocidad){
            this.velocidad = Mathf.Abs(velocidad);
            ActualizarVelocidad();
        }
        public void SetDireccionBasica(Vector3 direccion){
            direccionbasica = direccion;
            direccionbasica = direccionbasica / direccionbasica.magnitude;
        }
        public void SetPosicionDeseada(Vector3 posicion){
            posiciondeseada = posicion;
        }

    }

}

