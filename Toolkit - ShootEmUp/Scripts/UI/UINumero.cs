using UnityEngine;
using System.Collections;
using TMPro;

namespace SEUP{

    public enum UINumeroTipo{
        FLOAT,INT
    }

    [ExecuteInEditMode]
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UINumero : MonoBehaviour{


        [SerializeField]
        private UINumeroTipo tipo = UINumeroTipo.FLOAT;
        [SerializeField]
        private float uinumero = 0;
        [SerializeField]
        private string uiformato = "{0:0.00}";
        [Tooltip("Si la velocidad es menor o igual a cero, se actualiza inmediatamente")]
        [SerializeField]
        private float  velocidad = 0.0f;
       
        private float numero = 0;
        private TextMeshProUGUI uitexto = null;

        private void Awake(){
            uitexto = GetComponent<TextMeshProUGUI>();
            SetUINumero(uinumero);
        }
        private void Update(){

            #if UNITY_EDITOR
                
            if(!Application.isPlaying){
                if(uitexto==null)
                    uitexto = GetComponent<TextMeshProUGUI>();               
                Actualizar();
            }
            else{
                
                if(numero!=uinumero){
                    if(velocidad > 0.0f)
                        uinumero = Mathf.MoveTowards(uinumero,numero,velocidad*Time.deltaTime);
                    else 
                        uinumero = numero;
                    Actualizar();
                }

            }

            #else

            if(numero!=uinumero){
                if(velocidad!=0.0f)
                    uinumero = Mathf.MoveTowards(uinumero,numero,velocidad);
                Actualizar();
            }

            #endif

        }

        public void Actualizar(){
            if (tipo == UINumeroTipo.INT)
                uitexto.text = string.Format(uiformato, (int)uinumero);
            else
                uitexto.text = string.Format(uiformato, uinumero);
        }
            
        public void SetNumero(float numero){
            if (this.numero == numero)
                return;
            this.numero = numero;
            if (velocidad <= 0){
                uinumero = numero;
                Actualizar();
            }
        }
        public void SetUINumero(float uinumero){
            if (this.uinumero == uinumero)
                return;
            this.uinumero = uinumero;
            SetNumero(this.uinumero);  
            Actualizar();
        }

        public TextMeshProUGUI GetTexto(){
            return uitexto;
        }
        private float GetUINumero(){
            return uinumero;
        }
        private float GetNumero(){
            return numero;
        }

    }

}