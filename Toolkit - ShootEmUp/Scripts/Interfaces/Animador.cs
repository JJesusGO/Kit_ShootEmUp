using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace SEUP{

    [RequireComponent(typeof(Animator))]
    public class Animador : MonoBehaviour {

        [SerializeField]
        private UnityEvent []eventos = null;

        public void     ActivarEvento(int i){
            if(eventos!=null)
            if(i<eventos.Length)
                eventos[i].Invoke();
        }
        public void     ActivarTrigger(string nombre){        
            GetAnimator().SetTrigger(nombre);        
        }

        public Animator GetAnimator(){
            return GetComponent<Animator> ();
        }


        public void Pause(){        
            GetAnimator().enabled = false;
        }
        public void Play(){
            GetAnimator().enabled = true;
        }

    }

}
