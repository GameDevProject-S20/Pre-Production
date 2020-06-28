using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor.XR;
using UnityEngine.Events;

namespace Dialogue
{
    /// <summary>
    /// This represents a single Dialogue class.
    /// </summary>
    public interface IDialogue
    {
        // Properties
        int Id { get; }
        bool IsVisible { get; }

        // Events
        DialogueUpdatedEvent PageUpdated { get; }
        DialogueUpdatedEvent DialogueClosed { get; }
        DialogueUpdatedEvent DialogueOpened { get; }

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
