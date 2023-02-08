using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taggable : Interactable
{

    private String card_tag = null;

    public override void Interact(GameObject caller)
    {  
        AppState appstate = caller.GetComponent<AppState>();
        if (!appstate.IsTest())
        {
            if (card_tag == null)
            {
                
                card_tag = appstate.BindCardTag();
            }
            else
            {
                if (appstate.GetDetachableCardTag() == card_tag)
                {
                    appstate.UnbindCardTag();
                    card_tag = null;
                }

            }

            Debug.Log(card_tag);
        }
        
    }


}

