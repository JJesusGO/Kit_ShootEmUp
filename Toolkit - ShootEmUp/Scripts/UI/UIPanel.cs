using UnityEngine;
using System.Collections;

namespace SEUP{

    [RequireComponent(typeof(CanvasGroup))]
    [ExecuteInEditMode]
    public class UIPanel : MonoBehaviour{

        [SerializeField]
        private bool enable = true;

        private CanvasGroup grupo = null;

        private void Awake(){
            grupo = GetComponent<CanvasGroup>();
        }

        public void Update(){
        
            if (enable != (GetCanvasGroup().alpha >= 1.0f)){            
                GetCanvasGroup().alpha = (enable) ? 1.0f : 0.0f;
                GetCanvasGroup().blocksRaycasts = enable;
            }
                

        }

        public void SetEnable(bool enable,bool forzar = false){
                
            if (this.enable == enable && !forzar)
                return;
            this.enable = enable;

        }

        public CanvasGroup GetCanvasGroup(){
            #if UNITY_EDITOR
            if(grupo == null && !Application.isPlaying)
                grupo = GetComponent<CanvasGroup>();
            #endif
            return grupo;
        }

        public bool IsEnable(){
            return enable;
        }
            
        public void EventoSetEnable(bool enable){
            this.enable = enable;
        }

    }

}
