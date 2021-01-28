using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WorldTextInfo : MonoBehaviour
{
    public static WorldTextInfo singleton;
    public TMP_Text dynamicText;


    public void Awake()
    {
        singleton = this;
    }
    
    public void NewMessage(string text)
    {
        dynamicText.text = text;
    }
}
