using UnityEngine;
using System.Collections;

public class ControlSkinBasico : MonoBehaviour{

    [SerializeField]
    private int index = 0;
    [SerializeField]
    private Transform []skins = null;

    public void Awake(){
        SetSkin(index, true);
    }

    public void SetSkin(int index,bool forzar = false){
        if (this.index == index && !forzar)
            return;
        if (skins == null)
            return;
        if (index >= skins.Length  || index < 0)
            return;
        this.index = index;
        for (int i = 0; i < index; i++)
            skins[i].gameObject.SetActive(index == i);
    }

    public void EventoSetSkin(Transform skin){
        if (skins == null)
            return;
        for (int i = 0; i < skins.Length; i++)
            if (skin == skins[i]){
                SetSkin(i);
                break;
            }
                
    }
    public void EventoSetSkin(int index){
        SetSkin(index);
    }

}

