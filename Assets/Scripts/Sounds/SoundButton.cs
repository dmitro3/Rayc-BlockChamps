using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    public Component slider;

    public void OnClick()
    {
        slider.gameObject.SetActive(!slider.gameObject.activeSelf);
    }

    public void OnExitSettings()
    {
        slider.gameObject.SetActive(false);
    }

}
