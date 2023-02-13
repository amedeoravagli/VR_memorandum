using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;


public class MiniGames : Interactable
{
    [SerializeField] private GameObject _actionLauncherGO;
    [SerializeField] private ActionLauncher _actionLauncher;
    [SerializeField] private int _roomIndex = -1;
    [SerializeField] private GameObject _playerPosition;
    private int _indexTagVerified = 0;
    private int _numError = 0;
    private List<string> _cardTagsToTest = new List<string>();
    public Action<int> GameWinEvent;

    //private Highlight _highlight;
    private bool _isReady = false;
    void Start()
    {
        //_highlight = GetComponent<Highlight>();
        //_actionLauncher = _actionLauncherGO.GetComponent<ActionLauncher>();
        AppState appstate = _actionLauncherGO.GetComponent<AppState>();
        _cardTagsToTest = appstate.GetTestRoomTags(_roomIndex);
        this.GameWinEvent += OnGameWin;
        if (_roomIndex == 0)
        {
            AttachReadyEvent();

        }
    }

    public void AttachReadyEvent()
    {
        Debug.Log("Minigioco " + _roomIndex + " in ascolto");
        _actionLauncher.TestIsReadyEvent += OnActionReadyReceived;
       

    }

    public void DetachtReadyEvent()
    {
        _actionLauncher.TestIsReadyEvent -= OnActionReadyReceived;
    }

    private void OnActionReadyReceived(bool isReady)
    {
        _isReady = isReady;
        Debug.Log("IsReady Event launch: " + isReady);
        //_highlight.ToggleHighlight(true, true);
        //ToggleHighlight(true, true);
    }

    public override void Interact(GameObject caller)
    {
        AppState appstate = caller.GetComponent<AppState>();
        Debug.Log("Interazione con Minigioco: appstate è " + appstate.IsTest() + " _isReady " + _isReady);
        if (!appstate.IsTest() && _isReady)
        {
            //_highlight.ToggleHighlight(false, false);  
            //ToggleHighlight(false, false);  
             
            caller.transform.position = _playerPosition.transform.position;
            RandomizeTags();
            appstate.ChangeFase();
        }
    }

    private void RandomizeTags()
    {

        Debug.Log("Associazione random dei tag agli Element Test");
        int numCardTags = _cardTagsToTest.Count;
        List<string> tags = _cardTagsToTest;
        if (GetComponentsInChildren<TestElement>() == null || GetComponentsInChildren<TestElement>().Length == 0)
        {
            Debug.Log("TestElement non trovati");
        }
        else
        {
            Debug.Log("Numero di TestElement trovati: " + GetComponentsInChildren<TestElement>().Length+ " numCardTags: " + numCardTags);
        }
        string tag = "";
        foreach (var element in GetComponentsInChildren<TestElement>())
        {
            tag = tags[UnityEngine.Random.Range(0, numCardTags)];
            Debug.Log("Element Test name : "+element.name +" Tag associato "+tag);
            element.AddRandomTag(tag);
            
            tags.Remove(tag);
            numCardTags--;
        }
    }

    private void Win()
    {
        if ( GameWinEvent != null)
        {
            DetachtReadyEvent();
            GameWinEvent.Invoke(_roomIndex+1);
        }
    }

    private void OnGameWin(int newRoomIndex)
    {
        if(newRoomIndex == _roomIndex)
        {
            
            AttachReadyEvent();
        }
    }

    public void VerifyTags(string card_tag)
    {
        if(card_tag != _cardTagsToTest[_indexTagVerified + 1])
        {
            _numError++;
        }
        else
        {
            _indexTagVerified++;
            if(_indexTagVerified == _cardTagsToTest.Count)
            {
                Win();
            }
        }
    }



    // Start is called before the first frame update
    /*void Start()
    {
        
    }
    */
    // Update is called once per frame
    /*void Update()
    {
        if (_isReady)
            EmissionFaded();
    }*/

}
