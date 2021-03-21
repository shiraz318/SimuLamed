using Assets;
using Assets.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class QuestionsSceneVM : MonoBehaviour
{
    public TMP_Text subject;
    
    public TMP_Text questionText;
    //private static TMP_Text errorText;
    public List<Button> answers;
    public Image image;


    private IModel model;
    //IDatabaseHandler databaseHandler;
    public SceneLoader sceneLoader;
    private string selectedQuestionCategory;
    private Question[] questions;
    private static int questionNum;
    private bool isImageBigger;
    private float originalImageWidth;
    private float originalImageHeight;
    private float positionYImage;
    private bool isQuestionSet;


    private void Awake()
    {
        isQuestionSet = false;
    }

    void Start()
    {
        questions = new Question[1];
        isQuestionSet = false;
        isImageBigger = false;

        SetOriginalImageSize();

        model = Model.Instance;

        model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName.Equals("Questions") && !isQuestionSet)
            {
                questionNum = 0;
                questions = model.Questions.ToArray();

                isQuestionSet = true;
                PresetQuestion();
            }
        };

        //databaseHandler = FirebaseManager.Instance;

        ////errorText = GameObject.FindWithTag("ErrorMessage").GetComponent<TMP_Text>() as TMP_Text;
        ////errorText.alpha = 0f;

        //databaseHandler.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        //{
        //    if (eventArgs.PropertyName.Equals("Questions") && !isQuestionSet)
        //    {
        //        questionNum = 0;
        //        questions = databaseHandler.Questions.ToArray();

        //        isQuestionSet = true;
        //        PresetQuestion();
        //    }
        //};
        selectedQuestionCategory = Question.FromTypeToCategory(LearningFromQuestionsSceneVM.selectedSubject);
        subject.text = selectedQuestionCategory;
        model.SetQuestionsByCategory(selectedQuestionCategory);
        //databaseHandler.SetQuestionsByCategory(selectedQuestionCategory);

    }


    // Set the fields of the original image size.
    private void SetOriginalImageSize()
    {
        RectTransform rt = image.GetComponentInParent<Button>().image.rectTransform;
        positionYImage = rt.position.y;
        originalImageHeight = rt.rect.height;
        originalImageWidth = rt.rect.width; ;
    }

    // Present a question.
    private void PresetQuestion()
    {
        Question currentQuestion = questions[questionNum];
        questionText.text = currentQuestion.question;
        answers[0].GetComponentInChildren<TMP_Text>().text = currentQuestion.ans1;
        answers[1].GetComponentInChildren<TMP_Text>().text = currentQuestion.ans2;
        answers[2].GetComponentInChildren<TMP_Text>().text = currentQuestion.ans3;
        answers[3].GetComponentInChildren<TMP_Text>().text = currentQuestion.ans4;

        SetImage(currentQuestion.imageUrl);
        SetButtonsClickablity(true);
        ResetAnsColors();

    }

    // Set all answer buttons clickability to the given isClickable value.
    private void SetButtonsClickablity(bool isClickable)
    {
        foreach (Button button in answers)
        {
            button.interactable = isClickable;
        }
    }


    // Set the image size to the given width and height.
    private void SetImageSize(float width, float height)
    {
        image.GetComponentInParent<Button>().image.rectTransform.sizeDelta = new Vector2(width, height);

        Vector3 pos = image.GetComponentInParent<Button>().transform.position;
        pos.y = positionYImage + (height - originalImageHeight)/2;
        image.GetComponentInParent<Button>().transform.position = pos;
        
    }

    // Set the image object to hold the image in the given image url.
    private void SetImage(string imageUrl)
    {
        ResetImageSize();

        // No image.
        if (imageUrl.Equals(""))
        {
            image.enabled = false;
        }
        else
        {
            image.enabled = true;
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
                //errorText.alpha = 1f;
                
            }
            else
            {
                // Get downloaded asset bundle
                var texture = DownloadHandlerTexture.GetContent(uwr);
                image.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0, 0));
                //errorText.alpha = 0f;
            }
        }
    }

    // Reset the image size.
    private void ResetImageSize()
    {
        SetImageSize(originalImageWidth, originalImageHeight);
        isImageBigger = false;
    }

    // Make the image bigger.
    private void MakeImageBigger()
    {
        SetImageSize(originalImageWidth * 1.8f, originalImageHeight * 1.8f);
        isImageBigger = true;

    }

    // On click event handler for clicking the image.
    public void OnClickImage()
    {
        if (image.enabled)
        {

            if (isImageBigger)
            {
                // Make the image default size.
                ResetImageSize();
            }
            else
            {

                // Make the image bigger.
                MakeImageBigger();
            }
        }
    }

    // On click event handler for clicking the back button.
    public void OnClickBack()
    {
        sceneLoader.LoadNextScene("LearningFromQuestionsScene");
    }
    
    // On click event handler for clicking an answer.
    public void OnClickAnswer(Button button)
    {
        // Set the answer buttons to be not clickable.
        SetButtonsClickablity(false);
        
        string name = button.name;
        string correctAns = questions[questionNum].correctAns;
        bool isAnsCorrect = false;

        // Check if it the correct answer.
        // Mark the correct answer with green and the wrong (if exists) with red.
        if (!correctAns.Equals(button.GetComponentInChildren<TMP_Text>().text))
        {
            // Set color to red.
            SetButtonColor(button, 1f, Utils.redColor);
            foreach (Button ans in answers)
            {
                if (correctAns.Equals(ans.GetComponentInChildren<TMP_Text>().text))
                {
                    // Set color to green.
                    SetButtonColor(ans, 1f, Utils.greenColor);
                    break;
                }
            }
        }
        else
        {
            isAnsCorrect = true;
            // Set the correct ans to green.
            SetButtonColor(button, 1f, Utils.greenColor);
        }

        model.SetUserScore(questions[questionNum].questionNumber, isAnsCorrect);
    }

    // Set a given button's color and alpha.
    private void SetButtonColor(Button button, float alpha, Color32 color)
    {

        Color tempColor = button.image.color;
        tempColor = color;
        tempColor.a = alpha;
        button.image.color = tempColor;

    }

    // Reset the answer button's colors.
    private void ResetAnsColors()
    {
        // Reset the color of all ans
        foreach (Button ans in answers)
        {
            SetButtonColor(ans, 1f, Color.black);
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



}

