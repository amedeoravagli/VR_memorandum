using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static Unity.VisualScripting.Member;

public class NPCScript : Interactable
{
    [SerializeField] private AppState _appstate;
    private bool _active = false;
    [SerializeField] private Camera _target;
    [SerializeField] private List<AudioClip> _audio_dialogues;
    private int _index_audio = 0;
    private AudioSource _source;
    [SerializeField] private TextMeshPro _promptText;
    
    private List<string> _dialogues;
    private int _indexDialogues = 0;
    private float amp = 0.0003f;
    private float freq = 2.0f;
    private Vector3 _position;
    private int _numSeqAudio = 0;
    private bool _destinationA = false;
    private bool _destinationB = false;
    private bool _destinationC = false;

    [SerializeField] private GameObject _finishPosition;
    [SerializeField] private GameObject _npcPos;

    [SerializeField] private GameObject _npcPos_camera_da_letto;
    [SerializeField] private GameObject _npcPos_soggiorno;
    [SerializeField] private NavMeshAgent _tato;

    // Start is called before the first frame update
    void Start()
    {
        //_promptText = GetComponentInChildren<TextMeshPro>();
        _promptText.text = "Hei! Ti senti un po' spaesato chiedi pure a me, sono qui per aiutarti.";
        //_target = Camera.main;
        _source = GetComponent<AudioSource>();
        _appstate.gameObject.GetComponent<ActionLauncher>().TestIsReadyEvent += TestIsReady;
        _appstate.gameObject.GetComponent<ActionLauncher>().StatusIsTestEvent += StartTest;
        _dialogues = new List<string>();
        foreach (var d in InitAppState.GetLinesFromFile("dialoghi"))
        {
            if (!d.Contains("*"))
            {
                _dialogues.Add(d);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

        Floating();

        //Debug.Log("velocity" + _tato.velocity.sqrMagnitude);
        if (_tato.velocity == Vector3.zero && _appstate.GetRoomIndex() != 0)
        {
            if (_appstate.GetRoomIndex() == 1 && !_destinationA)
            {
                _destinationA = true;
                //Debug.Log("Sono in camera da letto");
                _tato.transform.rotation = _npcPos_camera_da_letto.transform.rotation;

            }
            else if (_appstate.GetRoomIndex() == 2 && !_destinationB)
            {
                _destinationB = true;
                _tato.transform.rotation = _npcPos_soggiorno.transform.rotation;
            }
            else if (_appstate.GetRoomIndex() == 3 && !_destinationC)
            {
                _destinationC = true;
                _tato.transform.rotation = _finishPosition.transform.rotation;
            }
        }

    }


    private IEnumerator playAudioSequentially()
    {
        yield return null;

        //1.Loop through each AudioClip
        for (int i = 0; i < _numSeqAudio; i++)
        {
            //2.Assign current AudioClip to audiosource
            _source.clip = _audio_dialogues[_index_audio+i];

            //3.Play Audio
            _source.Play();

            //4.Wait for it to finish playing
            while (_source.isPlaying)
            {
                yield return null;
            }

            //5. Go back to #2 and play the next audio in the adClips array
        }
        
    }

    private void TestIsReady(bool isReady)
    {
        if(_appstate.GetRoomIndex() == 0 && isReady && _active)
        {
            if(_source.isPlaying)
            {
                _source.Stop();
            }
            //2.Assign current AudioClip to audiosource
            _source.clip = _audio_dialogues[1];

            //3.Play Audio
            _source.Play();
        }
    }

    private void StartTest(bool isTest, int index)
    {
        if(isTest && index == 0)
        {
            if (_source.isPlaying)
            {
                _source.Stop();
            }

            //2.Assign current AudioClip to audiosource
            _source.clip = _audio_dialogues[2];

            //3.Play Audio
            _source.Play();
        }
    }

    public void WinMiniGame(int indexRoom)
    {
        if (_appstate.GetRoomIndex() == 0 && _active)
        {
            _source.clip = _audio_dialogues[3];

            //3.Play Audio
            _source.Play();
            
        }

        WalkingNPC(indexRoom);
    }


    private void WalkingNPC(int newRoomIndex)
    {
        //camminare
        if (newRoomIndex == 1)
        {
            _tato.SetDestination(_npcPos_camera_da_letto.transform.position);
            Debug.Log("SONO NELLA STANZA DA LETTO");
        }
        else if (newRoomIndex == 2)
        {
            _tato.SetDestination(_npcPos_soggiorno.transform.position);
            Debug.Log("SONO NEL SOGGIORNO");
        }
        else if (newRoomIndex == 3)
        {
            _tato.SetDestination(_finishPosition.transform.position);
        }

    }


    public void FinishGame()
    {
        //if (_appstate.GetRoomIndex() == _appstate.GetNumRoom())
        //{
        Debug.Log("Gioco finito : il tato si sposta in posizione " + _finishPosition.transform.position + " e la sua posizione attuale è " + _npcPos.transform.position);
        _promptText.text = _dialogues[3];
            
        _active = true;
        _npcPos.transform.position = _finishPosition.transform.position;
        _npcPos.transform.rotation = _finishPosition.transform.rotation;
        _index_audio = 4;
        _numSeqAudio = 2;
        StartCoroutine(playAudioSequentially());
            
       
        //}
    }

    private void Floating()
    {
        if(Time.timeScale != 0)
        {
            _position = new Vector3(0, Mathf.Sin(Time.time * freq) * amp, 0);
            gameObject.transform.position += new Vector3(0, Mathf.Sin(Time.time * freq) * amp, 0);

        }

    }

    public override void Interact(GameObject caller)
    {
        if (_tato.velocity == Vector3.zero)
        {
            WatchTMP();
        }
        if (!_active && caller.GetComponent<AppState>().GetRoomIndex() == 0)
        {
            _active = true;
            _source.clip = _audio_dialogues[0];

            //3.Play Audio
            _source.Play();
            
            StartCoroutine(TalkWithNPC());
        }
        if (caller.GetComponent<AppState>().GetRoomIndex() != 0)
        {
            _active = true;
            _index_audio = 1; _numSeqAudio = 2;
            StartCoroutine(TalkWithNPC());  
            StartCoroutine(TalkWithNPC());
        }

        if (caller.GetComponent<AppState>().GetRoomIndex() == caller.GetComponent<AppState>().GetNumRoom())
        {
            SceneManager.LoadScene(1);
        }
        //_promptText.text = NextPhrase();
    }
    
    

    private IEnumerator TalkWithNPC()
    {
        while(_indexDialogues < _dialogues.Count)
        {
            _promptText.text = _dialogues[_indexDialogues];
            _indexDialogues++;

            yield return new WaitForSeconds(6);
        }
        if (_indexDialogues == _dialogues.Count)
        {
            //_active = false;
            _promptText.text = "";
            _indexDialogues = 0;
        }
    }

    public void WatchTMP()
    {
        if (this.card_tag != null) GetComponentInChildren<TextMeshPro>().text = this.card_tag;
        Vector3 targetDirection = _target.transform.position - gameObject.transform.position;
        //targetDirection.y = 0f;
        //targetDirection.Normalize();

        //float rotationStep = 2f * Time.deltaTime;

        //Vector3 newDirection = Vector3.RotateTowards(gameObject.transform.forward, targetDirection, rotationStep, 0.0f);
        //gameObject.transform.rotation = Quaternion.LookRotation(newDirection, gameObject.transform.up);
        gameObject.transform.rotation = Quaternion.LookRotation(targetDirection, gameObject.transform.up);
        
    }

}
