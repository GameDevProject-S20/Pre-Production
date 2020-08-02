using Assets.Scripts.EscapeMenu.Frontend;
using Assets.Scripts.ExitMenu;
using SIEvents;
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
            // Hook up the buttons
            quitButton.onClick.AddListener(ExitControl.BringUpExitMenu);
            closeButton.onClick.AddListener(Hide);

            // Set up the sliders
            foreach (var slider in sliders)
            {
                // verify that the settings has the correct value
                UpdateSetting(slider.SettingName, slider.slider.value);

                // Update the shown text
                UpdateValueDisplay(slider);

                // hook up events
                HookupSlider(slider);
            }

            EventManager.Instance.OnSettingsChanged.AddListener((setting) =>
            {
                UnityEngine.Debug.Log($"The setting {setting} changed");
            });
            EventManager.Instance.HudMenuClicked.AddListener(Show);
        }

        /// <summary>
        /// Shows the Escape Menu canvas
        /// </summary>
        public void Show()
        {
            gameObject.GetComponent<Canvas>().enabled = true;
            EventManager.Instance.FreezeMap.Invoke();
        }

        /// <summary>
        /// Hide the escape menu canvas
        /// </summary>
        public void Hide()
        {
            gameObject.GetComponent<Canvas>().enabled = false;
            EventManager.Instance.UnfreezeMap.Invoke();
        }

        // Private Methods
        /// <summary>
        /// Hooks up a single slider. Should only be called on start up, but keeping this separate as a separation of concerns.
        /// Given the SettingsSliderData for the slider, hook it in to the UI & backend.
        /// </summary>
        /// <param name="sliderData"></param>
        private void HookupSlider(SettingsSliderData sliderData)
        {
            // Identify the classes we need access to
            var slider = sliderData.slider;
            var customSlider = slider.GetComponent<CustomEscapeMenuSlider>();

            // When the slider's value updates, we need to update the value in the UI
            slider.onValueChanged.AddListener((float _) =>
            {
                UpdateValueDisplay(sliderData);
            });

            // When the player finishes changing a value, we want to update the setting
            // Note that this is done after a value is changed, so that we don't call the method too often
            customSlider.MouseReleased.AddListener((value) =>
            {
                UpdateSetting(sliderData.SettingName, value);
            });
        }

        /// <summary>
        /// Updates a given setting with the provided value
        /// </summary>
        private void UpdateSetting(string settingName, float value)
        {
            Settings.Instance.UpdateSettingValue(settingName, value);
        }

        /// <summary>
        /// Updates the value displayed on the UI
        /// </summary>
        /// <param name="sliderData"></param>
        private void UpdateValueDisplay(SettingsSliderData sliderData)
        {
            var slider = sliderData.slider;
            var valueDisplay = slider.transform.Find("Background/ValueText").GetComponent<TMPro.TextMeshProUGUI>();
            valueDisplay.text = String.Format(sliderData.DisplayFormatPattern, slider.value).Replace(" ", "");
        }
    }
}
