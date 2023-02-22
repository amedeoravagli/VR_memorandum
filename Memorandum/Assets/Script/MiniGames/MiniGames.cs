using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
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
    private int _color = -1;
    [SerializeField] private TextMeshPro errors_text;

    //[SerializeField]
    private AudioSource _audioSource;

    private int _indexTagVerified = 0;
    private int _numError = 3;
    private List<string> _cardTagsToTest = new List<string>();
    //public Action<int> GameWinEvent;
    
    //private Highlight _highlight;
    private bool _isReady = false;
    private TestElement _elementToVerify;

   
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
            OnActiveMinigameEvent(0);
            //AttachReadyEvent();
        }
        _actionLauncher.ActiveMinigameEvent += OnActiveMinigameEvent;
    }

    public void OnActiveMinigameEvent(int index)
    {
        if(index == _roomIndex)
        {
            _actionLauncher.TestIsReadyEvent += OnActionReadyReceived;
            _actionLauncher.VerifyInputStringEvent += OnVerifyInput;
        }
        else
        {
            _actionLauncher.TestIsReadyEvent -= OnActionReadyReceived;
            _actionLauncher.VerifyInputStringEvent -= OnVerifyInput;
        }
        Debug.Log("Minigioco " + _roomIndex + " in ascolto");
    }


    private void OnActionReadyReceived(bool isReady)
    {
        
        AppState appstate = _actionLauncherGO.GetComponent<AppState>();
        Debug.Log("appstate.getRoomIndex: " + appstate.GetRoomIndex() + " Room index " + _roomIndex);
        if (appstate.GetRoomIndex() == _roomIndex)
        {
            _cardTagsToTest = appstate.GetTestRoomTags(_roomIndex);
            _isReady = isReady;
            if (isReady)
            {
                ToggleHighlight(true, true);
            }
            Debug.Log("IsReady Event launch: " + isReady + " Room index "+ _roomIndex);
        }
       
    }

    public override void Interact(GameObject caller)
    {
        AppState appstate = caller.GetComponent<AppState>();
        Debug.Log("Interazione con Minigioco: appstate è " + appstate.IsTest() + " _isReady " + _isReady);
        if (!appstate.IsTest() && appstate.GetRoomIndex() == _roomIndex && appstate.NumberCardtagLast() == 0)
        {
            _actionLauncher.OnStatusIsTest(true, _roomIndex);
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
        _actionLauncher.OnStatusIsTest(false, _roomIndex);
        _actionLauncherGO.GetComponent<AppState>().OnWinAction(_roomIndex + 1);
        StartCoroutine(Timer());
        WipeText();

        //DetachtReadyEvent();
        //  GameWinEvent.Invoke(_roomIndex+1);
        //}
    }

    private void Lose()
    {
        _audioSource.PlayOneShot(_audio_sconfitta);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene(3);//carico scena di sconfitta
        Debug.Log("Hai Perso");
    }

    public void SetElementToVerify(TestElement element)
    {
        _elementToVerify = element; 
    }

    public void OnVerifyInput(string input)
    {
        bool found = false;
        Debug.Log("Minigames: Input da verificare " + input);
        /*foreach (var element in GetComponentsInChildren<TestElement>())
        {
            
            Debug.Log("Minigames: Element selezionato trovato " + element.GetTag() + " == " + input + " lunghezza input " + input.Length + " lunghezza card tag " + element.GetTag().Trim().Length);

            if (element.GetTag() == input && VerifyOrderTag(element.GetTag()))
            {
                Debug.Log("Trovato uguaglianza in ordine in " + element.name + " con cardtag " + element.GetTag());
                element.ChangeColor(Color.green); // verde
                found = true;
                break;

            }
            else if (element.GetTag() == input)
            {
                Debug.Log("Trovato uguaglianza in " + element.name + " con cardtag " + element.GetTag());
                element.ChangeColor(Color.yellow); // giallo
                found = true;
                break;
            }
            
        }*/

        Debug.Log("Minigames: Element selezionato trovato " + _elementToVerify.GetTag() + " == " + input + " lunghezza input " + input.Length + " lunghezza card tag " + _elementToVerify.GetTag().Trim().Length);

        if (_elementToVerify.GetTag() == input && VerifyOrderTag(_elementToVerify.GetTag()))
        {
            Debug.Log("Trovato uguaglianza in ordine in " + _elementToVerify.name + " con cardtag " + _elementToVerify.GetTag());
            _elementToVerify.ChangeColor(Color.green, input); // verde
            found = true;
            if (_indexTagVerified == _cardTagsToTest.Count)
            {
                Debug.Log(" _indexTagVerified == _cardTagsToTest.Count Valore _indexTagVerified " + _indexTagVerified);
                _indexTagVerified = 0;
                Win();
            }

        }
        else if (_elementToVerify.GetTag() == input)
        {
            Debug.Log("Trovato uguaglianza in " + _elementToVerify.name + " con cardtag " + _elementToVerify.GetTag());
            _elementToVerify.ChangeColor(Color.yellow, input); // giallo
            found = true;
        }
        if (!found)
        {
            //changeColorInRed(); // rosso

            _elementToVerify.ChangeColor(Color.red, input);
            _numError--;
            errors_text.text = "Numero tentativi rimasti :" + _numError;
            //StartCoroutine(Timer());
            new WaitForSeconds(1.5f);
            _elementToVerify.ResetTextElement();
            //_elementToVerify.ChangeColor(Color.white);
            //_indexTagVerified = 0;
            if (_numError == 0)
            {
                Lose();
            }
            else
            {
                if (_audio_errore == null)
                {
                    Debug.Log("");
                }
                _audioSource.PlayOneShot(_audio_errore);
            }    
        }

    }

    public bool VerifyOrderTag(string card_tag)
    {
        Debug.Log("Lunghezza _cardTagsToTest: " + _cardTagsToTest.Count + " parola all'indice: " + _indexTagVerified);
        Debug.Log("Indice da verificare: " + _indexTagVerified + "parola " + _cardTagsToTest[_indexTagVerified] + " confronto card tag: " + card_tag);
        bool result = false;
        if(card_tag == _cardTagsToTest[_indexTagVerified].Trim())
        {
            result = true;
            _indexTagVerified++;
            
        }
        return result;
    }

    public int getChangeColor()
    {
        return _color;
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
    
    private void changeColorInRed()
    {
        foreach (var text in GetComponentsInChildren<TextMeshPro>())
        {
            text.color = Color.red;
        }
    }

    private void ResetText()
    {
        foreach (var element in GetComponentsInChildren<TestElement>())
        {
            element.ResetTextElement();
        }
    }

    private void WipeText()
    {
        foreach (var element in GetComponentsInChildren<TestElement>())
        {
            element.Blanking();
        }
    }

}
