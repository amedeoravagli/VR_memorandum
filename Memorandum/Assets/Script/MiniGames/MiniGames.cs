using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGames : Interactable
{
    public override void Interact(GameObject caller)
    {
        AppState appstate = caller.GetComponent<AppState>();
        if (!!appstate.IsTest())
        {
            appstate.ChangeFase();
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
        
    }
    */
}
