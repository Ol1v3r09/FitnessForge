namespace FitnessForgeApp.Services
{
    public class UnitConverter
    {
        private static Dictionary<string, double> massConversions = new Dictionary<string, double>
        {
            { "Gramm", 1 },
            { "Dekagramm", 10 },
            { "Kilogramm", 1000 }
        };

        private static Dictionary<string, double> volumeConversions = new Dictionary<string, double>
        {
            { "Liter", 1000 },
            { "Centiliter", 10 },
            { "Deciliter", 100 },
            { "Milliliter", 1 }
        };

        public static double ConvertMass(double value, string fromUnit, string toUnit)
        {
            if (!massConversions.ContainsKey(fromUnit) || !massConversions.ContainsKey(toUnit))
                throw new ArgumentException("Nem létező mértékegység");

            if (massConversions[toUnit] > massConversions[fromUnit])
            {
                return value / massConversions[toUnit];
            }
            else
            {
                return value * massConversions[fromUnit];
            }
        }

        public static double ConvertVolume(double value, string fromUnit, string toUnit)
        {
            if (!volumeConversions.ContainsKey(fromUnit) || !volumeConversions.ContainsKey(toUnit))
                throw new ArgumentException("Nem létező mértékegység");

            if (volumeConversions[toUnit] > volumeConversions[fromUnit])
            {
                return value / volumeConversions[toUnit];
            }
            else
            {
                return value * volumeConversions[fromUnit];
            }
        }
    }
}
