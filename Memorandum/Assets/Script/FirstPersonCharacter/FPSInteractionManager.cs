using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSInteractionManager : MonoBehaviour
{

    [SerializeField] private Transform _fpsCameraT;
    [SerializeField] private bool _debugRay;
    [SerializeField] private float _interactionDistance;

    private Interactable _pointingInteractable;
    private Taggable _pointingTaggable;
    private CharacterController _fpsController;
    private Vector3 _rayOrigin;

    private AppState _appstate;

    private float _minScrollLimit = 1.0f;
    //private Taggable _taggedObject = null;

    // Start is called before the first frame update
    void Start()
    {
        _appstate = GetComponent<AppState>();
        _fpsController = GetComponent<CharacterController>();
    }

    void Update()
    {
        _rayOrigin = _fpsCameraT.position + _fpsController.radius * _fpsCameraT.forward;

        
        CheckInteraction();
        CheckScrollMouseWheel();
        

        if (_debugRay)
            DebugRaycast();
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

                if (_pointingTaggable)
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
