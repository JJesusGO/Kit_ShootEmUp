using UnityEngine;
using System.Collections;
using SEUP;

public class ProyectilBasico : Proyectil{


    protected override void Awake(){
        base.Awake();
        GetModuloAtaque().AddAtaqueEvento(EventoAtaque);
    }

    public override void Generacion(Mapa mapa, int x, int y){
        
    }
    public override void Muerte(){

        GetModuloAtaque().SetEnable(false);
        GetModuloMovimiento().SetVelocidad(0);

        base.Muerte();

    }
  
    public override void Disparar(Vector3 direccion){
        ModuloMovimiento modulomovimiento = GetModuloMovimiento();
        modulomovimiento.SetDireccionBasica(direccion);
    }

    private void EventoAtaque(AtaqueInformacion info, ModuloAtaque ataque){

        ataque.SetEnable(false);
        Muerte();
    }


}

