using UnityEngine;
using System.Collections.Generic;

public class Probabilidad{

    private List<float> probabilidades = new List<float>();
    private float sumatoria = 0;

    public void Clear(){
        sumatoria = 0;
        probabilidades.Clear();
    }
    public void AddProbabilidad(float probabilidad){
        probabilidades.Add(probabilidad);
        sumatoria += probabilidad;
    }

    public int NextProbabilidad(){
        float v = Random.Range(0.0f, sumatoria),
              s = 0;
        for (int i = 0; i < probabilidades.Count; i++){
            s += probabilidades[i];
            if (v <= s)
                return i;
        }
        return probabilidades.Count - 1;            
    }

    public float GetProbabilidad(int i,bool relativa = true){
        if (relativa)
            return probabilidades[i] / sumatoria;
        return probabilidades[i];
    }
    public int   GetProbabilidadCount(){        
        return probabilidades.Count;
    }

    public float GetSumatoria(){
        return sumatoria;
    }
}

