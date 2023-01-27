using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Taggable : Interactable
{

    private String card_tag = null;

    public override void Interact(GameObject caller)
    {  
        if (card_tag == null) {

            card_tag = caller.GetComponent<FPSUI>().GetCardTag();
            
        }else
        {
            caller.GetComponent<FPSUI>().PushCardTag(card_tag);
            card_tag = null;
        }

        Debug.Log(card_tag);
    }


}

