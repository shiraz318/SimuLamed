using Assets;
using Assets.model;
using System;
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

        //public KeyCode Left { get { return ConvertToDigitString(LeftInput); } }
        //public KeyCode Right { get { return ConvertToDigitString(RightInput); } }
        //public KeyCode Forward { get { return ConvertToDigitString(ForwardInput); } }
        //public KeyCode Backwards { get { return ConvertToDigitString(BackwardsInput); } }

        private string leftInput = "a";
        [Binding]
        public string LeftInput
        {
            get { return leftInput; }
            set { leftInput = value; SetPlayerKeys(Utils.LEFT, value); }
        }

        private string rightInput = "d";
        [Binding]
        public string RightInput
        {
            get { return rightInput; }
            set { rightInput = value; SetPlayerKeys(Utils.RIGHT, value); }
        }

        private string forwardInput = "w";
        [Binding]
        public string ForwardInput
        {
            get { return forwardInput; }
            set { forwardInput = value; SetPlayerKeys(Utils.FORWARD, value); }
        }

        private string backwardsInput = "s";
        [Binding]
        public string BackwardsInput
        {
            get { return backwardsInput; }
            set { backwardsInput = value; SetPlayerKeys(Utils.BACKWARDS, value); }
        }

        //public SettingsVM()
        //{
        //    Left = "L";
        //    Right = "R";
        //    Forward = "F";
        //    Backwards = "B";
        //}


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
        }

        // Sign up.
        public void SignUp(Utils.OnSuccessFunc onSuccess)
        {
            ErrorMessage = "";

            //// Empty fields.
            //if (Username.Equals("") || Password.Equals("") || Email.Equals(""))
            //{
            //    ErrorMessage = Utils.EMPTY_FIELD_MESSAGE;
            //}
            //else
            //{
            //    model.SignUp(Username, Password, Email, onSuccess);
            //}
        }

        public void Back(Utils.OnSuccessFunc onSuccess)
        {
            // model.SaveUser(onSuccess);

        }

        private void SetPlayerKeys(string keyName, string keyValue)
        {
            char character = keyValue.ToLower().ToCharArray()[0];
            int ascii = System.Convert.ToInt32(character);
            //int alphaValue = character;
            PlayerPrefs.SetString(keyName, ascii.ToString());
        }

        //private string ConvertToDigitString(string input)
        //{
        //    char character = input.ToCharArray()[0];
        //    int alphaValue = character;
        //    PlayerPrefs.SetString("Right")
        //    return (KeyCode)Enum.Parse(typeof(KeyCode), alphaValue.ToString());

        //}

        //void Awake() 
        //{
        //    if(instance == null)
        //    {
        //        instance = this;
                
        //    }
        //    DontDestroyOnLoad(gameObject);
        //}

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
