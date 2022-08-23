using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoutButton : MonoBehaviour
{
    DialogueBox dialogueBox;

    void Start()
    {
        dialogueBox = FindObjectOfType<UIMonitor>().dialogueBox;
    }

    public void OnClick()
    {
        dialogueBox.SetFunctionToYesButton(() => MoralisUnity.Moralis.LogOutAsync());
        dialogueBox.SetFunctionToYesButton(() => SceneManager.LoadScene(0));
        dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);
        dialogueBox.ShowDialogue("Logout", "Are you sure you want to logout?", true);
    }
}
