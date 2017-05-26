using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;

namespace HerbalismAndAlchemy
{
    public class Herbalism
    {
        #region fields
        private List<LocationTable> LocationTables;
        private List<Reagent> Reagents;

        private static Random rand = new Random();
        #endregion

        #region properties
        public IEnumerable<string> ValidLocations
        {
            get
            {
                return LocationTables.Where(t => !t.Name.Equals("Common")).Select(t => t.Name).ToArray();
            }
        }
        #endregion

        #region constructors
        public Herbalism()
        {
            var locationTablesJson = GetEmbeddedFileString("LocationTables.json");
            var reagentsJson = GetEmbeddedFileString("Reagents.json");

            LoadJSONDatasources(locationTablesJson, reagentsJson);
        }

        public Herbalism(string locationTablesJSON, string reagentsJSON)
        {
            LoadJSONDatasources(locationTablesJSON, reagentsJSON);
        }

        public Herbalism(List<LocationTable> locationTables, List<Reagent> reagents)
        {
            LocationTables = locationTables;
            Reagents = reagents;
        }
        #endregion

        #region private
        private void LoadJSONDatasources(string locationTablesJSON, string reagentsJSON)
        {
            LocationTables = JsonConvert.DeserializeObject<List<LocationTable>>(locationTablesJSON);
            Reagents = JsonConvert.DeserializeObject<List<Reagent>>(reagentsJSON);
        }

        private string GetEmbeddedFileString(string fileName)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = string.Format("HerbalismAndAlchemy.{0}", fileName);

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                using (var reader = new StreamReader(stream))
                {
                    var result = reader.ReadToEnd();

                    return result;
                }
            }
        }

        private Reagent GetReagent(string name)
        {
            return Reagents.Single(r => r.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        private Dictionary<Reagent, int> ApplyRulesForAdditionalReagents(IEnumerable<OutcomeRule> rules, bool onCoast, bool isNight, bool inCave, bool isRaining, bool notTrackingProvisions)
        {
            var additionalReagents = new Dictionary<Reagent, int>();
            foreach (var rule in rules)
            {
                switch (rule)
                {
                    case OutcomeRule.ComeWith1ElementalWater:
                        var elementalWater = GetReagent("Elemental Water");
                        additionalReagents.Add(elementalWater, 1);
                        break;
                }
            }

            return additionalReagents;
        }

        private bool IsOutcomeInvalid(IEnumerable<OutcomeRule> rules, bool onCoast, bool isNight, bool inCave, bool isRaining, bool notTrackingProvisions)
        {
            var invalid = false;
            foreach (var rule in rules)
            {
                switch (rule)
                {
                    case OutcomeRule.CoastalOnly:
                        if (!onCoast)
                            invalid = true;
                        break;
                    case OutcomeRule.RerollDuringDay:
                        if (!isNight)
                            invalid = true;
                        break;
                    case OutcomeRule.RerollIfNotTrackingProvisions:
                        if (notTrackingProvisions)
                            invalid = true;
                        break;
                }
            }

            return invalid;
        }

        private int ApplyRulesForReagentAmount(IEnumerable<OutcomeRule> rules, int reagentAmount, bool onCoast = false, bool isNight = false, bool inCave = false, bool isRaining = false, bool notTrackingProvisions = false)
        {
            foreach (var rule in rules)
            {
                switch (rule)
                {
                    case OutcomeRule.Find2xRolledAmount:
                        reagentAmount = reagentAmount * 2;
                        break;
                    case OutcomeRule.Find1to2xRolledAmount:
                        var multiplier = (new Random()).Next(1, 3);
                        reagentAmount = reagentAmount * multiplier;
                        break;
                    case OutcomeRule.Find2xDuringNight:
                        if (isNight)
                            reagentAmount = reagentAmount * 2;
                        break;
                    case OutcomeRule.Find2xRolledAmountInCaves:
                        if (inCave)
                            reagentAmount = reagentAmount * 2;
                        break;
                    case OutcomeRule.Find2xRolledAmountInRain:
                        if (isRaining)
                            reagentAmount = reagentAmount * 2;
                        break;
                }
            }

            return reagentAmount;
        }
        #endregion

        #region public
        public IEnumerable<HerbalismResult> RollHerbalismResults(string locationName, bool nat20 = false, bool onCoast = false, bool isNight = false, bool inCave = false, bool isRaining = false, bool notTrackingProvisions = false)
        {
            var tableRoll = rand.Next(2, 12 + 1);

            Reagent reagent = null;
            var outcomeRules = new List<OutcomeRule>();

            //find elemental water?
            if ((tableRoll >= 2 && tableRoll <= 4) || (tableRoll >= 10 && tableRoll <= 12))
            {
                var elementalWaterRoll = rand.Next(1, 100 + 1);

                if (elementalWaterRoll >= 75)
                {
                    reagent = GetReagent("Elemental Water");
                }
            }

            //find a different reagent
            var tableName = locationName;
            var table = LocationTables.Single(t => t.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase));
            while (reagent == null)
            {
                var outcome = table.Outcomes.Single(o => o.Roll == tableRoll);

                if (outcome.OutcomeType == TableOutcomeType.Ingredient)
                {
                    try
                    {
                        reagent = Reagents.Single(r => r.Name.Equals(outcome.Name, StringComparison.InvariantCultureIgnoreCase));
                    }
                    catch (Exception e)
                    {
                        Console.Error.WriteLine(string.Format("Could not find matching Reagent for {0}.", outcome.Name));
                        throw e;
                    }
                    
                    outcomeRules = outcome.Rules;

                    //if we can't get that one, roll again on the same table
                    if (IsOutcomeInvalid(outcomeRules, onCoast, isNight, inCave, isRaining, notTrackingProvisions))
                    {
                        tableRoll = rand.Next(2, 12 + 1);
                        reagent = null;
                        outcomeRules.Clear();
                    }
                }
                else if (outcome.OutcomeType == TableOutcomeType.Table)
                {
                    //roll again on a different table
                    tableRoll = rand.Next(2, 12 + 1);
                    tableName = outcome.Name;
                    table = LocationTables.Single(t => t.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase));
                }
            }

            //how many did you find?
            var reagentAmount = nat20 ? rand.Next(2, 8 + 1) : rand.Next(1, 4 + 1);
            reagentAmount = ApplyRulesForReagentAmount(outcomeRules, reagentAmount);

            //did you get anything else?
            var additionalReagents = ApplyRulesForAdditionalReagents(outcomeRules, onCoast, isNight, inCave, isRaining, notTrackingProvisions);

            //return back with what we found
            var results = new List<HerbalismResult>()
            {
                new HerbalismResult()
                {
                    Reagent = reagent,
                    Amount = reagentAmount
                }
            };
            foreach (var additionalReagent in additionalReagents)
            {
                results.Add(new HerbalismResult()
                {
                    Reagent = additionalReagent.Key,
                    Amount = additionalReagent.Value
                });
            }

            return results;
        }
        #endregion
    }
}
