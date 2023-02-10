using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class AppState : MonoBehaviour
{

    [SerializeField] private InitAppState init_app;
    //private InitAppState _init_app;
    private List<string> _available_cardtags = new List<string>();
    private List<string> _binded_cardtags = new List<string>();
    private int _availableIndex = 0;
    //private bool _isTutorial = false;
    private bool _isTest = false;
    //private int _bindedIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        //_init_app = GetComponent<InitAppState>();
        _available_cardtags = init_app._cardList;
    }

    // Update is called once per frame
    /* void Update()
     {

     }*/

 
    public void ChangeFase()
    {
        _isTest = !_isTest;
    }

    public bool IsTest() { return _isTest; }
    public string GetDetachableCardTag()
    {
        return _binded_cardtags[GetBindedIndex()];    
    }

    public string GetCardTag()
    {
        string data = "";
        if (_available_cardtags.Count > 0)
        {
            data = _available_cardtags[_availableIndex];
        }

        return data;
    }

    
    public string BindCardTag()
    {
        string data = null;
        if (_available_cardtags.Count > 0)
        {
            data = _available_cardtags[_availableIndex];
            _binded_cardtags.Add(data);
            _available_cardtags.RemoveAt(_availableIndex);
            ChangeIndex(-1);
        }
        return data;
    }

    int GetBindedIndex()
    {
        if (_binded_cardtags.Count > 0)
            return _binded_cardtags.Count - 1;
        else 
            return 0;   
    }
    public void UnbindCardTag()
    {
        _available_cardtags.Add(_binded_cardtags[GetBindedIndex()]);
        _availableIndex = _available_cardtags.Count - 1;
        _binded_cardtags.RemoveAt(GetBindedIndex());
        GetBindedIndex();
        
    }

    public void ChangeIndex(float delta_index)
    {
        if (delta_index > 0)
        {
            if (_availableIndex + 1 < _available_cardtags.Count)
            {
                _availableIndex++;
            }
            else
            {
                _availableIndex = 0;
            }
        }
        else
        {
            if (_availableIndex - 1 >= 0)
            {
                _availableIndex--;
            }   
            else
            {
                _availableIndex = _available_cardtags.Count - 1;
            }
        }
        if (_available_cardtags.Count > 0)
            Debug.Log(Input.mouseScrollDelta.y + " Parola Selezionata: " +  _available_cardtags[_availableIndex]);
    }

    
}
