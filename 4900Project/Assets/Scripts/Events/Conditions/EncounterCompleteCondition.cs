using System;
using UnityEngine.Events;
using Encounters;

namespace SIEvents
{
    public class EncounterCompleteCondition : Condition
    {
        private readonly int encounterId;
        private readonly UnityAction<Encounter> listener;

        public EncounterCompleteCondition(string _description, int encounterId)
         : base(_description)
        {
            this.encounterId = encounterId;
            listener = new UnityAction<Encounter>((Encounter encounter) => Handler(encounter.Id));
        }

        public override void AllowProgression()
        {
            EventManager.Instance.OnEncounterComplete.AddListener(listener);
        }

        public override void DisallowProgression()
        {
            EventManager.Instance.OnEncounterComplete.RemoveListener(listener);
        }

        public void Handler(int encounterId)
        {
            if (encounterId == this.encounterId)
            {
                Satisfy();
            }
        }

        public override string ToString()
        {
            return string.Format("Encounter {0}", encounterId);
        }
    }
}