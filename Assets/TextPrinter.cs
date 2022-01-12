using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextPrinter : MonoBehaviour
{
    private TextMeshPro uiText;


    // Start is called before the first frame update
    void Start()
    {
        uiText = GetComponent<TextMeshPro>();
        uiText.text = "";
    }

    public void Loginfo(string s)
    {
        uiText.text = s + "\n";
    }

    public void Addinfo(string s)
    {
        uiText.text += s + "\n";
    }
    public void Clear()
    {
        uiText.text = "";
    }
}
