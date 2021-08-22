using System.Collections.Generic;
using JOIEnergy.Domain;
using JOIEnergy.Enums;

namespace JOIEnergy.Generator
{
    public static class PricePlanGenerator
    {
        public static IList<PricePlan> GeneratePricePlans()
        {
            return new List<PricePlan> {
                new PricePlan{
                    EnergySupplier = Supplier.DrEvilsDarkEnergy,
                    UnitRate = 10m,
                    PeakTimeMultiplier = new List<PeakTimeMultiplier>()
                },
                new PricePlan{
                    EnergySupplier = Supplier.TheGreenEco,
                    UnitRate = 2m,
                    PeakTimeMultiplier = new List<PeakTimeMultiplier>()
                },
                new PricePlan{
                    EnergySupplier = Supplier.PowerForEveryone,
                    UnitRate = 1m,
                    PeakTimeMultiplier = new List<PeakTimeMultiplier>()
                }
            };
        }
    }
}