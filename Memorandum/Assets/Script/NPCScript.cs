using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCScript : Interactable
{
    [SerializeField] private AppState _appstate;
    private bool _active = false;
    [SerializeField] private Camera _target;
    [SerializeField] private List<AudioClip> _audio_dialogues;
    [SerializeField] private TextMeshPro _promptText;

    private List<string> _dialogues;
    private int _indexDialogues = 0;
    private float amp = 0.0003f;
    private float freq = 2.0f;
    private Vector3 _position;
    [SerializeField] private GameObject _finishPosition;
    [SerializeField] private GameObject _npcPos;
    // Start is called before the first frame update
    void Start()
    {
        //_promptText = GetComponentInChildren<TextMeshPro>();
        _promptText.text = "Hei! Ti senti un po' spaesato chiedi pure a me, sono qui per aiutarti.";
        //_target = Camera.main;
        _appstate.gameObject.GetComponent<ActionLauncher>().TestIsReadyEvent += TestIsReady;
        _dialogues = InitAppState.GetLinesFromFile("dialoghi");
    }

    // Update is called once per frame
    void Update()
    {

        Floating();
        if (_active)
        {
            WatchTMP();
        }
    }

    private void TestIsReady(bool isReady)
    {
        if(_appstate.GetRoomIndex() == 0 && isReady && _active)
        {
            _promptText.text = _dialogues[1];
        }
    }

    public void WinMiniGame()
    {
        if (_appstate.GetRoomIndex() == 0 && _active)
        {
            _promptText.text = _dialogues[2];
        }
    }

    public void FinishGame()
    {
        //if (_appstate.GetRoomIndex() == _appstate.GetNumRoom())
        //{
            Debug.Log("Gioco finito : il tato si sposta in posizione " + _finishPosition.transform.position + " e la sua posizione attuale è " + _npcPos.transform.position);
            _promptText.text = _dialogues[3];
            if (_active)
            {
                //talk
            }
            _active = true;
            _npcPos.transform.position = _finishPosition.transform.position;
            _npcPos.transform.rotation = _finishPosition.transform.rotation;
        //}
    }

    private void Floating()
    {
        _position = new Vector3(0, Mathf.Sin(Time.time * freq) * amp, 0);
        gameObject.transform.position += _position;

    } 

    public override void Interact(GameObject caller)
    {
        if (!_active && caller.GetComponent<AppState>().GetRoomIndex() == 0)
        {
            _active = true;
            NextPhrase();
        }
        if(caller.GetComponent<AppState>().GetRoomIndex() == caller.GetComponent<AppState>().GetNumRoom())
        {
            SceneManager.LoadScene(1);
        }
        //_promptText.text = NextPhrase();
    }
    
    public string NextPhrase()
    {
        string result = "";

        if (_indexDialogues == _dialogues.Count)
        {
            _active = false;
            result = "";
            _indexDialogues = 0;
        }
        else if (_dialogues.Count > 0)
        {
            result = _dialogues[_indexDialogues];
            _indexDialogues++;
        }

        return result;
    }

    public void WatchTMP()
    {
        if (this.card_tag != null) GetComponentInChildren<TextMeshPro>().text = this.card_tag;
        Vector3 targetDirection = _target.transform.position - gameObject.transform.position;
        targetDirection.y = 0f;
        targetDirection.Normalize();

        float rotationStep = 2f * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, targetDirection, rotationStep, 0.0f);
        gameObject.transform.rotation = Quaternion.LookRotation(newDirection, gameObject.transform.up);
        
    }

    private List<string> LoadDialoguesFromDisk()
    {
        List<string> result = new List<string>();
        var file = Resources.Load<TextAsset>("dialoghi");
        if (file == null)
        {
            throw new FileNotFoundException("Il file " + "dialoghi" + " non è stato trovato");

        }

        result = new List<string>();

        foreach(var line in file.text.Split("\n").ToList())
        {
            //Debug.Log(line);    
            if (!line.Contains('*'))
            {
                result.Add(line);
            }
        }

        return result;
        
    }

}
