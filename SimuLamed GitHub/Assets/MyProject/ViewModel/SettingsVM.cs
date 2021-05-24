using Assets;
using Assets.model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityWeld.Binding;

namespace Assets.ViewModel
{
    [Binding]
    public class SettingsVM : BaseViewModel
    {
        // Private fields.
        private HashSet<string> keys = new HashSet<string>();
        private static string leftInput = Utils.DEFAULT_LEFT;
        private static string rightInput = Utils.DEFAULT_RIGHT;
        private static string forwardInput = Utils.DEFAULT_FORWARD;
        private static string backwardsInput = Utils.DEFAULT_BACKWARDS;
        private static bool toShowQuestions = Utils.DEFAULT_TO_SHOW_QUESTIONS;
        private static bool toMuteSound = Utils.DEFAULT_TO_MUTE_SOUND;

        // Properties.
        [Binding]
        public string LeftInput { get { return leftInput; } set { leftInput = value;  }
        }
        [Binding]
        public string RightInput { get { return rightInput; }set { rightInput = value; }
        }
        [Binding]
        public string ForwardInput { get { return forwardInput; } set { forwardInput = value; }
        }
        [Binding]
        public string BackwardsInput { get { return backwardsInput; } set { backwardsInput = value;  }
        }
        [Binding]
        public bool ToShowQuestions { get { return toShowQuestions; } set { toShowQuestions = value; }
        }
        [Binding]
        public bool ToMuteSound { get { return toMuteSound; } set { toMuteSound = value; }
        }


        // Save the chosen settings.
        public void SaveSettings()
        {
            if (!CheckValidityOfKeys())
            {
                ErrorMessage = Utils.ERROR_IN_KEYS_H;
            }
            else
            {
                ErrorMessage = "";
                SetPlayerKeys(Utils.LEFT, LeftInput);
                SetPlayerKeys(Utils.RIGHT, RightInput);
                SetPlayerKeys(Utils.FORWARD, ForwardInput);
                SetPlayerKeys(Utils.BACKWARDS, BackwardsInput);
                PlayerPrefs.SetString(Utils.SHOW_QUESTIONS, ToShowQuestions ? "show":string.Empty);
                PlayerPrefs.SetInt(Utils.MUTE_SOUND, ToMuteSound ? 1 : 0);
                
                NotifyPropertyChanged(GetPropertyName());
            }
        }

        public string GetPropertyName() {  return "SavedSettings"; }

        // Checks if the player chose a key more than once.
        private bool CheckValidityOfKeys()
        {
            // Hash set - contains every key only once.
            //Therefore if there are more than one key value - the length will be less than 4.
            keys.Add(LeftInput);
            keys.Add(RightInput);
            keys.Add(ForwardInput);
            keys.Add(BackwardsInput);
            return keys.Count == 4;
        }

        //private bool CheckAndSetMapKeys(string keyName, string value)
        //{
        //    value = value.ToLower();
        //    foreach (var pair in map)
        //    {
        //        if (value.Equals(pair.Value) && pair.Key != keyName)
        //        {
        //            return false;
        //        }
               
        //    }
        //    map[keyName] = value;
        //    return true;
        //}

        // Set the given key to it's matching ascii character.
        private void SetPlayerKeys(string keyName, string keyValue)
        {
            char character = keyValue.ToLower().ToCharArray()[0];
            int ascii = System.Convert.ToInt32(character);
            PlayerPrefs.SetString(keyName, ascii.ToString());
        }

    }

}
