using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gashapon : MonoBehaviour
{
    [SerializeField] GameObject gashapons;
    [SerializeField] float _forceMultipler;

    [SerializeField] float seconds;

    [SerializeField] Animator leverAnimator;

    [SerializeField] int price;

    DialogueBox dialogueBox;

    Player player;
 
    bool SuccessfulDeduction;

    void Start()
    {
        dialogueBox = FindObjectOfType<UIMonitor>().dialogueBox;
        player = FindObjectOfType<Player>();
    }


    void OnLeverPulled()
    {
        SuccessfulDeduction = player.DeductCoins(price);
        if (SuccessfulDeduction)
        {
            leverAnimator?.Play("Pull");
            StartCoroutine(WaitBeforePerturb());
            dialogueBox.HideDialogue();
        }
        else 
        {
            dialogueBox.HideDialogue();
            dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);
            dialogueBox.ShowDialogue("Insufficient Coins", "You don't have enough coins to do a lucky draw!", false);
        }
    }

    public void OnMachineClicked()
    {
        dialogueBox.SetFunctionToYesButton(OnLeverPulled);
        // dialogueBox.SetFunctionToYesButton(dialogueBox.HideDialogue);
        
        dialogueBox.SetFunctionToCloseButton(dialogueBox.HideDialogue);
        dialogueBox.ShowDialogue("Lucky Draw", "Spend " + price + " coins to test your luck?", true);
    }

    IEnumerator WaitBeforePerturb()
    {
        yield return new WaitForSeconds(seconds);
        foreach (Transform child in gashapons.transform)
        {
            if (Random.Range(0, 100) < 50)
            {
                child.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f) * _forceMultipler * 30f, 
                                                    Random.Range(-1f, 1f) * _forceMultipler * 30f), ForceMode2D.Impulse);
            }
        }
    }
}
