using System.Collections.Generic;
using JOIEnergy.Enums;

namespace JOIEnergy.Generator
{
    public static class SmartMeterGenerator
    {
        public static IDictionary<string, Supplier> GenerateSmartMeterToPricePlanAccounts() => new Dictionary<string, Supplier>
        {
            {"smart-meter-0", Supplier.DrEvilsDarkEnergy},
            {"smart-meter-1", Supplier.TheGreenEco},
            {"smart-meter-2", Supplier.DrEvilsDarkEnergy},
            {"smart-meter-3", Supplier.PowerForEveryone},
            {"smart-meter-4", Supplier.TheGreenEco}
        };
    }
}