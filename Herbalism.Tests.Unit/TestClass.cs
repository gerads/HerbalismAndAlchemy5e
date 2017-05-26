using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HerbalismAndAlchemy;

namespace HerbalismAndAlchemy.Tests.Unit
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void Herbalism_AllLocations()
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
