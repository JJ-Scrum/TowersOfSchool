using System.Collections.Generic;

namespace TowersOfSchool.Systems
{
    /// <summary>
    /// Verwaltetes Prestige-System für dauerhafte Upgrades
    /// Der Spieler kann Diamanten verdienen und diese für permanente Boni ausgeben
    /// </summary>
    public class PrestigeSystem
    {
        public int Diamonds { get; set; }
        
        /// <summary>
        /// Permanente Upgrades die mit Diamanten gekauft werden können
        /// </summary>
        public Dictionary<string, (int Cost, float Bonus)> AvailableUpgrades { get; private set; }
        public Dictionary<string, int> PurchasedUpgrades { get; private set; } // Level pro Upgrade

        public PrestigeSystem()
        {
            Diamonds = 0;
            PurchasedUpgrades = new Dictionary<string, int>();
            InitializeUpgrades();
        }

        private void InitializeUpgrades()
        {
            AvailableUpgrades = new Dictionary<string, (int, float)>
            {
                // Tower Stat Boosts (kaufbar mit Levels)
                { "TowerDamageBoost", (50, 0.1f) },      // +10% Schaden pro Level
                { "TowerRangeBoost", (40, 0.1f) },       // +10% Range pro Level
                { "TowerFireRateBoost", (45, 0.1f) },    // +10% Feuerrate pro Level
                
                // Economy Boosts
                { "IncomeBoost", (60, 0.15f) },          // +15% Einkommen pro Level
                { "TaxReduction", (75, 0.05f) },         // -5% Steuern pro Level
                
                // Special Bonuses
                { "ExtraStartingMoney", (100, 100) },    // +100 Startgeld pro Level
                { "CastleHealthBoost", (80, 5) }         // +5 Schloss-Leben pro Level
            };

            // Initialisiere alle Upgrades auf Level 0
            foreach (var upgrade in AvailableUpgrades.Keys)
            {
                PurchasedUpgrades[upgrade] = 0;
            }
        }

        /// <summary>
        /// Versucht, ein Prestige-Upgrade zu kaufen
        /// </summary>
        public bool TryBuyPrestigeUpgrade(string upgradeName)
        {
            if (!AvailableUpgrades.ContainsKey(upgradeName))
                return false;

            int cost = AvailableUpgrades[upgradeName].Cost;
            
            if (Diamonds >= cost)
            {
                Diamonds -= cost;
                PurchasedUpgrades[upgradeName]++;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Gibt den aktuellen Bonus für ein Upgrade zurück
        /// </summary>
        public float GetUpgradeBonus(string upgradeName)
        {
            if (!PurchasedUpgrades.ContainsKey(upgradeName))
                return 0f;

            int level = PurchasedUpgrades[upgradeName];
            float bonus = AvailableUpgrades[upgradeName].Bonus;
            return bonus * level;
        }

        /// <summary>
        /// Belohnt den Spieler mit Diamanten (z.B. beim Abschließen von Wellen)
        /// </summary>
        public void RewardDiamonds(int amount)
        {
            Diamonds += amount;
        }
    }
}
