using Dialogue;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Dialogue.Frontend
{
    /// <summary>
    /// The SimpleDialogueSetup MonoBehaviour can be applied to an object to use it as a test object for the Dialogue.
    /// It will create a new Dialogue on setup.
    /// </summary>
    class SimpleDialogueSetup : MonoBehaviour
    {
        void Start()
        {
            DialogueManager.Instance.CreateDialogue(new List<IDPage>()
            {
                new DPage()
                {
                    Text = @"This is a lot of text that will be coming through.",
                    Buttons = new List<IDButton>()
                    {
                        new DButton()
                        {
                            Text = "Hello",
                            OnButtonClick = DFunctions.GoToNextPage
                        }
                    }
                },
                new DPage()
                {
                    Text = @"It goes through one-by-one, line-by-line.",
                    Buttons = new List<IDButton>()
                    {
                        new DButton()
                        {
                            Text = "Hello",
                            OnButtonClick = DFunctions.GoToNextPage
                        }
                    }
                },
                new DPage()
                {
                    Text = @"It does build out quickly.",
                    Buttons = new List<IDButton>()
                    {
                        new DButton()
                        {
                            Text = "Hello",
                            OnButtonClick = DFunctions.GoToNextPage
                        }
                    }
                },
                new DPage()
                {
                    Text = @"But in the end, it's the text that gets too long.",
                    Buttons = new List<IDButton>()
                    {
                        new DButton()
                        {
                            Text = "Hello",
                            OnButtonClick = DFunctions.GoToNextPage
                        }
                    }
                },
                new DPage()
                {
                    Text = @"But that's what we're really searching for in the end, isn't it?",
                    Buttons = new List<IDButton>()
                    {
                        new DButton()
                        {
                            Text = "Hello",
                            OnButtonClick = DFunctions.GoToNextPage
                        }
                    }
                },
                new DPage()
                {
                    Text = @". . .",
                    Buttons = new List<IDButton>()
                    {
                        new DButton()
                        {
                            Text = "Hello",
                            OnButtonClick = DFunctions.GoToNextPage
                        }
                    }
                },
                new DPage()
                {
                    Text = @". . .",
                    Buttons = new List<IDButton>()
                    {
                        new DButton()
                        {
                            Text = "Hello",
                            OnButtonClick = DFunctions.GoToNextPage
                        }
                    }
                },
                new DPage()
                {
                    Text = @". . .",
                    Buttons = new List<IDButton>()
                    {
                        new DButton()
                        {
                            Text = "Hello",
                            OnButtonClick = DFunctions.GoToNextPage
                        }
                    }
                },
                new DPage()
                {
                    Text = @". . .",
                    Buttons = new List<IDButton>()
                    {
                        new DButton()
                        {
                            Text = "Hello",
                            OnButtonClick = DFunctions.CloseDialogue
                        }
                    }
                },
            });
        }
    }
}
