using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UI;

namespace Assets.Scripts.EscapeMenu.Interfaces
{
    /// <summary>
    /// A class for storing Setting Slider data.
    /// </summary>
    [System.Serializable]
    public class SettingsSliderData
    {
        /// <summary>
        /// A string pattern that will be used for displaying the value.
        /// The value will be parsed through this pattern before its text gets updated.
        /// </summary>
        public string DisplayFormatPattern;

        /// <summary>
        /// The name for the setting. This identifies what gets updated in the DataTracker class.
        /// </summary>
        public string SettingName;

        /// <summary>
        /// The Slider object itself. This is the main UI element for the setting.
        /// </summary>
        public Slider slider;
    }
}
