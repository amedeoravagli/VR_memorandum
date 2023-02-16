using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using StarterAssets;
using UnityEngine.SceneManagement;
//using UnityEditor.Experimental.GraphView;

public class FPSInteractionManager : MonoBehaviour
{

    [SerializeField] private Transform _fpsCameraT;
    [SerializeField] private bool _debugRay;
    [SerializeField] private float _interactionDistance;
    private AudioSource _audioSource;
    private Interactable _pointingInteractable;
    private Taggable _pointingTaggable;
    private CharacterController _fpsController;
    private Vector3 _rayOrigin;

    private AppState _appstate;

    private float _minScrollLimit = 1.0f;

    bool isPaused;
    public GameObject pnlPause;

    [SerializeField] private AudioClip _audio_associazione;
    [SerializeField] private AudioClip _audio_selezione;
    //private Taggable _taggedObject = null;

    // Start is called before the first frame update
    void Start()
    {
        _audioSource= GetComponent<AudioSource>(); 
        _appstate = GetComponent<AppState>();
        _fpsController = GetComponent<CharacterController>();
    }

    void Update()
    {
        _rayOrigin = _fpsCameraT.position + _fpsController.radius * _fpsCameraT.forward;

        if (!isPaused)
        {
            CheckInteraction();
            CheckScrollMouseWheel();
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            //se rilasciamo il tasto esc andiamo in pausa
            ChangePauseStatus();
        }

        if (_debugRay)
            DebugRaycast();
    }

    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

    void UpdateGamePause()
    {
        if (isPaused)
        {
            //ferma il gioco
            Time.timeScale = 0;

        }
        else
        {
            //riavvia il tempo
            Time.timeScale = 1;

        }
        pnlPause.SetActive(isPaused);
        //GetComponent<UnityStandardAssets.character.FirstPerson.RigidbodyFirstPersonController>().mouseLook.SetCursorLock(isPaused);
        if (!isPaused)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void ChangePauseStatus()
    {
        isPaused = !isPaused;
        UpdateGamePause();
    }

    private void CheckScrollMouseWheel()
    {
        float delta = Input.mouseScrollDelta.y;
        if (delta == _minScrollLimit || delta == -_minScrollLimit)
        {

            _appstate.ChangeIndex(delta);

        }
    }

    private void CheckInteraction()
    {
        Ray ray = new Ray(_rayOrigin, _fpsCameraT.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _interactionDistance))
        {
            //Check if is interactable
            Interactable _pointing = hit.transform.GetComponent<Interactable>();
            if (_pointing)
            {
                if (_pointingInteractable && _pointingInteractable.name != _pointing.name)
                {
                    _pointingInteractable.GetComponent<Taggable>()?.DisableTMP();
                }

                Taggable _pointingTaggable = _pointing.GetComponent<Taggable>();

                if (_pointingTaggable && !GetComponent<AppState>().IsTest())
                {
                    _pointingTaggable.WatchTMP();

                    if (_pointing != _pointingInteractable)
                    {
                        _pointingTaggable.EnableTMP();
                        if (_pointingInteractable)
                        {
                            //_pointingInteractable.GetComponent<Taggable>()?.DisableTMP();
                            _pointingInteractable.GetComponent<Highlight>()?.ToggleHighlight(false, false);
                        }
                        _pointing.GetComponent<Highlight>()?.ToggleHighlight(true, false);
                    }
                }

                _pointingInteractable = _pointing;
                if (Input.GetMouseButtonDown(0))
                {

                    _pointingInteractable.Interact(gameObject);
                    if (_pointing.GetComponent<TestElement>() != null) //
                    {
                        _audioSource.PlayOneShot(_audio_selezione);
                    }
                    else if (_pointing.GetComponent<Taggable>() != null)
                    {
                        _audioSource.PlayOneShot(_audio_associazione);

                    }
                    if (_pointingTaggable && !_appstate.IsTest())
                        _pointingTaggable.EnableTMP();
                }
                
            }
            else if (_pointingInteractable) // se inquadro qualcodisabilito l'ultimo testo mostrato
            {
                _pointingInteractable.GetComponent<Taggable>()?.DisableTMP();
            }

            

        }
        //If NOTHING is detected set all to null
        else
        {
            if(_pointingInteractable != null && _pointingInteractable.GetComponent<Taggable>())
            {   
                _pointingInteractable.GetComponent<Highlight>()?.ToggleHighlight(false,false);
                _pointingInteractable.GetComponent<Taggable>()?.DisableTMP();

            }
            _pointingInteractable = null;
        }
    }

    public Interactable GetInteractable()
    {
        return _pointingInteractable;
    }
    

    private void DebugRaycast()
    {
        Debug.DrawRay(_rayOrigin, _fpsCameraT.forward * _interactionDistance, Color.red);
    }
}
