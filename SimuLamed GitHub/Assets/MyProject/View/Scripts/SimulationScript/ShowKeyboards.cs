using Assets;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityWeld.Binding;

public class ShowKeyboards : MonoBehaviour
{
    public TMP_Text leftKey;
    public TMP_Text rightKey;
    public TMP_Text forwardKey;
    public TMP_Text backwardsKey;

    private void Awake()
    {

        leftKey.text = GetKey(Utils.LEFT, Utils.DEFAULT_LEFT);
        rightKey.text = GetKey(Utils.RIGHT, Utils.DEFAULT_RIGHT);
        forwardKey.text = GetKey(Utils.FORWARD, Utils.DEFAULT_FORWARD);
        backwardsKey.text = GetKey(Utils.BACKWARDS, Utils.DEFAULT_BACKWARDS);
    }
    
    private string GetKey(string keyName, string defaultKey)
    {
        string keyVal = PlayerPrefs.GetString(keyName);
        if (keyVal.Equals(""))
        {
            return defaultKey;
        }
        int ascii = int.Parse(keyVal);
        string key = ((char)ascii).ToString();
        return key.Equals("") ? defaultKey : key; 
    }
}
