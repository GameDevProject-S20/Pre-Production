using Assets.Scripts.EscapeMenu.Frontend;
using Assets.Scripts.ExitMenu;
using SIEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
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
        /// Button for opening the credits
        /// </summary>
        public Button creditsButton;

        /// <summary>
        /// Every Slider that the Escape Menu should be controlling, along with additional data
        /// </summary>
        public List<SettingsSliderData> sliders;
        bool isOpen = false;
        // Public Methods
        /// <summary>
        /// Startup - Hook everything up
        /// </summary>
        void Start()
        {
            // Hook up the buttons
            closeButton.onClick.AddListener(Hide);
            quitButton.onClick.AddListener(()=>
            {
                Hide();
                ExitControl.BringUpExitMenu();
            });
            creditsButton.onClick.AddListener(() =>
            {
                UnityEngine.Debug.Log("Credits");
                DataTracker.Current.EventManager.OnCreditsButtonClicked.Invoke();
                SceneManager.LoadSceneAsync("Credits", LoadSceneMode.Additive); 
            });

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

            // Listen for events
            DataTracker.Current.EventManager.OnSettingsChanged.AddListener((setting) =>
            {
                UnityEngine.Debug.Log($"The setting {setting} changed");
            });
            DataTracker.Current.EventManager.EscapeMenuRequested.AddListener(Toggle);
            DataTracker.Current.EventManager.EscapeMenuCloseRequested.AddListener(Hide);
        }

        /// <summary>
        /// Toggles the escape menu, either showing it or hiding it.
        /// </summary>
        public void Toggle()
        {
            // If the canvas is enabled, we want to disable it;
            // Otherwise, we want to enable it
            var canvas = gameObject.GetComponent<Canvas>();
            if (canvas.enabled)
            {
                // Note that we're calling the method here so that the other stuff,
                // like freezing/unfreezing the map, is automatic
                Hide();
            }
            else
            {
                Show();
            }
        }

        /// <summary>
        /// Shows the Escape Menu canvas
        /// </summary>
        public void Show()
        {
            isOpen = true;
            gameObject.GetComponent<Canvas>().enabled = true;
            DataTracker.Current.EventManager.FreezeMap.Invoke();
        }

        /// <summary>
        /// Hide the escape menu canvas
        /// </summary>
        public void Hide()
        {
            if(isOpen) {DataTracker.Current.EventManager.UnfreezeMap.Invoke();}

            isOpen = false;
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
            // Identify the classes we need access to
            var slider = sliderData.slider;
            var customSlider = slider.GetComponent<CustomEscapeMenuSlider>();

            // When the slider's value updates, we need to update the value in the UI
            slider.onValueChanged.AddListener((float value) =>
            {
                UpdateValueDisplay(sliderData);
                UpdateSetting(sliderData.SettingName, value);
            });
            /*
            // When the player finishes changing a value, we want to update the setting
            // Note that this is done after a value is changed, so that we don't call the method too often
            customSlider.MouseReleased.AddListener((value) =>
            {
            });*/
        }

        /// <summary>
        /// Updates a given setting with the provided value
        /// </summary>
        private void UpdateSetting(string settingName, float value)
        {
            DataTracker.Current.SettingsManager.UpdateSettingValue(settingName, value);
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
