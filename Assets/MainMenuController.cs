using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject LoginScreen, MenuScreen;

    private GameObject CurrentScreen;

    public Image ProfileImage;
    public Text UserName;

    private void Start()
    {
        //if (PlayerPrefs.GetString("UserId", "") == "")
        //{
        //    OpenLoginScreen();
        //}
        //else OpenMenuScreen();
    }

    public void OpenLoginScreen()
    {
        MenuScreen.SetActive(false);
        LoginScreen.SetActive(true);
    }

    public void OpenMenuScreen()
    {
        LoginScreen.SetActive(false);
        MenuScreen.SetActive(true);
        UserName.text = PersistanceManager.GetSavedData<UserData>(Constants.UserDataSavedKey).UsedId;
    }

}
