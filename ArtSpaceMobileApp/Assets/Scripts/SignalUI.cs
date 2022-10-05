using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignalUI : MonoBehaviour
{
    public TextMeshProUGUI TXT_Output;
    public string toSet;
    public void Print(string msg){
        if(TXT_Output)
            TXT_Output.text = msg;
    }

    void LateUpdate(){
        if(!string.IsNullOrEmpty(toSet) && TXT_Output){
            TXT_Output.text = toSet;
            toSet = string.Empty;
        }
    }
}
