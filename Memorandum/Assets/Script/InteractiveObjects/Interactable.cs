using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : Highlight
{
    protected string card_tag = null;
    public abstract void Interact(GameObject caller);
}
