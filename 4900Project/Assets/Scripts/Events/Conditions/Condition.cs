using System.Collections;
using System.Collections.Generic;

namespace SIEvents
{
    /// <summary>
    /// Specific functional conditions that make up a Quest Stage
    /// 
    /// They listen for events from the QuestManager and mark themselves as satisfied accordingly
    /// </summary>
    public abstract class Condition : IProgressor
    {
        public bool IsSatisfied { get; private set; } = false;
        public string Description;

        private Condition() { }
        public Condition(string _description)
        {
            Description = _description;
        }
        public abstract void AllowProgression();

        public abstract void DisallowProgression();

        protected void Satisfy()
        {
            IsSatisfied = true;
            DisallowProgression();
            EventManager.Instance.OnConditionComplete.Invoke(this);
        }

        public abstract override string ToString();
    }
}