using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Dialogue
{
    /// <summary>
    /// This represents a single Dialogue class.
    /// </summary>
    interface IDialogue
    {
        // Properties
        int Id { get; }

        // Events
        DEvent PageUpdated { get; }
        DEvent DialogueClosed { get; }
        DEvent DialogueOpened { get; }

        // Public Getters
        IDPage GetPage();
        bool HasMorePages();

        // Public Methods
        void GoToNextPage();
        void Hide();
        void PressButton(int buttonId);
        void Show();
    }
}
