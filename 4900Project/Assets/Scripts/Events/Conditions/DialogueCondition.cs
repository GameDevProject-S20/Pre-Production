using System;
using System.Collections;
using System.Collections.Generic;

namespace SIEvents
{
    public class DialogueCondition : Condition
    {
        string DialogueButtonId;
        int Count;
        int current;

        public DialogueCondition(string _description, string _id, int _count=1)
         : base(_description)
        {
            DialogueButtonId = _id;
            Count = _count;
            current = 0;

            //Todo: Use EventManager
        }

        //Todo
        public override void AllowProgression()
        {
            EventManager.Instance.OnDialogueSelected.AddListener(Handler);
        }

        //Todo
        public override void DisallowProgression()
        {
            EventManager.Instance.OnDialogueSelected.RemoveListener(Handler);
        }

        public void Handler(string dialogueButtonId)
        {
            if (dialogueButtonId == this.DialogueButtonId)
            {
                current++;
                if(current >= Count) Satisfy();
            }
        }

        //Todo
        public override string ToString()
        {
            return Description;
            //throw new NotImplementedException();
        }
    }
}
