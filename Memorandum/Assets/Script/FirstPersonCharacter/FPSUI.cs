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
    private List<string> available_cardtags = new List<string>();
    [SerializeField] private InitAppState init_app;
    private int cardtagsIndex = 0;
    private float minScrollLimit = 1;
    // Start is called before the first frame update
    void Start()
    {   
        
        if (init_app != null)
        {
            for (int i = 0; i < init_app.cardList.Count; i++)
            {
                //Debug.Log("cardTag di indice " + i);
                available_cardtags.Add(init_app.cardList[i]);
            }
            Debug.Log("Numero cardtag ancora non assegnate " + available_cardtags.Count);

        }
        else { ErrorManager.Error("AppState non inserito "); }

    }

    public string GetCardTag()
    {

        string data = available_cardtags[cardtagsIndex];
        available_cardtags.RemoveAt(cardtagsIndex);
        if (available_cardtags.Count -1 != 0)
        {
            if (cardtagsIndex != 0)
            {
                cardtagsIndex--;
            }else
            {
                cardtagsIndex = available_cardtags.Count-1;
            }
        }
        return data;
    }

    public void PushCardTag(string cardtag)
    {
        available_cardtags.Add(cardtag);
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckScrollMouseWheel();
        UpdateCardTagUI();
    }
    private void UpdateCardTagUI()
    {
        if (available_cardtags.Count > 0)
        {
            _text.text = available_cardtags[cardtagsIndex];
        }
        else
        {
            _text.text = "";
        }
    }
    void CheckScrollMouseWheel()
    {
        float sum = Input.mouseScrollDelta.y;
        if (sum == minScrollLimit || sum == -minScrollLimit)
        { 
            if (cardtagsIndex + sum < available_cardtags.Count && cardtagsIndex + sum >= 0 )
            {
                cardtagsIndex = (int)(cardtagsIndex + sum);
            }
            else if (sum > 0)
            {
                cardtagsIndex = 0;
            }else
            {
                cardtagsIndex = available_cardtags.Count - 1;
            }

            Debug.Log(Input.mouseScrollDelta.y + " Parola Selezionata: " + available_cardtags[cardtagsIndex]);
        }
    }
}
