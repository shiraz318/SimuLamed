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
    public class SettingsVM : MonoBehaviour, INotifyPropertyChanged
    {
        //public static SettingsVM instance;
        
        public event PropertyChangedEventHandler PropertyChanged;

        private IAppModel model;
        private string errorMessage;

       
        HashSet<string> keys = new HashSet<string>();
        [Binding]
        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                NotifyPropertyChanged("ErrorMessage");
            }
        }

       
        private static string leftInput = Utils.DEFAULT_LEFT;
        [Binding]
        public string LeftInput
        {
            get { return leftInput; }
            set { leftInput = value;  }
        }

        private static string rightInput = Utils.DEFAULT_RIGHT;
        [Binding]
        public string RightInput
        {
            get { return rightInput; }
            set { rightInput = value; }
        }

        private static string forwardInput = Utils.DEFAULT_FORWARD;
        [Binding]
        public string ForwardInput
        {
            get { return forwardInput; }
            set { forwardInput = value; }
        }

        private static string backwardsInput = Utils.DEFAULT_BACKWARDS;
        [Binding]
        public string BackwardsInput
        {
            get { return backwardsInput; }
            set { backwardsInput = value;  }
        }
        private static bool toShowQuestions = Utils.DEFAULT_TO_SHOW_QUESTIONS;
        [Binding]
        public bool ToShowQuestions
        {
            get { return toShowQuestions; }
            set { toShowQuestions = value; }
        }




        private void Start()
        {
            model = AppModel.Instance;
            model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
            {
                if (eventArgs.PropertyName.Equals("Error"))
                {
                    if (model.Error.ErrorType == ErrorTypes.SignUp)
                    {
                        ErrorMessage = model.Error.Message;
                    }
                }

            };
            
            // map
           
        }

        // Sign up.
        public void SignUp(Utils.OnSuccessFunc onSuccess)
        {
            ErrorMessage = "";

        }

        public void Back(Utils.OnSuccessFunc onSuccess)
        {
            if (!CheckValidityOfKeys())
            {
                ErrorMessage = Utils.ERROR_IN_KEYS;

            }
            else
            {
                ErrorMessage = "";
                SetPlayerKeys(Utils.LEFT, LeftInput);
                SetPlayerKeys(Utils.RIGHT, RightInput);
                SetPlayerKeys(Utils.FORWARD, ForwardInput);
                SetPlayerKeys(Utils.BACKWARDS, BackwardsInput);
                PlayerPrefs.SetString(Utils.SHOW_QUESTIONS, ToShowQuestions ? "show":string.Empty);
                onSuccess();
            }
        }

        private bool CheckValidityOfKeys()
        {
            
            bool valid = true;

            // hash set
            keys.Add(LeftInput);
            keys.Add(RightInput);
            keys.Add(ForwardInput);
            keys.Add(BackwardsInput);

            if (keys.Count != 4)
            {
                valid = false;
            }
            return valid;

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


        private void SetPlayerKeys(string keyName, string keyValue)
        {
            char character = keyValue.ToLower().ToCharArray()[0];
            int ascii = System.Convert.ToInt32(character);
            //int alphaValue = character;
            PlayerPrefs.SetString(keyName, ascii.ToString());
        }

       
        // On property changed.
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
    }

}

// TODO
/// 
/// ADD OPTION FOR THE USER TO CHANGE THE ANGLE OF THE VIEW.
/// 
/// </summary>

