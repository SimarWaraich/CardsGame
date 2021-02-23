using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    CanvasGroup Fader;

    void Awake()
    {
        Fader = GetComponent<CanvasGroup>();
        StartCoroutine(CloseSplash());
    }

    void Start()
    {
    }

    public void InvokeSplash()
    {
        StartCoroutine(CloseSplash());
    }

    IEnumerator CloseSplash()
    {
        yield return new WaitForSeconds(1f);
        while (Fader.alpha > 0)
        {
            yield return null;
            Fader.alpha -= Time.deltaTime;
        }
        yield return null;
        Fader.interactable = false;
        Fader.gameObject.SetActive(false);
    }

}
