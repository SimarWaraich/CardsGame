using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public class PopUp : MonoBehaviour
{
    public static Action<bool> OnPopUpStatusChange;
    public static PopUp Instance;

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
        ClosePopUp();
    }

    [SerializeField]
    GameObject PopupWindow;

    [SerializeField]
    Text HeaderText;

    [SerializeField]
    Text MessageText;

    [SerializeField]
    Button PositiveResponse;

    [SerializeField]
    Button NegativeResponse;

    public void ShowPopUp(string Title, string Message)
    {
        ShowPopUp(Title, Message, null);
        PositiveResponse.GetComponentInChildren<Text>().text = "Ok";
        NegativeResponse.gameObject.SetActive(false);
    }
    public void ShowPopUp(string Title, string Message, UnityAction onClickOk)
    {
        ShowPopUp(Title, Message, onClickOk, null);
        PositiveResponse.GetComponentInChildren<Text>().text = "Ok";
    }

    public void ShowPopUp(string Title, string Message, UnityAction onClickYes, UnityAction onClickNo)
    {
        OnPopUpStatusChange?.Invoke(true);

        PopupWindow.SetActive(true);
        HeaderText.text = Title;
        MessageText.text = Message;

        NegativeResponse.gameObject.SetActive(false);

        PositiveResponse.onClick.RemoveAllListeners();
        NegativeResponse.onClick.RemoveAllListeners();

        if (onClickYes != null)
        {
            PositiveResponse.onClick.AddListener(() =>
            {
                ClosePopUp();
                onClickYes();
            });
        }
        else
            PositiveResponse.onClick.AddListener(ClosePopUp);

        if (onClickNo != null)
        {
            NegativeResponse.gameObject.SetActive(true);
            PositiveResponse.GetComponentInChildren<Text>().text = "Si";
            NegativeResponse.onClick.AddListener(() =>
            {
                ClosePopUp();
                onClickNo();
            });
        }
        else
            NegativeResponse.gameObject.SetActive(false);

    }

    void ClosePopUp()
    {

        OnPopUpStatusChange?.Invoke(false);
        PopupWindow.SetActive(false);
    }
}
