using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class NPCScript : Interactable
{
    [SerializeField] private AppState _appstate;
    private bool _active = false;
    [SerializeField] private Camera _target;
    [SerializeField] private Dictionary<int, AudioClip> _audio_dialogues;
    
    private List<string> _dialogues;
    private int _indexDialogues = 0;
    private TextMeshPro _promptText;
    
    // Start is called before the first frame update
    void Start()
    {
        _promptText = GetComponentInChildren<TextMeshPro>();
        _promptText.text = "Hei! Ti senti un po' spaesato chiedi pure a me, sono qui per aiutarti.";
        //_target = Camera.main;
        _dialogues = LoadDialoguesFromDisk();
    }

    // Update is called once per frame
    void Update()
    {
        if (_active)
        {
            WatchTMP();
        }
    }

    public override void Interact(GameObject caller)
    {
        if (!_active && caller.GetComponent<AppState>().GetRoomIndex() == 0)
        {
            _active = true;
            NextPhrase();
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
