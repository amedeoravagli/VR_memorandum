using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLauncher : MonoBehaviour
{
    public Action<bool> TestIsReadyEvent;

    public void OnTestActivation(bool isReady)
    {
        if (TestIsReadyEvent != null)
        {
            Debug.Log("INVIO EVENTO DI ATTIVAZIONE MINIGIOCO: Invio Test Activation event");
            TestIsReadyEvent.Invoke(isReady);
        }
    }

    /*
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }*/
}
