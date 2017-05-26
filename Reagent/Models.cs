using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HerbalismAndAlchemy
{
    public class LocationTable
    {
        public string Name{ get; set; }
        public List<TableOutcome> Outcomes{ get; set; }
    }

    public class TableOutcome
    {
        public string Name{ get; set; }
        public int Roll{ get; set; }
        public TableOutcomeType OutcomeType{ get; set; }
        public List<OutcomeRule> Rules{ get; set; }
    }

    public enum TableOutcomeType
    {
        Ingredient,
        Table
    }

    public enum OutcomeRule
    {
        Find2xRolledAmount,
        RollOnCommonIngredientTable,
        Find1to2xRolledAmount,
        CoastalOnly,
        ComeWith1ElementalWater,
        Find2xDuringNight,
        RerollDuringDay,
        Find2xRolledAmountInCaves,
        Find2xRolledAmountInRain,
        RerollIfNotTrackingProvisions
    }

    public class Reagent
    {
        public string Name{ get; set; }
        public string Rarity{ get; set; }
        public string Details{ get; set; }
        public string Description{ get; set; }
        public int DC{ get; set; }
    }

    public class HerbalismResult
    {
        public Reagent Reagent { get; set; }
        public int Amount { get; set; }
    }
}
