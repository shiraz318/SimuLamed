using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Assets;
using UnityWeld.Binding;
using System.Linq;

[RequireComponent(typeof(Slider))]
public class SliderFill : MonoBehaviour
{

    private float fillSpeed = 0.08f;
    private Slider slider;
    
    private Text precent;
    private RectTransform fillRect;
    private float targetValue = 0f;
    private float curValue = 0f;
   


    void Awake()
    {
        slider = GetComponent<Slider>();
        precent = GetComponentInChildren<Text>();
        //Adds a listener to the main slider and invokes a method when the value changes.
        slider.onValueChanged.AddListener(delegate { ValueChange(); });

        fillRect = slider.fillRect;
        targetValue = curValue = slider.value;
    }

    // Invoked when the value of the slider changes.
    public void ValueChange()
    {
        targetValue = slider.value;
    }

    // Update is called once per frame
    void Update()
    {
        // Set fill progress.
        curValue = Mathf.MoveTowards(curValue, targetValue, Time.deltaTime * fillSpeed);
        Vector2 fillAnchor = fillRect.anchorMax;
        fillAnchor.x = Mathf.Clamp01(curValue / slider.maxValue);
        fillRect.anchorMax = fillAnchor;

        // Set precent text.
        precent.text = (System.Math.Round(curValue * 100, 1).ToString() + "%");
        
        // Set fill color.
        var fill = (slider as UnityEngine.UI.Slider).GetComponentsInChildren<UnityEngine.UI.Image>().FirstOrDefault(t => t.name == "Fill");
        if (fill != null)
        {
            fill.color = GetColor(curValue * 100);
        }

    }

    // Get the corresponsding color to the given value.
    private Color32 GetColor(float value)
    {
        if (value <= 25)
        {
            return Utils.progress1Color;
        }
        else if (value <= 50)
        {
            return Utils.progress2Color;
        }
        else if (value <= 75)
        {
            return Utils.progress3Color;
        }
        return Utils.progress4Color;

    }

}
