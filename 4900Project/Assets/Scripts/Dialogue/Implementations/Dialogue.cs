﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine;
using SIEvents;

namespace Dialogue
{
    /// <summary>
    /// The actual implementation for a Dialogue.
    /// </summary>
    [Serializable]
    class Dialogue : IDialogue
    {
        // Properties
        public int Id { get; protected set; }
        public bool IsVisible { get; protected set; }

        private IDPage FirstPage;
        [SerializeField]
        public IDPage CurrentPage { get; private set; }

        /// <summary>
        /// History of the Dialogue. 
        /// This refers to all the pages that the Dialogue has already gone through,
        /// and the responses that the user gave for those pages.
        /// </summary>
        public List<IDHistory> History { get; protected set; }

        // Events
        /// <summary>
        /// Called when the page is updated.
        /// </summary>
        public DialogueUpdatedEvent PageUpdated { get; private set; }
        
        /// <summary>
        /// Fires when the dialog should be closed.
        /// </summary>
        public DialogueUpdatedEvent DialogueClosed { get; private set; }

        /// <summary>
        /// Fired when the dialogue should be opened.
        /// </summary>
        public DialogueUpdatedEvent DialogueOpened { get; private set; }


        // Constructor
        public Dialogue(int id, IDPage root)
        {
            this.Id = id;
            this.IsVisible = true;
            this.FirstPage = root;
            this.CurrentPage = FirstPage;
            this.History = new List<IDHistory>();

            // initialize the events
            PageUpdated = new DialogueUpdatedEvent();
            DialogueClosed = new DialogueUpdatedEvent();
            DialogueOpened = new DialogueUpdatedEvent();
        }

        // Public Getters
        /// <summary>
        /// Retrieves the current page of the dialog.
        /// </summary>
        /// <returns></returns>
        public IDPage GetPage()
        {
            return CurrentPage;
        }

        /// <summary>
        /// Checks if the enumerator has reached the last page.
        /// 
        /// Reuqires a button index in order to determine whether there is more dialogue down 
        /// *that* particular branch.
        /// </summary>
        /// <param name="buttonId">The index of the button to check</param>
        /// <returns></returns>
        public bool HasNextPage(int buttonId)
        {
            // We're on the last page if our enumerator's current page is the last in the list
            return CurrentPage.GetButton(buttonId).NextPage != null;
        }


        // Public Methods
        /// <summary>
        /// Checks if a particular branch has any more dialogue.
        /// </summary>
        /// <param name="buttonId">The index of the button representing the branch of dialogue to check</param>
        public void GoToNextPage(int buttonId)
        {
            // Verify that we can move forward
            if (!HasNextPage(buttonId))
            {
                // TODO: Does this throw an error? Or do we just silently fail?
                throw new Exception("Attempted to move to the next page, but there are no more pages.");
            }

            CurrentPage = CurrentPage.GetButton(buttonId).NextPage;
            
            Update();
        }

        /// <summary>
        /// Hides the dialog.
        /// </summary>
        public void Hide()
        {
            IsVisible = false;
            History.Clear();
            DialogueClosed.Invoke();
            EventManager.Instance.UnfreezeMap.Invoke();
        }

        /// <summary>
        /// Convenience method for activating a button given the button's index.
        /// </summary>
        /// <param name="buttonIndex">The index of the button to activate</param>
        public void PressButton(int buttonIndex)
        {
            var page = GetPage();
            var button = page.GetButton(buttonIndex);

            if (button == null)
            {
                UnityEngine.Debug.LogWarning($"Attempted to press Button Index {buttonIndex}, but no button exists.");
                return;
            }

            AddToHistory(GetPage(), button.Text);

            button.OnButtonClick();

            if (button.NextPage != null)
            {
                GoToNextPage(buttonIndex);
            }
            else
            {
                DFunctions.CloseDialogue();
            }
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        public void Show()
        {
            CurrentPage = FirstPage; // ARL -- here??
            IsVisible = true;
            EventManager.Instance.FreezeMap.Invoke();
            DialogueOpened.Invoke();
        }

        // Protected Methods
        /// <summary>
        /// Updates the dialog.
        /// </summary>
        protected void Update()
        {
            PageUpdated.Invoke();
        }

        /// <summary>
        /// Adds a new page & response into the Dialogue history.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="response"></param>
        public void AddToHistory(IDPage page, string response)
        {
            // There's two situations here:
            // If the last page of our history is also our current page, then we just want to update that history.
            // Otherwise, we need to add a new History into our list.
            IDHistory history;
            if (History.Count > 0 && History.Last().Page == page)
            {
                // In this case, we're still on the same page - we don't want a new History object to be created,
                //  so just use the one that's already existing
                history = History.Last();
            } else
            {
                // Otherwise, we are on a new page, so we want to create & add a new page to our history
                history = new DHistory()
                {
                    Page = page,
                    Responses = new List<string>()
                };
                History.Add(history);
            }

            // In either case, the 'history' value is the IDHistory that we want to use;
            // So we need to add the response into that history's list of responses
            history.Responses.Add(response);
        }
    }
}
