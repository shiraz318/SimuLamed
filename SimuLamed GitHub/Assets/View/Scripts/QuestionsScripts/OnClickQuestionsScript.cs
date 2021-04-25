using Assets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnClickQuestionsScript : MonoBehaviour
{
    private QuestionsVM_2 viewModel;
    
    private static Image image;
    private static Animator animator;
    private static List<GameObject> answers;

    //private float positionYImage;
    //private float originalImageHeight;
    //private float originalImageWidth;
    //private bool isImageBigger;

    private void Start()
    {
        image = GameObject.Find("ImageButton").GetComponentInChildren<Image>();
        animator = GameObject.Find("PopStar").GetComponent<Animator>();
        SetViewModel();
        //SetOriginalImageSize();
        //isImageBigger = false;
        GameObject[] answersGameObject = GameObject.FindGameObjectsWithTag("AnsButton");
        answers = answersGameObject.ToList();
            
    }

    // Set the view model.
    private void SetViewModel()
    {
        viewModel = GameObject.Find("View").GetComponent<QuestionsVM_2>();
        //viewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        //{
        //    if (eventArgs.PropertyName.Equals("ResetImage"))
        //    {
        //        ResetImageSize();
        //    }
        //};
    }

    //// Set the original size of the image.
    //private void SetOriginalImageSize()
    //{
    //    RectTransform rt = image.GetComponentInParent<Button>().image.rectTransform;
    //    positionYImage = rt.position.y;
    //    originalImageHeight = rt.rect.height;
    //    originalImageWidth = rt.rect.width; ;
    //}

    //// Change the image size to the given width and height.
    //private void ChangeImageSize(float width, float height)
    //{
    //    image.GetComponentInParent<Button>().image.rectTransform.sizeDelta = new Vector2(width, height);

    //    Vector3 pos = image.GetComponentInParent<Button>().transform.position;
    //    pos.y = positionYImage + (height - originalImageHeight) / 2;
    //    image.GetComponentInParent<Button>().transform.position = pos;
    //}


    //// On click event handler for clicking the image.
    //public void OnClickImage()
    //{
    //    // If the image is enable - change it's size.
    //    if (viewModel.IsImageEnable)
    //    {
    //        if (isImageBigger)
    //        {
    //            // Make the image default size.
    //            ResetImageSize();
    //        }
    //        else
    //        {
    //            // Make the image bigger.
    //            MakeImageBigger();
    //        }
    //    }
    //}

    //// Reset the image size to the original size.
    //private void ResetImageSize()
    //{
    //    ChangeImageSize(originalImageWidth, originalImageHeight);
    //    isImageBigger = false;
    //}

    //// Make the image bigger.
    //private void MakeImageBigger()
    //{
    //    ChangeImageSize(originalImageWidth * 1.8f, originalImageHeight * 1.8f);
    //    isImageBigger = true;

    //}

    // Disable a wrong answer.
    private void DisableWrongAns()
    {
        string correctAns = viewModel.GetCorrectAns();

        // Shuffle the buttons so every time a differrent wrong answer will be disable.
        System.Random rnd = new System.Random();
        answers = answers.OrderBy(x => rnd.Next()).ToList();


        foreach (GameObject ans in answers)
        {
            Button button = ans.GetComponent<Button>();
            // This is a wrong answer.
            if (!correctAns.Equals(button.GetComponentInChildren<TMP_Text>().text))
            {
                button.interactable = false;
                button.image.color = Utils.disabledAnsColor;
                break;
            }
        }
    }

    // On click event handler for clicking the hint button.
    public void OnClickHint()
    {
        DisableWrongAns();
        viewModel.OnClickHint();
    }

    // On click event handler for clicking last question button.
    public void OnClickLastQuestion()
    {
        viewModel.OnClickLastQuestion();
    }

    // On click event handler for clicking next question button.
    public void OnClickNextQuestion()
    {
        viewModel.OnClickNextQuestion();

    }


}

//using Assets;
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class OnClickQuestionsScript : MonoBehaviour
//{
//    private QuestionsVM viewModel;
    
//    private static Image image;
//    private static Animator animator;
//    private static List<GameObject> answers;

//    private float positionYImage;
//    private float originalImageHeight;
//    private float originalImageWidth;
//    private bool isImageBigger;

//    private void Start()
//    {
//        image = GameObject.Find("ImageButton").GetComponentInChildren<Image>();
//        animator = GameObject.Find("PopStar").GetComponent<Animator>();
//        SetViewModel();
//        SetOriginalImageSize();
//        isImageBigger = false;
//        GameObject[] answersGameObject = GameObject.FindGameObjectsWithTag("AnsButton");
//        answers = answersGameObject.ToList();
            
//    }

//    // Set the view model.
//    private void SetViewModel()
//    {
//        viewModel = GameObject.Find("View").GetComponent<QuestionsVM>();
//        viewModel.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
//        {
//            if (eventArgs.PropertyName.Equals("ResetImage"))
//            {
//                ResetImageSize();
//            }
//        };
//    }

//    // Set the original size of the image.
//    private void SetOriginalImageSize()
//    {
//        RectTransform rt = image.GetComponentInParent<Button>().image.rectTransform;
//        positionYImage = rt.position.y;
//        originalImageHeight = rt.rect.height;
//        originalImageWidth = rt.rect.width; ;
//    }

//    // Change the image size to the given width and height.
//    private void ChangeImageSize(float width, float height)
//    {
//        image.GetComponentInParent<Button>().image.rectTransform.sizeDelta = new Vector2(width, height);

//        Vector3 pos = image.GetComponentInParent<Button>().transform.position;
//        pos.y = positionYImage + (height - originalImageHeight) / 2;
//        image.GetComponentInParent<Button>().transform.position = pos;
//    }


//    // On click event handler for clicking the image.
//    public void OnClickImage()
//    {
//        // If the image is enable - change it's size.
//        if (viewModel.IsImageEnable)
//        {
//            if (isImageBigger)
//            {
//                // Make the image default size.
//                ResetImageSize();
//            }
//            else
//            {
//                // Make the image bigger.
//                MakeImageBigger();
//            }
//        }
//    }

//    // Reset the image size to the original size.
//    private void ResetImageSize()
//    {
//        ChangeImageSize(originalImageWidth, originalImageHeight);
//        isImageBigger = false;
//    }

//    // Make the image bigger.
//    private void MakeImageBigger()
//    {
//        ChangeImageSize(originalImageWidth * 1.8f, originalImageHeight * 1.8f);
//        isImageBigger = true;

//    }

//    // Disable a wrong answer.
//    private void DisableWrongAns()
//    {
//        string correctAns = viewModel.GetCorrectAns();

//        // Shuffle the buttons so every time a differrent wrong answer will be disable.
//        System.Random rnd = new System.Random();
//        answers = answers.OrderBy(x => rnd.Next()).ToList();


//        foreach (GameObject ans in answers)
//        {
//            Button button = ans.GetComponent<Button>();
//            // This is a wrong answer.
//            if (!correctAns.Equals(button.GetComponentInChildren<TMP_Text>().text))
//            {
//                button.interactable = false;
//                button.image.color = Utils.disabledAnsColor;
//                break;
//            }
//        }
//    }

//    // On click event handler for clicking the hint button.
//    public void OnClickHint()
//    {
//        DisableWrongAns();
//        viewModel.OnClickHint();
//    }

//    // On click event handler for clicking last question button.
//    public void OnClickLastQuestion()
//    {
//        viewModel.OnClickLastQuestion();
//    }

//    // On click event handler for clicking next question button.
//    public void OnClickNextQuestion()
//    {
//        viewModel.OnClickNextQuestion();

//    }


//}

