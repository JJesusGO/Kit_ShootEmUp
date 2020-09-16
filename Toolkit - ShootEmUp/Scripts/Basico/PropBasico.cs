using UnityEngine;
using System.Collections;
using SEUP;

public class PropBasico : Prop{

    [Header("PropBasico - Posicion")]
    [SerializeField]
    private Vector2 []posicionesdisponibles = null;

    public override void Generacion(Mapa mapa, int x, int y){
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

}

