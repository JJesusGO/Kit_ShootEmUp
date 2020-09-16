using UnityEngine;
using System.Collections;

[System.Serializable]
public class Tecla{

    [SerializeField]
    private string nombre = "";
    [SerializeField]
    private string key = "";

    public Tecla(string nombre,string key){
        this.nombre = nombre;
        this.key = key;        
    }
    public Tecla(string nombre){
        this.nombre = nombre;
        this.key = "";
    }

    public bool IsClick(){
        return Input.GetKey(key);
    }
    public bool IsClickDown(){
        return Input.GetKeyDown(key);
    }
    public bool IsClickUp(){
        return Input.GetKeyUp(key);
    }
 

}

