using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace SEUP{

    public enum GameplayTipo{
        INFINITO, PUNTAJE, TIEMPO
    }
    public class ControlGameplayBasico : ManagerGameplay{


        [Header("General - Tipo")]
        [SerializeField]
        private GameplayTipo tipo = GameplayTipo.INFINITO;
        [Header("General - Variables")]
        [SerializeField]
        private int   vidas   = 0;
        [SerializeField]
        private int   puntajedeseado = 20; 
        [SerializeField]
        private float tiempodeseado  = 60.0f;
        [Header("General - Informacion")]
        [SerializeField]
        private string puntajereferencia = "Puntaje";
        [Header("General - UI - Controles")]
        [SerializeField]
        private UINumero uipuntaje = null;
        [SerializeField]
        private UINumero uipuntajemaximo = null;
        [SerializeField]
        private UIBarra  uividabarra = null;
        [SerializeField]
        private UINumero uividanumero = null;
        [SerializeField]
        private UINumero uividasnumero = null;
        [SerializeField]
        private UIBarra uienergiabarra = null;
        [SerializeField]
        private UINumero uienergianumero = null;
        [SerializeField]
        private UIBarra uidificultadbarra = null;
        [Header("Eventos")]
        [SerializeField]
        private UnityEvent  eventoinicio = new UnityEvent();
        [SerializeField]
        private UnityEvent  eventojugar = new UnityEvent();
        [SerializeField]
        private UnityEvent  eventoperder = new UnityEvent();
        [SerializeField]
        private UnityEvent  eventoganar = new UnityEvent();
        [SerializeField]
        private UnityEvent  eventopuntaje = new UnityEvent();

        private static ControlGameplayBasico instancia = null;

        private int puntaje = 0, 
                    puntajemaximo = 0;

        private float velocidadoriginal = 0;
        private Temporizador temporizador = null;

        private JugadorBasico jugador = null;
        private Guardado info = null;

        protected override void Awake(){
            base.Awake();
            velocidadoriginal = GetVelocidad();
            temporizador = new Temporizador();
        }
        protected override void Start(){

            info    = Guardado.GetInstancia();
            jugador = JugadorBasico.GetInstancia();                 

            if (tipo != GameplayTipo.PUNTAJE){
                if (info != null){
                    Data data = info.GetData(puntajereferencia);
                    if (data.referencia == puntajereferencia)
                        puntajemaximo = int.Parse(data.valor);                
                }
            }
            else{
                puntajemaximo = puntajedeseado;
            }

            base.Start();

        }
       
        private void Update(){
            temporizador.Update();

            if (uipuntaje != null)
                uipuntaje.SetNumero(puntaje);
            if (uipuntajemaximo != null)
                uipuntajemaximo.SetNumero(puntajemaximo);

            if(uividabarra!=null)                
                uividabarra.SetValor(jugador.GetVida(true));
            if(uividanumero!=null)   
                uividanumero.SetNumero(jugador.GetVida());
            if(uividasnumero!=null)
                uividasnumero.SetNumero(vidas);

            if(uienergiabarra!=null)
                uienergiabarra.SetValor(jugador.GetEnergia(true));
            if(uienergianumero!=null)
                uienergianumero.SetNumero(jugador.GetEnergia());
            if(uidificultadbarra!=null)
                uidificultadbarra.SetValor(GetDificultad());

            if (IsEstado(GameplayEstado.JUGANDO)){
            
                if (jugador != null){
                    if (jugador.GetVida(true) <= 0.0f){
                        jugador.Muerte();
                        vidas--;                        
                    }
                    if (jugador.IsMuerto()){
                        if (vidas <= 0)
                        {
                            vidas = 0;
                            SetEstado(GameplayEstado.PERDER);    
                        }
                        else{
                            if(!jugador.IsReviviendo())
                                jugador.Revivir();
                        }
                    }
                }                    
               
                switch (tipo){
                    case GameplayTipo.INFINITO:
                        int puntajedificultad = (puntajedeseado > puntajemaximo) ? puntajedeseado : puntajemaximo;
                        SetDificultad((float)puntaje / (float)puntajedificultad);
                        break;
                    case GameplayTipo.PUNTAJE:
                        SetDificultad((float)puntaje / (float)puntajemaximo);
                        if (puntaje >= puntajemaximo)
                            SetEstado(GameplayEstado.GANAR);                                                    
                        break;
                    case GameplayTipo.TIEMPO:
                        SetDificultad(temporizador.GetTiempo(true));
                        if (temporizador.GetTiempo(true) >= 1.0f)
                            SetEstado(GameplayEstado.GANAR);                                                    
                        break;
                }                        
            }

        }

        protected override void Inicio(){     

            ReiniciarValores();
            SetVelocidadBase(0);

            eventoinicio.Invoke();

            switch (tipo){
                case GameplayTipo.INFINITO:
                    temporizador.SetTiempoTarget(Temporizador.INFINITO);
                    break;
                case GameplayTipo.PUNTAJE:
                    temporizador.SetTiempoTarget(Temporizador.INFINITO);
                    break;
                case GameplayTipo.TIEMPO:
                    temporizador.SetTiempoTarget(tiempodeseado);
                    break;
            }

            temporizador.Reset();
       
        }
        protected override void Jugando(){
           
            eventojugar.Invoke();
            ReiniciarValores();
            SetVelocidadBase(velocidadoriginal);
        
            temporizador.Start();

        }
        protected override void Ganar(){
            eventoganar.Invoke();
            SetVelocidadBase(0);
            temporizador.SetEnable(false);
        }
        protected override void Perder(){            
            eventoperder.Invoke();
            SetVelocidadBase(0);
            temporizador.SetEnable(false);
        }

        private void ReiniciarValores(){

            SetPuntaje(0);
            SetDificultad(0);

            jugador.Revivir();
            if(uipuntaje!=null)
                uipuntaje.SetUINumero(puntaje);
            if (uipuntajemaximo != null)
                uipuntajemaximo.SetUINumero(puntajemaximo);

            if(uividabarra!=null)
                uividabarra.SetUIValor(jugador.GetVida(true));
            if(uividanumero!=null)
                uividanumero.SetUINumero(jugador.GetVida());

            if(uividasnumero!=null)
                uividasnumero.SetUINumero(vidas);

            if(uienergiabarra!=null)
                uienergiabarra.SetUIValor(jugador.GetEnergia(true));
            if(uividasnumero!=null)
                uividasnumero.SetUINumero(jugador.GetEnergia());

            if(uidificultadbarra!=null)
                uidificultadbarra.SetUIValor(GetDificultad());

        }

        public void ModPuntaje(int puntaje){
            SetPuntaje(this.puntaje + puntaje);
        }
        public void SetPuntaje(int puntaje){
            this.puntaje = puntaje;
            if (this.puntaje <= 0)
                this.puntaje = 0;

            switch (tipo){
                case GameplayTipo.INFINITO:
                    if (this.puntaje > puntajemaximo)
                        puntajemaximo = this.puntaje;
                        if (info != null){                            
                            info.SetData(puntajereferencia, puntajemaximo.ToString());
                            info.Guardar();
                        }
                    break;
                case GameplayTipo.PUNTAJE:
                    if (this.puntaje >= puntajemaximo)
                        this.puntaje = puntajemaximo;
                    break;
                case GameplayTipo.TIEMPO:
                    if (this.puntaje > puntajemaximo)
                        puntajemaximo = this.puntaje;
                    if (info != null){                            
                        info.SetData(puntajereferencia, puntajemaximo.ToString());
                        info.Guardar();
                    }
                    break;
            }
            eventopuntaje.Invoke();
        }

        public void ModVidas(int vidas){
            SetVidas(this.vidas + vidas);
        }
        public void SetVidas(int vidas){
            if (vidas <= 0)
                vidas = 0;
            this.vidas = vidas;
        }

        public int GetPuntaje(){
            return puntaje;
        }

        public void EventoIniciarPartida(){
            if (IsEstado(GameplayEstado.INICIO))
                SetEstado(GameplayEstado.JUGANDO);           
        }
        public void EventoReiniciarPartida(){
            ReiniciarNivel();
        }

        public static ControlGameplayBasico GetInstancia(){
            if (instancia == null)
                instancia = GameObject.FindObjectOfType<ControlGameplayBasico>();
            return instancia;
        }

    }


}

