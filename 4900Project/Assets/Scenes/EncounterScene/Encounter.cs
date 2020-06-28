using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;


namespace Encounters
{
    public class Encounter
    {
        public string Name
        { get; }

        public string Tag
        { get; }

        public string BodyText
        { get; }

        public string ResultText
        { get; }

        public ReadOnlyCollection<string> ButtonText
        { get; }

        public Encounter(string name, string tag, string bodyText, string resultText, IEnumerable<string> buttonText)
        {
            Name = name;
            Tag = tag;
            BodyText = bodyText;
            ResultText = resultText;
            ButtonText = new ReadOnlyCollection<string>(new List<string>(buttonText));
        }
    }
}
