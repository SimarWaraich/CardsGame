using System;
using System.Collections;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

public class WebServices : MonoBehaviour
{
    public MainMenuController mainMenuController;

    private void Start()
    {
        UserData user = PersistanceManager.GetSavedData<UserData>(Constants.UserDataSavedKey);

        if (user == null)
            mainMenuController.OpenLoginScreen();
        else
        {
            if (user.LoginType == LoginTypes.Guest)
            {
                GuestLogIn(true);
            }
        }



    }

    public void RegisterFacebookUser(string accessToken, bool isAutoLogin = false)
    {
        print("FB Login request");
        var request = new LoginWithFacebookRequest { CreateAccount = !isAutoLogin, AccessToken = accessToken };
        PlayFabClientAPI.LoginWithFacebook(request, OnFBLoginSucess, OnLoginFailure);
    }

    public void GuestLogIn(bool isAutoLogin = false)
    {
        print("Login request");
        var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = !isAutoLogin };
        PlayFabClientAPI.LoginWithCustomID(request, OnGuestLoginSucess, OnLoginFailure);

    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    private void OnGuestLoginSucess(LoginResult result)
    {
        UserData userData = new UserData();
        userData.UsedId = result.PlayFabId;
        userData.LoginType = LoginTypes.Guest;
        PersistanceManager.SaveToPrefs<UserData>(Constants.UserDataSavedKey, userData);
        OnLoginSuccess();

        print("Logged In" + result);
    }

    private void OnFBLoginSucess(LoginResult result)
    {
        UserData userData = new UserData();
        userData.UsedId = result.PlayFabId;
        userData.LoginType = LoginTypes.Facebook;
        PersistanceManager.SaveToPrefs(Constants.UserDataSavedKey, userData);
        OnLoginSuccess();
        print("Logged In" + result);
    }

    private void OnLoginSuccess()
    {
        PopUp.Instance.ShowPopUp("Success", "Logged In successfully.");
        mainMenuController.OpenMenuScreen();
    }
}

public class UserData
{
    public string UsedId;
    public LoginTypes LoginType;
}

public enum LoginTypes
{
    Guest,
    Facebook
}

public class Constants
{
    public const string UserDataSavedKey = "UserDataSavedKey";
}
