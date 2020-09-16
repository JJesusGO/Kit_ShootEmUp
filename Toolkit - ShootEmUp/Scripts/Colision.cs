using UnityEngine;
using System;
using System.Collections;


namespace SEUP{

    public enum ColisionTipo{
        FISICA,TRIGGER
    }
    public enum ColisionEstado {
        ENTER,EXIT
    }
    public delegate void ColisionEvento(ColisionInformacion informacion);

    public struct ColisionInformacion{

        private Entidad         entidad  ;
        private GameObject      objeto  ;
        private ColisionEstado estado;
        private ColisionTipo    tipo;
        private Colision        colision1,
                                colision2;  

        public ColisionInformacion(GameObject objeto,Entidad entidad,Colision colision1,Colision colision2, ColisionTipo tipo, ColisionEstado estado){

            this.entidad = entidad;
            this.colision1 = colision1;
            this.colision2 = colision2;
            this.objeto = objeto;
            this.estado = estado;
            this.tipo   = tipo;

        }

        public Colision       GetColision(){
            return colision1;
        }

        public Entidad        GetEntidadImpacto() {
            return entidad;
        }
        public GameObject     GetObjetoImpacto() {
            return objeto;
        }
        public Colision       GetColisionImpacto() {
            return colision2;
        }
       
        public ColisionEstado GetColisionEstado() {
            return estado;
        }
        public ColisionTipo   GetColisionTipo(){
            return tipo;
        }


        public bool IsColisionTipo(ColisionTipo tipo){
            return GetColisionTipo() == tipo;
        }
        public bool IsColisionEstado(ColisionEstado estado){
            return GetColisionEstado() == estado;
        }

    }
    [RequireComponent(typeof(Collider))]
    public class Colision : MonoBehaviour{

        [SerializeField]
        private string nombre = "Desconocido";

        private event ColisionEvento colisionevento = null;
        private Entidad                entidadpadre = null;
        private ColisionTipo         colisiontipo   = ColisionTipo.TRIGGER;
        private Collider                  colision  = null;

        private void Awake(){
            colision = GetComponent<Collider>();
            colisiontipo = (colision.isTrigger)?ColisionTipo.TRIGGER:ColisionTipo.FISICA;
            CalcularColision();
        }

        private void OnTriggerEnter(Collider otro) { 
        
            Rigidbody rigidbody = otro.attachedRigidbody;

            GameObject objeto   = null;
            Entidad    entidad  = null;
            Colision   colision = null;                      

            if (rigidbody != null){
                objeto   = rigidbody.gameObject;
                entidad  = objeto.GetComponent<Entidad>();
                colision = otro.GetComponent<Colision>();
            }
            else{
                objeto = otro.gameObject;
                entidad = objeto.GetComponent<Entidad>();
                colision = otro.gameObject.GetComponent<Colision>();
            }
            ActivarEvento(new ColisionInformacion(objeto, entidad, this,colision, ColisionTipo.TRIGGER, ColisionEstado.ENTER));

        }
        private void OnTriggerExit(Collider otro) { 
            
            Rigidbody rigidbody = otro.attachedRigidbody;

            GameObject objeto   = null;
            Entidad    entidad  = null;
            Colision   colision = null;                      

            if (rigidbody != null){
                objeto   = rigidbody.gameObject;
                entidad  = objeto.GetComponent<Entidad>();
                colision = otro.GetComponent<Colision>();
            }
            else{
                objeto   = otro.gameObject;
                entidad  = objeto.GetComponent<Entidad>();
                colision = otro.gameObject.GetComponent<Colision>();
            }
            ActivarEvento(new ColisionInformacion(objeto, entidad, this,colision, ColisionTipo.TRIGGER, ColisionEstado.EXIT));

        }
        private void OnCollisionEnter(Collision otro) { 
        
            Rigidbody rigidbody = otro.collider.attachedRigidbody;

            GameObject objeto   = null;
            Entidad    entidad  = null;
            Colision   colision = null;                      

            if (rigidbody != null){
                objeto   = rigidbody.gameObject;
                entidad  = objeto.GetComponent<Entidad>();
                colision = otro.collider.GetComponent<Colision>();
            }
            else{
                objeto   = otro.gameObject;
                entidad  = objeto.GetComponent<Entidad>();
                colision = otro.collider.GetComponent<Colision>();
            }
            ActivarEvento(new ColisionInformacion(objeto, entidad, this,colision, ColisionTipo.FISICA, ColisionEstado.ENTER));

        }
        private void OnCollisionExit(Collision otro) { 
        
            Rigidbody rigidbody = otro.collider.attachedRigidbody;

            GameObject objeto   = null;
            Entidad    entidad  = null;
            Colision   colision = null;                      

            if (rigidbody != null){
                objeto   = rigidbody.gameObject;
                entidad  = objeto.GetComponent<Entidad>();
                colision = otro.collider.GetComponent<Colision>();
            }
            else{
                objeto   = otro.gameObject;
                entidad  = objeto.GetComponent<Entidad>();
                colision = otro.collider.GetComponent<Colision>();
            }
            ActivarEvento(new ColisionInformacion(objeto, entidad, this,colision, ColisionTipo.FISICA, ColisionEstado.EXIT));

        }

        private void ActivarEvento(ColisionInformacion informacion){
            if (colisionevento != null)
                colisionevento(informacion);
        }

        private void CalcularColision(){
            Transform padre = transform.parent;
            while (padre != null){
                entidadpadre = padre.gameObject.GetComponent<Entidad>();
                if (entidadpadre != null)
                    break;
                padre = padre.transform.parent;
            }
        }
            
        public void AddColisionEvento(ColisionEvento evento) {
            RemoveColisionEvento(evento);
            colisionevento += (evento);
        }
        public void RemoveColisionEvento(ColisionEvento evento){
            try{              
                colisionevento -= (evento);
            }
            catch (Exception e) {
                Debug.LogError(e.Message);
            }
        }

        public Entidad              GetEntidad() {
            return entidadpadre;
        }
        public ColisionTipo         GetColisionTipo(){
            return colisiontipo;
        }
        public string               GetNombre() {
            return nombre;
        }

    }

}