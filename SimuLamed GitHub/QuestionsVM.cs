using Assets;
using Assets.model;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityWeld.Binding;

[Binding]
public class QuestionsVM : MonoBehaviour, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    [Binding]
    public int HintsNumber { get { return AppModel.Instance.HintsNumber; } }
    [Binding]
    public string SelectedSubject { 
        get { return Question.FromTypeToCategory(AppModel.Instance.SelectedSubject); } }
    
    private bool isHintButtonOn;
    [Binding]
    public bool IsHintButtonOn 
    { 
        get { return isHintButtonOn; } 
        set 
        { 
            if (HintsNumber == 0) 
            { isHintButtonOn = false; } 
            else 
            { isHintButtonOn = value; }
            NotifyPropertyChanged("IsHintButtonOn"); }
    }
    private bool isImageEnable;
    [Binding]
    public bool IsImageEnable { get { return isImageEnable; } set { isImageEnable = value; NotifyPropertyChanged("IsImageEnable"); } }

    private string questionText;
    [Binding]
    public string QuestionText { get { return questionText; } set { questionText = value; NotifyPropertyChanged("QuestionText"); } }
    
    private string ans1Text;
    [Binding]
    public string Ans1Text
    {
        get { return ans1Text; }
        set { ans1Text = value;
            NotifyPropertyChanged("Ans1Text");
        }
    }
    private string ans2Text;
    [Binding]
    public string Ans2Text
    {
        get { return ans2Text; }
        set { ans2Text = value;
            NotifyPropertyChanged("Ans2Text");
        }
    }
    private string ans3Text;
    [Binding]
    public string Ans3Text
    {
        get { return ans3Text; }
        set { ans3Text = value;
            NotifyPropertyChanged("Ans3Text");
        }
    }
    private string ans4Text;
    [Binding]
    public string Ans4Text
    {
        get { return ans4Text; }
        set { ans4Text = value;
            NotifyPropertyChanged("Ans4Text");
        }
    }

    private Color ans1Color;
    [Binding]
    public Color Ans1Color
    {
        get { return ans1Color; }
        set { ans1Color = value;
            NotifyPropertyChanged("Ans1Color");
        }
    }
    private Color ans2Color;
    [Binding]
    public Color Ans2Color
    {
        get { return ans2Color; }
        set { ans2Color = value;
            NotifyPropertyChanged("Ans2Color");
        }
    }
    private Color ans3Color;
    [Binding]
    public Color Ans3Color
    {
        get { return ans3Color; }
        set { ans3Color = value;
            NotifyPropertyChanged("Ans3Color");
        }
    }
    private Color ans4Color;
    [Binding]
    public Color Ans4Color
    {
        get { return ans4Color; }
        set { ans4Color = value;
            NotifyPropertyChanged("Ans4Color");
        }
    }
    private bool isAnsInteractable;
    [Binding]
    public bool IsAnsInteractable { get { return isAnsInteractable; } set { isAnsInteractable = value; NotifyPropertyChanged("IsAnsInteractable"); } }

    private Sprite imageSprite;
    [Binding]
    public Sprite ImageSprite { get { return imageSprite; } set { imageSprite = value; NotifyPropertyChanged("ImageSprite"); } }

    [Binding]
    public string QuestionNumText { get { if (!isQuestionSet) { return "0 / 0"; } return (questionNum + 1).ToString() + " / " + questions.Length.ToString(); }}

    public string SelectedAnsName { get; set; }
    public string SelectedAnsText { get; set; }



    private IAppModel model;
    private Question[] questions;

    private static int questionNum;
    private bool isQuestionSet;

    void Start()
    {
        questions = new Question[0];
        isQuestionSet = false;

        model = AppModel.Instance;
        model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName.Equals("Questions") && !isQuestionSet)
            {
                questionNum = 0;
                questions = model.Questions.ToArray();

                isQuestionSet = true;
                PresetQuestion();
            } 
            else if (eventArgs.PropertyName.Equals("HintsNumber"))
            {
                NotifyPropertyChanged("HintsNumber");
            }
        };

        model.SetQuestionsByCategory(SelectedSubject);
        IsHintButtonOn = true;
        NotifyPropertyChanged("HintsNumber");
    }

    // Present a question.
    private void PresetQuestion()
    {
        NotifyPropertyChanged("QuestionNumText");
        Question currentQuestion = questions[questionNum];
        QuestionText = currentQuestion.question;
        Ans1Text = currentQuestion.ans1;
        Ans2Text = currentQuestion.ans2;
        Ans3Text = currentQuestion.ans3;
        Ans4Text = currentQuestion.ans4;
           
        SetImage(currentQuestion.imageUrl);
        IsHintButtonOn = true;
        IsAnsInteractable = true;
        ResetAnsColors();

    }
    private void ResetAnsColors()
    {
        Color color = GetEditedColor(1f, Color.black);
        Ans1Color = color;
        Ans2Color = color;
        Ans3Color = color;
        Ans4Color = color;
    }
    private Color GetEditedColor(float alpha, Color color)
    {
        color.a = alpha;
        return color;
    }


    // Set the image object to hold the image in the given image url.
    private void SetImage(string imageUrl)
    {
         NotifyPropertyChanged("ResetImage");
         IsImageEnable = !imageUrl.Equals("");

        if (IsImageEnable)
        {
            // Set the image.
            StartCoroutine(GetImage(imageUrl));
        }
    }

    // Get an image from the given image url.
    private IEnumerator GetImage(string imageUrl)
    {
        using (UnityWebRequest uwr = UnityWebRequestTexture.GetTexture(imageUrl))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);

            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);
                ImageSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0 ,0));
            }
        }
    }


    // On click event handler for clicking an answer.
    public void OnClickAnswer()
    {
        // Set the answer buttons to be not clickable.
        IsAnsInteractable = false;

        string correctAns = questions[questionNum].correctAns;
        bool isAnsCorrect = false;

        // Check if it the correct answer.
        // Mark the correct answer with green and the wrong (if exists) with red.
        if (!correctAns.Equals(SelectedAnsText))
        {
            // Set color to red.
            SetAnsColor(SelectedAnsName, 1f, Utils.redColor);
            SetCorrectAnsColor(correctAns, 1f, Utils.greenColor);
        }
        else
        {
            isAnsCorrect = true;
            // Set the correct ans to green.
            SetAnsColor(SelectedAnsName, 1f, Utils.greenColor);
        }
        IsHintButtonOn = false;
        model.SetUserScore(questions[questionNum].questionNumber, isAnsCorrect);
    }

    private void SetCorrectAnsColor(string correctAnsText, float alpha, Color32 color)
    {
        string correctAnsName = "";

        if (correctAnsText.Equals(Ans1Text))
        {
            correctAnsName = Utils.ANS_1_NAME;
        } 
        else if (correctAnsText.Equals(Ans2Text))
        {
            correctAnsName = Utils.ANS_2_NAME;
        }
        else if (correctAnsText.Equals(Ans3Text))
        {
            correctAnsName = Utils.ANS_3_NAME;
        }
        else
        {
            correctAnsName = Utils.ANS_4_NAME;
        }

        SetAnsColor(correctAnsName, alpha, color);
    }
    public void SetAnsColor(string ansName, float alpha, Color32 color)
    {
        switch (ansName)
        {
            case Utils.ANS_1_NAME:
                Ans1Color = GetEditedColor(alpha, color);
                break;
            case Utils.ANS_2_NAME:
                Ans2Color = GetEditedColor(alpha, color);
                break;
            case Utils.ANS_3_NAME:
                Ans3Color = GetEditedColor(alpha, color);
                break;
            case Utils.ANS_4_NAME:
                Ans4Color = GetEditedColor(alpha, color);
                break;
            default:
                break;

        }
    }


    // On click event handler for clicking next question button.
    public void OnClickNextQuestion()
    {

        questionNum = (questionNum + 1) % questions.Length;
        PresetQuestion();
    }

    // On click event handler for clicking last question button.
    public void OnClickLastQuestion()
    {
        if (questionNum != 0)
        {
            questionNum = (questionNum - 1) % questions.Length;
            PresetQuestion();

        }

    }

    public string GetCorrectAns() 
    { 
        return questions[questionNum].correctAns;
    }


    public void OnClickHint()
    {

        model.DecreaseHint();
        // Disable the hint button - can't have more then one hint per question.
        IsHintButtonOn = false;
    }

    public void NotifyPropertyChanged(string propName)
    {
        if (this.PropertyChanged != null)
        {
            this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
