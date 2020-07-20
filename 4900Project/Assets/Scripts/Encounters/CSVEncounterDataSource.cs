using Encounters;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SIEvents;
using Dialogue;
using System;

public class CSVEncounterDataSource : IEncounterDataSource
{
    private readonly static char CONDITION_CHAR = '@';
    private readonly static char EFFECT_CHAR = '!';
    private readonly static char COMMAND_SPLIT_CHAR = ';';

    // PUBLIC //

    public IEnumerator<Encounter> GetEncounterEnumerator()
    {
        // Load External file
        return enumerable.Select(raw => parseEncounter(raw));
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
    private Encounter parseEncounter(/**/)
    {
        //### These are the properties we need to load in from file ###// 
        var rawDialogue;
        var rawConditions;
        var rawEncounterTownId;
        //###                                                       ###//

        var dialogue = parseDialogue(rawDialogue);
        var conditions = rawConditions.Select(rc => parseEncounterCondition(rc));

        Encounter encounter = new Encounter()
        {
            Dialogue = dialogue,
            Conditions = conditions,
            FixedEncounterTownId = rawEncounterTownId
        };

    }

    /// <summary>
    /// Handles creation of dialogue by passing work off to relevent parsers
    /// 
    /// Relevant parsers:
    /// - parsePage
    /// </summary>
    /// <returns>Dialogue</returns>
    private IDialogue parseDialogue(/**/)
    {
        //#############################################################//
        //### These are the properties we need to load in from file ###// 
        var rawAvatarName; // Don't store in memory -- just look up the file when it's time to render it
        var rawPages;
        //###                                                       ###//
        //#############################################################//

        // Because we don't have all the IDPages by the time we go to create most all of the buttons,
        // we create them "disconnected", by keeping reference to the page id and buttons' next page ids
        // and then connecting them in the following step
        var disconnectedPages = new Dictionary<int, Tuple<IDPage, Dictionary<IDButton, int?>>>();
        rawPages.Select(rp => disconnectedPages.Add(parsePage(rp)));

        // This should be the first page (id 0)
        var rootPage = disconnectedPages[0];

        // Connect pages
        disconnectedPages.Select(kvp => kvp.Value.Item1.Buttons = kvp.Value.Item2.Select(kvp2 =>
        {
            var nextPage = kvp2.Value;
            if (nextPage.HasValue)
            {
                kvp2.Key.NextPage = disconnectedPages[nextPage.Value].Item1;
            }
        }));

        // Create page
        DialogueManager.Instance.CreateDialogue(rootPage);
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
    private KeyValuePair<int, Tuple<IDPage, Dictionary<IDButton, int?>>> parsePage(/**/)
    {
        //#############################################################//
        //### These are the properties we need to load in from file ###// 
        var rawId;
        var rawText;
        var rawButtons; // Nested object
        //###                                                       ###//
        //#############################################################//

        var page = new DPage()
        {
            Text = rawText
        };

        var id = Int16.Parse(rawId);

        // This is messy, but it just parses the buttons and returns the relevent information, pages not currently
        // linked together
        var buttons = rawButtons.Select(rb => parseButton(rb)).ToDictionary(b => b.Key, b => b.Value);
        return new KeyValuePair<int, Tuple<IDPage, Dictionary<IDButton, int?>>>(id, new Tuple<IDPage, Dictionary<IDButton, int?>>(page, buttons));
    }

    /// <summary>
    /// Handles creation of button by passing work off to relevent parsers
    /// 
    /// Relevant parsers:
    /// - parsePageOptionCondition
    /// - parsePageOptionEffect
    /// </summary>
    /// <returns>Button and the id for the next page</returns>
    private KeyValuePair<IDButton, int?> parseButton(/**/)
    {
        //#############################################################//
        //### These are the properties we need to load in from file ###// 
        var rawText;
        var rawStatements; // These are the sciptable commands (see Issue #135) that can be given as a single string of text
        var rawNextPageId;
        //###                                                       ###//
        //#############################################################//

        var condition_statements = new List<string[]>();
        var effects_statements = new List<string[]>();

        // Split script commands into individual statements and separate
        // conditions from effects
        var command_dict = rawStatements.Split(COMMAND_SPLIT_CHAR)
            .Select(c => c.Trim())
            .Select(sc => sc.Split(' '))
            .GroupBy(sc => sc.First().FirstOrDefault())
            .ToDictionary(sc => sc.Key, sc => sc.ToList())
            .ToList().ForEach(kvp =>
            {
                if (kvp.Key == CONDITION_CHAR)
                {
                    condition_statements.AddRange(kvp.Value);
                }
                else if (kvp.Key == EFFECT_CHAR)
                {
                    effects_statements.AddRange(kvp.Value);
                }
                else
                {
                    Debug.WriteLine(string.Format("Command identifier {0} not recognized", kvp.Key));
                }
            });

        var conditions = condition_statements.Select(args => parsePageOptionCondition(args));
        var effects = effects_statements.Select(args => parseEffect(args));

        var button = new DButton()
        {
            Text = rawText,
            Conditions = conditions,
            Effects = effects
        };

        var nextPageId = Int16.Parse(rawText);

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
