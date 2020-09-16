using UnityEngine;
using System.Collections;

namespace SEUP{

    public abstract class Prop : Entidad{

        protected override void Awake(){
            base.Awake();
            SetTipo(EntidadTipo.DESCONOCIDO);
        }

    }

}
