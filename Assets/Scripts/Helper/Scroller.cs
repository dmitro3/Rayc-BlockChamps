using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scroller : MonoBehaviour
{
    [SerializeField] RawImage _img;
    [SerializeField] float _rightDisplacement;

    [SerializeField] float _upDisplacement;

    void Update()
    {
        _img.uvRect = new Rect(_img.uvRect.position + new Vector2(_rightDisplacement, _upDisplacement) * Time.deltaTime, _img.uvRect.size);
    }
}
