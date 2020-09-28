using UnityEngine;
using System.Collections;
using TMPro;

namespace SEUP{

    [RequireComponent(typeof(TextMeshProUGUI))]
    public class UIDato : MonoBehaviour{

        [SerializeField]
        private string referencia = "";
        [SerializeField]
        private string uiformato = "{0}";

        private TextMeshProUGUI uitexto = null;
        private Guardado guardado = null; 
        private Data data;

        private string valor = "";

        private void Awake(){
            uitexto = GetComponent<TextMeshProUGUI>();        
        }

        private void Start(){
            guardado = Guardado.GetInstancia();
            if(guardado!=null)
                data = guardado.GetData(referencia);
        }

        private void Update(){
            if(guardado!=null)
                data = guardado.GetData(referencia);            
            if (data.referencia != referencia)
                return;
            if (valor != data.valor){
                uitexto.text = string.Format(uiformato, data.valor);
                valor = data.valor;
            }
        }
      
    }

}

