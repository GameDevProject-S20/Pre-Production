using System;
using System.Collections;
using System.Collections.Generic;

namespace SIEvents
{
    public class DialogueCondition : Condition
    {
        string ButtonText;

        public DialogueCondition(string _description, string _buttonText)
         : base(_description)
        {
            ButtonText = _buttonText;

            //Todo: Use EventManager
        }

        //Todo
        public override void AllowProgression()
        {
            throw new NotImplementedException();
        }

        //Todo
        public override void DisallowProgression()
        {
            throw new NotImplementedException();
        }

        public void Handler(string buttonText)
        {
            if (buttonText == this.ButtonText)
            {
                Satisfy();
            }
        }

        //Todo
        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
