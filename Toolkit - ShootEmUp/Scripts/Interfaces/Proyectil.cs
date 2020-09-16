using UnityEngine;
using System.Collections;

namespace SEUP{

    public abstract class Proyectil : Entidad{

        [Header("Proyectil")]
        [SerializeField]
        private EntidadTipo tipoproyectil = EntidadTipo.DESCONOCIDO;
        [SerializeField]
        protected ModuloAtaque ataque = new ModuloAtaque(); 

        protected override void Awake(){
            base.Awake();
            SetTipo(tipoproyectil);
            AddModulo(ataque);
        }
     
        public abstract void Disparar(Vector3 direccion);

        public void AddAtaqueEvento(AtaqueEvento evento){
            ataque.AddAtaqueEvento(evento);
        }
        public void RemoveAtaqueEvento(AtaqueEvento evento){
            ataque.RemoveAtaqueEvento(evento);
        }

        public override ModuloAtaque GetModuloAtaque(){
            return ataque;
        }

    }

}

