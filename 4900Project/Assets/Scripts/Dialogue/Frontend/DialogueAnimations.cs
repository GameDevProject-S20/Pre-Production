using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Assertions;

namespace Assets.Scripts.Dialogue.Frontend
{
    /*class DialogueAnimations
    {
        ICollection<Action> animationSteps;
        bool isAnimating;

        public DialogueAnimations()
        {
            animationSteps = new LinkedList<Action>();
        }
        public void AddStep(Action step)
        {
            animationSteps.Add(step);
        }
        public void Play()
        {
            Assert.IsFalse(isAnimating, "Is already animating");

        }

        protected static IEnumerator AnimateTransition(TextMeshProUGUI textMeshPro, string currentText, string fadeInText, string fullText)
        {
            var fading = false;
            PlayTextFadeInAnimation(textMeshPro, currentText, fadeInText, fading);

            textMeshPro.maxVisibleCharacters = System.Text.RegularExpressions.Regex.Replace(textMeshPro.text, "<.*?>", String.Empty).Length;
            textMeshPro.text = fullText;

            PlayTextTypingAnimation(textMeshPro);
        }

        /// <summary>
        /// Plays an animation of each character being typed out one-by-one.
        /// Animates from the currently displayed text to the next page.
        /// </summary>
        /// <returns></returns>
        protected static IEnumerator PlayTextTypingAnimation(TextMeshProUGUI textMeshPro)
        {
            int charCount = System.Text.RegularExpressions.Regex.Replace(textMeshPro.text, "<.*?>", String.Empty).Length;
            for (var i = textMeshPro.maxVisibleCharacters; i < charCount; i++)
            {
                textMeshPro.maxVisibleCharacters = i;
                yield return new WaitForSeconds(0.01f);
            }
            textMeshPro.maxVisibleCharacters = charCount;
        }

        /// <summary>
        /// Plays a text fade in animation. This will insert the fadeInText at the bottom and incrementally shift its alpha from 0 to FF.
        /// </summary>
        /// <param name="textMeshPro"></param>
        /// <param name="mainText"></param>
        /// <param name="fadeInText"></param>
        /// <returns></returns>
        protected static IEnumerator PlayTextFadeInAnimation(TextMeshProUGUI textMeshPro, string mainText, string fadeInText)
        {
            for (var i = 0; i < 255; i += 17)
            {
                var alpha = i.ToString("X");
                textMeshPro.text = $"{mainText}<alpha=#{alpha}>{fadeInText}</alpha>";

                yield return new WaitForSeconds(0.01f);
            }
        }
    }*/
}
