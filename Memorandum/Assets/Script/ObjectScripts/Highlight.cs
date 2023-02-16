
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
    private List<Material> materials;
    private float _rgb = 0f;
    //private List<bool> emission = new List<bool>();
    //Gets all the materials from each renderer
    private void Awake()
    {
        materials = new List<Material>();
        foreach (var renderer in renderers)
        {
            //A single child-object might have mutliple materials on it
            //that is why we need to all materials with "s"
            materials.AddRange(new List<Material>(renderer.materials));
            //emission.Add(false);
        }
    }

    IEnumerator Fade(Material material) // coroutine per l'animazione di lampeggio
    {
        Color c = material.GetColor("_EmissionColor");
        
        c.a = 1f; 
        float inc = 1f;
        _rgb = 0f;
        float r , g , b ;
        while (true)
        {
            //_rgb += inc;//(inc * Time.deltaTime) ;
            //_rgb %= 256;
            
            r = c.r * _rgb;
            g = c.g * _rgb;
            b = c.b * _rgb;
            
            material.SetColor("_EmissionColor", new Color(r,g,b));
            
            if (_rgb+inc < 0 || _rgb+inc >= 256)
                inc = -inc;
            _rgb += inc;
            yield return new WaitForSeconds(1f);
        }
    }

    /*protected void EmissionFaded()
    {
        int i = 0;
        float inc = 10f;
        foreach (var material in materials)
        {
            if (!emission[i])
            {
                material.EnableKeyword("_EMISSION");
                emission[i] = true;
            }
            Color c = material.GetColor("_EmissionColor");
            c.r = _rgb;
            c.g = _rgb;
            c.b = _rgb;

            material.SetColor("_EmissionColor", c);

            if (_rgb + inc <= -1 || _rgb + inc >= 256)
                inc = -inc;
            _rgb += inc;
            
            i++;
        }
    }*/

    public void ToggleHighlight(bool val, bool isTestObject)
    {
        if (val)
        {
            if (!isTestObject)
            {
                foreach (var material in materials)
                {

                    //We need to enable the EMISSION
                    material.EnableKeyword("_EMISSION");
                    //before we can set the color
                    material.SetColor("_EmissionColor", color);
                }
            }
            else
            {
                foreach (var material in materials)
                {
                    material.EnableKeyword("_EMISSION");
                    StartCoroutine(Fade(material));
                }
            }
            
        }
        else
        {
            //int i = 0;
            foreach (var material in materials)
            {
                if(isTestObject) {
                    StopCoroutine(Fade(material));
                }
                //we can just disable the EMISSION
                //if we don't use emission color anywhere else
                material.DisableKeyword("_EMISSION");
                //emission[i] = false;
                //i++;
            }
        }
    }
}