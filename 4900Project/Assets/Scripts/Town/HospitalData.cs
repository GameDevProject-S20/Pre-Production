using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Town
{
    /// <summary>
    /// Stores the constants relating to the Hospital, such as name, description, and encounter ID.
    /// </summary>
    [Serializable]
    public class HospitalData
    {
        public string Name = "Hospital";
        public string Description = "Heal yourself";
        public int EncounterId = 12;
    }
}
