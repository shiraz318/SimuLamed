using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AlertDisplayer : MonoBehaviour
{
    SpriteRenderer rend;
    public GameObject alert;
    public TMP_Text alertText;
    private static SoundManager soundManager;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();
        //rend = GetComponent<SpriteRenderer>();
        //Color c = rend.material.color;
        //c.a = 0f;
        //rend.material.color = c;
        
    }
    public void DisplayAlert(string message)
    {
        soundManager.DisplayAlert();
        alertText.text = message;
        StartCoroutine("Show");
        //StartFadeIn();
        //StartFadeOut();
    }
    IEnumerator Show()
    {
        alert.SetActive(true);
        yield return new WaitForSeconds(3f);
        alert.SetActive(false);
    }

    public void StartFadeIn()
    {
        StartCoroutine("FadeIn");
    }
    public void StartFadeOut()
    {
        StartCoroutine("FadeOut");
    }

    IEnumerator FadeIn()
    {
        for (float f = 0.05f; f <= 1; f += 0.05f)
        {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2f);
        for (float f = 1f; f >= -0.05f; f -= 0.05f)
        {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
    }

}
