using NUnit.Framework;
using HerbalismAndAlchemy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HerbalismAndAlchemy.Tests
{
    [TestFixture()]
    public class HerbalismTests
    {
        #region common data
        List<LocationTable> Tables = new List<LocationTable>()
        {
            new LocationTable()
            {
                Name = "testTable1",
                Outcomes = new List<TableOutcome>()
                {
                    new TableOutcome()
                    {
                        Name = "testOutcomeIngredient",
                        OutcomeType = 0,
                        Roll = 2,
                        Rules = new List<OutcomeRule>()
                    }
                }
            }
        };
        List<Reagent> Reagents = new List<Reagent>()
        {
            new Reagent()
            {
                Name = "testReagent",
                Description = "A testy reagent.",
                Details = "Gives +1 to test",
                DC = 0,
                Rarity = "Common"
            }
        };
        #endregion

        [Test()]
        public void HerbalismTest_None()
        {
            var herb = new Herbalism();
            Assert.IsNotNull(herb);
        }

        [Test()]
        public void HerbalismTest_JSON()
        {
            var herb = new Herbalism(JsonConvert.SerializeObject(Tables), JsonConvert.SerializeObject(Reagents));
            Assert.IsNotNull(herb);
        }

        [Test()]
        public void HerbalismTest_Objects()
        {
            var herb = new Herbalism(Tables, Reagents);
            Assert.IsNotNull(herb);
        }

        [Ignore("too lazy to write")]
        [Test()]
        public void RollHerbalismResultsTest()
        {
            Assert.Fail();
        }

        [Test()]
        public void RollHerbalismResultsTest_AllValidLocations_Once()
        {
            var herb = new Herbalism();

            foreach (var location in herb.ValidLocations)
            {
                var results = herb.RollHerbalismResults(location);

                Assert.IsNotNull(results);

                foreach (var result in results)
                {
                    Assert.IsNotNull(result.Amount);
                    Assert.IsTrue(result.Amount >= 1);
                    Assert.IsNotNull(result.Reagent);
                    Assert.IsFalse(string.IsNullOrEmpty(result.Reagent.Name));
                    Assert.IsFalse(string.IsNullOrEmpty(result.Reagent.Rarity));
                    Assert.IsFalse(string.IsNullOrEmpty(result.Reagent.Description));
                    Assert.IsFalse(string.IsNullOrEmpty(result.Reagent.Details));
                    Assert.IsNotNull(result.Reagent.DC);
                }
            }
        }
    }
}