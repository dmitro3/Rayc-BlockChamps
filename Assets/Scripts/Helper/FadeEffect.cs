using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeEffect : MonoBehaviour
{
    public CanvasGroup ui;

    bool isFadingIn = false;
    bool isFadingOut = false;

    public void FadeIn()
    {
        Debug.Log("FadeIn called");
        ui.alpha = 0;
        isFadingIn = true;
        isFadingOut = false;
    }

    public void FadeOut()
    {
        ui.alpha = 1;
        isFadingIn = false;
        isFadingOut = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFadingIn)
        {
            if (ui.alpha < 1)
                ui.alpha += Time.deltaTime;
            else isFadingIn = false;
        }
        if (isFadingOut)
        {
            if (ui.alpha > 0)
                ui.alpha -= Time.deltaTime;
            else isFadingOut = false;
        }
    }
}
