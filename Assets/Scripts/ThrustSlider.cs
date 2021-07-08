using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThrustSlider : MonoBehaviour
{
    public Slider slider;

   public void ChangeSliderValue(float value)         //Method to call when changing the value of the slider.
    {
        slider.value = value;
    }

    public float GetSliderValue()
    {
        return slider.value;
    }
}
