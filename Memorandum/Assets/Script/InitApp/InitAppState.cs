using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.IO;
using NUnit.Framework.Constraints;
using NUnit.Framework;

public class InitAppState : MonoBehaviour
{
    public List<string> cardList = new List<string>();
    public bool isTutorial = false;
  

    // Start is called before the first frame update
    void OnEnable()
    {
        
        cardList = GetCardTagsFromFile("Assets/SrcWords/Words.txt");
        Debug.Log("InitAppState inizializzato");
        
    }

    List<string> GetCardTagsFromFile(string path_file)
    {   
        if (!File.Exists(path_file))
        {
            throw new FileNotFoundException("Il file " + path_file + " non è stato trovato");
        }
        string content = File.ReadAllText(path_file);

        return content.Split("\n").ToList();
    }

    // Update is called once per frame
   /* void Update()
    {
        
    }*/
}
