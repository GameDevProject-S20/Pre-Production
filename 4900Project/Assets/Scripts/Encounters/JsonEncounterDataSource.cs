using Encounters;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SIEvents;
using Dialogue;
using System;
using FileConstants;
using UnityEngine;
using Extentions;
using TMPro;

public class JsonEncounterDataSource : IEncounterDataSource
{
    [System.Serializable]
    public class RawJSON
    {
        public RawFixedEncounter[] fixed_encounters;
        public RawRandomEncounter[] random_encounters;
    }

    [System.Serializable]
    public class RawFixedEncounter
    {
        public int encounter_id;
        public bool valid;
        public string[] conditions;
        public string town_name;
        public RawPage[] dialogue_tree;
    }

    [System.Serializable]
    public class RawRandomEncounter
    {
        public int encounter_id;
        public bool valid;
        public string[] tags;
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


    private readonly static char CONDITION_CHAR = '!';
    private readonly static char EFFECT_CHAR = '@';

    // PUBLIC //

    public IEnumerable<Encounter> GetEncounterEnumerator()
    {
        GameData.LoadJson<RawJSON>(Files.Encounter, out RawJSON result);
        return parseFixedEncounters(result.fixed_encounters).Union(parseRandomEncounters(result.random_encounters));
    }

    // PRIVATE //

    /// <summary>
    /// Helper function for handling iterable raw fixed encounters
    /// </summary>
    /// <param name="encounters"></param>
    /// <returns></returns>
    private IEnumerable<Encounter> parseFixedEncounters(RawFixedEncounter[] encounters)
    {
        return encounters.Select(raw => parseFixedEncounter(raw)).Where(e => e != null);
    }

    /// <summary>
    /// Helper function for handling iterable raw random encounters
    /// </summary>
    /// <param name="encounters"></param>
    /// <returns></returns>
    private IEnumerable<Encounter> parseRandomEncounters(RawRandomEncounter[] encounters)
    {
        return encounters.Select(raw => parseRandomEncounter(raw)).Where(e => e != null);
    }

    /// <summary>
    /// Handles creation of fixed encounters by passing work off to relevent parsers
    ///
    /// Relevant parsers:
    /// - parseDialogue
    /// - parseEncounterCondition
    /// </summary>
    /// <returns>Encounter</returns>
    private Encounter parseFixedEncounter(RawFixedEncounter rawEncounter)
    {
        //### These are the properties we need to load in from file ###//
        var id = rawEncounter.encounter_id;
        var valid = rawEncounter.valid;
        var rawDialogue = rawEncounter.dialogue_tree;
        var rawConditions = rawEncounter.conditions;
        var rawEncounterTownId = rawEncounter.town_name;
        //###                                                       ###//

        if (!valid) return null;

        var dialogue = parseDialogue(rawDialogue, id);
        var conditions = parseEncounterConditions(rawConditions);
        var townId = -1;
        if (rawEncounterTownId != null){
            if (rawEncounterTownId.Count() > 0){
                townId = TownManager.Instance.GetTownByName(rawEncounterTownId).Id;
            }
        }

        Encounter encounter = new FixedEncounter()
        {
            Id = id,
            Dialogue = dialogue,
            Conditions = conditions,
            FixedEncounterTownId = townId
        };

        return encounter;
    }

    /// <summary>
    /// Handles creation of random encounters by passing work off to relevent parsers
    ///
    /// Relevant parsers:
    /// - parseDialogue
    /// - parseEncounterCondition
    /// </summary>
    /// <returns>Encounter</returns>
    private Encounter parseRandomEncounter(RawRandomEncounter rawEncounter)
    {
        //### These are the properties we need to load in from file ###//
        var id = rawEncounter.encounter_id;
        var valid = rawEncounter.valid;
        var tags = rawEncounter.tags;
        var rawDialogue = rawEncounter.dialogue_tree;
        //###                                                       ###//

        if (!valid) return null;
        if (id == 0) throw new ArgumentException("Encounter id must not be 0!");

        var dialogue = parseDialogue(rawDialogue, id);

        Encounter encounter = new RandomEncounter()
        {
            Id = id,
            Tags = tags,
            Dialogue = dialogue
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
    private IDialogue parseDialogue(RawPage[] rawPages, int encounterId)
    {

        // Because we don't have all the IDPages by the time we go to create most all of the buttons,
        // we create them "disconnected", by keeping reference to the page id and buttons' next page ids
        // and then connecting them in the following step
        var disconnectedPages =
            rawPages
                .Select(rp => parsePage(rp, encounterId))
                .ToDictionary(p => p.Item1, p => p.Item2);


        // This should be the first page (id 0)
        var rootPage = disconnectedPages[1].Item1;
        var avatar = rootPage.Avatar;

        // Connect pages
        foreach (var kvp in disconnectedPages)
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
                    button.NextPage = disconnectedPages[nextPage.Value].Item1;
                }
                return button;
            });

            if (page.Buttons.Any(b => b.Text == "Done."))
            {
                UnityEngine.Debug.Log(string.Format("Exit button exists on page {0}\n{1}", kvp.Key, page.Text));
            }
        }

        // Create page
        return DialogueManager.Instance.CreateDialogue(rootPage);
    }

    /// <summary>
    /// Splits effects and conditions into the relevant information.
    /// </summary>
    /// <param name="statement">The full, unparsed string</param>
    /// <param name="identifier">The non-alphanumeric symbol representing the type of command </param>
    /// <param name="command">The script command</param>
    /// <param name="args">The script arguments</param>
    private void SplitCommand(string statement, out char identifier, out string command, out string[] args)
    {
        if (statement.Length == 0)
        {
            throw new ArgumentException("Command Empty");
        }

        statement = statement.Trim();
        identifier = statement.FirstOrDefault();

        var statement_arr = string.Concat(statement.Skip(1)).Split();
        command = statement_arr.FirstOrDefault().ToLower();
        args = statement_arr.Skip(1).Select(a => a.Replace('_', ' ')).ToArray();
    }

    /// <summary>
    /// Helper function for parsing a series of Encounter Conditions
    /// </summary>
    /// <returns>Condition</returns>
    private List<Condition> parseEncounterConditions(string[] rawConditions)
    {
        if (rawConditions == null)
        {
            return new List<Condition>();
        }
        else
        {
            return rawConditions.Select(rc => parseEncounterCondition(rc)).ToList();
        }
    }

    /// <summary>
    /// Handles creation of conditions that must be satisfied before the encounter can run
    /// </summary>
    /// <returns>Condition</returns>
    private Condition parseEncounterCondition(string statement)
    {
        Condition c = null;

        SplitCommand(statement, out char identifier, out string command, out string[] args);

        if (identifier != CONDITION_CHAR)
        {
            throw new ArgumentException(string.Format("Incorrect Identifier for condition {0}. Got '{2}'.\n{3}", statement, CONDITION_CHAR, identifier, statement));
        }

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
        else if (command == "stage_complete")
        {
            var quest_id = Int16.Parse(args[0]);
            var stage_num = Int16.Parse(args[1]);
            c = new StageCompleteCondition("", quest_id, stage_num);
        }
        else
        {
            throw new ArgumentException(string.Format("Command {0} not recognized.", command));
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
    private Tuple<int, Tuple<IDPage, Dictionary<IDButton, int?>>> parsePage(RawPage rawPage, int encounterId)
    {
        //#############################################################//
        //### These are the properties we need to load in from file ###//
        var id = rawPage.id;
        var rawText = rawPage.text;
        var rawButtons = rawPage.buttons; // Nested object
        //###                                                       ###//
        //#############################################################//

        if (id == 0)
        {
            throw new ArgumentException("Page cannot have ID 0!");
        }

        var page = new DPage()
        {
            Text = rawText
        };

        // This is messy, but it just parses the buttons and returns the relevent information, pages not currently
        // linked together
        Dictionary<IDButton, int?> buttons;
        if (rawButtons == null)
        {
            buttons = new Dictionary<IDButton, int?>();
            buttons.Add(DButton.Exit, null);
        }
        else
        {
            buttons = rawButtons.Select(rb => parseButton(rb, encounterId)).ToDictionary(b => b.Key, b => b.Value);
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
    private KeyValuePair<IDButton, int?> parseButton(RawButton rawButton, int encounterId)
    {
        //#############################################################//
        //### These are the properties we need to load in from file ###//
        var rawText = rawButton.text;
        var rawConditions = rawButton.conditions;
        var rawEffects = rawButton.effects;
        var rawNextPageId = rawButton.next_page_id;
        //###                                                       ###//
        //#############################################################//

        var conditions = parsePageOptionConditions(rawConditions);

        var effects = parseEffects(rawEffects, encounterId);

        var text = (rawText != null) ? rawText : DButton.DefaultText;

        var button = new DButton()
        {
            Text = text,
            Conditions = conditions,
            Effects = effects
        };

        // Ignore if next id not set
        int? nextPageId;
        if (rawNextPageId != 0)
        {
            nextPageId = rawNextPageId;
        }
        else
        {
            nextPageId = null;
        }

        return new KeyValuePair<IDButton, int?>(button, nextPageId);
    }

    /// <summary>
    /// Helper function for parsing a series of Page Option (Button) Condiitons
    /// </summary>
    /// <returns>Condition</returns>
    private IEnumerable<IPresentCondition> parsePageOptionConditions(string[] rawConditions)
    {
        if (rawConditions == null)
        {
            return Enumerable.Empty<IPresentCondition>();
        }
        else
        {
            return rawConditions.Select(e => parsePageOptionCondition(e));
        }
    }

    /// <summary>
    /// Handles creation of conditions that must be satisfied in order to press the button
    ///
    /// </summary>
    /// <returns>Condition</returns>
    private IPresentCondition parsePageOptionCondition(string statement)
    {
        IPresentCondition c = null;

        SplitCommand(statement, out char identifier, out string command, out string[] args);

        if (identifier != CONDITION_CHAR)
        {
            throw new ArgumentException(string.Format("Incorrect identifier for condition {0}. Got '{2}'.\n{3}", statement, CONDITION_CHAR, identifier, statement));
        }

        if (command == "has")
        {
            var iname = args[0];
            var iamount = Int16.Parse(args[1]);
            c = new HasItemPresentConditon(iname, iamount);
        }

        return c;
    }

    /// <summary>
    /// Helper function for parsing a series of Page Option (Button) effects
    /// </summary>
    /// <returns>Condition</returns>
    private IEnumerable<IEffect> parseEffects(string[] rawEffects, int encounterId)
    {
        if (rawEffects == null)
        {
            return Enumerable.Empty<IEffect>();
        }
        else
        {
            return rawEffects.Select(e => parseEffect(e, encounterId));
        }
    }

    /// <summary>
    /// Handles creation of effects that are invoked after pressing the button
    ///
    /// </summary>
    /// <returns>Effect</returns>
    private IEffect parseEffect(string statement, int encounterId)
    {
        IEffect e = null;
        string command = null;
        string[] args = null;

        try
        {
            SplitCommand(statement, out char identifier, out command, out args);

            if (identifier != EFFECT_CHAR)
            {
                throw new ArgumentException(string.Format("Incorrect identifier for effect {0}. Expected '{1}'. Got '{2}'.\n{3}", statement, EFFECT_CHAR, identifier, statement));
            }
            if (command == "give")
            {
                var iname = args[0];
                var iamount = Int16.Parse(args[1]);
                e = new GiveItem(iname, iamount);
            }
            else if (command == "give_tag")
            {
                var itag = args[0];
                var iamount = Int16.Parse(args[1]);
                e = new GiveItemWithTag(itag, iamount);
            }
            else if (command == "take")
            {
                var iname = args[0];
                var iamount = Int16.Parse(args[1]);
                e = new TakeItem(iname, iamount);
            }
            else if (command == "take_tag")
            {
                var itag = args[0];
                var iamount = Int16.Parse(args[1]);
                e = new TakeItemWithTag(itag, iamount);
            }
            else if (command == "resolve")
            {
                e = new ResolveEncounterEffect(encounterId);
            }
            else if (command == "health")
            {
                var mod = Int16.Parse(args[0]);
                e = new HealthEffect(mod);
            }
            else if (command == "health%")
            {
                var perc = Int16.Parse(args[0]);
                e = new HealthPercentEffect(perc);
            }
            else if (command == "health_max")
            {
                var mod = Int16.Parse(args[0]);
                e = new MaxHealthEffect(mod);
            }
            else if (command == "health_max%")
            {
                var perc = Int16.Parse(args[0]);
                e = new MaxHealthPercentEffect(perc);
            }
            else if (command == "start_quest")
            {
                var name = args[0];
                e = new StartQuestEffect(name);
            }
            else if (command == "set_node_encounter")
            {
                var nodeId = Int16.Parse(args[0]);
                var encId = Int16.Parse(args[1]);
                e = new SetNodeEncounterEffect(nodeId, encId);
            }
            else if (command == "set_dialogue_encounter")
            {
                var townId = Int16.Parse(args[0]);
                var encId = Int16.Parse(args[1]);
                e = new SetDialogueEncounterEffect(townId, encId);
            }
            else if (command == "add_edge")
            {
                var node1 = Int16.Parse(args[0]);
                var node2 = Int16.Parse(args[1]);
                e = new AddEdgeEffect(node1, node2);
            }
            else if (command == "add_shop")
            {
                var townId = Int16.Parse(args[0]);
                var shopName = args[1];
                var shopDescription = args[2];
                var shopType = args[3];
                e = new AddShopEffect(townId, shopName, shopDescription, shopType);
            }
            else if (command == "trigger")
            {
                var name = args[0];
                e = new TriggerEventEffect(name);
            }
            else if (command == "resident")
            {
                var addOrRemove = args[0];
                var residentName = args[1];
                var townName = args[2];

                e = new ResidentEffect(addOrRemove, residentName, townName);
            }
            else if (command == "random")
            {
                // Parsing works differently, so args must be overhauled
                GetRandomCommandArgs(statement, out double percentFirst, out string es1, out string es2);

                UnityEngine.Debug.Log(string.Format("Parsing Random\n{0}\n{1}", es1, es2));

                // Parse (will recurse if multiple randoms used)
                var effect1 = parseEffect(es1, encounterId);
                var effect2 = parseEffect(es2, encounterId);

                e = new RandomEffect(effect1, effect2, percentFirst);
                UnityEngine.Debug.Log(e);
            }
            else if (command == "toggle_random_encounters")
            {
                var on = (Int16.Parse(args[0]) == 0) ? false : true;
                e = new ToggleRandomEncountersEffect(on);
            }
        }
        catch (Exception exception)
        {
            throw new Exception(string.Format("{0} thrown by effect parser.\nEffect: {1}\nCommand: {2}\nArgs: {3}\n\nDetails:\n{4}", exception.GetType(), statement, command, string.Join(", ", args.Select(a => string.Format("'{0}'", a))), exception.Message));
        }

        return e;
    }

    /// <summary>
    /// Exclusively used by parseEffect.
    ///
    /// Random's formatting works differently, so this parses that.
    /// </summary>
    /// <param name="statement">The full effect statement</param>
    /// <param name="percentFirst">The chance argument</param>
    /// <param name="effectA">The first of the possible effects</param>
    /// <param name="effectB">The second of the possible effects</param>
    private void GetRandomCommandArgs(string statement, out double percentFirst, out string effectA, out string effectB)
    {
        if (statement.Length == 0)
        {
            throw new ArgumentException("Command Empty");
        }

        statement = statement.Trim();

        var statement_arr = string.Concat(statement.Skip(1)).Split();
        percentFirst = double.Parse(statement_arr[1]);

        ParseParenthesis(statement, out int startIndex, out int endIndex);
        effectA = statement.Substring(startIndex, endIndex - startIndex + 1);

        var remainingStatement = statement.Substring(endIndex + 2);
        ParseParenthesis(remainingStatement, out startIndex, out endIndex);
        effectB = remainingStatement.Substring(startIndex, endIndex - startIndex + 1);
    }

    /// <summary>
    /// Exlusively used by GetRandomCommandArgs
    ///
    /// Used to extract effect between parentheses.
    /// </summary>
    /// <param name="statement">The statement to parse</param>
    /// <param name="startIndex">The starting index of the effect (immediately following the first open parenthesis)</param>
    /// <param name="endIndex">The end index of the effect (immediately before the first *closing* parenthesis)</param>
    private void ParseParenthesis(string statement, out int startIndex, out int endIndex)
    {
        startIndex = -1;
        endIndex = -1;

        int queue = 0;
        for (int i = 0; i < statement.Length; i++)
        {
            if (statement[i] == '(')
            {
                queue++;
                if (startIndex == -1)
                {
                    startIndex = i+1;
                }
            }
            else if (statement[i] == ')')
            {
                queue--;
            }

            if (queue < 0)
            {
                throw new ArgumentException(string.Format("Error with parentheses. Thrown at index {0}\n{1}", i, statement));
            }

            if (startIndex != -1 && queue == 0)
            {
                endIndex = i-1;
                return;
            }
        }
    }
}
