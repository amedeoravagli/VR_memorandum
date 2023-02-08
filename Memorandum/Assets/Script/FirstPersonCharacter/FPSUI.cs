using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UI;

public class FPSUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _text;
    [SerializeField] private Image _target;
    private AppState _appstate;
    private FPSInteractionManager _interactionManager;
    
    // Start is called before the first frame update
    void Start()
    {   
        _appstate = GetComponent<AppState>();
        _interactionManager = GetComponent<FPSInteractionManager>();
    }

    // Update is called once per frame
    void OnGUI()
    {
        UpdateUITarget();
        UpdateCardTagUI();
    }

    private void UpdateCardTagUI()
    {
        _text.text = _appstate.GetCardTag();
    }
    void UpdateUITarget()
    {
        if (_interactionManager.GetInteractable())
            _target.color = Color.green;
        else
            _target.color = Color.red;
    }
}
