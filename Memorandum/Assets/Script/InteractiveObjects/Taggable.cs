using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taggable : Interactable
{

    public override void Interact(GameObject caller)
    {  
        AppState appstate = caller.GetComponent<AppState>();
        if (!appstate.IsTest())
        {
            if (this.card_tag == null)
            {
                this.card_tag = appstate.BindCardTag();
            }
            else
            {
                if (appstate.GetDetachableCardTag() == this.card_tag)
                {
                    appstate.UnbindCardTag();
                    this.card_tag = null;
                }

            }

            Debug.Log(this.card_tag);
        }
        
    }


}

