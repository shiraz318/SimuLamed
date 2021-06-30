using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.MyProject.View.Scripts;

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
    // Consts.
    private const string MAIN_TEXTURE = "_MainTex";

    private const float RED_LIGHT_OFFSET = 0f;
    private const float YELLOW_LIGHT_OFFSET = 0.34f;
    private const float GREEN_LIGHT_OFFSET = 0.67f;
    private const float YELLOW_RED_OFFSET = 0f;
    private const float EMPTY_OFFSET = 0.34f;
    private const int NUMBER_OF_BLINKS = 4;

    

    // Textures.
    private static Texture regularLightsTexture;
    private static Texture emptyAndRedYellowTexture;

    // Duration parts.
    private float YELLOW_DURATION_PART = (2f / 18f);
    private float GREEN_DURATION_PART = (1f / 3f);
    private float YELLOW_RED_DURATION_PART = (4f / 18f);
    private float RED_DURATION_PART = (1f / 3f);
    private const float TIME_OFFSET = 0.16f;
    private const float EMPTY_TIME = 0.3f;

    // Total durations.
    private float oneDuration = 16f; // duration of one cycle.
    private float yellowDuration;
    private float yellowRedDuration;
    private float redDuration;
    private float greenDuration;
    private float emptyDuration;
    
    private int greenCounter = 0; // counts number of blinking in the green light.
    private Dictionary<Lights, Tuple<float, float>> lightsOffsetAndDuration 
        = new Dictionary<Lights, Tuple<float, float>>();  // light, offset, duration.
    private Material myMat;
    
    

    [SerializeField]
    private int numberOfTrafficLights = 1; // number of traffic lights participate in the cycle.
    [SerializeField]
    private int trafficLightNumber = 1; // the index of the current traffic light in the cycle it participates in.
    [SerializeField]
    private bool isBlink = false;
    [SerializeField]
    private Light[] lights; // the lights in the traffic light.

    public Lights currentLight;  // the current light that is lit.


    void Start()
    {
        // Set the duration of each light.
        SetDurations();
        // Set the dictionary of offset and duration.
        SetLightsOffsetAndDuration();
        // Set textures.
        SetTextures();
        myMat = GetComponent<Renderer>().material;

        // Starting with red light.
        currentLight = Lights.Red;
        SetLight(true);
        StartCoroutine("StartAnimation");
    }

    // Set the textures.
    private void SetTextures()
    {
        MyTrafficLightTextures textures = GameObject.Find(GameObjectNames.MY_TEXTURES).GetComponent<MyTrafficLightTextures>();
        emptyAndRedYellowTexture = textures.emptyAndRedYellowTexture;
        regularLightsTexture = textures.regularLightsTexture;
    }

    // Set the duration of each light.
    private void SetDurations()
    {
        oneDuration /= (float)numberOfTrafficLights;

        /*
         * The duration of each light is it's part of oneDuration multiple
         * number of traffic lights that are participating in the cycle.
         * Green light gets a little bit less so that there will be no collisions with other traffic light green light.
         */
        greenDuration = ((float)numberOfTrafficLights * oneDuration * GREEN_DURATION_PART) - (TIME_OFFSET * (float)numberOfTrafficLights);
        if (isBlink)
        {
            emptyDuration = EMPTY_TIME;
            oneDuration += (emptyDuration * (NUMBER_OF_BLINKS - 1) * 2);
            greenDuration -= EMPTY_TIME;
        }
        yellowDuration = (float)numberOfTrafficLights * oneDuration * YELLOW_DURATION_PART;
        yellowRedDuration = (float)numberOfTrafficLights * oneDuration * YELLOW_RED_DURATION_PART;
        redDuration = (float)numberOfTrafficLights * oneDuration * RED_DURATION_PART + (TIME_OFFSET * (float)numberOfTrafficLights);
    }

    // Set the dictionary of the offset and duration.
    private void SetLightsOffsetAndDuration()
    {
        lightsOffsetAndDuration.Add(Lights.Red, new Tuple<float, float>(RED_LIGHT_OFFSET, redDuration));
        lightsOffsetAndDuration.Add(Lights.Yellow, new Tuple<float, float>(YELLOW_LIGHT_OFFSET, yellowDuration));
        lightsOffsetAndDuration.Add(Lights.RedAndYellow, new Tuple<float, float>(YELLOW_RED_OFFSET, yellowRedDuration));
        lightsOffsetAndDuration.Add(Lights.Green, new Tuple<float, float>(GREEN_LIGHT_OFFSET, greenDuration));
        lightsOffsetAndDuration.Add(Lights.Empty, new Tuple<float, float>(EMPTY_OFFSET, emptyDuration));

    }

    // Start the traffic lights animation.
    IEnumerator StartAnimation()
    {
        // Wait to syncronize the cycles of all the traffic lights that participate in the cycle.
        yield return new WaitForSeconds((trafficLightNumber - 1) * oneDuration);
        StartCoroutine(nameof(ChangeLight));

    }

    // Changing color and lights in the traffic light.
    IEnumerator ChangeLight()
    {
        float lightDuration = lightsOffsetAndDuration[currentLight].Item2;
        /*
         * If we are in the middle of blinking - 
         * green light gets the duration of empty light instead of the duration of a green light.
         */
        if (isBlink && currentLight.Equals(Lights.Green) && greenCounter != 0)
        {
            lightDuration = emptyDuration;
        }

        // Wait for the current light duration.
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
        StartCoroutine(nameof(ChangeLight));
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
                    myMat.SetTexture(MAIN_TEXTURE, regularLightsTexture);
                    break;
                }
            case Lights.RedAndYellow:
                {
                    myMat.SetTexture(MAIN_TEXTURE, emptyAndRedYellowTexture);
                    break;
                }
            case Lights.Yellow:
                {
                    myMat.SetTexture(MAIN_TEXTURE, regularLightsTexture);
                    break;

                }
            case Lights.Green:
                {
                    myMat.SetTexture(MAIN_TEXTURE, regularLightsTexture);
                    break;
                }
            case Lights.Empty:
                {
                    myMat.SetTexture(MAIN_TEXTURE, emptyAndRedYellowTexture);
                    break;

                }
            default:
                break;

        }
        myMat.SetTextureOffset(MAIN_TEXTURE, new Vector2(lightsOffsetAndDuration[currentLight].Item1, 0f));

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
                        // After the right amount of blinks - this is the yellow light turn.
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