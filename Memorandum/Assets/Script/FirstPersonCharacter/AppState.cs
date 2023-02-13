using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class AppState : MonoBehaviour
{

    [SerializeField] private InitAppState init_app;
    [SerializeField] private MiniGames _winLauncher;

    private ActionLauncher _actionLauncher;
    //private InitAppState _init_app;
    private int _roomIndex = 0;
    private Dictionary<int, List<string>> _available_cardtags = new Dictionary<int, List<string>>();
    private List<string> _binded_cardtags = new List<string>();
    private int _availableIndex = 0;
    //private bool _isTutorial = false;
    private bool _isTest = false;
    private bool _isTestReady = false;
    //private int _bindedIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        _actionLauncher = GetComponent<ActionLauncher>();
        _available_cardtags = init_app._cardList;
        _winLauncher.GameWinEvent += OnWinAction;
    }


    public List<string> GetTestRoomTags(int roomIndex)
    {
        List<string> tags = new List<string>(); 
        for(int j = 0; j < roomIndex+1; j++)
        {
            tags.AddRange(_available_cardtags[j]);
        }
        return tags;
    }


    // Update is called once per frame
    /* void Update()
     {

     }*/

   /* public int GetRoomIndex()
    {
        return _roomIndex;
    }

    public bool IncrementRoomIndex()
    {
        _roomIndex++;
        return _roomIndex < _available_cardtags.Count;
    }
 */


    // Action Handler for finish the game

    private void OnWinAction(int newRoomIndex)
    {
        if(newRoomIndex == _available_cardtags.Count || newRoomIndex == 7)
        {
            Debug.Log("AppState riceve evento win con newRoomIndex = " + newRoomIndex);
            GoToGameMenu();
        }
    }

    public void GoToGameMenu()
    {
        return;
    }

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
        if (_available_cardtags[_roomIndex].Count > 0)
        {
            data = _available_cardtags[_roomIndex][_availableIndex];
        }
        return data;
    }

    public bool IsTestReady()
    {
        return _isTestReady;
    }
    
    public string BindCardTag()
    {
        string data = null;
        if (_available_cardtags.Count > 0)
        {
            data = GetCardTag();
            _binded_cardtags.Add(data);
            _available_cardtags[_roomIndex].RemoveAt(_availableIndex);
            ChangeIndex(-1);
        }
        if (_available_cardtags[_roomIndex].Count == 0)
        {
            Debug.Log("Finito assegnamento");
            _actionLauncher.OnTestActivation(true);
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
        _actionLauncher.OnTestActivation(false);
        _available_cardtags[_roomIndex].Add(_binded_cardtags[GetBindedIndex()]);
        _availableIndex = _available_cardtags.Count - 1;
        _binded_cardtags.RemoveAt(GetBindedIndex());
        GetBindedIndex();
    }

    public void ChangeIndex(float delta_index)
    {
        if (delta_index > 0 && _available_cardtags[_roomIndex].Count > 0)
        {
            if (_availableIndex + 1 < _available_cardtags[_roomIndex].Count)
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
                _availableIndex = _available_cardtags[_roomIndex].Count - 1;
            }
        }
        if (_available_cardtags[_roomIndex].Count > 0)
            Debug.Log(_availableIndex + " Parola Selezionata: " +  _available_cardtags[_roomIndex][_availableIndex]);
    }

    
}
