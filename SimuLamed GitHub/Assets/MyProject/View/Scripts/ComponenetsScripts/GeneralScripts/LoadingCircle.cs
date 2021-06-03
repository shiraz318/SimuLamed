using UnityEngine;
using UnityEngine.UI;

public class LoadingCircle : MonoBehaviour
{
    private RectTransform rectComponent;
    private float rotateSpeed = 200f;


    private void Start()
    {
        rectComponent = GetComponent<RectTransform>();
    }

    // Rotate the circle each time update is called.
    private void Update()
    {
         rectComponent.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
    }

    // Set the visability of the loading circle according to the given toShow.
    public static void SetLoadingCircleVisability(bool toShow, Image loadingCircle)
    {
        float alpha = toShow? 1f: 0f;

        Image[] children = loadingCircle.GetComponentsInChildren<Image>();
        foreach (Image child in children)
        {
            SetAlpha(alpha, child);
        }
        SetAlpha(alpha, loadingCircle);

    }

    // Set the alpha of the given image.
    private static void SetAlpha(float alpha, Image image)
    {
        var tempColor = image.color;
        tempColor.a = alpha;
        image.color = tempColor;
    }


}