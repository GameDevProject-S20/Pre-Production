using System;
using System.Collections;
using System.Collections.Generic;

namespace SIEvents
{
    public class DialogueCondition : Condition
    {
        string DialogueButtonId;

        public DialogueCondition(string _description, string _id)
         : base(_description)
        {
            DialogueButtonId = _id;

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
                Satisfy();
            }
        }

        //Todo
        public override string ToString()
        {
            return "";
            //throw new NotImplementedException();
        }
    }
}
