﻿using Assets;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityWeld.Binding;

public class ShowKeyboards : MonoBehaviour
{
    private const int LEFT_KEY = 0;
    private const int RIGHT_KEY = 1;
    private const int FORWARD_KEY = 2;
    private const int BACKWARDS_KEY = 3;

    //[SerializeField]
    //private TMP_Text leftKey;
    [SerializeField]
    private TMP_Text[] keys;
    //public TMP_Text rightKey;
    //public TMP_Text forwardKey;
    //public TMP_Text backwardsKey;

    private void Awake()
    {
        keys[LEFT_KEY].text = GetKey(Utils.LEFT, Utils.DEFAULT_LEFT);
        keys[RIGHT_KEY].text = GetKey(Utils.RIGHT, Utils.DEFAULT_RIGHT);
        keys[FORWARD_KEY].text = GetKey(Utils.FORWARD, Utils.DEFAULT_FORWARD);
        keys[BACKWARDS_KEY].text = GetKey(Utils.BACKWARDS, Utils.DEFAULT_BACKWARDS);
    }
    
    // Get the given key name ascii value or if not set - return the default value.
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
