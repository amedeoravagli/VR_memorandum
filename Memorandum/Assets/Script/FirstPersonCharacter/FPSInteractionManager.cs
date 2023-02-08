using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSInteractionManager : MonoBehaviour
{

    [SerializeField] private Transform _fpsCameraT;
    [SerializeField] private bool _debugRay;
    [SerializeField] private float _interactionDistance;

    private Interactable _pointingInteractable;
    
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
            Interactable _pointing = hit.transform.GetComponent<Taggable>();
            if (_pointing)
            {
                if(_pointing != _pointingInteractable)
                {
                    if (_pointingInteractable)
                    {
                        _pointingInteractable.GetComponent<Highlight>()?.ToggleHighlight(false);
                    }
                    _pointingInteractable = _pointing;
                    _pointingInteractable.GetComponent<Highlight>()?.ToggleHighlight(true);
                }
                if (Input.GetMouseButtonDown(0))
                    _pointingInteractable.Interact(gameObject);

            }

            
        }
        //If NOTHING is detected set all to null
        else
        {
            if(_pointingInteractable != null)
            {
                _pointingInteractable.GetComponent<Highlight>()?.ToggleHighlight(false);

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
