using UnityEngine;
using System;
using System.Collections;

namespace SEUP{

    public enum   VitalidadTipo{
        DAÑO,CARGAS
    }
    public enum   VitalidadEventoTipo{
        PREDAÑO,DAÑO
    }
    public struct VitalidadInformacion{

        private PerfilVitalidad perfil;
        private float daño;
        private VitalidadEventoTipo tipo;
        private Entidad entidadatacante;

        public VitalidadInformacion(PerfilVitalidad perfil,float ataque,Entidad entidadatacante,VitalidadEventoTipo tipo){
            this.perfil = perfil;
            this.daño = ataque;
            this.entidadatacante = entidadatacante;
            this.tipo = tipo;
        }

        public PerfilVitalidad GetPerfil(){
            return perfil;
        }
        public float GetDaño(){
            return daño;
        }
        public VitalidadEventoTipo GetTipo(){
            return tipo;
        }
        public Entidad GetEntidadAtacante(){
            return entidadatacante;
        }

    }
  
    public delegate void VitalidadEvento(VitalidadInformacion info,ModuloVitalidad vitalidad);

    [System.Serializable]
    public class PerfilVitalidad{
       

        [SerializeField]
        private Colision []colisiones = null;
        [SerializeField]
        private float vida = 100.0f;
        [SerializeField]
        private float vidamaxima = 100.0f;
        [SerializeField]
        private VitalidadTipo tipo = VitalidadTipo.DAÑO; 
        [Header("Daño")]
        [Range(0.0f,1.0f)]
        [SerializeField]
        private float reducciondaño = 0.0f;
        [Header("Cargas")]
        [SerializeField]
        private int cargasporimpacto = 1;

        public void Start(){
            SetVida(vida);
            SetVidaMaxima(vidamaxima);
        }           
       
        public void ModVida(float valor){
            SetVida(vida + valor);
        }
        public void SetVida(float valor){
            vida = valor;
            if (tipo == VitalidadTipo.CARGAS)
                vida = (float)Math.Floor(vida);                        
            vida = Mathf.Clamp(vida, 0, vidamaxima);
        }

        public void ModVidaMaxima(float valor){
            SetVidaMaxima(vida + valor);
        }
        public void SetVidaMaxima(float valor){            
            vidamaxima = valor;
            if (tipo == VitalidadTipo.CARGAS)
                vidamaxima = (float)Math.Floor(vidamaxima);    
            if (vidamaxima <= 0)
                vidamaxima = 0;
            if (vida >= vidamaxima)
                vida = vidamaxima;
        }
            
        public void ModReduccionDaño(float valor){
            SetReduccionDaño(reducciondaño + valor);
        }
        public void SetReduccionDaño(float valor){
            reducciondaño = valor;
            reducciondaño = Mathf.Clamp(reducciondaño, 0, 1);
        }
         
        public float GetVida(bool relativa = false){
            if (relativa)
                return vida / vidamaxima;
            return vida;
        }
        public float GetVidaMaxima(){
            return vidamaxima;
        }
        public float GetReduccionDaño(){
            return reducciondaño;
        }
        public int   GetCargas(){
            return cargasporimpacto;
        }           
            
        public VitalidadTipo GetTipo(){
            return tipo;
        }

        public bool IsColision(Colision colision){
            for (int i = 0; i < colisiones.Length; i++)
                if (colisiones[i].Equals(colision))
                    return true;
            return false;
        }


    }
    [System.Serializable]
    public class ModuloVitalidad : EntidadModulo{

        [SerializeField]
        private PerfilVitalidad perfilvitalidad =null;

        private event VitalidadEvento vitalidadevento = null;

        public override void Start(){
            base.Start();
            perfilvitalidad.Start();
        }

        public void AddDaño(float ataquebasico,Entidad entidad,Colision colision){
            if (!IsEnable())
                return;                        

            if (perfilvitalidad.IsColision(colision)){
            ;

                    float daño = ataquebasico;
                    daño -= daño * perfilvitalidad.GetReduccionDaño(); 
                    if (perfilvitalidad.GetTipo() == VitalidadTipo.CARGAS)
                        daño = perfilvitalidad.GetCargas();

                    SolicitarVitalidadEvento(new VitalidadInformacion(perfilvitalidad, 
                        daño,
                        entidad,
                        VitalidadEventoTipo.PREDAÑO));    

                    if (IsEnable())
                        perfilvitalidad.ModVida(-daño);
                    else
                        daño = 0;

                    SolicitarVitalidadEvento(new VitalidadInformacion(perfilvitalidad, 
                        daño,
                        entidad,
                        VitalidadEventoTipo.DAÑO));                                    

                }
                    
        }
      
        public void AddVitalidadEvento(VitalidadEvento evento) {
            RemoveColisionEvento(evento);
            this.vitalidadevento += (evento);
        }
        public void RemoveColisionEvento(VitalidadEvento evento){
            try{
                this.vitalidadevento -= (evento);            
            }
            catch (Exception e) {
                Debug.LogError(e.Message);
            }

        }

        private void SolicitarVitalidadEvento(VitalidadInformacion info){
            if (vitalidadevento != null)
                vitalidadevento(info,this);
        }

        public void ModReduccionDaño(float reduccion){
            perfilvitalidad.ModReduccionDaño(reduccion);
        }


        public PerfilVitalidad GetPerfilVitalidad(){
            return perfilvitalidad;
        }

    }


}
