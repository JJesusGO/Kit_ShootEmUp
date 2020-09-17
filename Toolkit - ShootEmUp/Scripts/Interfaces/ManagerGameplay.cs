using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace SEUP{

    public enum GameplayEstado{
        INICIO, JUGANDO,PERDER, GANAR
    }

    public abstract class ManagerGameplay : MonoBehaviour{

        [Header("General")]
        [SerializeField]
        private float velocidad = 25.0f;
        [SerializeField]
        private AnimationCurve velocidaddificultad = new AnimationCurve(new Keyframe(0,1),new Keyframe(1,1));

        private float dificultad = 1.0f;

        private static ManagerGameplay instanciabase = null;
        private GameplayEstado estado = GameplayEstado.INICIO;

        protected virtual void Awake(){
            instanciabase = this;        
        }

        protected virtual void Start(){
            SetEstado(GameplayEstado.INICIO, true);
        }

        protected abstract void Inicio();
        protected abstract void Jugando();
        protected abstract void Perder();
        protected abstract void Ganar();

        public void ReiniciarNivel(){
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void SetEstado(GameplayEstado estado,bool forzar = false){
            if (this.estado == estado && !forzar)
                return;
            this.estado = estado;
            switch (this.estado){
                case GameplayEstado.INICIO:
                    Inicio();
                    break;
                case GameplayEstado.JUGANDO:
                    Jugando();
                    break;
                case GameplayEstado.GANAR:
                    Ganar();
                    break;
                case GameplayEstado.PERDER:
                    Perder();
                    break;
            }
        }

        public void SetVelocidadBase(float velocidad){            
            this.velocidad = Mathf.Abs(velocidad);
        }
        public void SetDificultad(float dificultad){            
            this.dificultad = Mathf.Clamp(dificultad,0,1);
        }

        public float GetVelocidad(){
            return GetVelocidadBase() * velocidaddificultad.Evaluate(dificultad);
        }
        public float GetVelocidadBase(){
            return Mathf.Abs(velocidad);
        }

        public float GetDificultad(){
            return dificultad;
        }

        public bool IsEstado(GameplayEstado estado){
            return this.estado == estado;
        }

        public static ManagerGameplay GetInstanciaBase(){
            if (instanciabase == null)
                instanciabase = GameObject.FindObjectOfType<ManagerGameplay>();
            return instanciabase;
        }
            

    }


}
