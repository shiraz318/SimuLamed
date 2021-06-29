using Assets;
using Assets.model;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityWeld.Binding;

[Binding]
public class QuestionsManager : BaseViewModel
{
    // Private fields.
    private string questionText;
    private string ans1Text;
    private string ans2Text;
    private string ans3Text;
    private string ans4Text;
    private string errorText;
    private bool isAnsInteractable;
    private Tuple<int, bool> lastAnswerResults;
    private Question[] questions;
    private static int currentQuestionNumber;
    private bool isImageEnable;
    private Sprite imageSprite;
    private string imageErrorMessag;
    private bool isLoadingCircleOn;
    private const string DEFAULT_IMAGE_URL = "https://www.gov.il/BlobFolder/generalpage/tq_pic_02/he/TQ_PIC_3530.jpg";


    public const string RESET_ANS_COLOR = "ResetAnsColor";
    public const string RESET_IMAGE = "ResetImage";


    // Properties.
    public Tuple<int, bool> LastAnswerResults 
    { 
        get { return lastAnswerResults; } set { lastAnswerResults = value;  NotifyPropertyChanged(); } 
    }
    [Binding]
    public string QuestionText 
    { 
        get { return questionText; }  set { questionText = value; NotifyPropertyChanged(); } 
    }
    [Binding]
    public string ErrorText
    {
        get { return errorText; } set { errorText = value; NotifyPropertyChanged(); }
    }

    [Binding]
    public string Ans1Text 
    {
        get { return ans1Text; }  set { ans1Text = value;  NotifyPropertyChanged(); } 
    }
    [Binding]
    public string Ans2Text 
    {
        get { return ans2Text; } set { ans2Text = value; NotifyPropertyChanged(); }
    }
    
    [Binding]
    public string Ans3Text 
    {
        get { return ans3Text; } set { ans3Text = value; NotifyPropertyChanged(); } 
    }
    
    [Binding]
    public string Ans4Text 
    {
        get { return ans4Text; } set { ans4Text = value; NotifyPropertyChanged(); } 
    }

    [Binding]
    public bool IsAnsInteractable 
    {
        get { return isAnsInteractable; } set { isAnsInteractable = value; NotifyPropertyChanged(); } 
    }
    [Binding]
    public bool IsImageEnable 
    {
        get { return isImageEnable; } set { isImageEnable = value; NotifyPropertyChanged(); } 
    }

    [Binding]
    public Sprite ImageSprite 
    {
        get { return imageSprite; } set { imageSprite = value; NotifyPropertyChanged(); } 
    }

    [Binding]
    public string ImageErrorMessage 
    { 
        get { return imageErrorMessag; } set { imageErrorMessag = value; NotifyPropertyChanged(); } 
    }
    [Binding]
    public bool IsLoadingCircleOn 
    {
        get { return isLoadingCircleOn; } set { isLoadingCircleOn = value; NotifyPropertyChanged(); } 
    }
    public bool IsQuestionSet { get; set; }
    public Question CurrentQuestion { get { return GetQuestionByQuestionNum(); } }
    public int NumberOfQuestions { get { return questions != null ? questions.Length : 0; } }
    public bool IsSimulationQuestinos { get; set; }
    
    
    void Awake()
    {
        currentQuestionNumber = 0;
        // Get default image.
        StartCoroutine(GetImage(DEFAULT_IMAGE_URL));
    }

    // Display the given nubmer question.
    public void DisplayQuestion(int questionNUmber)
    {

        // Set the current question number.
        SetQuestionNumber(questionNUmber);
        if (CurrentQuestion == null)
        {
            ErrorMessage = ErrorObject.FAIL_LOAD_QUESTION_MESSAGE;
        }
        else
        {
            ErrorMessage = "";
            // Display the current question number.
            SetCurrentQuestion();
            // Set the image if needed.
            SetImage(CurrentQuestion.imageUrl);
        }

    }

    // Get a question by the current question number.
    private Question GetQuestionByQuestionNum()
    {
        if (questions == null) { return null; }
        
        /*
         * If this is questions of the simulation - currentQuestionNumber is the quetion.questionNumber.
         * Else - currentQuestionNubmer is the index of the current quetion in the questions array -
         * just return qustions[currentQuestionNumbe]
         */
        if (IsSimulationQuestinos)
        {
            return questions.SingleOrDefault(question => question.questionNumber == currentQuestionNumber);
        }
        return questions[currentQuestionNumber];
    }

    // Set the questions to the given questions.
    public void SetQuestions(Question[] questions)
    {
        this.questions = questions;
        IsQuestionSet = true;
    }

    // Set the current question number.
    private void SetQuestionNumber(int questionNumber) { currentQuestionNumber = questionNumber; }
    
    // Set the matching properties of the current question.
    private void SetCurrentQuestion()
    {
        if (CurrentQuestion != null)
        {
            QuestionText = CurrentQuestion.question;
            Ans1Text = CurrentQuestion.answers[0];
            Ans2Text = CurrentQuestion.answers[1];
            Ans3Text = CurrentQuestion.answers[2];
            Ans4Text = CurrentQuestion.answers[3];
            IsAnsInteractable = true;
            
            // Notify that the answers color needs to be reset.
            NotifyPropertyChanged(RESET_ANS_COLOR);
        }

    }

    // On click event handler for clicking an answer.
    public void OnClickAnswer(bool isAnsCorrect)
    { 
        // Set the answer buttons to be not clickable.
        IsAnsInteractable = false;
        LastAnswerResults = new Tuple<int, bool>(CurrentQuestion.questionNumber, isAnsCorrect);

    }

    // Set the image object to hold the image in the given image url.
    private void SetImage(string imageUrl)
    {
        // Notify the image needs to resets it's size.
        NotifyPropertyChanged(RESET_IMAGE);

        // If imageUrl is empty - disable the image. Else - enable.
        IsImageEnable = !imageUrl.Equals("");
        IsLoadingCircleOn = IsImageEnable;
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
                ImageErrorMessage = ErrorObject.FAIL_LOAD_IMAGE_MESSAGE;
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);
                ImageSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                IsLoadingCircleOn = false;
                ImageErrorMessage = "";
            }
        }
    }

}
