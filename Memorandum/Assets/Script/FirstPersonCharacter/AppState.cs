using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AppState : MonoBehaviour
{

    [SerializeField] private InitAppState init_app;
    [SerializeField] private MiniGames _winLauncher;
    [SerializeField] private NPCScript _npc;
    private ActionLauncher _actionLauncher;
    //private InitAppState _init_app;
    private int _roomIndex = 0;
    private Dictionary<int, List<string>> _available_cardtags = new Dictionary<int, List<string>>();
    private List<string> _binded_cardtags = new List<string>();
    private int _availableIndex = 0;
    private bool _isTest = false;
    //private int _bindedIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        _actionLauncher = GetComponent<ActionLauncher>();
        _available_cardtags = init_app._cardList;
        Debug.Log("Appstate: numRoom = " + _available_cardtags.Count);
        

        //_winLauncher.GameWinEvent += OnWinAction;
    }

    public List<string> GetTestRoomTags(int roomIndex)
    {
        return _binded_cardtags;
    }
    
    public int GetRoomIndex()
    {
        return _roomIndex;
    }

    public int GetNumRoom()
    {
        return _available_cardtags.Count;
    }

    public void SetRoomIndex(int newRoomIndex)
    {
        _roomIndex= newRoomIndex;   
    }

    // Action Handler for finish the game
    public void OnWinAction(int newRoomIndex)
    {
        ChangePhase();
        if (newRoomIndex == _available_cardtags.Count)
        {
            Debug.Log("AppState riceve evento win con newRoomIndex = " + newRoomIndex);
            _npc.FinishGame();
            //GoToGameMenu();
        }
        
       
        _npc.WinMiniGame(newRoomIndex);
        _actionLauncher.ActivateMinigame(newRoomIndex);
        _roomIndex = newRoomIndex;
        Debug.Log(_roomIndex);
    }

    public void GoToGameMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(2); // carico scena vittoria
    }

    public int NumberCardtagLast()
    {
        return _available_cardtags[_roomIndex].Count;
    }

    public void ChangePhase()
    {
        _availableIndex = 0;
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
        if (_roomIndex < _available_cardtags.Count && _available_cardtags[_roomIndex].Count > 0 && !_isTest)
        {
            data = _available_cardtags[_roomIndex][_availableIndex];
        }
        return data;
    }

    public string BindCardTag()
    {
        string data = null;
        if (_available_cardtags[_roomIndex].Count > 0 && !_isTest)
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
        _availableIndex = _available_cardtags[_roomIndex].Count - 1;
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
    }

    
}
