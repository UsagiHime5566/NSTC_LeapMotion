using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IPConnect : MonoBehaviour
{
    public InputField INP_IP;
    public SignalClient client;
    void Start()
    {
        INP_IP?.onEndEdit.AddListener(x => {
            Debug.Log("Start Connect to " + x);
            client.serverIP = x;
            client.InitSocket();
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
