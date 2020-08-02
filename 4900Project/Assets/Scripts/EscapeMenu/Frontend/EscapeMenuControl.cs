using Assets.Scripts.ExitMenu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.EscapeMenu.Interfaces
{
    class EscapeMenuControl : MonoBehaviour
    {
        public Button quitButton;
        public Button closeButton;
        public List<SettingsSliderData> sliders;

        void Start()
        {
            // Hook up the quit button
            quitButton.onClick.AddListener(ExitControl.BringUpExitMenu);
            closeButton.onClick.AddListener(Hide);

            // Hook up the sliders
            foreach (var slider in sliders)
            {
                HookupSlider(slider);
            }


        }

        public void Hide()
        {
            gameObject.GetComponent<Canvas>().enabled = false;
        }

        private void HookupSlider(SettingsSliderData sliderData)
        {
            var slider = sliderData.slider;
            var valueDisplay = slider.transform.Find("Background/ValueText").GetComponent<TMPro.TextMeshProUGUI>();
            slider.onValueChanged.AddListener((float value) =>
            {
                valueDisplay.text = value.ToString(sliderData.DisplayFormatPattern);
                UnityEngine.Debug.Log($"{sliderData.SettingName} updated to {value}");
            });
        }
    }
}
