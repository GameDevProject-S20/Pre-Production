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
    class Settings
    {
        private static Settings instance;

        /// <summary>
        /// Maintains all Settings for the current game.
        /// </summary>
        public static Settings Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Settings();
                }
                return instance;
            }
        }

        // Public Settings
        /// <summary>
        /// Multiplier for the typing speed animation. Lower value = slower animation.
        /// </summary>
        public float TypingSpeedMultiplier { get; set; }

        /// <summary>
        /// Multiplier for the game volume. Lower value = lower volume.
        /// </summary>
        public float VolumeMultiplier { get; set; }

        /// <summary>
        /// Multiplier for the map's driving animation. Lower value = slower animation.
        /// </summary>
        public float VehicleSpeedMultiplier { get; set; }

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
            prop.SetValue(this, value);

            UnityEngine.Debug.Log($"The setting {settingName} has been updated to the value {value}");

            // Fire the event in the EventManager
            // TODO
        }

    }
}
