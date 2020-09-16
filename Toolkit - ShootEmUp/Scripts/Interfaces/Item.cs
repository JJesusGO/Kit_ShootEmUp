using UnityEngine;
using System.Collections;

namespace SEUP{

    public abstract class Item : Entidad{

        [Header("Item")]
        [SerializeField]
        private ModuloDeteccion deteccion =new ModuloDeteccion();

        protected override void Awake(){
            base.Awake();
            SetTipo(EntidadTipo.DESCONOCIDO);

            AddModulo(deteccion);
            deteccion.AddDeteccionEvento(Deteccion);
        }
             
        protected abstract void Deteccion(DeteccionInformacion info);

        public override ModuloDeteccion GetModuloDeteccion(){
            return deteccion;
        }
    }

}

