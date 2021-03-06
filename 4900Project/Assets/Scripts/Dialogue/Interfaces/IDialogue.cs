﻿using System;
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
    public interface IDialogue
    {
        // Properties
        int Id { get; }
        bool IsVisible { get; }
        List<IDHistory> History { get; }

        // Events
        DialogueUpdatedEvent PageUpdated { get; }
        DialogueUpdatedEvent DialogueClosed { get; }
        DialogueUpdatedEvent DialogueOpened { get; }

        // Public Getters
        IDPage GetPage();
        bool HasNextPage(int buttonId);

        // Public Methods
        void GoToNextPage(int buttonId);
        void Hide();
        void PressButton(int buttonId);
        void Show();
    }
}
