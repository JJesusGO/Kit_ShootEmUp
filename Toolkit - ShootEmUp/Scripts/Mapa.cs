using UnityEngine;
using System.Collections;

namespace SEUP{

    [ExecuteInEditMode]
    [RequireComponent(typeof(BoxCollider))]
    public class Mapa : MonoBehaviour{

        [Header("Area")]
        [SerializeField]
        private Vector3 offset    = Vector3.zero;
        [SerializeField]
        private Vector2 tamaño    = Vector2.one * 15.0f;
        [SerializeField]
        private Vector2 secciones = Vector2.one * 3.0f;
        [Header("Debug")]
        [SerializeField]
        private bool mostrarguias = true;
        [SerializeField]
        private Color color = Color.yellow;

        private static Mapa instancia = null;

        private BoxCollider mapa = null;
        private Vector3 posicion = Vector3.zero;

        private void Awake(){            
            instancia = null;
            mapa     = GetComponent<BoxCollider>();
            posicion = transform.position + offset;
        }
            
        public Vector3 GetCeldaPosicion(int x,int y){
            Vector2 tamañoindividual      = GetCeldaTamaño();
            Vector3 posicionbase = GetPosicion() + new Vector3((tamañoindividual.x-tamaño.x)/ 2.0f, (tamañoindividual.y-tamaño.y) / 2.0f, 0);
           
            return posicionbase + new Vector3(x * tamañoindividual.x, y * tamañoindividual.y, 0);
        }
        public Vector2 GetCeldaTamaño(){
            Vector2 sizeindividual = new Vector2(tamaño.x / secciones.x, tamaño.y / secciones.y);
            return sizeindividual;
        }
      
        public Vector2 GetTamaño(){
            return tamaño;
        }
        public Vector3 GetPosicion(){
            return transform.position + offset;
        }
        public Vector2 GetSecciones(){
            return secciones;
        }

        public bool IsMapa(Entidad entidad){            
            return mapa.bounds.Contains(entidad.transform.position);
        }


        public static Mapa GetInstancia(){
            if (instancia == null)
                instancia = GameObject.FindObjectOfType<Mapa>();
            return instancia;
        }
            
        #if UNITY_EDITOR
            void OnDrawGizmosSelected(){
                if (mostrarguias)
                    return;
                Gizmos.color = color;
                Vector2 sizeindividual = GetCeldaTamaño();
               
                for (int x = 0; x < (int)secciones.x; x++)
                    for (int y = 0; y < (int)secciones.y; y++)
                        Gizmos.DrawWireCube(GetCeldaPosicion(x,y),sizeindividual);             
                
            }
            void OnDrawGizmos(){
                if (!mostrarguias)
                    return;

                Gizmos.color = color;
                Vector2 sizeindividual = GetCeldaTamaño();

                for (int x = 0; x < (int)secciones.x; x++)
                    for (int y = 0; y < (int)secciones.y; y++)
                        Gizmos.DrawWireCube(GetCeldaPosicion(x,y),sizeindividual);             

            }
        #endif
    }

}
