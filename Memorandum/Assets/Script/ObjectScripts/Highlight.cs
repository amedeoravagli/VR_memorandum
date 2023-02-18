
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    //we assign all the renderers here through the inspector
    [SerializeField] private List<Renderer> renderers;
    //[SerializeField]
    private Color color = Color.white;

    //helper list to cache all the materials ofd this object
    
    private float _rgb = 0f;
    private bool _enabledFade = false;  
    //private List<bool> emission = new List<bool>();
    //Gets all the materials from each renderer
   

    IEnumerator Fade() // coroutine per l'animazione di lampeggio
    {

        Color c = Color.black;
        float inc = 0.01f;
        _rgb = 0.1f;
        while (_enabledFade)
        {

            c.r = _rgb;
            c.g = _rgb;
            c.b = _rgb;
            
            for (int i = 0; i < renderers.Count; i++)
            {
                for (int j = 0; j < renderers[i].materials.Length; j++)
                {
                    renderers[i].materials[j].SetColor("_EmissionColor", c);
                }
            }
            
            if (_rgb+inc < 0 || _rgb+inc > 1)
                inc = -inc;
            if (_rgb + inc < 0.2 && _rgb + inc > 0.0)
            { 
                if (inc > 0)
                {
                    inc = 0.005f;
                }
                else
                {
                    inc = -0.005f;
                }
            }
            if( _rgb + inc > 0.8 && _rgb + inc < 1.0)
            {
                if (inc > 0)
                {
                    inc = 0.005f;
                }
                else
                {
                    inc = -0.005f;
                }
            }
            _rgb += inc;
            yield return new WaitForSeconds(0.01f);
        }
    }



    public void ToggleHighlight(bool val, bool isTestObject)
    {
        if (val)
        {
            if (!isTestObject)
            {

                for(int  i = 0; i < renderers.Count; i++)
                {
                    for (int j = 0; j < renderers[i].materials.Length; j++)
                    {
                        renderers[i].materials[j].SetColor("_EmissionColor", color);
                    }
                }
            
            }
            else
            {
                _enabledFade = true;
                StartCoroutine(Fade());
            }
            
        }
        else
        {

            if (isTestObject)
            {
                _enabledFade = false;
                StopCoroutine(Fade());
            }
            for (int i = 0; i < renderers.Count; i++)
            {
                for (int j = 0; j < renderers[i].materials.Length; j++)
                {
                    renderers[i].materials[j].SetColor("_EmissionColor", Color.black);
                }
            }
        }
    }
}