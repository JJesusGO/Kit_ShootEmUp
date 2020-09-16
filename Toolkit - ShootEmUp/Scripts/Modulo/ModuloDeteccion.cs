using UnityEngine;
using System;
using System.Collections;

namespace SEUP{


    public enum   DeteccionObjetivo{
        ALIADO, ENEMIGO, AMBOS, TODOS
    }
    public struct DeteccionInformacion{
        private PerfilDeteccion perfil;
        private Entidad  entidad,entidaddetectada;
        private Colision colisiondetectada;

        public DeteccionInformacion(PerfilDeteccion perfil, Colision colisiondetectada,Entidad entidad,Entidad entidaddetectada){
            this.perfil = perfil;
            this.colisiondetectada = colisiondetectada;
            this.entidad = entidad;           
            this.entidaddetectada = entidaddetectada;
        }

        public PerfilDeteccion GetPerfil(){
            return perfil;
        }
        public Colision     GetColisionDetectada(){
            return colisiondetectada;
        }
        public Entidad      GetEntidad(){
            return entidad;
        }
        public Entidad      GetEntidadDetectada(){
            return entidaddetectada;
        }

    }

    public delegate void DeteccionEvento(DeteccionInformacion info);

    [System.Serializable]
    public class PerfilDeteccion{
 
        [SerializeField]
        private DeteccionObjetivo objetivo = DeteccionObjetivo.TODOS;
        [SerializeField]
        private Colision []colisiones = null;
    
        public void Start(ColisionEvento evento){
            if(colisiones!=null)
                for(int i=0;i<colisiones.Length;i++)
                    colisiones[i].AddColisionEvento(evento);
        }

        public DeteccionObjetivo GetObjetivo(){
            return objetivo;
        }


        public bool IsColision(Colision colision){
            for (int i = 0; i < colisiones.Length; i++)
                if (colision == colisiones[i])
                    return true;
            return false;

        }
    }

    [System.Serializable]
    public class ModuloDeteccion : EntidadModulo{


        [SerializeField]
        private PerfilDeteccion perfildeteccion = new PerfilDeteccion();

        private ManagerGameplay game = null;
        private event DeteccionEvento deteccionevento;

        public override void Start(){
            base.Start();
            game = ManagerGameplay.GetInstanciaBase();
            perfildeteccion.Start(EventoColision);
        }

        public void AddDeteccionEvento(DeteccionEvento evento) {
            RemoveDeteccionEvento(evento);
            deteccionevento += (evento);
        }
        public void RemoveDeteccionEvento(DeteccionEvento evento){
            try{
                deteccionevento -= (evento);            
            }
            catch (Exception e) {
                Debug.LogError(e.Message);
            }

        }

        private void SolicitarEvento(DeteccionInformacion info){
            if (deteccionevento != null)
                deteccionevento(info);
        }
            
        public PerfilDeteccion GetPerfilDeteccion(Colision colision){
            if (perfildeteccion.IsColision(colision))
                return perfildeteccion;
            return null;
        }
        public PerfilDeteccion GetPerfilDeteccion(){
            return perfildeteccion;
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
            PerfilDeteccion perfil = GetPerfilDeteccion(info.GetColision());
            if (perfil == null)
                return;

            switch(perfil.GetObjetivo()){
                case DeteccionObjetivo.ALIADO:

                    if (entidad.GetTipo() == EntidadTipo.ALIADO)
                        SolicitarEvento(new DeteccionInformacion(
                            perfil,
                            info.GetColisionImpacto(),
                            GetEntidad(),
                            entidad
                        ));
                    

                    break;
                case DeteccionObjetivo.ENEMIGO:

                    if (entidad.GetTipo() == EntidadTipo.ENEMIGO)
                        SolicitarEvento(new DeteccionInformacion(
                            perfil,
                            info.GetColisionImpacto(),
                            GetEntidad(),
                            entidad
                        ));
                    

                    break;
                case DeteccionObjetivo.AMBOS:

                    if (entidad.GetTipo() != EntidadTipo.DESCONOCIDO)
                        SolicitarEvento(new DeteccionInformacion(
                            perfil,
                            info.GetColisionImpacto(),
                            GetEntidad(),
                            entidad
                        ));

                    break;
                case DeteccionObjetivo.TODOS:
                    
                        SolicitarEvento(new DeteccionInformacion(
                            perfil,
                            info.GetColisionImpacto(),
                            GetEntidad(),
                            entidad
                        ));

                    break;
            }

        }    

    }

}


