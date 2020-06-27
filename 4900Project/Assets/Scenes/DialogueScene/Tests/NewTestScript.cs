using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Dialogue;

namespace Tests
{
    public class NewTestScript
    {
        // A Test behaves as an ordinary method
        [Test]
        public void DialogueCanBeCreatedTest()
        {
            // Create a new page template
            var pages = new List<IDPage>() {
                new DPage()
                {
                    Text = "Hello World!",
                    Buttons = new List<IDButton>()
                }
            };

            // Create the dialog & verify that it exists
            var dialog = DialogueManager.CreateDialog(pages);
            Assert.IsNotNull(dialog, "The dialog creation failed: Received a null result");

            // Verify that the dialog can be found by ID
            Assert.IsTrue(DialogueManager.DialogExists(dialog.Id), "The dialog was not correctly created.");
        }

        [Test]
        public void DialogHelperFunctionsTest()
        {
            // Set up a pages object that uses the DFunctions actions for advancing 
            var pages = new List<IDPage>()
            {
                new DPage()
                {
                    Text = "Page 1",
                    Buttons = new List<IDButton>()
                    {
                        new DButton()
                        {
                            Text = "Next",
                            OnButtonClick = DFunctions.GoToNextPage
                        }
                    }
                },
                new DPage()
                {
                    Text = "Page 2",
                    Buttons = new List<IDButton>()
                    {
                        new DButton()
                        {
                            Text = "Next",
                            OnButtonClick = DFunctions.CloseDialogue
                        }
                    }
                }
            };

            // Create the dialog
            var dialog = DialogueManager.CreateDialog(pages);

            // Verify that it starts on the first page
            Assert.AreEqual("Page 1", dialog.GetPage().Text, $"The actual page text did not match what was expected. Received {dialog.GetPage().Text}");
            Assert.IsTrue(dialog.IsVisible, "The dialog did not become visible when it was created.");

            // Proceed to the next page & verify that it works
            dialog.PressButton(0);

            Assert.AreEqual("Page 2", dialog.GetPage().Text, $"The actual page text did not match what was expected. Received {dialog.GetPage().Text}");
            Assert.IsTrue(dialog.IsVisible, "The dialog did not stay visible on proceeding to the second page.");

            // Press the close button & verify that the dialog is closed
            dialog.PressButton(0);
            Assert.IsFalse(dialog.IsVisible, "The dialog did not close.");
        }
    }
}
