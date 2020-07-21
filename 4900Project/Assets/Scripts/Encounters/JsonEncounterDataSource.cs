using Encounters;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SIEvents;
using Dialogue;
using System;
using FileConstants;

public class JsonEncounterDataSource : IEncounterDataSource
{
    [System.Serializable]
    public class RawJSON
    {
        public RawEncounter[] encounters;
    }

    [System.Serializable]
    public class RawEncounter
    {
        public int encounter_id;
        public string[] conditions;
        public string town_name;
        public RawPage[] dialogue_tree;
    }

    [System.Serializable]
    public class RawPage
    {
        public int id;
        public string text;
        public string avatar_name;
        public RawButton[] buttons;
    }

    [System.Serializable]
    public class RawButton
    {
        public string text;
        public string[] conditions;
        public string[] effects;
        public int next_page_id;
    }


    private readonly static char CONDITION_CHAR = '@';
    private readonly static char EFFECT_CHAR = '!';

    // PUBLIC //

    public IEnumerable<Encounter> GetEncounterEnumerator()
    {
        GameData.LoadJson<RawJSON>(Files.Encounter, out RawJSON result);
        return result.encounters.Select(raw => parseEncounter(raw));
    }


    // PRIVATE //

    /// <summary>
    /// Handles creation of encounter by passing work off to relevent parsers
    /// 
    /// Relevant parsers:
    /// - parseDialogue
    /// - parseEncounterCondition
    /// </summary>
    /// <returns>Encounter</returns>
    private Encounter parseEncounter(RawEncounter rawEncounter)
    {
        //### These are the properties we need to load in from file ###// 
        var rawDialogue = rawEncounter.dialogue_tree;
        var rawConditions = rawEncounter.conditions;
        var rawEncounterTownId = rawEncounter.town_name;
        //###                                                       ###//

        var dialogue = parseDialogue(rawDialogue);
        var conditions = rawConditions.Select(rc => parseEncounterCondition(rc)).ToList();
        var townId = TownManager.Instance.GetTownByName(rawEncounterTownId).Id;

        Encounter encounter = new Encounter()
        {
            Dialogue = dialogue,
            Conditions = conditions,
            FixedEncounterTownId = townId
        };

        return encounter;
    }

    /// <summary>
    /// Handles creation of dialogue by passing work off to relevent parsers
    /// 
    /// Relevant parsers:
    /// - parsePage
    /// </summary>
    /// <returns>Dialogue</returns>
    private IDialogue parseDialogue(RawPage[] rawPages)
    {

        // Because we don't have all the IDPages by the time we go to create most all of the buttons,
        // we create them "disconnected", by keeping reference to the page id and buttons' next page ids
        // and then connecting them in the following step
        var disconnectedPages =
            rawPages
                .Select(rp => parsePage(rp))
                .ToDictionary(p => p.Item1, p => p.Item2);
       

        // This should be the first page (id 0)
        var rootPage = disconnectedPages[0].Item1;
        var avatar = rootPage.Avatar;

        // Connect pages
        disconnectedPages.Select(kvp =>
        {
            IDPage page = kvp.Value.Item1;
            IEnumerable<KeyValuePair<IDButton, int?>> buttons = kvp.Value.Item2.AsEnumerable();

            // If no avatar specified, default to using the root page's avatar
            if (page.Avatar == null)
            {
                page.Avatar = avatar;
            }

            page.Buttons = buttons.Select(kvp2 =>
            {
                IDButton button = kvp2.Key;
                int? nextPage = kvp2.Value;
                if (nextPage.HasValue)
                {
                    kvp2.Key.NextPage = disconnectedPages[nextPage.Value].Item1;
                }
                return button;
            });

            return page;
        });

        // Create page
        return DialogueManager.Instance.CreateDialogue(rootPage);
    }

    /// <summary>
    /// Handles creation of conditions that must be satisfied before the encounter can run
    /// </summary>
    /// <returns>Condition</returns>
    private Condition parseEncounterCondition(params string[] statement)
    {
        Condition c = null;
        var command = statement[0].TrimStart(CONDITION_CHAR).ToUpper();
        var args = statement.Skip(1).ToArray();

        if (command == "encounter_complete")
        {
            var enc_id = Int16.Parse(args[0]);
            c = new EncounterCompleteCondition("", enc_id);
        }
        else if (command == "quest_complete")
        {
            var quest_id = Int16.Parse(args[0]);
            c = new QuestCompleteCondition("", quest_id);
        }
        return c;
    }

    /// <summary>
    /// Handles creation of page by passing work off to relevent parsers
    /// 
    /// Relevant parsers:
    /// - parseButton
    /// </summary>
    /// <returns>A dictionary entry containing pages mapped to buttons and the button nextpage ids</returns>
    private Tuple<int, Tuple<IDPage, Dictionary<IDButton, int?>>> parsePage(RawPage rawPage)
    {
        //#############################################################//
        //### These are the properties we need to load in from file ###// 
        var id = rawPage.id;
        var rawText = rawPage.text;
        var rawButtons = rawPage.buttons; // Nested object
        //###                                                       ###//
        //#############################################################//

        var page = new DPage()
        {
            Text = rawText
        };

        // This is messy, but it just parses the buttons and returns the relevent information, pages not currently
        // linked together
        Dictionary<IDButton, int?> buttons;
        if (rawButtons.Length > 0)
        {
            buttons = rawButtons.Select(rb => parseButton(rb)).ToDictionary(b => b.Key, b => b.Value);
        }
        else
        {
            buttons = new Dictionary<IDButton, int?>();
            buttons.Add(DButton.Exit, null);
        }
        
        return new Tuple<int, Tuple<IDPage, Dictionary<IDButton, int?>>>(id, new Tuple<IDPage, Dictionary<IDButton, int?>>(page, buttons));
    }

    /// <summary>
    /// Handles creation of button by passing work off to relevent parsers
    /// 
    /// Relevant parsers:
    /// - parsePageOptionCondition
    /// - parsePageOptionEffect
    /// </summary>
    /// <returns>Button and the id for the next page</returns>
    private KeyValuePair<IDButton, int?> parseButton(RawButton rawButton)
    {
        //#############################################################//
        //### These are the properties we need to load in from file ###// 
        var rawText = rawButton.text;
        var rawConditions = rawButton.conditions;
        var rawEffects = rawButton.effects;
        var nextPageId = rawButton.next_page_id;
        //###                                                       ###//
        //#############################################################//

        var conditions =
            rawConditions
                .Select(e => e.Trim())
                .Select(e => e.Split(' '))
                .Select(e => parsePageOptionCondition(e));

        var effects =
            rawEffects
                .Select(e => e.Trim())
                .Select(e => e.Split(' '))
                .Select(e => parseEffect(e));

        var button = new DButton()
        {
            Text = rawText,
            Conditions = conditions,
            Effects = effects
        };

        return new KeyValuePair<IDButton, int?>(button, nextPageId);
    }

    /// <summary>
    /// Handles creation of conditions that must be satisfied in order to press the button
    /// 
    /// </summary>
    /// <returns>Condition</returns>
    private IPresentCondition parsePageOptionCondition(params string[] statement)
    {
        IPresentCondition c = null;
        var command = statement[0].TrimStart(CONDITION_CHAR).ToUpper();
        var args = statement.Skip(1).ToArray();

        if (command == "has")
        {
            var iname = args[0];
            var iamount = Int16.Parse(args[1]);
            c = new HasItemPresentConditon(iname, iamount);
        }

        return c;
    }

    /// <summary>
    /// Handles creation of effects that are invoked after pressing the button
    /// 
    /// </summary>
    /// <returns>Effect</returns>
    private IEffect parseEffect(params string[] statement)
    {
        IEffect e = null;
        var command = statement[0].TrimStart(EFFECT_CHAR).ToUpper();
        var args = statement.Skip(1).ToArray();

        if (command == "GIVE")
        {
            var iname = args[0];
            var iamount = Int16.Parse(args[1]);
            e = new GiveItem(iname, iamount);
        }
        else if (command == "TAKE")
        {
            var iname = args[0];
            var iamount = Int16.Parse(args[1]);
            e = new TakeItem(iname, iamount);
        }
        // Resolve?
        return e;
    }
}
