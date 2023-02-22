using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TestElement : Interactable
{
    private TextMeshPro _text;
    private MiniGames _test = null;
    private string _randomCardTag = "";
    private string _cardTag = "";
    private bool _selected = false;
    private FPSUI _ui;

    private void Start()
    {
        GetComponent<BoxCollider>().enabled = false;
        _test = GetComponentInParent<MiniGames>();
        if (_text == null)
        {
            _text = GetComponentInChildren<TextMeshPro>();
            if (_text != null)
            {
                _text.text = "";
                
            }
            else Debug.Log("TextMeshPro è null ");
        }
    }

    public string GetTag()
    {
        return _cardTag;
    }

    public bool IsSelected()
    {
        return _selected;
    }

    public void SetSelected(bool select)
    {
        _selected = select;
    }

    public void SetColor(int color)
    {
        if (color == 2)
        {
            _text.color = Color.red;
        }
        else if (color == 1)
        {
            _text.color = Color.yellow;
        }
        else if (color == 0)
        {
            _text.color = Color.green;
        }
    }

    public void ChangeColor(Color color, string input)
    {
        _text.text = input;
        _text.color = color;
    }

    public void Blanking()
    {
        _text.text = "";
    }

    public void ResetTextElement()
    {
        _text.text = _randomCardTag;
    }

    private string MakeConfusedTag()
    {
        string result = "";
        List<int> orderChar = new List<int>();
        for(int i = 0; i < _cardTag.Length; i++)
        {
            int index = UnityEngine.Random.Range(0, _cardTag.Length);
            while (orderChar.Contains(index))
            {
                index = UnityEngine.Random.Range(0, _cardTag.Length);
            }
            result += _cardTag[index];
            orderChar.Add(index);
        }
        _randomCardTag = result;

        return result;
    }

    public override void Interact(GameObject caller)
    {
        if (caller.GetComponent<AppState>().IsTest())
        {
            Debug.Log("L'oggetto ha come cardTag: " + _randomCardTag);

            _selected = true;
            if(_ui == null)
                _ui = caller.GetComponentInChildren<FPSUI>();
            _test.SetElementToVerify(this);
            _ui.ActivateInput(_randomCardTag);

        }
    }
    public void superaTest(GameObject caller)
    {
        if (caller.GetComponent<AppState>().IsTest())
        {
            Debug.Log("L'oggetto ha come cardTag: " + _cardTag + " e randomcardTag = " + _randomCardTag);
            _test.Win();
        }
    }
    public void AddRandomTag(string tag)
    {
        _cardTag = tag.Trim();
        if(_text == null)
        {
            Debug.Log("Perchè _text è nullo????");
            _text = GetComponentInChildren<TextMeshPro>();
        }
        
        if(_cardTag.Length != 0)
        {
            _text.text = MakeConfusedTag();
        }
    }

}
