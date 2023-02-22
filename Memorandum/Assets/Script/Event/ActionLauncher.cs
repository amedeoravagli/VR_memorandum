using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class ActionLauncher : MonoBehaviour
{
    public Action<bool> TestIsReadyEvent;
    public Action<int> ActiveMinigameEvent;
    public Action<bool, int> StatusIsTestEvent;
    public Action<string> VerifyInputStringEvent;
    public void OnTestActivation(bool isReady)
    {
        if (TestIsReadyEvent != null)
        {
            Debug.Log("INVIO EVENTO DI ATTIVAZIONE MINIGIOCO: Invio Test Activation event");
            TestIsReadyEvent.Invoke(isReady);
        }
    }

    public void OnStatusIsTest(bool isTest, int index)
    {
        if (StatusIsTestEvent != null)
        {
            Debug.Log("INVIO EVENTO DI INIZIO TEST: Invio isTest event");
            StatusIsTestEvent.Invoke(isTest, index);
        }
    }

    public void VerifyInputString(string input)
    {
        if (VerifyInputStringEvent != null)
        {
            Debug.Log("INVIO EVENTO VERIFICA PAROLA INPUT TEST: Invio parola "+ input);
            VerifyInputStringEvent.Invoke(input);
        }
    }

    public void ActivateMinigame(int index)
    {
        if (ActiveMinigameEvent != null)
        {
            Debug.Log("INVIO EVENTO VERIFICA PAROLA INPUT TEST: Invio indexRoom " + index);
            ActiveMinigameEvent.Invoke(index);
        }
    }

}
