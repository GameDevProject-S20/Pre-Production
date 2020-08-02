using SIEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.EscapeMenu.Interfaces
{
    /// <summary>
    /// The Settings class. This class stores values for every setting in the game.
    /// </summary>
    public class SettingsManager
    {
        private static SettingsManager instance;

        /// <summary>
        /// Maintains all Settings for the current game.
        /// </summary>
        public static SettingsManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SettingsManager();
                }
                return instance;
            }
        }

        // Public Settings
        /// <summary>
        /// Multiplier for the typing speed animation. Lower value = slower animation.
        /// </summary>
        public float TypingSpeed { get; set; }

        /// <summary>
        /// Multiplier for the game volume. Lower value = lower volume.
        /// </summary>
        public float VolumeMultiplier { get; set; }

        /// <summary>
        /// Multiplier for the map's driving animation. Lower value = slower animation.
        /// </summary>
        public float VehicleSpeed { get; set; }

        /// <summary>
        /// Multiplier for how fast the map scrolls (either by mouse or by WASD). Lower value = slower scrolling.
        /// </summary>
        public float ScrollingSpeedMultiplier { get; set; }

        // Public Methods
        /// <summary>
        /// Updates the value of a setting given by name.
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="value"></param>
        public void UpdateSettingValue(string settingName, object value)
        {
            // Update the property
            var prop = this.GetType().GetProperty(settingName);
            var currentValue = prop.GetValue(this);

            // If the value didn't change, don't do anything
            if (value.Equals(currentValue))
            {
                return;
            }

            // Update the value
            prop.SetValue(this, value);

            // Fire the notification event
            EventManager.Instance.OnSettingsChanged.Invoke(settingName);
        }

    }
}
