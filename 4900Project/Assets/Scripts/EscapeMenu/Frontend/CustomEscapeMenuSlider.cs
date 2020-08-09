using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.EscapeMenu.Frontend
{
    /// <summary>
    /// A custom slider used with the Escape Menu. It adds an event for Mouse Release,
    /// so that full setting updates only go through when the mouse is released.
    /// </summary>
    class CustomEscapeMenuSlider : MonoBehaviour, IPointerUpHandler
    {
        // Public Events
        public class CustomSliderEvent : UnityEvent<float> { }
        public CustomSliderEvent MouseReleased;

        // Private Members
        private Slider slider;

        // Public Methods
        void Start()
        {
            slider = GetComponent<Slider>();
            MouseReleased = new CustomSliderEvent();
        }

        /// <summary>
        /// Listens for the mouse to be released. Fires the MouseReleased event.
        /// </summary>
        /// <param name="eventData"></param>
        public void OnPointerUp(PointerEventData eventData)
        {
            MouseReleased.Invoke(slider.value);
        }
    }
}
