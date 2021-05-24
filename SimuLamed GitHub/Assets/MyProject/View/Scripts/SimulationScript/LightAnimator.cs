using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum Lights
{
    Red = 0,
    Yellow,
    Green,
    RedAndYellow,
    Empty
}


[System.Serializable]
public class LightAnimator : MonoBehaviour
{
    public int numberOfTrafficLights = 1;
    public int trafficLightNumber = 1;
    public bool isBlink = false;

    private static Texture regularLightsTexture;
    private static Texture emptyAndRedYellowTexture;


    // Consts.
    private const float RED_LIGHT_OFFSET = 0f;
    private const float YELLOW_LIGHT_OFFSET = 0.34f;
    private const float GREEN_LIGHT_OFFSET = 0.67f;
    private const float YELLOW_RED_OFFSET = 0f;
    private const float EMPTY_OFFSET = 0.34f;


    private float YELLOW_DURATION_PART = (2f/18f);
    private float GREEN_DURATION_PART = (1f/3f);
    private float YELLOW_RED_DURATION_PART = (4f/18f);
    private float RED_DURATION_PART = (1f/3f);

    private const int NUMBER_OF_BLINKS = 4;

    private Material myMat;
  
    private float oneDuration = 8f; // duration of one cycle.
    private float yellowDuration;
    private float yellowRedDuration;
    private float redDuration;
    private float greenDuration;
    private float emptyDuration;  
    private int greenCounter = 0; // counts number of blinking in the green light.
    private Dictionary<Lights, Tuple<float, float>> lightsOffsetAndDuration = new Dictionary<Lights, Tuple<float, float>>();  // light, offset, duration.
    public Light[] lights;

    private Lights currentLight;  // the current light that is lit.

   

    void Start()
    {
        oneDuration /= (float)numberOfTrafficLights;
        
        // The duration of each light is it's part of oneDuration multiple number of traffic lights that are participating in the cycle.
        greenDuration = ((float)numberOfTrafficLights * oneDuration * GREEN_DURATION_PART) - (0.16f * (float)numberOfTrafficLights);

        if (isBlink)
        {
            emptyDuration = 0.3f;
            oneDuration += (emptyDuration * 6);
        }
        yellowDuration = (float)numberOfTrafficLights * oneDuration * YELLOW_DURATION_PART;
        yellowRedDuration = (float)numberOfTrafficLights * oneDuration * YELLOW_RED_DURATION_PART;
        redDuration =  (float)numberOfTrafficLights * oneDuration * RED_DURATION_PART + (0.16f * (float)numberOfTrafficLights);
        
        //oneDuration += (0.04f * (float)numberOfTrafficLights);
        
        if (isBlink)
        {
            greenDuration -= 0.3f;

        }
        // Set the dictionary of offset and duration.
        SetLightsOffsetAndDuration();

        MyTrafficLightTextures textures = GameObject.Find("MyTextures").GetComponent<MyTrafficLightTextures>();
        emptyAndRedYellowTexture = textures.emptyAndRedYellowTexture;
        regularLightsTexture = textures.regularLightsTexture;

        myMat = GetComponent<Renderer>().material;
        

        // Starting with red light.
        currentLight = Lights.Red;
        SetLight(true);
        //SetColor();

        StartCoroutine("StartAnimation");
    }
    private void SetLightsOffsetAndDuration()
    {
        lightsOffsetAndDuration.Add(Lights.Red, new Tuple<float, float>(RED_LIGHT_OFFSET, redDuration));
        lightsOffsetAndDuration.Add(Lights.Yellow, new Tuple<float, float>(YELLOW_LIGHT_OFFSET, yellowDuration));
        lightsOffsetAndDuration.Add(Lights.RedAndYellow, new Tuple<float, float>(YELLOW_RED_OFFSET, yellowRedDuration));
        lightsOffsetAndDuration.Add(Lights.Green, new Tuple<float, float>(GREEN_LIGHT_OFFSET, greenDuration));
        lightsOffsetAndDuration.Add(Lights.Empty, new Tuple<float, float>(EMPTY_OFFSET, emptyDuration));

    }

    IEnumerator StartAnimation()
    {
        // Wait to syncronize the cycles of all the traffic lights that participate in the cycle.
        yield return new WaitForSeconds((trafficLightNumber - 1) * oneDuration);
        StartCoroutine("ChangeLight");

    }

    IEnumerator ChangeLight()
    {

        float lightDuration = lightsOffsetAndDuration[currentLight].Item2;
        if (isBlink && currentLight.Equals(Lights.Green) && greenCounter != 0)
        {
            lightDuration = emptyDuration;
        }

        yield return new WaitForSeconds(lightDuration);

        // Disable the current light.
        SetLight(false);

        // Get the next light.
        currentLight = GetNextLight();

        // Enable the next light.
        SetLight(true);

        // Set the color.
        SetColor();

        // Call this method again to create an infinite loop.
        StartCoroutine("ChangeLight");

    }

    // Set the visability of the current light to the given value.
    private void SetLight(bool toEnable)
    {
        if (currentLight.Equals(Lights.RedAndYellow))
        {
            lights[(int)Lights.Red].enabled = toEnable;
            lights[(int)Lights.Yellow].enabled = toEnable;

        }
        else if (!currentLight.Equals(Lights.Empty))
        {
            lights[(int)currentLight].enabled = toEnable;
        }
    }

    // Set the color - change the texture and offset if needed.
    private void SetColor()
    {
        switch (currentLight)
        {
            case Lights.Red:
                {
                    myMat.SetTexture("_MainTex", regularLightsTexture);
                    break;
                }
            case Lights.RedAndYellow:
                {
                    myMat.SetTexture("_MainTex", emptyAndRedYellowTexture);
                    break;
                }
            case Lights.Yellow:
                {
                    myMat.SetTexture("_MainTex", regularLightsTexture);
                    break;

                }
            case Lights.Green:
                {
                    myMat.SetTexture("_MainTex", regularLightsTexture);
                    break;
                }
            case Lights.Empty:
                {
                    myMat.SetTexture("_MainTex", emptyAndRedYellowTexture);
                    break;

                }
            default:
                break;

        }
        myMat.SetTextureOffset("_MainTex", new Vector2(lightsOffsetAndDuration[currentLight].Item1, 0f));

    }

    // Get the next light by the order: Red, Red and Yellow, Green, (blinking Green 3 times), Yellow.
     private Lights GetNextLight()
    {
        switch (currentLight)
        {
            case Lights.Red:
                {
                    return Lights.RedAndYellow;
                }
            case Lights.RedAndYellow:
                {
                    return Lights.Green;
                }
            case Lights.Yellow:
                {
                    greenCounter = 0;
                    return Lights.Red;
                }
            case Lights.Green:
                {
                    if (isBlink)
                    {
                        greenCounter = (greenCounter + 1) % (NUMBER_OF_BLINKS + 1);
                        return (greenCounter < NUMBER_OF_BLINKS) ? Lights.Empty : Lights.Yellow;
                    }
                    return Lights.Yellow;

                }
            case Lights.Empty:
                {
                    return Lights.Green;
                }
            default:
                return Lights.Red;
        }

    }

}