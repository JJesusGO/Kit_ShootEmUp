using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using SEUP;


public enum HabilidadTipo{
   DISPARAR, CURAR
}

public class JugadorBasico : Aliado{

    [Header("Jugador")]
    [SerializeField]
    private bool revivirautomatico = true;
    [SerializeField]
    private ModuloNavegacionBasico navegacion = new ModuloNavegacionBasico();
    [SerializeField]
    private ModuloVitalidad         vitalidad = new ModuloVitalidad();
    [SerializeField]
    private ModuloAtaque            ataque    = new ModuloAtaque();
    [Header("Input")]
    [SerializeField]
    private Tecla    acciontecla      = new Tecla("Accion","space");
    [SerializeField]
    private Tecla    habilidad1tecla  = new Tecla("Habilidad1","1");
    [SerializeField]
    private Tecla    habilidad2tecla  = new Tecla("Habilidad2","2");
    [Header("Habilidades")]
    [SerializeField]
    private float energia       = 100.0f; 
    [SerializeField]
    private float energiamaxima = 100.0f;
    [SerializeField]
    private HabilidadTipo habilidad1  = HabilidadTipo.DISPARAR;
    [SerializeField]
    private HabilidadTipo habilidad2  = HabilidadTipo.CURAR;
    [Header("Habilidad - Curación")]
    [SerializeField]
    private float    costocuracion     = 0.2f; 
    [SerializeField]
    private float    curacionvelocidad = 2;
    [Header("Habilidad - Disparo")]
    [SerializeField]
    private float       costodisparo = 25.0f;
    [SerializeField]
    private DisparadorProyectilBasico disparador = null;
    [Header("Jugador - Eventos")]
    [SerializeField]
    private UnityEvent eventodisparo = new UnityEvent();
    [SerializeField]
    private UnityEvent eventoiniciocuracion = new UnityEvent();
    [SerializeField]
    private UnityEvent eventofincuracion = new UnityEvent();
    [SerializeField]
    private UnityEvent eventohabilidad1 = new UnityEvent();
    [SerializeField]
    private UnityEvent eventohabilidad2 = new UnityEvent();
    [SerializeField]
    private UnityEvent eventodaño = new UnityEvent();

    private static JugadorBasico instancia = null;

    private bool reviviendo = false, curando = false;

    private ControlGameplayBasico  game = null;

    private HabilidadTipo habilidad = HabilidadTipo.DISPARAR;

    private PerfilVitalidad perfilvitalidad = null;

    protected override void Awake(){
        base.Awake();

        instancia = this;

        game = ControlGameplayBasico.GetInstancia();

        AddModulo(navegacion);
        AddModulo(vitalidad);
        AddModulo(ataque);

        perfilvitalidad = GetModuloVitalidad().GetPerfilVitalidad();       
        vitalidad.AddVitalidadEvento(EventoVitalidad);

    }
    protected override void Update(){
        base.Update();

        if (game.IsEstado(GameplayEstado.JUGANDO)){

            if (habilidad1tecla.IsClickDown())
                SetHabilidad(habilidad1);
            else if (habilidad2tecla.IsClickDown())
                SetHabilidad(habilidad2);
         
            if (habilidad == HabilidadTipo.DISPARAR)            
                Disparar();   
            if (habilidad == HabilidadTipo.CURAR)
                Curar();
        }
    }

    public override void Generacion(Mapa mapa,int x,int y){

    }        
        
    public void Revivir(){


        SetEnergia(energiamaxima);
        SetVida(GetVidaMaxima());

        reviviendo = true;
        if (revivirautomatico)
            EventoVivir();
    

    }        
    public void EventoVivir(){

        SetMuerto(false);
        reviviendo = false;
        eventovivir.Invoke();

    }

    #region habilidades

    private void Disparar(){

        if (disparador.IsActivo()){

            float costo = costodisparo;

            if (acciontecla.IsClick() && GetEnergia() >= costo){                

                disparador.Disparar(this);
                ModEnergia(-costo);
                eventodisparo.Invoke();

            }
        }
            
    }   
    private void Curar(){
        
        float valor = curacionvelocidad * Time.deltaTime;
        float costo = valor * costocuracion;

        if (acciontecla.IsClick() && GetEnergia() >= costo){

            if (!curando){
                curando = true;
                eventoiniciocuracion.Invoke();
            }                
            if (perfilvitalidad.GetVida(true) < 1.0f){
                perfilvitalidad.ModVida(valor);
                ModEnergia(-costo);
            }
            else if(curando){
                curando = false;
                eventofincuracion.Invoke();
            }
        }
        else if(curando){
            curando = false;
            eventofincuracion.Invoke();
        }
        

    }

    #endregion
  
    public void  SetVida(float vida){
        perfilvitalidad.SetVida(vida);
    }
    public void  ModVida(float vida){
        perfilvitalidad.ModVida(vida);
    }

    public void  SetEnergia(float energia){
        this.energia = energia;
        this.energia = Mathf.Clamp(energia, 0.0f, energiamaxima);

    }
    public void  ModEnergia(float energia){
        SetEnergia(this.energia + energia);
    }

    public void ModAtaque(float ataque){
        this.ataque.ModAtaqueBasico(ataque);
    }

    public void  SetHabilidad(HabilidadTipo habilidad){      
        if (this.habilidad == habilidad)
            return;
        this.habilidad = habilidad;

        if (habilidad == habilidad1)
            eventohabilidad1.Invoke();
        if (habilidad == habilidad2)
            eventohabilidad2.Invoke();        
    }
        
    public float GetEnergia(bool relativa = false){
        if (relativa)
            return energia / energiamaxima;
        return energia;
    }
    public float GetEnergiaMaxima(){
        return energiamaxima;
    }

    public float GetVida(bool relativa = false){
        return perfilvitalidad.GetVida(relativa);
    }
    public float GetVidaMaxima(){
        return perfilvitalidad.GetVidaMaxima();
    }

    public HabilidadTipo GetHabilidad(){
        return habilidad;
    }
 
    public HabilidadTipo GetHabilidad1(){
        return habilidad1;
    }
    public HabilidadTipo GetHabilidad2(){
        return habilidad2;
    }

    public override ModuloAtaque    GetModuloAtaque(){
        return ataque;
    }
    public override ModuloVitalidad GetModuloVitalidad(){
        return vitalidad;
    }        

    public bool IsReviviendo(){
        return reviviendo;
    }

    #region eventos
      
    public void EventoVitalidad(VitalidadInformacion info,ModuloVitalidad vitalidad){
        if (info.GetTipo() != VitalidadEventoTipo.DAÑO)
            return;
        if (info.GetPerfil() != perfilvitalidad)
            return;
        float daño = info.GetDaño();
        eventodaño.Invoke();
    }

    #endregion

    public static JugadorBasico GetInstancia(){
        if (instancia == null)
            instancia = GameObject.FindObjectOfType<JugadorBasico>();
        return instancia;
    }

}

