using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace JsonTests
{
    [TestFixture]
    public class JSONTest
    {
        private IJsonDeserializer _deserializer;
        private const string JsonFilePath = "Cost Analysis.json";

        [SetUp]
        public void Setup()
        {
            _deserializer = new JsonDeserializer();
        }

        [Test]
        public void DeserializeCostAnalysisJson_ShouldValidateListAndLinqOperations()
        {
            // Arrange
            const int expectedCount = 53; // From the JSON array
            const int expectedTopCostCountryId = 0; // From 69437.0683739 in 2016
            const decimal expected2016CostSum = 80991.3744551m; // Calculated from 2016 entries

            // Act
            List<CostRecord> costRecords;
            try
            {
                costRecords = _deserializer.DeserializeList<CostRecord>(JsonFilePath);
            }
            catch (FileNotFoundException ex)
            {
                Assert.Fail($"Test failed: JSON file not found - {ex.Message}");
                return;
            }
            catch (JsonException ex)
            {
                Assert.Fail($"Test failed: JSON deserialization error - {ex.Message}");
                return;
            }
            catch (Exception ex)
            {
                Assert.Fail($"Test failed: Unexpected error - {ex.Message}");
                return;
            }

            // Assert: List count
            Assert.IsNotNull(costRecords, "Deserialized list should not be null");
            Assert.AreEqual(expectedCount, costRecords.Count,
                $"Expected {expectedCount} items, but got {costRecords.Count}");

            // LINQ: Top item by Cost descending
            var topCostItem = costRecords.OrderByDescending(x => x.Cost).FirstOrDefault();
            Assert.IsNotNull(topCostItem, "No top cost item found");
            Assert.AreEqual(expectedTopCostCountryId, topCostItem.CountryId,
                $"Expected CountryId {expectedTopCostCountryId} for top cost, but got {topCostItem.CountryId}");

            // LINQ: Sum Cost for 2016
            var totalCost2016 = costRecords
                .Where(x => x.YearId == "2016")
                .Sum(x => x.Cost);
            Assert.AreEqual(expected2016CostSum, totalCost2016, 0.0000001m,
                $"Expected 2016 cost sum {expected2016CostSum}, but got {totalCost2016}");
        }
    }
}
