using UnityEngine;
using System.Collections;
using SEUP;


public enum ItemTipo{
   PUNTAJE,ENERGIA,VIDA, ATAQUE, REDUCCIONDAÑO
}

public class ItemBasico : Item{

    [SerializeField]
    private ItemTipo tipoitem = ItemTipo.PUNTAJE;
    [SerializeField]
    private float valor = 1;

    [Header("ItemBasico - Posicion")]
    [SerializeField]
    private Vector2 []posicionesdisponibles = null;

    private bool activo = true;

    public override void    Generacion(Mapa mapa, int x, int y){
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
    }
    protected override void Deteccion(DeteccionInformacion info){

        if (!activo)
            return;

        Entidad entidad   = info.GetEntidadDetectada();
        Colision colision = info.GetColisionDetectada();
        JugadorBasico personaje = (JugadorBasico)entidad;

        if (personaje == null)
            return;
        switch(tipoitem){
            case ItemTipo.PUNTAJE:
                ControlGameplayBasico.GetInstancia().ModPuntaje((int)valor);
                break;
            case ItemTipo.ENERGIA:
                personaje.ModEnergia(valor);
                break;
            case ItemTipo.VIDA:
                personaje.ModVida(valor);
                break;
            case ItemTipo.ATAQUE:
                personaje.ModAtaque(valor);
                break;
            case ItemTipo.REDUCCIONDAÑO:
                personaje.ModReduccionDaño(valor);
                break;
        }
       
        Muerte();
    }

}

