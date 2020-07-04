using FileConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Foo
{
    public int Id { get; set; }
    public string Name { get; set; }
}

public class LoadCsvTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Load in all the data
        GameData.LoadCsv<Foo>(Files.TestCsv, out IEnumerable<Foo> result);

        // Outputs all the data in the logs
        var resultString = new System.Text.StringBuilder();
        resultString.AppendLine("Loading in CSV data:");
        foreach (Foo data in result)
        {
            resultString.AppendLine("\tId: " + data.Id + " has Name " + data.Name);
        }
        UnityEngine.Debug.Log(resultString);
    }
}
