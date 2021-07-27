using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class SceneTransition : MonoBehaviour
{
    public static event Action JustBeforeSceneTransition;
    public static event Action JustAfterSceneTransition;    

    public Image img;
    public AnimationCurve curve;

    private void Start()
    {
        StartCoroutine(FadeIn());        
        JustAfterSceneTransition?.Invoke();
    }

    public void FadeTo(string scene)
    {
        //AudioManager.instance.Play("ButtonPress");

        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
        }

        JustBeforeSceneTransition?.Invoke();
        StartCoroutine(FadeOut(scene));
    }
    IEnumerator FadeIn()
    {
        float t = 1f;
        while (t > 0f)
        {
            t -= Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
    }

    IEnumerator FadeOut(string scene)
    {
        float t = 0f;
        while (t > 1f)
        {
            t += Time.deltaTime;
            float a = curve.Evaluate(t);
            img.color = new Color(0f, 0f, 0f, a);
            yield return 0;
        }
        SceneManager.LoadScene(scene);
    }
}
