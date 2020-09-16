using UnityEngine;
using System.Collections;


namespace SEUP{

    [System.Serializable]
    public class ModuloNavegacionBasico : EntidadModulo{

        [Header("General")]
        [SerializeField]
        private Vector2 posicion = Vector2.zero; 
        [Header("Control")]
        [SerializeField]
        private Eje horizontal = new Eje();
        [SerializeField]
        private Eje vertical = new Eje(); 

        private Mapa mapa = null;
        private ManagerGameplay gameplay = null;

        public override void Start(){
            base.Start();

            mapa     = Mapa.GetInstancia();
            gameplay = ManagerGameplay.GetInstanciaBase();


            SetPosicionX((int)posicion.x);
            SetPosicionY((int)posicion.y);
        }

        public override void Update(){
            base.Update();

            if (!IsEnable())
                return;                
            if (gameplay.IsEstado(GameplayEstado.JUGANDO)){
                if (horizontal.IsClickDown())
                    SetPosicionX((int)posicion.x + horizontal.GetValor());          
                if (vertical.IsClickDown())
                    SetPosicionY((int)posicion.y + vertical.GetValor());            
            }
        }

        public void SetPosicionX(int x){
            if (mapa == null)
                return;
            int n = (int)mapa.GetSecciones().x;
            posicion.x = Mathf.Clamp(x, 0, n - 1);
            ActualizarPosicion();
        }
        public void SetPosicionY(int y){
            if (mapa == null)
                return;
            int n = (int)mapa.GetSecciones().y;
            posicion.y = Mathf.Clamp(y, 0, n - 1);
            ActualizarPosicion();
        }

        protected void ActualizarPosicion(){
            if (mapa == null || !IsEnable())
                return;            
            GetEntidad().GetModuloMovimiento().SetPosicionDeseada(mapa.GetCeldaPosicion((int)posicion.x,(int)posicion.y));
        }

    }


}