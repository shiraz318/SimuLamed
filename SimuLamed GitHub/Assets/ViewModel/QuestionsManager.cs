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
public class QuestionsManager : MonoBehaviour, INotifyPropertyChanged
{
    private string questionText;
    private string ans1Text;
    private string ans2Text;
    private string ans3Text;
    private string ans4Text;
    private bool isAnsInteractable;
    // questionNumber, isAnsCorrect
    private Tuple<int, bool> lastAnswerResults;
    private Question[] questions;
    private static int currentQuestionNumber;



    public event PropertyChangedEventHandler PropertyChanged;

    public Tuple<int, bool> LastAnswerResults { get { return lastAnswerResults; } set { lastAnswerResults = value; NotifyPropertyChanged("LastAnswerResults"); } }

    [Binding]
    public string QuestionText { get { return questionText; } set { questionText = value; NotifyPropertyChanged("QuestionText"); } }

    [Binding]
    public string Ans1Text { get { return ans1Text; } set { ans1Text = value; NotifyPropertyChanged("Ans1Text"); } }
    [Binding]
    public string Ans2Text { get { return ans2Text; } set { ans2Text = value; NotifyPropertyChanged("Ans2Text"); } }
    
    [Binding]
    public string Ans3Text { get { return ans3Text; } set { ans3Text = value; NotifyPropertyChanged("Ans3Text"); } }
    
    [Binding]
    public string Ans4Text { get { return ans4Text; } set { ans4Text = value; NotifyPropertyChanged("Ans4Text"); } }

    [Binding]
    public bool IsAnsInteractable { get { return isAnsInteractable; } set { isAnsInteractable = value; NotifyPropertyChanged("IsAnsInteractable"); } }
    private bool isImageEnable;
    [Binding]
    public bool IsImageEnable { get { return isImageEnable; } set { isImageEnable = value; NotifyPropertyChanged("IsImageEnable"); } }

    private Sprite imageSprite;
    [Binding]
    public Sprite ImageSprite { get { return imageSprite; } set { imageSprite = value; NotifyPropertyChanged("ImageSprite"); } }

    public bool IsQuestionSet { get; set; }
    public Question CurrentQuestion { get { return questions[currentQuestionNumber]; } }

    
    
    void Awake()
    {
        IsQuestionSet = false;
        currentQuestionNumber = 0;
    }


    public void DisplayQuestion(int questionNUmber)
    {
        SetQuestionNumber(questionNUmber);
        DisplayCurrentQuestion();
        SetImage(CurrentQuestion.imageUrl);
    }

    public void SetQuestions(Question[] questions)
    {
        this.questions = questions;
        IsQuestionSet = true;
    }


    private void SetQuestionNumber(int questionNumber)
    {
        currentQuestionNumber = questionNumber;
    }

    private void DisplayCurrentQuestion()
    {
        SetCurrentQuestion();
    }
    private void SetCurrentQuestion()
    {
        QuestionText = CurrentQuestion.question;
        Ans1Text = CurrentQuestion.ans1;
        Ans2Text = CurrentQuestion.ans2;
        Ans3Text = CurrentQuestion.ans3;
        Ans4Text = CurrentQuestion.ans4;

        IsAnsInteractable = true;
        
        // Notify that the answers color needs to be reset.
        NotifyPropertyChanged("ResetAnsColor");
    }

    // On click event handler for clicking an answer.
    public void OnClickAnswer(bool isAnsCorrect)
    {

        // Set the answer buttons to be not clickable.
        IsAnsInteractable = false;
        if (!isAnsCorrect) { NotifyPropertyChanged("WrongAnswer"); }
        LastAnswerResults = new Tuple<int, bool>(questions[currentQuestionNumber].questionNumber, isAnsCorrect);
    }


    public int GetNumberOfQuestions()
    {
        return questions != null ? questions.Length : 0;
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
                ImageSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
            }
        }
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
