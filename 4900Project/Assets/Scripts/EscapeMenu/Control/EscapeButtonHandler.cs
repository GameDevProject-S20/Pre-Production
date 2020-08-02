using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.ExitMenu
{
    class EscapeButtonHandler : MonoBehaviour
    {
        bool wasKeyDown = false;

        void Update()
        {
            var isDown = Input.GetKey(KeyCode.Escape);
            
            // Don't do anything if the input hasn't changed
            if (isDown == wasKeyDown)
            {
                return;
            }
            wasKeyDown = isDown;


            // If the key is down, fire the EscapeMenuRequested event
            if (isDown)
            {
                DataTracker.Current.EventManager.EscapeMenuRequested.Invoke();
            }
        }
    }
}
