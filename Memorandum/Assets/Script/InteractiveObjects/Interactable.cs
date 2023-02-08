using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : Highlight
{
    public abstract void Interact(GameObject caller);
}
