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
    public Dictionary<int, List<string>> _cardList = new Dictionary<int, List<string>>();
    public bool isTutorial = false;
    [SerializeField] public int tagPerRoom = 5;
    [SerializeField] public int numRooms = 4;

    // Start is called before the first frame update
    void OnEnable()
    {
        List<string> list;
        list = GetCardTagsFromFile("Assets/SrcWords/Words.txt");
        int i = 0;
        _cardList.Add(0, list.GetRange(i, 3));
        i = 3;
        for (int j = 1; j < numRooms ; j++)
        {
            _cardList.Add(j, list.GetRange(i, 5));
            /*if ( k > list.Count-i)
            {
                _cardList.Add(j, list.GetRange(i, list.Count - i));
                //numRooms = j+1;
                break;
            }*/
            //_cardList.Add(j, list.GetRange(i, k));

            i += 5; 
            //k = 5;
        }
        Debug.Log("InitAppState inizializzato");
        
    }

    private List<string> GetCardTagsFromFile(string path_file)
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
