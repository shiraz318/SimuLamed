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




    private int lastHintsNumber;
    [Binding]
    public int HintsNumber { get { lastHintsNumber = AppModel.Instance.HintsNumber; return lastHintsNumber; } }

    private bool isNewHint;
    [Binding]
    public bool IsNewHint { get { return isNewHint; } set { isNewHint = value; NotifyPropertyChanged("IsNewHint"); } }

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
        set
        {
            ans1Text = value;
            NotifyPropertyChanged("Ans1Text");
        }
    }
    private string ans2Text;
    [Binding]
    public string Ans2Text
    {
        get { return ans2Text; }
        set
        {
            ans2Text = value;
            NotifyPropertyChanged("Ans2Text");
        }
    }
    private string ans3Text;
    [Binding]
    public string Ans3Text
    {
        get { return ans3Text; }
        set
        {
            ans3Text = value;
            NotifyPropertyChanged("Ans3Text");
        }
    }
    private string ans4Text;
    [Binding]
    public string Ans4Text
    {
        get { return ans4Text; }
        set
        {
            ans4Text = value;
            NotifyPropertyChanged("Ans4Text");
        }
    }

    private bool isAnsInteractable;
    [Binding]
    public bool IsAnsInteractable { get { return isAnsInteractable; } set { isAnsInteractable = value; NotifyPropertyChanged("IsAnsInteractable"); } }

    private Sprite imageSprite;
    [Binding]
    public Sprite ImageSprite { get { return imageSprite; } set { imageSprite = value; NotifyPropertyChanged("ImageSprite"); } }

    [Binding]
    public string QuestionNumText { get { if (!IsQuestionSet) { return "0 / 0"; } return (questionNum + 1).ToString() + " / " + questions.Length.ToString(); }}


    private IAppModel model;
    private Question[] questions;

    private static int questionNum;
    public bool IsQuestionSet { get; set; }


    void Start()
    {
        questions = new Question[0];
        IsQuestionSet = false;

        SetModel();

        lastHintsNumber = model.HintsNumber;
        
        // Get the questions of the selected subject category.
        model.SetQuestionsByCategory(SelectedSubject);
        
        IsHintButtonOn = true;
        NotifyPropertyChanged("HintsNumber");
    }

    // Set the model.
    private void SetModel()
    {
        model = AppModel.Instance;
        model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            // The model got the questions from the database.
            if (eventArgs.PropertyName.Equals("Questions") && !IsQuestionSet)
            {
                questionNum = 0;
                questions = model.Questions.ToArray();
                IsQuestionSet = true;
                PresetQuestion();
            }
            else if (eventArgs.PropertyName.Equals("HintsNumber"))
            {
                IsNewHint = (lastHintsNumber < HintsNumber);
                NotifyPropertyChanged("HintsNumber");
            }
        };
    }

    // Present a question.
    private void PresetQuestion()
    {
        // New question - update the question number.
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
        
        // Notify that the answers color needs to be reset.
        NotifyPropertyChanged("ResetAnsColor");
    }
    

    // Set the image object to hold the image in the given image url.
    private void SetImage(string imageUrl)
    {
        // Notify the image needs to resets it's size.
         NotifyPropertyChanged("ResetImage");
        
        // If imageUrl is empty - disable the image. Else - enable.
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
    public void OnClickAnswer(bool isAnsCorrect)
    {
        // Set the answer buttons to be not clickable.
        IsAnsInteractable = false;

        IsHintButtonOn = false;
        if (!isAnsCorrect) { NotifyPropertyChanged("WrongAnswer"); }
        model.SetUserScore(questions[questionNum].questionNumber, isAnsCorrect);
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

    // Get the correct answer of the current question.
    public string GetCorrectAns() 
    { 
        return questions[questionNum].correctAns;
    }

    // On click event handler for clicking hint button.
    public void OnClickHint()
    {
        model.DecreaseHint();
        // Disable the hint button - can't have more then one hint per question.
        IsHintButtonOn = false;
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
