using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;


namespace SEUP{

    public enum EntidadTipo{
        DESCONOCIDO, ALIADO, ENEMIGO
    }
    [RequireComponent(typeof(Rigidbody))]
    public abstract class Entidad : MonoBehaviour{
           
        [Header("Entidad - General")]
        [SerializeField]
        private bool destruir = true;
        [SerializeField]
        private bool destruirautomatico = true;
        [SerializeField]
        private ModuloMovimiento movimiento = new ModuloMovimiento();
        [Header("Entidad - Eventos")]
        [SerializeField]
        protected UnityEvent eventovivir = new UnityEvent();
        [SerializeField]
        protected UnityEvent eventomorir = new UnityEvent();
        [SerializeField]
        protected UnityEvent eventodestruir = new UnityEvent();

        private List<EntidadModulo> modulos = new List<EntidadModulo>();

        private   EntidadTipo tipo      = EntidadTipo.DESCONOCIDO;
        protected Rigidbody   cuerporigido = null;
        protected Animador   animador = null;
        private   Mapa        mapa      = null;
        private   Entidad     entidadpadre = null;

        private   bool        muerto = false;

        public Entidad Create(Transform padre,Vector3 posicion,Entidad entidadpadre = null){            
            Entidad instancia = GameObject.Instantiate(gameObject,posicion,Quaternion.identity,padre).GetComponent<Entidad>();
            instancia.entidadpadre = entidadpadre;
            return instancia;
        }
        public void    Destruir(){
            if (!destruir)
                return;
            eventodestruir.Invoke();
            GameObject.Destroy(gameObject);
        }

        protected virtual void Awake(){
            cuerporigido = GetComponent<Rigidbody>();
            animador = GetComponentInChildren<Animador>();
            AddModulo(movimiento);
        }
        protected virtual void Start(){
            for (int i = 0; i < modulos.Count; i++)
                modulos[i].Start();            
            mapa = Mapa.GetInstancia();
            eventovivir.Invoke();
        }
        protected virtual void Update(){            

            for (int i = 0; i < modulos.Count; i++){                
                modulos[i].Update();
            }
            if (mapa != null && destruir)
                if (!mapa.IsMapa(this))
                Destruir();

        }     

        public abstract void Generacion(Mapa mapa,int x,int y);
        public virtual  void Muerte(){
            SetMuerto(true);
            eventomorir.Invoke();
            if(destruirautomatico)
                Destruir();
        }

        protected void AddModulo(EntidadModulo modulo){
            if (!modulos.Contains(modulo)){
                modulo.SetEntidad(this);       
                modulos.Add(modulo);
            }
        }

        protected void     SetMuerto(bool muerto){
            this.muerto = muerto;
        }
        protected void     SetTipo(EntidadTipo tipo){
            this.tipo = tipo;
        }
        public    void     SetPosicion(Vector3 posicion){
            if (GetRigidbody() != null)
                GetRigidbody().position = posicion;
            else
                transform.position = posicion;
        }
        public    void     SetPosicion(Vector2 posicion){
            if (GetRigidbody() != null)
                GetRigidbody().position = new Vector3(posicion.x,posicion.y,GetRigidbody().position.z);
            else
                transform.position = new Vector3(posicion.x,posicion.y,transform.position.z);;
        }
            
        public EntidadTipo      GetTipo(){
            return tipo;
        }
        public Entidad          GetEntidadPadre(){
            return entidadpadre;
        }

        public Rigidbody        GetRigidbody(){
            return cuerporigido;
        }
        public Animador        GetAnimador(){
            return animador;
        }
        public Vector3          GetPosicion(){
            return transform.position;
        }
        public ModuloMovimiento GetModuloMovimiento(){
            return movimiento;
        }            

        public virtual ModuloNavegacionBasico GetModuloNavegacionBasico(){
            return null;
        }
        public virtual ModuloDeteccion        GetModuloDeteccion(){
            return null;
        }

        public virtual ModuloAtaque           GetModuloAtaque(){
            return null;
        }
        public virtual ModuloVitalidad        GetModuloVitalidad(){
            return null;
        }

        public bool IsMuerto(){
            return muerto;
        }

        public void EventoMuerte(){
            Muerte();
        }
        public void EventoDestruir(){
            Destruir();
        }


    }
        
}
