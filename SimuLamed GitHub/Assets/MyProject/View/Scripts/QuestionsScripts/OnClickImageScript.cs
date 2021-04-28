using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class OnClickImageScript : MonoBehaviour
{
    public QuestionsManager questionManager;
    private Image image;

    private float positionYImage;
    private float originalImageHeight;
    private float originalImageWidth;
    private bool isImageBigger;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        SetOriginalImageSize();
        isImageBigger = false;
        SetQuestionManager();
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { OnClickMyImage(); });

    }

    // Set the original size of the image.
    private void SetOriginalImageSize()
    {
        RectTransform rt = image.GetComponentInParent<Button>().image.rectTransform;
        positionYImage = rt.position.y;
        originalImageHeight = rt.rect.height;
        originalImageWidth = rt.rect.width; ;
    }

    // Change the image size to the given width and height.
    private void ChangeImageSize(float width, float height)
    {
        image.GetComponentInParent<Button>().image.rectTransform.sizeDelta = new Vector2(width, height);

        Vector3 pos = image.GetComponentInParent<Button>().transform.position;
        pos.y = positionYImage + (height - originalImageHeight) / 2;
        image.GetComponentInParent<Button>().transform.position = pos;
    }
    // Set the view model.
    private void SetQuestionManager()
    {
        questionManager.PropertyChanged += delegate (object sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName.Equals("ResetImage"))
            {
                ResetImageSize();
            }
        };
    }
    // On click event handler for clicking the image.
    public void OnClickMyImage()
    {
        // If the image is enable - change it's size.
        if (questionManager.IsImageEnable)
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
    // Reset the image size to the original size.
    private void ResetImageSize()
    {
        ChangeImageSize(originalImageWidth, originalImageHeight);
        isImageBigger = false;
    }
    // Make the image bigger.
    private void MakeImageBigger()
    {
        ChangeImageSize(originalImageWidth * 1.8f, originalImageHeight * 1.8f);
        isImageBigger = true;

    }


}
