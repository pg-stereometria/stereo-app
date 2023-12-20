using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace StereoApp.Presenter.UIHandlers.MainMenu
{
    public class UpdateValueFromSlider : MonoBehaviour
    {
        [SerializeField]
        private Slider slider;
        [SerializeField]
        private TMP_Text text;

        public void OnSliderValueChanged()
        {
            text.text = slider.value.ToString();
        }
    }
}
