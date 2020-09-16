using UnityEngine;
using System.Collections;

namespace SEUP{

    public class Temporizador{

        public const float INFINITO = -1;

        private float tiempo = 0.0f, target = 0.0f;
        private bool  enable = false, activo = true;

        public Temporizador(){
            activo = enable = true;
            tiempo = target = 0.0f;
        }
        public Temporizador(float tiempotarget){
            activo = enable = true;
            tiempo = target = tiempotarget;
        }

        public void Start(){
            activo = false;
            SetTiempo(0.0f);
        }
        public void Reset(){
            if (activo)
                return;
            SetTiempo(0.0f);
        }

        public void Update(){
            if (!enable || activo)
                return;
            ModTiempo(Time.deltaTime);
            activo = GetTiempo(true) >= 1.0f;
        }

        public void SetEnable(bool enable){
            this.enable = enable;
        }

        public void  ModTiempo(float tiempo,bool relativo = false){
            if (relativo && target >= 0)
                tiempo *= target;
            SetTiempo(this.tiempo + tiempo);
        }
        public void  SetTiempo(float tiempo,bool relativo = false){
            if (relativo && target >= 0)
                tiempo *= target;  
            if(target < 0){
                if (tiempo < 0)
                    this.tiempo = 0;
                this.tiempo = tiempo;
            }
            else 
                this.tiempo = Mathf.Clamp(tiempo, 0, target);
        }

        public void  SetTiempoTarget(float tiempo){
            target = tiempo;
            SetTiempo(this.tiempo);
        }

        public float GetTiempo(bool relativo = false){
            if (relativo){
                if (target == 0.0f)
                    return 1.0f;
                else if(target < 0.0f)
                    return 0.0f;
                return tiempo / target;
            }
            return tiempo;
        }
        public float GetTiempoTarget(){
            return target;
        }

        public bool IsActivo(){
            return activo;
        }
        public bool IsEnable(){
            return enable;
        }

    }

}