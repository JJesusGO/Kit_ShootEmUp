using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace SEUP{

    [System.Serializable]
    public struct AudioPerfil{
        [SerializeField]
        public Transform carpeta;
        [SerializeField]
        public string nombre;
        [SerializeField]
        public Audio  prefab;
    }
    [System.Serializable]
    public struct ClipPerfil{
        [SerializeField]
        public string nombre;
        [SerializeField]
        public AudioClip audio;
    }

    public abstract class ManagerAplicacion : MonoBehaviour{

        [Header("Data")]
        [SerializeField]
        private Guardado informacionprefab = null;
        [Header("Eventos")]
        [SerializeField]
        private UnityEvent eventoinicioaplicacion = null;
        [Header("Audio")]
        [SerializeField]
        private AudioPerfil []perfiles = null;
        [SerializeField]
        private ClipPerfil[] clips = null;

        public static ManagerAplicacion instanciabase;

        protected virtual void Awake(){

            instanciabase = this;
        
            if(informacionprefab != null)
            if (Guardado.GetInstancia() == null){
                
                Guardado info = Instantiate(informacionprefab.gameObject).GetComponent<Guardado>();
                info.Cargar();
                DontDestroyOnLoad(info.gameObject);

            }

        }
        protected virtual void Start(){
            ActivarInicioAplicacion();
        }

        private void ActivarInicioAplicacion(){
            if (eventoinicioaplicacion == null)
                return;
            eventoinicioaplicacion.Invoke();
        }   

        public void PlayAudio(string codigo)
        {

            string[] data = codigo.Split('_');

            if (data.Length < 2)
                return;

            string perfil = data[0],
            clip = data[1];


            AudioClip sonido = null;
            for (int i = 0; i < clips.Length; i++)
                if (clip == clips[i].nombre) {
                    sonido = clips[i].audio;
                    break;
                }            

            for (int i = 0; i < perfiles.Length; i++)
                if (perfil == perfiles[i].nombre)
                {
                    PlayAudio(i, sonido, transform.position);
                    break;
                }
        }

        public void PlayAudio(string perfil, AudioClip clip, Vector3 posicion)
        {
            for (int i = 0; i < perfiles.Length; i++)
                if (perfil == perfiles[i].nombre) { 
                    PlayAudio(i, clip, posicion);
                    break;
                }   
        }
        public void PlayAudio(int perfil,AudioClip clip, Vector3 posicion){
            Audio audio = perfiles[perfil].prefab;
            audio.Create(clip,perfiles[perfil].carpeta,posicion);
        }

        public static ManagerAplicacion GetInstanciaBase(){
            if (instanciabase == null)
                instanciabase = GameObject.FindObjectOfType<ManagerAplicacion>();
            return instanciabase;
        }

    }

}