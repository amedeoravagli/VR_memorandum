using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TestElement : Interactable
{
    //[SerializeField]
    private TextMeshPro _text;
    private MiniGames _test = null;
    private string _randomCardTag = "";
    private void Start()
    {
        GetComponent<BoxCollider>().enabled = false;
        _test = GetComponentInParent<MiniGames>();
        if (_text == null)
        {
            _text = GetComponentInChildren<TextMeshPro>();
            if (_text != null)
                _text.text = _randomCardTag;
            else Debug.Log("TextMeshPro è null ");
        }
    }
    public override void Interact(GameObject caller)
    {
        if (caller.GetComponent<AppState>().IsTest())
        {
            Debug.Log("L'oggetto ha come cardTag: " + _randomCardTag);
            _test.VerifyTags(_randomCardTag);
            if (_test.getChangeColor())
            {
                _text.color = Color.red;
            }
            else
            {
                _text.color = Color.green;
            }
        }
    }
    public void superaTest(GameObject caller)
    {
        if (caller.GetComponent<AppState>().IsTest())
        {
            Debug.Log("L'oggetto ha come cardTag: " + _randomCardTag);
            _test.Win();
        }
    }
    public void AddRandomTag(string tag)
    {
        _randomCardTag = tag;
        if(_text == null)
        {
            Debug.Log("Perchè _text è nullo????");
            _text = GetComponentInChildren<TextMeshPro>();
        }
        _text.text = tag;
    }

}
