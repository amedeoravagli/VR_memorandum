using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FPSInteractionManager : MonoBehaviour
{

    [SerializeField] private Transform _fpsCameraT;
    [SerializeField] private bool _debugRay;
    [SerializeField] private float _interactionDistance;

    [SerializeField] private Image _target;

    private Interactable _pointingInteractable;

    private CharacterController _fpsController;
    private Vector3 _rayOrigin;

    //private Taggable _taggedObject = null;

    // Start is called before the first frame update
    void Start()
    {
        _fpsController = GetComponent<CharacterController>();
    }

    void Update()
    {
        _rayOrigin = _fpsCameraT.position + _fpsController.radius * _fpsCameraT.forward;

        
        CheckInteraction();

        UpdateUITarget();

        if (_debugRay)
            DebugRaycast();
    }

    private void CheckInteraction()
    {
        Ray ray = new Ray(_rayOrigin, _fpsCameraT.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, _interactionDistance))
        {
            //Check if is interactable
            _pointingInteractable = hit.transform.GetComponent<Taggable>();
            if (_pointingInteractable)
            {
                if (Input.GetMouseButtonDown(0))
                    _pointingInteractable.Interact(gameObject);
            }
            
        }
        //If NOTHING is detected set all to null
        else
        {
            _pointingInteractable = null;
        }
    }

    void UpdateUITarget()
    {
        if (_pointingInteractable)
            _target.color = Color.green;
        else
            _target.color = Color.red;
    }

    private void DebugRaycast()
    {
        Debug.DrawRay(_rayOrigin, _fpsCameraT.forward * _interactionDistance, Color.red);
    }
}
