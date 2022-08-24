using System.Collections;
using System.Collections.Generic;
using MoralisUnity.Kits.AuthenticationKit;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginPage : MonoBehaviour
{
    public AuthenticationKit authKit;
    public UserInfo userInfo;

    public void OnPlayButtonClicked()
    {
        authKit.InitializeAsync();
    }

    public void OnConnect()
    {
        SceneManager.LoadScene(1);
    }
}
