using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace SEUP{

    [System.Serializable]
    public class EntidadGeneracion{
        
        [SerializeField]
        string nombre = "Desconocido";
        [SerializeField]
        private Entidad entidad = null;
        [SerializeField]
        private float   distanciaminima = 0;
        [SerializeField]
        private AnimationCurve probabilidad = new AnimationCurve(new Keyframe(0,1),new Keyframe(1,1));
        [SerializeField]
        private AnimationCurve enfriamiento = new AnimationCurve(new Keyframe(0,1),new Keyframe(1,1));

        private Temporizador temporizador;

        public void Start(float dificultad){
            temporizador = new Temporizador(enfriamiento.Evaluate(dificultad));
        }
        public void Update(){
            temporizador.Update();           
        }    

        public void StartTemporizador(float dificultad){
            temporizador.Start();
            temporizador.SetTiempoTarget(enfriamiento.Evaluate(dificultad));
        }

        public Entidad GetEntidad(){
            return entidad;
        }
        public float   GetProbabilidad(float t){
            return probabilidad.Evaluate(t);
        }
        public float   GetEnfriamiento(float t){
            return probabilidad.Evaluate(t);
        }

        public float   GetDistanciaMinima(){
            return distanciaminima;
        }          
        public string GetNombre(){
            return nombre;
        }

        public bool IsNombre(string nombre){
            return this.nombre == nombre;
        }

        public bool IsActivo(){
            return temporizador.IsActivo();
        }
        public bool IsEntidad(Entidad entidad){
            return this.entidad == entidad;
        }
    }

    [ExecuteInEditMode]
    public class MapaGenerador : MonoBehaviour{
        
        [Header("General")]
        [SerializeField]
        private bool enable = true;
        [Header("Generacion - General")]
        [SerializeField]
        private List<EntidadGeneracion> generacion = new List<EntidadGeneracion>();
        [SerializeField]
        private Transform carpeta = null;
        [SerializeField]
        private Entidad   entidadpadre = null;
        [Header("Generacion - Configuracion")]
        [SerializeField]
        private Entidad entidadreferencia = null;
        [SerializeField]
        private float distanciaminima = 10;
        [SerializeField]
        private float distanciamaxima = 20;
        [Header("Debug")]
        [SerializeField]
        private bool mostrarguias = true;
        [SerializeField]
        private Color color = Color.red;

        private Mapa mapa = null;
        private ManagerGameplay game = null;
        private List<Entidad> entidades    = new List<Entidad>();
        private Probabilidad probabilidades = new Probabilidad();

        private float distancia = 0;

        private void Start(){
            mapa = Mapa.GetInstancia();
            game = ManagerGameplay.GetInstanciaBase();

            for (int i = 0; i < generacion.Count; i++)
                generacion[i].Start(game.GetDificultad());

        }
        private void Update(){    
            #if UNITY_EDITOR
            if(!Application.isPlaying)
                return;
            #endif
            if (!enable)
                return;

            if (game.IsEstado(GameplayEstado.JUGANDO)) {
                for (int i = 0; i < generacion.Count; i++)
                    generacion[i].Update();

                if (entidadreferencia == null)
                    Generar();
                else
                {
                    if (Mathf.Abs(entidadreferencia.GetPosicion().z - GetPosicion().z) >= distancia)
                        Generar();
                }
            }
        }
            
        public  void Generar(Entidad entidad,Entidad entidadpadre = null){
            if (entidad == null)
                return;
            entidadreferencia = entidad.Create(carpeta,GetPosicion(),entidadpadre);                                           
            entidadreferencia.Generacion(mapa,
                Random.Range(0,(int)mapa.GetSecciones().x),
                Random.Range(0,(int)mapa.GetSecciones().y));
            distancia = Random.Range(distanciaminima, distanciamaxima);
        }
        private void Generar(){        
            if (!enable)
                return;

            if (entidadreferencia == null)
                distancia = distanciamaxima;
            else
                distancia = Mathf.Abs(entidadreferencia.GetPosicion().z - GetPosicion().z);           

            float dificultad = 0;
            if (game != null)
                dificultad = game.GetDificultad();

            entidades.Clear();
            probabilidades.Clear();

            for (int i = 0; i < generacion.Count; i++)
                if (generacion[i].GetDistanciaMinima() < distancia && 
                    generacion[i].IsActivo() &&
                    generacion[i].GetProbabilidad(dificultad) > 0.0f)
                {                    
                    entidades.Add(generacion[i].GetEntidad());
                    probabilidades.AddProbabilidad(generacion[i].GetProbabilidad(dificultad));
                }

            if (probabilidades.GetProbabilidadCount() == 0)
                return;

            int n = probabilidades.NextProbabilidad();


            Generar(entidades[n],entidadpadre);
            for (int i = 0; i < generacion.Count; i++)
                if (generacion[i].IsEntidad(entidades[n])){
                    if(game!=null)
                        generacion[i].StartTemporizador(game.GetDificultad());
                    else 
                        generacion[i].StartTemporizador(0);
                    break;
                }
                    
           

        }
            
        public  void Reiniciar(){
            distancia = 0;
            entidadreferencia = null;
        }

        public Vector3 GetPosicion(){
            if (mapa == null)
                return transform.position;            
            return new Vector3(mapa.GetPosicion().x, mapa.GetPosicion().y, transform.position.z);
        }

        #if UNITY_EDITOR

            void OnDrawGizmosSelected(){
                if (mostrarguias)
                    return;

                if(mapa!=null)
                mapa = Mapa.GetInstancia();
                if (mapa == null)
                    return;

                Gizmos.color = color;
                Gizmos.DrawWireCube(GetPosicion(),mapa.GetTamaño());             

            }
            void OnDrawGizmos(){
                if (!mostrarguias)
                    return;

                if(mapa!=null)
                    mapa = Mapa.GetInstancia();
                if (mapa == null)
                    return;

                Gizmos.color = color;
                Gizmos.DrawWireCube(GetPosicion(),mapa.GetTamaño());        

            }

        #endif
    }

}