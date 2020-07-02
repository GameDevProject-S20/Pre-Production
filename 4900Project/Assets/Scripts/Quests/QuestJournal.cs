using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestJournal
{
    public static QuestJournal Instance
    {
        get
        {
            if (instance == null) instance = new QuestJournal();
            return instance;
        }
    }

    private static QuestJournal instance = null;

    private QuestJournal()
    { }
}