using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Taggable : Interactable
{
    //private string card_tag = null;
    private TMP_Text _visualCardTag;
    private Camera _target;

    private void Start()
    {
        _visualCardTag = GetComponentInChildren<TMP_Text>(true);
        _target = FindObjectOfType<Camera>();
        //PositionigTMP();
    }

    private void OnTestActiveEvent(bool isTest, int index)
    {
        if (isTest)
        _visualCardTag.text = "";
        else
        {
            //if(card_tag!= null)
            _visualCardTag.text = this.card_tag;
        }
    }

    /*public void DisableTMP()
    {
        if (_visualCardTag)
            _visualCardTag.text = "";
    }*/
    /*public void EnableTMP()
    {
        if(_visualCardTag != null)_visualCardTag.text = "";
        else
        {
        //    Debug.Log("EnableTMP(): _visualCardTag è NULLO");

        }
        if (this.card_tag != null)
        {
            
            //_visualCardTag = GetComponentInChildren<TMP_Text>();
            //Debug.Log("EnableTMP: " + _visualCardTag.name + " con card_tag: " + this.card_tag);
            if (_visualCardTag) { 
                _visualCardTag.text = card_tag;
                Debug.Log(_visualCardTag.name + " " + _visualCardTag.text);
            }
            else
            {
            //    Debug.Log("EnableTMP(): _visualCardTag è NULLO");

            }
            

        }
    }*/
   /* public void WatchTMP()
    {
        if (_visualCardTag)
        {
            //Debug.Log("Sto guardando: " + this.card_tag);
            if(this.card_tag != null )GetComponentInChildren<TMP_Text>().text =  this.card_tag;
            Vector3 targetDirection = _target.transform.position - _visualCardTag.transform.position;
            targetDirection.y = 0f;
            targetDirection.Normalize();

            float rotationStep = 2f * Time.deltaTime;

            Vector3 newDirection = Vector3.RotateTowards(_visualCardTag.transform.forward, targetDirection, rotationStep, 0.0f);
            _visualCardTag.transform.rotation = Quaternion.LookRotation(newDirection, _visualCardTag.transform.up);
        }
        else
        {
        //    Debug.Log("WatchTMP(): _visualCardTag è NULLO");
        }
    }*/

    public override void Interact(GameObject caller)
    {  
        AppState appstate = caller.GetComponent<AppState>();
        if (!appstate.IsTest())
        {
            if (this.card_tag == null)
            {
                this.card_tag = appstate.BindCardTag();
                _visualCardTag.text = this.card_tag;
                caller.GetComponent<ActionLauncher>().StatusIsTestEvent += OnTestActiveEvent;
            }
            else
            {
                if (appstate.GetDetachableCardTag() == this.card_tag)
                {
                    appstate.UnbindCardTag();
                    this.card_tag = null;
                    _visualCardTag.text = "";
                    caller.GetComponent<ActionLauncher>().StatusIsTestEvent -= OnTestActiveEvent;

                }

            }

            Debug.Log(this.card_tag);
        }
        
    }


}

