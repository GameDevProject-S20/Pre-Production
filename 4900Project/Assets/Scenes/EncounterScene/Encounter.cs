using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UnityEngine;


namespace Encounters
{
    /// <summary>
    /// Encounter data structure
    /// Just a simple class to hold the encounter data
    /// </summary>
    public class Encounter
    {
        private static int nextId = 0;

        /// <summary>
        /// Static id counter
        /// </summary>
        public int Id
        { get; }

        /// <summary>
        /// Name...
        /// </summary>
        public string Name
        { get; }

        public string Tag
        { get; }

        /// <summary>
        /// Initial text presented to the player.
        /// </summary>
        public string BodyText
        { get; }
        ///
        /// <summary>
        /// Choice text presented to the player via buttons.
        /// </summary>
        public ReadOnlyCollection<string> ButtonText
        { get; }

        /// <summary>
        /// Result text dependent on the action selected by the player.
        /// Indexes correspond to ButtonText.
        /// </summary>
        public ReadOnlyCollection<string> ResultText
        { get; }


        public Encounter(string name, string tag, string bodyText,
                         IEnumerable<string> buttonText, IEnumerable<string> resultText)
        {
            Id = nextId++;
            Name = name;
            Tag = tag;
            BodyText = bodyText;
            ButtonText = new ReadOnlyCollection<string>(new List<string>(buttonText));
            ResultText = new ReadOnlyCollection<string>(new List<string>(resultText));
        }

        public override string ToString()
        {
            var sb = new StringBuilder("Encounter {\n");
            sb.AppendLine($"\tId = {Id}");
            sb.AppendLine($"\tName = {Name}");
            sb.AppendLine($"\tTag = {Tag}");
            sb.AppendLine($"\tBodyText = {BodyText}");

            for (int i = 0; i < ButtonText.Count; i++)
            { sb.AppendLine($"\tButton {i} = {ButtonText[i]}"); }

            for (int i = 0; i < ResultText.Count; i++)
            { sb.AppendLine($"\tResult {i} = {ResultText[i]}"); }

            sb.AppendLine("}");

            return sb.ToString();
        }
    }
}
