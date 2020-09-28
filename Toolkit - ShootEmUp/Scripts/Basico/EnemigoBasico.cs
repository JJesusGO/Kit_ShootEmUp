using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using SEUP;

public class EnemigoBasico : Enemigo{

    [Header("EnemigoBasico - General")]
    [SerializeField]
    private ModuloVitalidad vitalidad = new ModuloVitalidad();
    [SerializeField]
    private ModuloAtaque ataque = new ModuloAtaque();
    [Header("EnemigoBasico - Posicion")]
    [SerializeField]
    private Vector2 []posicionesdisponibles = null;
    [Header("EnemigoBasico - Proyectiles")]
    [SerializeField]
    private DisparadorProyectilBasico []disparadores = null;
    [Header("EnemigoBasico - Eventos")]
    [SerializeField]
    private UnityEvent eventodisparo = new UnityEvent();
    [SerializeField]
    private UnityEvent eventoataque = new UnityEvent();
    [SerializeField]
    private UnityEvent eventodaño = new UnityEvent();

    private bool disparar = false;
    private int  dispararframes = 3; 

    protected override void Awake(){
        base.Awake();
       
        AddModulo(ataque);
        AddModulo(vitalidad);

        vitalidad.AddVitalidadEvento(EventoVitalidad);
        ataque.AddAtaqueEvento(EventoAtaque);
    }
    protected override void Update(){
        base.Update();

        if (disparadores != null && dispararframes <= 0){
            for (int i = 0; i < disparadores.Length; i++)
                if (disparadores[i].IsActivo())
                {
                    disparadores[i].Disparar(this);
                    eventodisparo.Invoke();
                }
        }

        if (disparar && dispararframes > 0)
            dispararframes--;
    }

    public override void    Generacion(Mapa mapa,int x,int y){
        if (posicionesdisponibles != null){
            if (posicionesdisponibles.Length == 0)
                SetPosicion((Vector2)mapa.GetCeldaPosicion(x, y));
            else{
                Vector2 aleatoria = posicionesdisponibles[Random.Range(0,posicionesdisponibles.Length)];
                SetPosicion((Vector2)mapa.GetCeldaPosicion((int)aleatoria.x, (int)aleatoria.y));
            }
        }
        else
            SetPosicion((Vector2)mapa.GetCeldaPosicion(x,y));
        disparar = true;
    }
    public override void    Muerte(){
        GetModuloAtaque().SetEnable(false);
        base.Muerte();       
    }
        
    #region eventos

    private void EventoVitalidad(VitalidadInformacion info,ModuloVitalidad vitalidad){
        if (info.GetTipo() == VitalidadEventoTipo.DAÑO){
            PerfilVitalidad perfil = info.GetPerfil();
            if (perfil == null)
                return; 
            eventodaño.Invoke();
            if (perfil.GetVida() <= 0)
                Muerte();
        }
    }
    private void EventoAtaque(AtaqueInformacion info,ModuloAtaque ataque){
        eventoataque.Invoke();        
    }
            
    #endregion

    public override ModuloAtaque    GetModuloAtaque(){
        return ataque;
    }
    public override ModuloVitalidad GetModuloVitalidad(){
        return vitalidad;
    }


}

