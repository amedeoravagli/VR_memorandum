using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MiniGames : Interactable
{
    [SerializeField] private GameObject _actionLauncherGO;
    [SerializeField] private ActionLauncher _actionLauncher;
    [SerializeField] private int _roomIndex = -1;
    [SerializeField] private Door _door;
    [SerializeField] private AudioClip _audio_errore;
    [SerializeField] private AudioClip _audio_sconfitta;
    [SerializeField] private AudioClip _audio_vittoria;
    private bool _changeColor;
    [SerializeField] private TextMeshPro errors_text;

    //[SerializeField]
    private AudioSource _audioSource;

    private int _indexTagVerified = 0;
    private int _numError = 3;
    private List<string> _cardTagsToTest = new List<string>();
    //public Action<int> GameWinEvent;
    
    //private Highlight _highlight;
    private bool _isReady = false;
   
    void Start()
    {
        //_highlight = GetComponent<Highlight>();
        //_actionLauncher = _actionLauncherGO.GetComponent<ActionLauncher>();
        AppState appstate = _actionLauncherGO.GetComponent<AppState>();
        _audioSource = _actionLauncherGO.GetComponent<AudioSource>();
        //GameWinEvent += OnGameWin;
        if (_roomIndex == 0)
        {
            _cardTagsToTest = appstate.GetTestRoomTags(_roomIndex);
            //AttachReadyEvent();
        }
        AttachReadyEvent();
    }

    public void AttachReadyEvent()
    {
        Debug.Log("Minigioco " + _roomIndex + " in ascolto");
        _actionLauncher.TestIsReadyEvent += OnActionReadyReceived;
    }

    public void DetachtReadyEvent()
    {
        _actionLauncher.TestIsReadyEvent -= OnActionReadyReceived;
        errors_text.text = "";
    }

    private void OnActionReadyReceived(bool isReady)
    {
        
        AppState appstate = _actionLauncherGO.GetComponent<AppState>();
        _cardTagsToTest = appstate.GetTestRoomTags(_roomIndex);
        if (appstate.GetRoomIndex() == _roomIndex)
        {
            
            _isReady = isReady;
            if (isReady)
            {
                ToggleHighlight(true, true);
            }
            Debug.Log("IsReady Event launch: " + isReady + " Room index "+ _roomIndex);
            //ToggleHighlight(true, true);
        }
       
    }

    public override void Interact(GameObject caller)
    {
        AppState appstate = caller.GetComponent<AppState>();
        Debug.Log("Interazione con Minigioco: appstate è " + appstate.IsTest() + " _isReady " + _isReady);
        if (!appstate.IsTest() && appstate.GetRoomIndex() == _roomIndex && appstate.NumberCardtagLast() == 0)
        {
            errors_text.text = "Numero tentativi rimasti :" + _numError;
            ToggleHighlight(false, true);
            _cardTagsToTest = appstate.GetTestRoomTags(_roomIndex);
            RandomizeTagsOnElement();
            appstate.ChangePhase();
            GetComponent<BoxCollider>().enabled = false;
            MakeElementInteractive(true);
        }
    }

    private void MakeElementInteractive(bool interactable)
    {
        foreach (var element in GetComponentsInChildren<TestElement>())
        {
            element.gameObject.GetComponent<BoxCollider>().enabled = interactable;

            Debug.Log("Element Test name : " + element.name + " collider abilitato " + element.gameObject.GetComponent<BoxCollider>().enabled);

        }
    }

    private void RandomizeTagsOnElement()
    {

        Debug.Log("Associazione random dei tag agli Element Test");
        int numCardTags = _cardTagsToTest.Count;
        List<string> tags = new List<string>(_cardTagsToTest);
        if (GetComponentsInChildren<TestElement>() == null || GetComponentsInChildren<TestElement>().Length == 0)
        {
            Debug.Log("TestElement non trovati");
        }
        else
        {
            Debug.Log("RoomIndex: "+_roomIndex +" Numero di TestElement trovati: " + GetComponentsInChildren<TestElement>().Length+ " numCardTags: " + numCardTags);
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

    public void Win()
    {
        //if ( GameWinEvent != null)
        //{
        Debug.Log("Game win event stanza :" + _roomIndex);
        _audioSource.PlayOneShot(_audio_vittoria);
        MakeElementInteractive(false);

        _door.OpenDoor(-90);
        _actionLauncherGO.GetComponent<AppState>().OnWinAction(_roomIndex + 1);
        //DetachtReadyEvent();
      //  GameWinEvent.Invoke(_roomIndex+1);
        //}
    }

    private void Lose()
    {
        _audioSource.PlayOneShot(_audio_sconfitta);
        foreach (var text in GetComponentsInChildren<TextMeshPro>())
        {
            text.color = Color.red;
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(7);//carico scena di sconfitta
        Debug.Log("Hai Perso");
    }

   /* private void OnGameWin(int newRoomIndex)
    {
        Debug.Log("Vittoria il nuovo indice è : " + newRoomIndex);
        Debug.Log("la stanza " + _roomIndex + " riceve nuovo indice " + newRoomIndex);
        if(newRoomIndex == _roomIndex)
        {
            Debug.Log("la stanza " + _roomIndex + " riceve nuovo indice " + newRoomIndex);
            AttachReadyEvent();
        }
    }*/

    public void VerifyTags(string card_tag)
    {
        Debug.Log("Lunghezza _cardTagsToTest: " + _cardTagsToTest.Count + " parola all'indice: " + _indexTagVerified);
        Debug.Log("Indice da verificare: " + _indexTagVerified + " confronto card tag: " + card_tag);
        if(card_tag != _cardTagsToTest[_indexTagVerified])
        {

            _changeColor = true;
            _numError--;
            errors_text.text = "Numero tentativi rimasti :" + _numError;
            StartCoroutine(Timer());
            
            _indexTagVerified = 0;
            if(_numError == 0)
            {
                Lose();
            }
            else
            {
                if(_audio_errore == null)
                {
                    Debug.Log("");
                }
                _audioSource.PlayOneShot(_audio_errore);
            }
        }
        else
        {
            _changeColor = false;
            _indexTagVerified++;
            if(_indexTagVerified == _cardTagsToTest.Count)
            {
                Debug.Log(" _indexTagVerified == _cardTagsToTest.Count Valore _indexTagVerified " + _indexTagVerified);
                _indexTagVerified = 0;
                Win();
            }
        }

    }

    public bool getChangeColor()
    {
        return _changeColor;
    }

    //creo una courite
    public IEnumerator Timer()
    {
        // courutineStarted = true;
        yield return new WaitForSeconds(1.5f);
        changeColorInWhite();
        // courutineStarted = false;
    }

    private void changeColorInWhite()
    {
        foreach (var text in GetComponentsInChildren<TextMeshPro>())
        {
            text.color = Color.white;
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
