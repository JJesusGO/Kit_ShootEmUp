using UnityEngine;
using System.Collections;

namespace SEUP{

    public abstract class Enemigo : Entidad{

        protected override void Awake(){
            base.Awake();
            SetTipo(EntidadTipo.ENEMIGO);
        }

    }

}

