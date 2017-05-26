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
    class Program
    {
        public static List<LocationTable> Tables;
        public static List<Reagent> Reagents;

        static string GetEmbeddedFileString(string fileName)
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

        static void Init()
        {
            var tablesJson = GetEmbeddedFileString("Tables.json");
            Tables = JsonConvert.DeserializeObject<List<LocationTable>>(tablesJson);

            var reagentsJson = GetEmbeddedFileString("Reagents.json");
            Reagents = JsonConvert.DeserializeObject<List<Reagent>>(reagentsJson);
        }

        static void Main(string[] args)
        {
            Init();

            var rand = new Random();

            var validLocations = Tables.Where(t => !t.Name.Equals("Common"));
            var locationName = validLocations.ElementAt(rand.Next(0, validLocations.Count())).Name;
            var numberRolled = rand.Next(1, 20 + 1);
            var nat20 = rand.Next(2) == 1;

            var onCoast = false;
            var isNight = false;
            var inCave = false;
            var isRaining = false;
            var notTrackingProvisions = false;

            var tableRoll = rand.Next(2, 12 + 1);

            Reagent reagent = null;
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
            var table = Tables.Single(t => t.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase));
            var outcomeRules = new List<OutcomeRule>();
            while(reagent == null)
            {
                var outcome = table.Outcomes.Single(o => o.Roll == tableRoll);

                if (outcome.OutcomeType == TableOutcomeType.Ingredient)
                {
                    reagent = Reagents.Single(r => r.Name.Equals(outcome.Name, StringComparison.InvariantCultureIgnoreCase));
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
                    table = Tables.Single(t => t.Name.Equals(tableName, StringComparison.InvariantCultureIgnoreCase));
                }
            }

            //how many did you find?
            var reagentAmount = nat20 ? rand.Next(2, 8 + 1) : rand.Next(1, 4 + 1);
            reagentAmount = ApplyRulesForReagentAmount(outcomeRules, reagentAmount);

            //did you get anything else?
            var additionalReagents = ApplyRulesForAdditionalReagents(outcomeRules, onCoast, isNight, inCave, isRaining, notTrackingProvisions);

            //return back with what we found
            Console.WriteLine(string.Format("Found {0} {1} {2} in the {3}.", reagentAmount, reagent.Name, reagentAmount <= 1 ? "ingredient" : "ingredients", locationName.ToLowerInvariant()));
            foreach (var additionalReagent in additionalReagents)
            {
                Console.WriteLine(string.Format("Also found {0} {1} {2}.", additionalReagent.Value, additionalReagent.Key.Name, additionalReagent.Value <= 1 ? "ingredient" : "ingredients"));
            }
            Console.ReadLine();
        }

        static Reagent GetReagent(string name)
        {
            return Reagents.Single(r => r.Name.Equals(name, StringComparison.InvariantCultureIgnoreCase));
        }

        static Dictionary<Reagent, int> ApplyRulesForAdditionalReagents(IEnumerable<OutcomeRule> rules, bool onCoast, bool isNight, bool inCave, bool isRaining, bool notTrackingProvisions)
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

        static bool IsOutcomeInvalid(IEnumerable<OutcomeRule> rules, bool onCoast, bool isNight, bool inCave, bool isRaining, bool notTrackingProvisions)
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

        static int ApplyRulesForReagentAmount(IEnumerable<OutcomeRule> rules, int reagentAmount, bool onCoast = false, bool isNight = false, bool inCave = false, bool isRaining = false, bool notTrackingProvisions = false)
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
    }
}
