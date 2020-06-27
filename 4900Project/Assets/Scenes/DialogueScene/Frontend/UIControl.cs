using Dialogue;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIControl : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject textDisplay;
    public GameObject buttonsPanel;
    public GameObject buttonTemplate;
    
    void Start()
    {
        DialogueManager.ActiveDialogChanged.AddListener(() =>
        {
            UpdateDialogDisplay();
        });

        UpdateDialogDisplay();

        // TODO: Remove when actual Dialogs are in the game
        DialogueManager.CreateDialog(new List<IDPage>()
        {
            new DPage()
            {
                Text = "Hello World",
                Buttons = new List<IDButton>()
                {
                    new DButton()
                    {
                        Text = "next",
                        OnButtonClick = DFunctions.GoToNextPage
                    },
                    new DButton()
                    {
                        Text = "this is also a next page button",
                        OnButtonClick = DFunctions.GoToNextPage
                    },
                    new DButton()
                    {
                        Text = "but this one is a trap",
                        OnButtonClick = () =>
                        {
                            Debug.Log("The trap got you. Goodbye");
                            DFunctions.CloseDialogue();
                        }
                    }
                }
            },
            new DPage()
            {
                Text = "Goobye World",
                Buttons = new List<IDButton>()
                {
                    new DButton()
                    {
                        Text = "goodbye",
                        OnButtonClick = DFunctions.CloseDialogue
                    }
                }
            }
        });
    }
     
    protected void UpdateDialogDisplay()
    {
        var dialog = DialogueManager.GetActiveDialog();

        // If we don't have an active dialog, hide the UI
        if (dialog == null)
        {
            gameObject.GetComponent<Canvas>().enabled = false;
            return;
        }

        // Otherwise, update the page text & the buttons
        var page = dialog.GetPage();
        textDisplay.GetComponent<Text>().text = page.Text;
        AddButtons(page.Buttons);

        gameObject.GetComponent<Canvas>().enabled = true;
    }

    protected void AddButtons(IEnumerable<IDButton> buttons)
    {
        ClearButtons();
        foreach (var button in buttons)
        {
            AddButton(button);
        }
    }

    protected void AddButton(IDButton button)
    {
        var newButton = GameObject.Instantiate(buttonTemplate);
        newButton.GetComponentInChildren<Text>().text = button.Text;
        newButton.GetComponent<Button>().onClick.AddListener(()=>
        {
            button.OnButtonClick();
        });
        newButton.transform.SetParent(buttonsPanel.transform);
    }

    protected void ClearButtons()
    {
        foreach (var button in buttonsPanel.transform.GetComponentsInChildren<Button>())
        {
            Destroy(button.gameObject);
        }
    }
}
