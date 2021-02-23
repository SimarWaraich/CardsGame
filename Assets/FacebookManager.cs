using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;


public class FacebookManager : MonoBehaviour
{
    string accessToken = "EAALRxMeGtT8BADFwg23uFLOA5UrYguAkb0KvHWEM12XZAxl5qFGSKCjYJsJ8SJrH9ncPYNLWA6kQSEU2xAxU2f0FNfYvoJ7aRRRZAyyZAGqM22ATl4Kl6haiQ6o5aN4LnDlcHkhunRxSZBYAxrSzV5PaGtPIEFXlgABZCWT4CCAZDZD";

    private PopUp popUp;
    public WebServices services;

    void Start()
    {
        popUp = PopUp.Instance;

        if (!FB.IsInitialized)
        {
            FB.Init();
            print("Facebook INIT");
        }

    }

    public void CallFBLogin()
    {
        LoadingScreen.Instance.LoadingStatus(true);
        FB.LogInWithReadPermissions(new List<string>() { "public_profile", "email", "user_friends" }, this.HandleResult);
#if UNITY_EDITOR
        var mockLoginDialog = GameObject.FindObjectOfType<Facebook.Unity.Editor.Dialogs.MockLoginDialog>();
        if (mockLoginDialog != null)
        {
            typeof(Facebook.Unity.Editor.Dialogs.MockLoginDialog).GetField("accessToken", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(mockLoginDialog, accessToken);
            typeof(Facebook.Unity.Editor.Dialogs.MockLoginDialog).GetMethod("SendSuccessResult", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(mockLoginDialog, null);
            MonoBehaviour.Destroy(mockLoginDialog);
        }
#endif
    }

    void HandleResult(IResult result)
    {
        if (result == null)
        {

            return;
        }

        if (!string.IsNullOrEmpty(result.Error))
        {
            popUp.ShowPopUp("Facebook Error", result.Error);
        }
        else if (result.Cancelled)
        {
            popUp.ShowPopUp("Canclled", "Facebook login canncelled by user.");
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            AccessToken aToken = AccessToken.CurrentAccessToken;
            services.RegisterFacebookUser(aToken.TokenString);
        }
        else
        {
            popUp.ShowPopUp("Error", "Unknown error occcured.");
        }
        LoadingScreen.Instance.LoadingStatus(false);

    }

    void Update()
    {

    }
}
