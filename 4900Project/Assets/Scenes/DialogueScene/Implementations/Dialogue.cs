﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

namespace Dialogue {
    /// <summary>
    /// The actual implementation for a Dialogue.
    /// </summary>
    class Dialogue : IDialogue {
        // Properties
        public int Id { get; protected set; }
        protected IEnumerable<IDPage> Pages { get; set; }
        protected IEnumerator<IDPage> PageEnumerator;

        // Events
        /// <summary>
        /// Called when the page is updated, passes in the ID of the dialog. Should be used for updating UI.
        /// </summary>
        public DEvent PageUpdated { get; private set; }
        public DEvent DialogueClosed { get; private set; }
        public DEvent DialogueOpened { get; private set; }


        // Constructor
        public Dialogue(int id, IEnumerable<IDPage> pages) {
            this.Id = id;
            this.Pages = pages;
            this.PageEnumerator = pages.GetEnumerator();

            // initialize the events
            PageUpdated = new DEvent();
            DialogueClosed = new DEvent();
            DialogueOpened = new DEvent();
        }

        // Public Getters
        /// <summary>
        /// Retrieves the current page of the dialog.
        /// </summary>
        /// <returns></returns>
        public IDPage GetPage() {
            return PageEnumerator.Current;
        }

        /// <summary>
        /// Checks if the enumerator has reached the last page.
        /// </summary>
        /// <returns></returns>
        public bool HasMorePages() {
            // We're on the last page if our enumerator's current page is the last in the list
            return PageEnumerator.Current == Pages.LastOrDefault();
        }


        // Public Methods
        /// <summary>
        /// Advances the dialog to the next page.
        /// </summary>
        public void GoToNextPage() {
            // Verify that we can move forward
            if (!HasMorePages()) {
                // TODO: Does this throw an error? Or do we just silently fail?
                throw new Exception("Attempted to move to the next page, but there are no more pages.");
            }

            // Push forward to the next page
            PageEnumerator.MoveNext();
            Update();
        }

        /// <summary>
        /// Hides the dialog.
        /// </summary>
        public void Hide() {
            DialogueClosed.Invoke(Id);
        }

        /// <summary>
        /// Activates a button, given its index.
        /// </summary>
        /// <param name="buttonIndex">The index of the button to activate</param>
        public void PressButton(int buttonId) {
            var page = GetPage();
            var button = page.GetButton(buttonId);

            button.OnButtonClick(Id);
        }

        /// <summary>
        /// Shows the dialog.
        /// </summary>
        public void Show() {
            DialogueOpened.Invoke(Id);
        }

        // Protected Methods
        /// <summary>
        /// Updates the dialog.
        /// </summary>
        protected void Update() {
            PageUpdated.Invoke(Id);
        }
    }
}
