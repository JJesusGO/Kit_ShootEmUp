using UnityEngine;
using System;
using System.Collections;

namespace SEUP{


    public enum   AtaqueObjetivo{
        ALIADO, ENEMIGO, AMBOS
    }
    public struct AtaqueInformacion{
        private PerfilAtaque perfil;
        private float ataquebasico;
        private Entidad entidad,entidadatacada;

        public AtaqueInformacion(PerfilAtaque perfil,float ataquebasico, Entidad entidad,Entidad entidadatacada){
            this.perfil = perfil;
            this.ataquebasico = ataquebasico;
            this.entidad = entidad;
            this.entidadatacada = entidadatacada;
        }

        public PerfilAtaque GetPerfil(){
            return perfil;
        }
        public float        GetAtaqueBasico(){
            return ataquebasico;
        }
        public Entidad      GetEntidad(){
            return entidad;
        }
        public Entidad      GetEntidadAtacada(){
            return entidadatacada;
        }

    }

    public delegate void AtaqueEvento(AtaqueInformacion info,ModuloAtaque ataque);

    [System.Serializable]
    public class PerfilAtaque{
        [SerializeField]
        private string nombre = "Desconocido";
        [SerializeField]
        private Colision []colisiones = null;
        [SerializeField]
        private AtaqueObjetivo objetivo = AtaqueObjetivo.AMBOS;
        [Range(0,1)]
        [SerializeField]
        private float ataquebasico = 1.0f;

        public void Start(ColisionEvento evento){
            for(int i=0;i<colisiones.Length;i++)
                colisiones[i].AddColisionEvento(evento);
        }
                                        
        public float GetAtaqueBasico(){
            return ataquebasico;
        }
        public AtaqueObjetivo GetObjetivo(){
            return objetivo;
        }
        public string GetNombre(){
            return nombre;
        }

        public bool IsColision(Colision colision){
            for (int i = 0; i < colisiones.Length; i++)
                if (colision == colisiones[i])
                    return true;
            return false;
            
        }
    }
   
    [System.Serializable]
    public class ModuloAtaque : EntidadModulo{

        [Header("Ataque - Base")]
        [SerializeField]
        private float ataquebasico = 0.0f;
        [Header("Ataque - Herencia")]
        [Range(0,1)]
        [SerializeField]
        private float ataquebasicoherencia = 0.0f;
        [Header("Ataque - Dificultad")]
        [SerializeField]
        private AnimationCurve ataquebasicodificultad      = new AnimationCurve(new Keyframe(0,1),new Keyframe(1,1));
        [Header("Perfiles de ataque")]
        [SerializeField]
        private PerfilAtaque[] perfiles = null;

        private ManagerGameplay game = null;
        private event AtaqueEvento ataqueevento;
        private float ataqueheredado = 0.0f;

        public override void Start(){
            base.Start();
            game = ManagerGameplay.GetInstanciaBase();
            for (int i = 0; i < perfiles.Length; i++)
                perfiles[i].Start(EventoColision);

            ataqueheredado = 0.0f;
            if(GetEntidad().GetEntidadPadre() != null){
                ModuloAtaque ataque = GetEntidad().GetEntidadPadre().GetModuloAtaque();
                if (ataque != null)
                    ataqueheredado = ataque.GetAtaqueBasico();              
            }

        }

        public void AddAtaqueEvento(AtaqueEvento evento) {
            RemoveAtaqueEvento(evento);
            ataqueevento += (evento);
        }
        public void RemoveAtaqueEvento(AtaqueEvento evento){
            try{
                ataqueevento -= (evento);            
            }
            catch (Exception e) {
                Debug.LogError(e.Message);
            }

        }
            
        private void SolicitarEvento(AtaqueInformacion info){
            if (ataqueevento != null)
                ataqueevento(info,this);
        }

        public void ModAtaqueBasico(float mod){
            ataquebasico += mod;
            if (ataquebasico < 0)
                ataquebasico = 0;
        }

                   
        public float GetAtaqueBasico(PerfilAtaque perfil = null){
            float a = GetAtaqueBasicoBase() + ataqueheredado * GetAtaqueBasicoHerencia();                
            if (perfil != null)
                a *= perfil.GetAtaqueBasico();
            return a;
        }
   

        public float GetAtaqueBasicoBase(){
            float k = 0.0f;
            if (game != null)
                k = game.GetDificultad();
            return ataquebasico*ataquebasicodificultad.Evaluate(k);
        }
        public float GetAtaqueBasicoHerencia(){
            return ataquebasicoherencia;
        }


        public PerfilAtaque GetPerfil(Colision colision){
            for (int i = 0; i < perfiles.Length; i++)
                if (perfiles[i].IsColision(colision))
                    return perfiles[i];
            return null;
        }
        public PerfilAtaque GetPerfiles(int i){
            return perfiles[i];
        }
        public int          GetPerfilesCount(){
            return perfiles.Length;
        }


        private void EventoColision(ColisionInformacion info){

            if (!IsEnable())
                return;

            Entidad entidad = info.GetEntidadImpacto();
            if (entidad == null || info.GetColisionEstado() != ColisionEstado.ENTER)
                return;
            ModuloVitalidad vitalidad = entidad.GetModuloVitalidad();
            if (vitalidad == null)
                return;
            PerfilAtaque perfil = GetPerfil(info.GetColision());
            if (perfil == null)
                return;

            switch(perfil.GetObjetivo()){
                case AtaqueObjetivo.ALIADO:

                    if (entidad.GetTipo() == EntidadTipo.ALIADO)
                    {

 
                        SolicitarEvento(new AtaqueInformacion(
                                perfil,
                                GetAtaqueBasico(perfil),
                                GetEntidad(),
                                entidad
                            ));
                        vitalidad.AddDaño(GetAtaqueBasico(perfil),entidad, info.GetColisionImpacto());
                    }

                    break;
                case AtaqueObjetivo.ENEMIGO:

                    if (entidad.GetTipo() == EntidadTipo.ENEMIGO){
                        
                        SolicitarEvento(new AtaqueInformacion(
                            perfil,
                            GetAtaqueBasico(perfil),
                            GetEntidad(),
                            entidad
                        ));
                        vitalidad.AddDaño(GetAtaqueBasico(perfil),entidad, info.GetColisionImpacto());
                    }

                    break;
                case AtaqueObjetivo.AMBOS:

                    if (entidad.GetTipo() != EntidadTipo.DESCONOCIDO)
                    {
                        SolicitarEvento(new AtaqueInformacion(
                            perfil,
                            GetAtaqueBasico(perfil),
                            GetEntidad(),
                            entidad
                        ));
                        vitalidad.AddDaño(GetAtaqueBasico(perfil),entidad, info.GetColisionImpacto());

                    }

                    break;
            }

        }    

    }

}

