using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;
    [SerializeField]
    GameObject Loading;

    [SerializeField] Image Indicator;
    float rotationDelta = 0.1f;
    [Range(0.1f, 1)]
    public float Speed = 0.5f;
    float value;
    private int loadingCallsCount;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this);
    }

    public void LoadingStatus(bool status)
    {
        if (!status)
            loadingCallsCount--;


        if (loadingCallsCount <= 0)
        {
            Loading.SetActive(status);
            loadingCallsCount = 0;
        }
        if (status)
            loadingCallsCount++;
    }


    void Update()
    {
        if (Indicator.fillAmount == 0 || Indicator.fillAmount == 1)
        {
            Indicator.fillClockwise = !Indicator.fillClockwise;
            rotationDelta = -rotationDelta;
        }
        value = rotationDelta * Speed;

        Indicator.fillAmount -= value;
    }
}
