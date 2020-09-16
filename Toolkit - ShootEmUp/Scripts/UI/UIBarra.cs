using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace SEUP{

    public enum UIBarraTipo{
        NIVEL, SECCIONES
    }

    [System.Serializable]
    public struct UIBarraNivel{
        [SerializeField]
        private Image barra;
        [SerializeField]
        private float velocidad;

        private UIBarraNivel(Image barra,float velocidad){
            this.barra = barra;
            this.velocidad = velocidad;
        }

        public Image GetBarra(){
            return barra;
        }
        public float GetVelocidad(){
            return velocidad;
        }
    }
    [System.Serializable]
    public class UIBarraSecciones{
        [SerializeField]
        private int secciones = 3;
        [SerializeField]
        private Transform contenedor = null;
        [SerializeField]
        private Sprite []sprites = null;
        [SerializeField]
        private float velocidad = 0.1f;

        private List<Image> imagenes = new List<Image>();
        private float valor = 0.0f;

        private void ActualizarSlots(){
        
            if (imagenes.Count != secciones){
            
                for (int i = 0; i < imagenes.Count; i++)
                    GameObject.Destroy(imagenes[i].gameObject);
                imagenes.Clear();

                for (int i = 0; i < secciones; i++){
                    GameObject obj = new GameObject(string.Format("Seccion{0}", i),typeof(Image));        
                    obj.transform.SetParent(contenedor);
                    obj.transform.localScale = Vector3.one;
                    obj.transform.localPosition = Vector3.zero;
                    imagenes.Add(obj.GetComponent<Image>());
                }

            }
        
        }
        public  void SetValor(float valor){
            ActualizarSlots();
            if (this.valor == valor)
                return;
            this.valor = Mathf.Clamp(valor, 0, 1);

            float valorseccion = 1.0f / secciones, 
                  valorsprite = valorseccion/(sprites.Length-1);
            float sumatoria = 0.0f;


            for (int i = 0; i < imagenes.Count; i++){
                if (valor <= sumatoria)
                    imagenes[i].sprite = sprites[0];
                else if(valor >= (sumatoria+valorseccion)){
                    imagenes[i].sprite = sprites[sprites.Length-1];
                }
                else{
                    float offset = valorsprite;
                    for (int j = 1; j < sprites.Length; j++){
                        if (valor <= (sumatoria + offset)){
                            imagenes[i].sprite = sprites[j];
                            break;
                        }
                         offset += valorsprite;
                    }                                              
                }
                sumatoria += valorseccion;
            }
                

        }

        public Transform GetContenedor(){
            return contenedor;
        }

        public Sprite GetSprites(int i){
            return sprites[i];
        }
        public int GetIconosCount(){
            return sprites.Length;
        }

        public int   GetSecciones(){
            return secciones;
        }
        public float GetValor(){
            return valor;
        }
        public float GetVelocidad(){
            return velocidad;
        }

    }

    public class UIBarra : MonoBehaviour{

        [Header("General")]
        [SerializeField]
        private UIBarraTipo tipo = UIBarraTipo.NIVEL;
        [Header("Nivel")]
        [SerializeField]
        private UIBarraNivel []uibarras = null;
        [Header("Secciones")]
        [SerializeField]
        private UIBarraSecciones []uisecciones = null;

        private float valor = 0;


        public void  SetValor(float valor){
            if (this.valor == valor)
                return;
            this.valor = Mathf.Clamp(valor,0,1);
        }
        public void  SetUIValor(float valor){
            SetValor(valor);
            Actualizar(true);
        }

        private void Update(){
            Actualizar();
        }

        public float GetValor(){
            return valor;
        }

        public void Actualizar(bool forzar = false){
         
            switch (tipo){
                case UIBarraTipo.NIVEL:

                    for (int i = 0; i < uibarras.Length; i++){
                        if (uibarras[i].GetBarra().fillAmount != valor){
                            if (forzar)
                                uibarras[i].GetBarra().fillAmount = valor;
                            else
                                uibarras[i].GetBarra().fillAmount = Mathf.MoveTowards(uibarras[i].GetBarra().fillAmount, valor, uibarras[i].GetVelocidad() * Time.deltaTime);
                        }
                    }
                        
                    break;
                case UIBarraTipo.SECCIONES:


                    for (int i = 0; i < uisecciones.Length; i++){
                        if (uisecciones[i].GetValor() != valor){
                            if (forzar)
                                uisecciones[i].SetValor(valor);
                            else
                                uisecciones[i].SetValor(Mathf.MoveTowards(uisecciones[i].GetValor(), valor, uisecciones[i].GetVelocidad() * Time.deltaTime));
                        }
                    }

                    break;
            }

        }

    }


}