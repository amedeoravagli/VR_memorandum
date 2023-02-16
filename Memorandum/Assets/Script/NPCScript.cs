using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class NPCScript : Interactable
{
    [SerializeField] private AppState _appstate;
    private bool _active = false;
    [SerializeField] private Camera _target;
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
        if(!_active) _active= true;

        if (_indexDialogues == _dialogues.Count)
        {
            _active = false;
            _promptText.text = "";
            _indexDialogues = 0;
        }
        else if (_dialogues.Count > 0)
        {
            _promptText.text = _dialogues[_indexDialogues];
            _indexDialogues++;
        }

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
        string path_file = "Assets/SrcWords/dialoghi.txt";
        if (!File.Exists(path_file))
        {
            throw new FileNotFoundException("Il file " + path_file + " non è stato trovato");
        }
        string content = File.ReadAllText(path_file);

        result = new List<string>();

        foreach(var line in content.Split("\n").ToList())
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
