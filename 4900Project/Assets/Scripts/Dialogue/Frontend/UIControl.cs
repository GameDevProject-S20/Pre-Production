using Dialogue;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Dialogue.Frontend
{
    /// <summary>
    /// This class controls the UI for the Dialogue system.
    /// </summary>
    class UIControl : MonoBehaviour
    {
        // Constants
        /// <summary>
        /// TextTags defines all the tags for hooking in with TextMesh Pro.
        /// In particular, this allows us to do things like:
        ///     - Anchor the Page Text to the right side, while leaving responses on the left
        ///     - Coloring all history in with gray
        /// </summary>
        private class TextTags
        {
            public class StartTags
            {
                public static readonly string GrayColorTag = "<color=#918b7e>";
                public static readonly string RightAlign = "<margin-left=25%><align=right>";
                public static readonly string LeftAlign = "<margin-right=50%><align=left>";
            }
            public class EndTags
            {
                public static readonly string Color = "</color>";
                public static readonly string Align = "</align></margin>";
            }
            public static readonly string NewLine = "\n";
        }

        // Properties
        /// <summary>
        /// The list of buttons. Having this as a list lets us easily re-arrange, add new buttons,
        /// and populate the data of the buttons.
        /// </summary>
        public List<Button> buttons;

        /// <summary>
        /// The Text Display object. Will be populated with the page dialog.
        /// </summary>
        public GameObject textDisplay;

        /// <summary>
        /// The Avatar's image
        /// </summary>
        public GameObject avatarDisplayImg;

        /// <summary>
        /// The Avatar's name
        /// </summary>
        public GameObject avatarDisplayName;

        // Startup / constructor
        void Start()
        {
            // Hook in all the buttons - this allows the button to fire into the Dialogue system,
            //  which will in turn press the button we're looking at
            for (var i = 0; i < buttons.Count; i++)
            {
                SetupButton(buttons[i], i);
            }

            // Listen for the DialogueManager to update, so that we can update our display
            DialogueManager.ActiveDialogueChanged.AddListener(() =>
            {
                UpdateDisplay();
            });
            UpdateDisplay();
        }

        // Protected Methods
        // Initial Setup - This hooks in a single button to fire off its index to the DialogueManager
        protected void SetupButton(Button button, int index)
        {
            button.onClick.AddListener(() =>
            {
                DialogueManager.GetActiveDialogue().PressButton(index);
            });
        }

        // Dialogue Data Updating
        /// <summary>
        /// The main Update method. Updates the page display text, the avatar, and the buttons.
        /// </summary>
        protected void UpdateDisplay()
        {
            var dialogue = DialogueManager.GetActiveDialogue();

            // If we don't have an active dialog, hide the UI
            if (dialogue == null)
            {
                Hide();
            } else
            {
                var activePage = dialogue.GetPage();
                var history = dialogue.History;

                // Update all the data
                UpdateButtons(activePage.Buttons);
                UpdateAvatarDisplay(activePage.Avatar);
                UpdatePageTextDisplay(history, activePage);

                // Show the display
                Show();
            }
        }

        /// <summary>
        /// Hooks up the buttons to display according to the active page's Buttons data.
        /// </summary>
        /// <param name="activePage"></param>
        protected void UpdateButtons(IEnumerable<IDButton> pageButtons)
        {
            var buttonsCount = pageButtons.Count();
            
            // If the page has more buttons than we have support for, log out a warning.
            // We'll continue past this point, but the Dialogue should be updated with more buttons if this becomes a problem.
            if (buttonsCount > buttons.Count)
            {
                Debug.LogWarning($"The Dialogue page has {buttonsCount} buttons, but we're only set up to handle {buttons.Count} buttons. Only taking the first {buttons.Count}.");
            }

            // Update the buttons
            for (var i = 0; i < buttons.Count; i++)
            {
                var physicalButton = buttons.ElementAt(i);
                var buttonData = pageButtons.ElementAtOrDefault(i);

                // Update the text
                physicalButton.GetComponentInChildren<Text>().text = buttonData != null ? buttonData.Text : "";
            }
        }

        /// <summary>
        /// Updates the text for the current page.
        /// </summary>
        /// <param name="history"></param>
        /// <param name="currentPage"></param>
        protected void UpdatePageTextDisplay(IEnumerable<IDHistory> history, IDPage currentPage)
        {
            textDisplay.GetComponent<TextMeshProUGUI>().text = BuildPageString(history, currentPage);
        }

        /// <summary>
        /// Updates the avatar display to the provided avatar details.
        /// </summary>
        /// <param name="avatar"></param>
        protected void UpdateAvatarDisplay(IDAvatar avatar)
        {
            // Note: Error case - if no Avatar is being passed in, eg. for old code,
            //   we want to just hide the avatar & bypass it
            var hasAvatar = (avatar != null);
            avatarDisplayImg.SetActive(hasAvatar);
            avatarDisplayName.SetActive(hasAvatar);

            if (avatar == null)
            {
                return;
            }

            // otherwise, since we have an avatar, we can update that
            avatarDisplayImg.GetComponent<Image>().sprite = avatar.Icon;
            avatarDisplayName.GetComponent<Text>().text = avatar.Name;
        }

        // Visibility Updates
        /// <summary>
        /// Hides the Dialogue UI.
        /// </summary>
        protected void Hide()
        {
            SetVisible(false);
        }
        /// <summary>
        /// Shows the Dialog UI.
        /// </summary>
        protected void Show()
        {
            SetVisible(true);
        }
        /// <summary>
        /// Helper for Hiding & Showing. Makes it easy to adjust if need be.
        /// </summary>
        /// <param name="isVis"></param>
        protected void SetVisible(bool isVis)
        {
            gameObject.GetComponent<Canvas>().enabled = isVis;
        }

        // Misc
        /// <summary>
        /// Builds a TextMesh Pro string for the given history & page data.
        /// </summary>
        /// <returns></returns>
        protected string BuildPageString(IEnumerable<IDHistory> history, IDPage activePage)
        {
            // This one is pretty massive. Things to note:
            //   1. All Page Text should be displayed on the right. All responses are on the left.
            //   2. All History should be displayed in gray (<color=#918b7e>).
            //   3. It's possible for someone to press a button and stay on the same page. In this case ...

            var builder = new StringBuilder();
            
            // Add in all the history
            builder.Append(TextTags.StartTags.GrayColorTag);
            foreach (var page in history)
            {
                // Add the page's text
                builder.Append(TextTags.StartTags.RightAlign);
                builder.Append($"{page.Page.Text}{TextTags.NewLine}");
                builder.Append(TextTags.EndTags.Align);

                // Add the response
                builder.Append(TextTags.StartTags.LeftAlign);
                foreach (var response in page.Responses)
                {
                    builder.Append($"{response}{TextTags.NewLine}");
                }
                builder.Append(TextTags.EndTags.Align);
            }
            builder.Append(TextTags.EndTags.Color);

            // Add in the current page text
            builder.Append(TextTags.StartTags.RightAlign);
            builder.Append(activePage.Text);
            builder.Append(TextTags.EndTags.Align);

            return builder.ToString();
        }

    }
}
