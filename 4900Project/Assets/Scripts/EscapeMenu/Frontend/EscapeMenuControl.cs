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
        // Public Properties
        /// <summary>
        /// Button for quitting the game
        /// </summary>
        public Button quitButton;

        /// <summary>
        /// Button for closing the Escape Menu
        /// </summary>
        public Button closeButton;
        
        /// <summary>
        /// Every Slider that the Escape Menu should be controlling, along with additional data
        /// </summary>
        public List<SettingsSliderData> sliders;

        // Public Methods
        /// <summary>
        /// Startup - Hook everything up
        /// </summary>
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

        /// <summary>
        /// Hide the escape menu canvas
        /// </summary>
        public void Hide()
        {
            gameObject.GetComponent<Canvas>().enabled = false;
        }

        // Private Methods
        /// <summary>
        /// Hooks up a single slider. Should only be called on start up, but keeping this separate as a separation of concerns.
        /// Given the SettingsSliderData for the slider, hook it in to the UI & backend.
        /// </summary>
        /// <param name="sliderData"></param>
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
