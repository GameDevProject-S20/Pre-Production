using System;
using UnityEngine.Events;

namespace SIEvents
{
    public class TownEnterCondition : Condition
    {
        private readonly int townId;
        private readonly UnityAction<Town> listener;

        public TownEnterCondition(string _description, int townId)
         : base(_description)
        {
            this.townId = townId;
            listener = (Town town) => Handler(town.Id);
        }

        public override void AllowProgression()
        {
            EventManager.Instance.OnTownEnter.AddListener(listener);
        }

        public override void DisallowProgression()
        {
            EventManager.Instance.OnTownEnter.RemoveListener(listener);
        }

        public void Handler(int townId)
        {
            if (townId == this.townId)
            {
                Satisfy();
            }
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
