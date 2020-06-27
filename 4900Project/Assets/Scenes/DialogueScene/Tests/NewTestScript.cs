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
        /// <summary>
        /// Tests that new Dialog objects can be created.
        /// </summary>
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

            // Cleanup - hide the dialog
            dialog.Hide();
        }

        /// <summary>
        /// Tests that the DFunction methods correctly manipulate the dialog.
        /// </summary>
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

        /// <summary>
        /// Tests that activating and de-activating dialogs correctly updates the Active Dialog.
        /// </summary>
        [Test]
        public void MultipleActiveDialogsTest()
        {
            var pageObject = new List<IDPage>(){
                new DPage()
                {
                    Text = "Page",
                    Buttons = new List<IDButton>()
                    {
                        new DButton()
                        {
                            Text = "Hello World",
                            OnButtonClick = DFunctions.CloseDialogue
                        }
                    }
                }
            };

            // Create two dialogs
            var dialogOne = DialogueManager.CreateDialog(pageObject);
            var dialogTwo = DialogueManager.CreateDialog(pageObject);

            // Verify that the second dialog is active
            var activeDialog = DialogueManager.GetActiveDialog();
            Assert.AreEqual(dialogTwo, activeDialog, $"Active dialog check failed: Expected Dialog {dialogTwo.Id}, but received Dialog {activeDialog.Id}");

            // Close the dialog
            dialogTwo.PressButton(0);

            // The active dialog should now be dialog one
            activeDialog = DialogueManager.GetActiveDialog();
            Assert.AreEqual(dialogOne, activeDialog, $"Active dialog check failed: Expected Dialog {dialogOne.Id}, but received Dialog {activeDialog.Id}");

            // Now close the first dialog and verify that there is no active dialog
            dialogOne.PressButton(0);

            activeDialog = DialogueManager.GetActiveDialog();
            Assert.IsNull(activeDialog, $"Active dialog check failed: Expected null, but received Dialog {(activeDialog != null ? activeDialog.Id : -1)}");

            // Re-open the second dialog and verify that it is again the active dialog
            dialogTwo.Show();

            activeDialog = DialogueManager.GetActiveDialog();
            Assert.AreEqual(dialogTwo, activeDialog, $"Active dialog check failed: Expected Dialog {dialogTwo.Id}, but received Dialog {activeDialog.Id}");

            // Hide the second dialog and verify that we again have no active dialog
            dialogTwo.Hide();

            activeDialog = DialogueManager.GetActiveDialog();
            Assert.IsNull(activeDialog, $"Active dialog check failed: Expected null, but received Dialog {(activeDialog != null ? activeDialog.Id : -1)}");

        }
    }
}
