using Assets.Scripts.EscapeMenu.Interfaces;
using Dialogue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEditor;
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

        /// <summary>
        /// The Content of the scroll view
        /// </summary>
        public RectTransform contentRect;

        /// <summary>
        /// This is the main rect of the scroll view.
        /// It determines the minimum size for content.
        /// </summary>
        public RectTransform scrollViewContainerRect;

        /// <summary>
        /// Text colour for (un)interactable buttons
        /// </summary>
        public Color interactableButtonTextColour = new Color(204, 196, 179, 255);
        public Color uninteractableButtonTextColour = new Color(145, 139, 126, 255);

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
            DialogueManager.Instance.ActiveDialogueChanged.AddListener(() =>
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
                if (button.GetComponentInChildren<Text>().text == "")
                {
                    return;
                }

                DialogueManager.Instance.GetActiveDialogue().PressButton(index);
            });
        }

        // Dialogue Data Updating
        /// <summary>
        /// The main Update method. Updates the page display text, the avatar, and the buttons.
        /// </summary>
        protected void UpdateDisplay()
        {
            var dialogue = DialogueManager.Instance.GetActiveDialogue();

            // If we don't have an active dialog, hide the UI
            if (dialogue == null)
            {
                Hide();
            } else
            {
                var activePage = dialogue.GetPage();
                var history = dialogue.History;

                // Show the display
                Show();

                Debug.Log(string.Format("Active Page\n\n{0}", activePage));

                // Update all the data
                UpdateAvatarDisplay(activePage.Avatar);
                UpdatePageTextDisplay(history, activePage);
                 
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
                Text text = physicalButton.GetComponentInChildren<Text>();
                text.text = buttonData != null ? buttonData.Text : "";

                if (buttonData != null)
                {
                    bool interactable = buttonData.Conditions.All(c => c.IsSatisfied());
                    physicalButton.interactable = interactable;
                    text.color = (interactable) ? interactableButtonTextColour : uninteractableButtonTextColour;
                }
            }
        }

        /// <summary>
        /// Updates the text for the current page.
        /// </summary>
        /// <param name="history"></param>
        /// <param name="currentPage"></param>
        protected void UpdatePageTextDisplay(IEnumerable<IDHistory> history, IDPage currentPage)
        {

            var textMeshPro = textDisplay.GetComponent<TextMeshProUGUI>();
            var nextPageText = BuildPageString(history, currentPage);
            if (history.Count() == 0)
            {
                textMeshPro.maxVisibleCharacters = 0;
                textMeshPro.text = "";
            }

            // Resize the content based on what's already in the text
            UpdatePageScrolling();

            textMeshPro.text = nextPageText;
            StartCoroutine(UpdatePage(currentPage.Buttons));
        }

        protected IEnumerator UpdatePage(IEnumerable<IDButton> buttons)
        {
            UpdateButtons(new List<IDButton>());
            yield return StartCoroutine(PlayTextTypingAnimation());
            UpdateButtons(buttons);
            UpdatePageScrolling();
        }

        /// <summary>
        /// Plays an animation of each character being typed out one-by-one.
        /// Animates from the currently displayed text to the next page.
        /// </summary>
        /// <returns></returns>
        protected IEnumerator PlayTextTypingAnimation()
        {
            var textMeshPro = textDisplay.GetComponent<TextMeshProUGUI>();
            int charCount = System.Text.RegularExpressions.Regex.Replace(textMeshPro.text, "<.*?>", String.Empty).Length;
            for (var i = textMeshPro.maxVisibleCharacters; i < charCount; i++)
            {
                textMeshPro.maxVisibleCharacters = i;

                // Typing Speed goes from 10 to 100, where 10 is the slowest and 100 is the fastest.
                // The fastest speed we can go is 0.01, which is 1/100.
                // The slowest speed we want to go is 0.10, which is 1/10. So use 1/TypingSpeed
                float speed = 100f;
                try
                {
                    speed = DataTracker.Current.SettingsManager.DialogueSpeed;
                }
                catch { } 

                yield return new WaitForSeconds(1 / speed);
            }
            textMeshPro.maxVisibleCharacters = charCount;
        }

        /// <summary>
        /// This method controls the Dialogue's scroll bar for the text display.
        /// The scroll bar will update as text is added in, so that the user is able to scroll through all text.
        /// </summary>
        protected void UpdatePageScrolling()
        {
            // Setting up variables
            var textMesh = textDisplay.GetComponent<TextMeshProUGUI>();
            var textRect = textDisplay.GetComponent<RectTransform>();
            var scrollRect = scrollViewContainerRect.GetComponent<ScrollRect>();

            // Store the height of the components for reference
            var textHeight = textMesh.preferredHeight;
            var scrollHeight = scrollViewContainerRect.sizeDelta.y;

            // BUG: Sometimes this will be called before the layout updates the height.
            if (scrollHeight == 0)
            {
                // In this case, we want to delay the function call
                Invoke("UpdatePageScrolling", 0.01f);
            }
            else
            {
                // Based on that, we need to decide on alignment & positioning of the text.
                // If we don't yet have a full Dialogue, we want everything displaying to the bottom (in which case we need to position it to the bottom);
                //  otherwise, we want it to be displaying from the top, so that everything will be displayed. In this case, it positions to the top.
                var alignment = (textHeight < scrollHeight) ? TextAlignmentOptions.Bottom : TextAlignmentOptions.Top;
                var textPosition = (textHeight < scrollHeight) ? -scrollHeight : 0;

                // Now that we have the variables stored, we can just go through and update:
                // The contentRect displays the content of the frame. It needs to be sized to newHeight.
                contentRect.sizeDelta = new Vector2(contentRect.sizeDelta.x, textHeight);

                // Update the text alignment & positioning, based on what was mentioned above
                textMesh.alignment = alignment;
                textRect.localPosition = new Vector3(textRect.localPosition.x, textPosition, textRect.localPosition.z);

                // And we want to move them to the bottom of the Dialogue, so that they see the latest text update
                scrollRect.normalizedPosition = new Vector2(0, 0);
            }
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
