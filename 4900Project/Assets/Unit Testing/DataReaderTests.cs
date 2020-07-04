using FileConstants;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    public class DataReaderTests
    {
        // Test Classes - These are used to store the data
        protected class CsvTestClass
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        [Serializable]
        protected class JsonTestClass
        {
            public bool CanLoadJson;
            public string Name;
        }

        /// <summary>
        /// Runs a CSV test. Loads in the TestCsv file, and verifies that all outputs match what we expect.
        /// </summary>
        [Test]
        public void TestCsvReading()
        {

            GameData.LoadCsv<CsvTestClass>(Files.TestCsv, out IEnumerable<CsvTestClass> results);

            // Should have two results
            Assert.AreEqual(2, results.Count(), $"The results count {results.Count()} did not match the expected count of 2.");

            // Verify the names & IDs match with what's expected
            VerifyCsvContents(results.ElementAt(0), 1, "Google");
            VerifyCsvContents(results.ElementAt(1), 2, "Drive");
        }

        /// <summary>
        /// Runs a JSON test. Loads in the TestJson file, and verifies that output values match what we expect
        /// </summary>
        [Test]
        public void TestJsonReading()
        {
            GameData.LoadJson<JsonTestClass>(Files.TestJson, out JsonTestClass result);

            // Verify that the data loaded correctly
            Assert.AreEqual(true, result.CanLoadJson, "The JSON data could not be read.");
            Assert.AreEqual("Test Json", result.Name, $"The name field {result.Name} did not match what was expected (Test Json).");
        }

        /// <summary>
        /// Runs through a single CSV Test Class to verify its values against what we expect
        /// </summary>
        /// <param name="csvData"></param>
        /// <param name="expectedId"></param>
        /// <param name="expectedName"></param>
        protected void VerifyCsvContents(CsvTestClass csvData, int expectedId, string expectedName)
        {
            // Assert that the ID matches
            Assert.AreEqual(expectedId, csvData.Id, $"The data's ID {csvData.Id} did not match our expected ID of {expectedId}.");

            // Assert that the name matches
            Assert.AreEqual(expectedName, csvData.Name, $"The data's Name {csvData.Name} did not match our expected name of {expectedName}.");
        }
    }
}
