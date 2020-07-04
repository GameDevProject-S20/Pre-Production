using FileConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scenes.DataReadingScene
{
    /// <summary>
    /// A simple class. This will be constructed from the test JSON file.
    /// </summary>
    [Serializable]
    class FooJson
    {
        public bool IsATest;
        public string Name;
    }

    /// <summary>
    /// The test class for loading in JSON. Should output "IsATest: true with Name Google Drive"
    /// </summary>
    class LoadJsonTest : MonoBehaviour
    {
        void Start()
        {
            GameData.LoadJson<FooJson>(Files.TestJson, out FooJson result);
            Debug.Log($"Received a result of IsATest: {result.IsATest} with Name {result.Name}");
            Debug.Log($"JSON Test passed: {result.IsATest && result.Name == "Google Drive"}");
        }
    }
}
