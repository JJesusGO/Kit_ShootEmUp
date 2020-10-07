using UnityEngine;
using System.Collections;
using SEUP;

public class AudioCarpeta : MonoBehaviour{

    public void EventoStopAudios(){
        Audio []audios = GetComponentsInChildren<Audio>();
        if (audios != null)
            for (int i = 0; i < audios.Length; i++)
                audios[i].Stop();
    }
   
}

