using UnityEngine;
using System.Collections;

[System.Serializable]
public class Eje{

    [SerializeField]
    private Tecla positivo = new Tecla("+");
    [SerializeField]
    private Tecla negativo = new Tecla("-");

    public int  GetValor(){
        if (positivo.IsClick())
            return 1;
        if (negativo.IsClick())
            return -1;
        return 0;
    }
   
    public bool IsClick(){
        return positivo.IsClick() || negativo.IsClick();
    }
    public bool IsClickDown(){
        return positivo.IsClickDown() || negativo.IsClickDown();
    }
    public bool IsClickUp(){
        return positivo.IsClickUp() || negativo.IsClickUp();
    }


}

