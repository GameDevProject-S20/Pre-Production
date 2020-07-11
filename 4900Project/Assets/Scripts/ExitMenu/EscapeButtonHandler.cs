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
        void Update()
        {
            // Escape Button => Exit Menu
            if (Input.GetKey(KeyCode.Escape))
            {
                ExitMenuControl.BringUpExitMenu();
            }
        }
    }
}
