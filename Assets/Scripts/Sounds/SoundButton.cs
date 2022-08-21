using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SoundButton : MonoBehaviour
{
    public Slider slider;

    public void OnClick()
    {
        slider.gameObject.SetActive(!slider.IsActive());
    }

    public void OnExitSettings()
    {
        slider.gameObject.SetActive(false);
    }

}
