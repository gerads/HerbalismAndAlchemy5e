using NUnit.Framework;
using HerbalismAndAlchemy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HerbalismAndAlchemyTests
{
    [TestFixture()]
    public class HerbalismTests
    {
        #region common data
        List<LocationTable> Tables = new List<LocationTable>()
        {
            new LocationTable()
            {
                Name = "testDefaultLocation",
                Outcomes = new List<TableOutcome>()
                {
                    new TableOutcome()
                    {
                        Name = "testReagent",
                        OutcomeType = 0,
                        Roll = 2,
                        Rules = new List<OutcomeRule>()
                        {
                            OutcomeRule.Find2xRolledAmount
                        }
                    },
                    new TableOutcome()
                    {
                        Name = "testOtherTable",
                        OutcomeType = TableOutcomeType.Table,
                        Roll = 3,
                        Rules = new List<OutcomeRule>()
                        {
                            OutcomeRule.RollOnCommonIngredientTable
                        }
                    },
                    new TableOutcome()
                    {
                        Name = "testReagent",
                        OutcomeType = 0,
                        Roll = 4,
                        Rules = new List<OutcomeRule>()
                        {
                            OutcomeRule.Find1to2xRolledAmount
                        }
                    },
                    new TableOutcome()
                    {
                        Name = "testReagent",
                        OutcomeType = 0,
                        Roll = 5,
                        Rules = new List<OutcomeRule>()
                        {
                            OutcomeRule.CoastalOnly
                        }
                    },
                    new TableOutcome()
                    {
                        Name = "testReagent",
                        OutcomeType = 0,
                        Roll = 6,
                        Rules = new List<OutcomeRule>()
                        {
                            OutcomeRule.ComeWith1ElementalWater
                        }
                    },
                    new TableOutcome()
                    {
                        Name = "testReagent",
                        OutcomeType = 0,
                        Roll = 7,
                        Rules = new List<OutcomeRule>()
                        {
                            OutcomeRule.Find2xDuringNight
                        }
                    },
                    new TableOutcome()
                    {
                        Name = "testReagent",
                        OutcomeType = 0,
                        Roll = 8,
                        Rules = new List<OutcomeRule>()
                        {
                            OutcomeRule.RerollDuringDay
                        }
                    },
                    new TableOutcome()
                    {
                        Name = "testReagent",
                        OutcomeType = 0,
                        Roll = 9,
                        Rules = new List<OutcomeRule>()
                        {
                            OutcomeRule.Find2xRolledAmountInCaves
                        }
                    },
                    new TableOutcome()
                    {
                        Name = "testReagent",
                        OutcomeType = 0,
                        Roll = 10,
                        Rules = new List<OutcomeRule>()
                        {
                            OutcomeRule.Find2xRolledAmountInRain
                        }
                    },
                    new TableOutcome()
                    {
                        Name = "testReagent",
                        OutcomeType = 0,
                        Roll = 11,
                        Rules = new List<OutcomeRule>()
                        {
                            OutcomeRule.RerollIfNotTrackingProvisions
                        }
                    },
                    new TableOutcome()
                    {
                        Name = "testReagent",
                        OutcomeType = 0,
                        Roll = 12,
                        Rules = new List<OutcomeRule>()
                        {

                        }
                    }
                }
            },
            new LocationTable()
            {
                Name = "testOtherTable",
                Outcomes = new List<TableOutcome>()
                {
                    new TableOutcome()
                    {
                        Name = "testOtherReagent",
                        OutcomeType = 0,
                        Roll = 2,
                        Rules = new List<OutcomeRule>()
                    },
                    new TableOutcome()
                    {
                        Name = "testOtherReagent",
                        OutcomeType = 0,
                        Roll = 3,
                        Rules = new List<OutcomeRule>()
                    },
                    new TableOutcome()
                    {
                        Name = "testOtherReagent",
                        OutcomeType = 0,
                        Roll = 4,
                        Rules = new List<OutcomeRule>()
                    },
                    new TableOutcome()
                    {
                        Name = "testOtherReagent",
                        OutcomeType = 0,
                        Roll = 5,
                        Rules = new List<OutcomeRule>()
                    },
                    new TableOutcome()
                    {
                        Name = "testOtherReagent",
                        OutcomeType = 0,
                        Roll = 6,
                        Rules = new List<OutcomeRule>()
                    },
                    new TableOutcome()
                    {
                        Name = "testOtherReagent",
                        OutcomeType = 0,
                        Roll = 7,
                        Rules = new List<OutcomeRule>()
                    },
                    new TableOutcome()
                    {
                        Name = "testOtherReagent",
                        OutcomeType = 0,
                        Roll = 8,
                        Rules = new List<OutcomeRule>()
                    },
                    new TableOutcome()
                    {
                        Name = "testOtherReagent",
                        OutcomeType = 0,
                        Roll = 9,
                        Rules = new List<OutcomeRule>()
                    },
                    new TableOutcome()
                    {
                        Name = "testOtherReagent",
                        OutcomeType = 0,
                        Roll = 10,
                        Rules = new List<OutcomeRule>()
                    },
                    new TableOutcome()
                    {
                        Name = "testOtherReagent",
                        OutcomeType = 0,
                        Roll = 11,
                        Rules = new List<OutcomeRule>()
                    },
                    new TableOutcome()
                    {
                        Name = "testOtherReagent",
                        OutcomeType = 0,
                        Roll = 12,
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
            },
            new Reagent()
            {
                Name = "testOtherReagent",
                Description = "A different reagent.",
                Details = "Gives +1 to differences",
                DC = 0,
                Rarity = "Rare"
            },
            new Reagent()
            {
                Name = "Elemental Water",
                Description = "Magic water.",
                Details = "+1 to yum",
                DC = 1,
                Rarity = "Uncommon"
            }
        };
        #endregion

        #region helpers
        private bool AreHerbalismResultsValid(IEnumerable<HerbalismResult> results)
        {
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

            return true;
        }
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

        [Test()]
        public void GetHerbalismResultsTest_Find2xRolledAmount()
        {
            var herb = new Herbalism(Tables, Reagents);

            var location = "testDefaultLocation";
            var tableRoll = 2;

            var results = herb.GetHerbalismResults(location, tableRoll);

            AreHerbalismResultsValid(results);
        }

        [Test()]
        public void GetHerbalismResultsTest_RollOnCommonIngredientTable()
        {
            var herb = new Herbalism(Tables, Reagents);

            var location = "testDefaultLocation";
            var tableRoll = 3;

            var results = herb.GetHerbalismResults(location, tableRoll);

            AreHerbalismResultsValid(results);

            Assert.IsTrue(results.First().Reagent.Name.Equals("testOtherReagent", StringComparison.InvariantCultureIgnoreCase));
        }

        [Test()]
        public void GetHerbalismResultsTest_Find1to2xRolledAmount()
        {
            var herb = new Herbalism(Tables, Reagents);

            var location = "testDefaultLocation";
            var tableRoll = 4;

            var results = herb.GetHerbalismResults(location, tableRoll);

            AreHerbalismResultsValid(results);
        }

        [Test()]
        public void GetHerbalismResultsTest_CoastalOnly_NotOnCoast()
        {
            var herb = new Herbalism(Tables, Reagents);

            var location = "testDefaultLocation";
            var tableRoll = 5;

            var results = herb.GetHerbalismResults(location, tableRoll, onCoast: false);

            AreHerbalismResultsValid(results);
        }

        [Test()]
        public void GetHerbalismResultsTest_CoastalOnly_OnCoast()
        {
            var herb = new Herbalism(Tables, Reagents);

            var location = "testDefaultLocation";
            var tableRoll = 5;

            var results = herb.GetHerbalismResults(location, tableRoll, onCoast: true);

            AreHerbalismResultsValid(results);
        }

        [Test()]
        public void GetHerbalismResultsTest_ComeWith1ElementalWater()
        {
            var herb = new Herbalism(Tables, Reagents);

            var location = "testDefaultLocation";
            var tableRoll = 6;

            var results = herb.GetHerbalismResults(location, tableRoll);

            AreHerbalismResultsValid(results);

            Assert.IsTrue(results.Count() == 2);
        }

        [Test()]
        public void GetHerbalismResultsTest_Find2xDuringNight_IsNotNight()
        {
            var herb = new Herbalism(Tables, Reagents);

            var location = "testDefaultLocation";
            var tableRoll = 7;

            var results = herb.GetHerbalismResults(location, tableRoll, isNight: false);

            AreHerbalismResultsValid(results);
        }

        [Test()]
        public void GetHerbalismResultsTest_Find2xDuringNight_IsNight()
        {
            var herb = new Herbalism(Tables, Reagents);

            var location = "testDefaultLocation";
            var tableRoll = 7;

            var results = herb.GetHerbalismResults(location, tableRoll, isNight: true);

            AreHerbalismResultsValid(results);
        }

        [Test()]
        public void GetHerbalismResultsTest_RerollDuringDay_IsNotDay()
        {
            var herb = new Herbalism(Tables, Reagents);

            var location = "testDefaultLocation";
            var tableRoll = 8;

            var results = herb.GetHerbalismResults(location, tableRoll, isNight: true);

            AreHerbalismResultsValid(results);
        }

        [Test()]
        public void GetHerbalismResultsTest_RerollDuringDay_IsDay()
        {
            var herb = new Herbalism(Tables, Reagents);

            var location = "testDefaultLocation";
            var tableRoll = 8;

            var results = herb.GetHerbalismResults(location, tableRoll, isNight: false);

            AreHerbalismResultsValid(results);
        }

        [Test()]
        public void GetHerbalismResultsTest_Find2xRolledAmountInCaves_NotInCave()
        {
            var herb = new Herbalism(Tables, Reagents);

            var location = "testDefaultLocation";
            var tableRoll = 9;

            var results = herb.GetHerbalismResults(location, tableRoll, inCave: false);

            AreHerbalismResultsValid(results);
        }

        [Test()]
        public void GetHerbalismResultsTest_Find2xRolledAmountInCaves_InCave()
        {
            var herb = new Herbalism(Tables, Reagents);

            var location = "testDefaultLocation";
            var tableRoll = 9;

            var results = herb.GetHerbalismResults(location, tableRoll, inCave: true);

            AreHerbalismResultsValid(results);
        }

        [Test()]
        public void GetHerbalismResultsTest_Find2xRolledAmountInRain_IsNotRaining()
        {
            var herb = new Herbalism(Tables, Reagents);

            var location = "testDefaultLocation";
            var tableRoll = 10;

            var results = herb.GetHerbalismResults(location, tableRoll, isRaining: false);

            AreHerbalismResultsValid(results);
        }

        [Test()]
        public void GetHerbalismResultsTest_Find2xRolledAmountInRain_IsRaining()
        {
            var herb = new Herbalism(Tables, Reagents);

            var location = "testDefaultLocation";
            var tableRoll = 10;

            var results = herb.GetHerbalismResults(location, tableRoll, isRaining: true);

            AreHerbalismResultsValid(results);
        }

        [Test()]
        public void GetHerbalismResultsTest_RerollIfNotTrackingProvisions_NotTrackingProvisions()
        {
            var herb = new Herbalism(Tables, Reagents);

            var location = "testDefaultLocation";
            var tableRoll = 11;

            var results = herb.GetHerbalismResults(location, tableRoll, notTrackingProvisions: true);

            AreHerbalismResultsValid(results);
        }

        [Test()]
        public void GetHerbalismResultsTest_RerollIfNotTrackingProvisions_TrackingProvisions()
        {
            var herb = new Herbalism(Tables, Reagents);

            var location = "testDefaultLocation";
            var tableRoll = 11;

            var results = herb.GetHerbalismResults(location, tableRoll, notTrackingProvisions: false);

            AreHerbalismResultsValid(results);
        }

        [Test()]
        public void RollHerbalismResultsTest_Nat20()
        {
            var herb = new Herbalism();

            var location = herb.ValidLocations.First();

            var results = herb.RollHerbalismResults(location, nat20: true);

            AreHerbalismResultsValid(results);
        }

        [Ignore("Not stable")]
        [Test()]
        public void RollHerbalismResultsTest_AllValidLocations_Once()
        {
            var herb = new Herbalism();

            foreach (var location in herb.ValidLocations)
            {
                var results = herb.RollHerbalismResults(location);

                AreHerbalismResultsValid(results);
            }
        }
    }
}