using UnityEngine;
using System.Collections;


namespace SEUP{

    [System.Serializable]
    public struct AudioInformacion{
        [SerializeField]
        public AudioClip clip;
        [SerializeField]
        public string perfil;
    }

    [RequireComponent(typeof(AudioSource))]
    public class Audio : MonoBehaviour{

        private AudioSource fuente = null; 

        private bool play = false;
        private bool pause = false;

        public Audio Create(AudioClip clip,Transform carpeta,Vector3 posicion){
            Audio audio = Instantiate(gameObject,posicion,Quaternion.identity,carpeta).GetComponent<Audio>();
            audio.SetAudio(clip);
            return audio;
        }

        private void Awake(){
            fuente = GetComponent<AudioSource>();
            fuente.playOnAwake = true;
        }
        private void Start(){
            play = true;
        }
            
        private void Update(){
            if (play && !GetAudioSource().isPlaying && !pause && !GetAudioSource().loop)     
                Stop();
        }

        private void OnApplicationFocus(bool hasFocus){
            pause = !hasFocus;
        }
        private void OnApplicationPause(bool pauseStatus){
            pause = pauseStatus;
        }            

        public void Stop(){
            GameObject.Destroy(gameObject);
        }
        public void SetAudio(AudioClip clip){

            GetAudioSource().clip = clip;
            GetAudioSource().Stop();
            GetAudioSource().Play();

        }

        public AudioSource GetAudioSource(){
            if (fuente == null)
                fuente = GetComponent<AudioSource>();
            return fuente;
        }
 
}

}

