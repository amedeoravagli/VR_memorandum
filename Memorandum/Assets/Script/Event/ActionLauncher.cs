using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionLauncher : MonoBehaviour
{
    public Action<bool> TestIsReadyEvent;
    public Action<bool> StatusIsTestEvent;

    public void OnTestActivation(bool isReady)
    {
        if (TestIsReadyEvent != null)
        {
            Debug.Log("INVIO EVENTO DI ATTIVAZIONE MINIGIOCO: Invio Test Activation event");
            TestIsReadyEvent.Invoke(isReady);
        }
    }

    public void OnStatusIsTest(bool isTest)
    {
        if (StatusIsTestEvent != null)
        {
            Debug.Log("INVIO EVENTO DI INIZIO TEST: Invio isTest event");
            StatusIsTestEvent.Invoke(isTest);
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
