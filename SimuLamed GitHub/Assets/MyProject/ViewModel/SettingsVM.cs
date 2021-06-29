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

        public const string RIGHT = "Right";
        public const string LEFT = "Left";
        public const string FORWARD = "Forward";
        public const string BACKWARDS = "Backwards";
        public const string SHOW_QUESTIONS = "ShowQuestions";
        public const string MUTE_SOUND = "MuteSound";
        public const string SHOW = "show";


        // Default settings.
        public const string DEFAULT_FORWARD = "w";
        public const string DEFAULT_BACKWARDS = "s";
        public const string DEFAULT_RIGHT = "d";
        public const string DEFAULT_LEFT = "a";
        public const bool DEFAULT_TO_SHOW_QUESTIONS = true;
        public const bool DEFAULT_TO_MUTE_SOUND = false;


        private HashSet<string> keys = new HashSet<string>();
        private static string leftInput = DEFAULT_LEFT;
        private static string rightInput = DEFAULT_RIGHT;
        private static string forwardInput = DEFAULT_FORWARD;
        private static string backwardsInput = DEFAULT_BACKWARDS;
        private static bool toShowQuestions = DEFAULT_TO_SHOW_QUESTIONS;
        private static bool toMuteSound = DEFAULT_TO_MUTE_SOUND;

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
        public bool ToMuteSound { get { return toMuteSound; } set { toMuteSound = value; } }


        // Save the chosen settings.
        public void SaveSettings()
        {
            if (CheckValidityOfKeys())
            {
                ErrorMessage = "";
                SetPlayerKeys(LEFT, LeftInput);
                SetPlayerKeys(RIGHT, RightInput);
                SetPlayerKeys(FORWARD, ForwardInput);
                SetPlayerKeys(BACKWARDS, BackwardsInput);
                PlayerPrefs.SetString(SHOW_QUESTIONS, ToShowQuestions ? SHOW :string.Empty);
                PlayerPrefs.SetInt(MUTE_SOUND, ToMuteSound ? 1 : 0);
                
                // Notify that we finished saving the settings.
                NotifyPropertyChanged(GetOnFinishActionPropertyName());
            }
        }

        // Get the property name of finishing the main action of the view model.
        public string GetOnFinishActionPropertyName() {  return "SavedSettings"; }

        // Checks if the player chose a key more than once.
        private bool CheckValidityOfKeys()
        {
            if (LeftInput.Equals("") || RightInput.Equals("") || ForwardInput.Equals("") || BackwardsInput.Equals(""))
            {
                ErrorMessage = ErrorObject.KEY_IS_MISSING;
                return false;
            }

            /*
            * Hash set - contains every key only once.
            * Therefore if there are more than one key value - the length will be less than 4.
            */
            keys.Add(LeftInput);
            keys.Add(RightInput);
            keys.Add(ForwardInput);
            keys.Add(BackwardsInput);
            if(keys.Count == 4)
            {
                return true;
            }
            ErrorMessage = ErrorObject.ERROR_IN_KEYS;
            return false;
        }

        // Set the given key to it's matching ascii character.
        private void SetPlayerKeys(string keyName, string keyValue)
        {
            char character = keyValue.ToLower().ToCharArray()[0];
            int ascii = System.Convert.ToInt32(character);
            PlayerPrefs.SetString(keyName, ascii.ToString());
        }

    }

}
