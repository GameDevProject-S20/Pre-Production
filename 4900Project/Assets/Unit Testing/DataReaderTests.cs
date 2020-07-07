using FileConstants;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class DataReaderTests : GameData
    {
        // Test Classes - These are used to store the data
        [Serializable]
        protected class TestClass
        {
            public int Id;
            public string Name;
        }

        [Serializable]
        protected class BasicJsonClass
        {
            public bool CanLoadJson;
            public string Name;
        }

        [Serializable]
        protected class JsonStringsList
        {
            public List<string> List;
        }
        
        [Serializable]
        protected class JsonClassesList
        {
            public List<TestClass> List;
        }

        /// <summary>
        /// Runs a CSV test. Loads in the TestCsv file, and verifies that all outputs match what we expect.
        /// </summary>
        [Test]
        public void TestCsvReading()
        {

            GameData.LoadCsv<TestClass>(Files.TestCsv, out IEnumerable<TestClass> results);
            VerifyTestClassList(results);
        }

        /// <summary>
        /// Runs a JSON test. Loads in the TestJson file, and verifies that output values match what we expect
        /// </summary>
        [Test]
        public void BasicJsonReading()
        {
            GameData.LoadJson<BasicJsonClass>(Files.TestBasicJson, out BasicJsonClass result);

            // Verify that the data loaded correctly
            Assert.AreEqual(true, result.CanLoadJson, "The JSON data could not be read.");
            Assert.AreEqual("Test Json", result.Name, $"The name field {result.Name} did not match what was expected (Test Json).");
        }

        /// <summary>
        /// Verifies that a simple string list can be loaded in from JSON.
        /// </summary>
        [Test]
        public void JsonListOfStrings()
        {
            GameData.LoadJson<JsonStringsList>(Files.TestJsonStringsList, out JsonStringsList result);

            // Verify that the list loaded with two entities
            Assert.AreEqual(2, result.List.Count, "The list did not load with expected number of entities (Expected 2).");

            // Verify that the values match
            Assert.AreEqual("Value 1", result.List.ElementAt(0), "The first element did not match what was expected.");
            Assert.AreEqual("Value 2", result.List.ElementAt(1), "The second element did not match what was expected.");
        }

        /// <summary>
        /// Verifies that we can load in a list of classes.
        /// </summary>
        [Test]
        public void JsonListOfClasses()
        {
            GameData.LoadJson<JsonClassesList>(Files.TestJsonListOfClass, out JsonClassesList result);
            VerifyTestClassList(result.List);
        }

        /// <summary>
        /// Creates local backups of all remote files.
        /// Ideally, this shouldn't be a test -- but running it as a test allows us to automate it at the click of a button;
        /// so leaving it like this until we get a better solution
        /// </summary>
        [Test]
        public void CreateBackups()
        {
            GameData.CreateBackups();
        }
        
        /// <summary>
        /// Verifies that the GameData reader can hook up to Google Drive for downloading JSON.
        /// </summary>
        [Test]
        public void CanReadJsonFromGoogleDrive()
        {
            var canRead = GameData.DownloadFileFromGoogleDrive(Files.TestBasicJson.GoogleDriveFileId, out _);
            Assert.IsTrue(canRead, "Failed to read the JSON data.");
        }

        /// <summary>
        /// Verifies that the GameData reader can hook up to Google Drive for downloading CSV.
        /// </summary>
        [Test]
        public void CanReadCsvFromGoogleDrive()
        {
            var canRead = GameData.DownloadFileFromGoogleDrive(Files.TestCsv.GoogleDriveFileId, out _);
            Assert.IsTrue(canRead, "Failed to load CSV data from Google Drive.");
        }

        /// <summary>
        /// Verifies that the test class was loaded in with two values,
        /// with the values matching what's expected (first - Id 1, Name Google; second - Id 2, Name Drive)
        /// </summary>
        /// <param name="testClassList"></param>
        protected void VerifyTestClassList(IEnumerable<TestClass> testClassList)
        {
            // Should have two results
            Assert.AreEqual(2, testClassList.Count(), $"The results count {testClassList.Count()} did not match the expected count of 2.");

            // Verify the names & IDs match with what's expected
            VerifyCsvContents(testClassList.ElementAt(0), 1, "Google");
            VerifyCsvContents(testClassList.ElementAt(1), 2, "Drive");
        }

        /// <summary>
        /// Runs through a single CSV Test Class to verify its values against what we expect
        /// </summary>
        /// <param name="csvData"></param>
        /// <param name="expectedId"></param>
        /// <param name="expectedName"></param>
        protected void VerifyCsvContents(TestClass csvData, int expectedId, string expectedName)
        {
            // Assert that the ID matches
            Assert.AreEqual(expectedId, csvData.Id, $"The data's ID {csvData.Id} did not match our expected ID of {expectedId}.");

            // Assert that the name matches
            Assert.AreEqual(expectedName, csvData.Name, $"The data's Name {csvData.Name} did not match our expected name of {expectedName}.");
        }
    }
}
