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
    [SerializeField] public int numRooms = 3;

    // Start is called before the first frame update
    void OnEnable()
    {
        List<string> list;
        list = GetCardTagsFromFile("Words");
        List<int> indexTaken = new List<int>();
        for(int j = 0; j < ((numRooms-1)*tagPerRoom)+3; j++)
        {
            int index = Random.Range(0, list.Count);
            while (indexTaken.Contains(index) && indexTaken.Count != 0)
            {
                index = Random.Range(0, list.Count);
            }
            indexTaken.Add(index);
            if (j == 0)
            {
                _cardList.Add(0, new List<string>());
            }
            else if ((j-3)%5 == 0)
            {
                int a = (j - 3) / 5 + 1;
                _cardList.Add(a, new List<string>());
            }
            _cardList[_cardList.Count - 1].Add(list[indexTaken.Last()]);

        }
        
       /* 
        int i = 0;
        _cardList.Add(0, list.GetRange(i, 3));
        i = 3;
        for (int j = 1; j < numRooms ; j++)
        {
            _cardList.Add(j, list.GetRange(i, 5));
            //if ( k > list.Count-i)
            //{
            //    _cardList.Add(j, list.GetRange(i, list.Count - i));
                //numRooms = j+1;
            //    break;
            //}
            //_cardList.Add(j, list.GetRange(i, k));

            i += 5; 
            //k = 5;
        }*/
        Debug.Log("InitAppState inizializzato");
        
    }

    private List<string> GetCardTagsFromFile(string path_file)
    {   
        
        var file = Resources.Load<TextAsset>(path_file);
        if(file == null)
        {
            throw new FileNotFoundException("Il file " + path_file + " non è stato trovato");

        }
        //string content = File.ReadAllText(path_file);

        return file.text.Split("\n").ToList();
    }

    // Update is called once per frame
   /* void Update()
    {
        
    }*/
}
