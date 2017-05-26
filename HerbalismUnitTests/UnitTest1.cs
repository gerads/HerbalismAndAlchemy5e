using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using HerbalismAndAlchemy;

namespace HerbalismUnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void RollHerbalismResult_AllValidLocations()
        {
            var herbalism = new Herbalism();

            foreach (var location in herbalism.ValidLocations)
            {
                var results = herbalism.RollHerbalismResults(location);

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
