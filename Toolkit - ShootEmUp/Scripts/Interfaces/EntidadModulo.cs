using UnityEngine;
using System.Collections;

namespace SEUP{

    public abstract class EntidadModulo{

        [SerializeField]
        private bool enable = false;

        private Entidad entidad = null;
       
        public virtual void Start(){

        }
        public virtual void Update(){
            
        }

        public void SetEntidad(Entidad entidad){
            this.entidad = entidad;
        }
        public void SetEnable(bool enable){
            this.enable = enable;
        }

        public Entidad GetEntidad(){
            return entidad;
        }
        public bool IsEnable(){
            return enable;
        }

    }

}
