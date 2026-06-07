using System.Collections.Generic;
using TowersOfSchool.Models;
using TowersOfSchool.Systems;

namespace TowersOfSchool
{
    /// <summary>
        /// Repr�sentiert einen Turm im Spiel
        /// </summary>
        public class Tower
        {
        public string Name { get; set; } = "Tower";
        public string SpriteImage { get; set; } = "tower.png";
        public Point Position { get; set; } = new Point(0, 0);
        public float BaseDamage { get; set; }
        public float CurrentDamage { get; set; }
        public float Range { get; set; }
        public float FireRate { get; set; }
        public int Cost { get; set; }
        public int SellPrice { get; set; }
        public float TimeSinceLastShot { get; set; }
        public List<string> Upgrades { get; set; } = new List<string>();
        public TowerSlot? Slot { get; set; }
        public TowerAbilitiesSystem.TowerAbility PrimaryAbility { get; set; }
        public float AbilityCooldownTimer { get; set; }
        public bool IsPaused { get; set; }
        public float PausedTimeRemaining { get; set; }

            public Tower()
            {
                BaseDamage = 10.0f;
                CurrentDamage = 10.0f;
                Range = 100.0f;
                FireRate = 1.0f; // Sch�sse pro Sekunde
                Cost = 100;
                SellPrice = Cost / 2;
                TimeSinceLastShot = 0;
                Upgrades = new List<string>();
                PrimaryAbility = TowerAbilitiesSystem.TowerAbility.BasicAttack;
                AbilityCooldownTimer = 0;
                IsPaused = false;
                PausedTimeRemaining = 0;
            }

        /// <summary>
        /// Versucht, einen Gegner anzugreifen, wenn dieser im Bereich ist
        /// </summary>
        public bool CanShoot(Enemy enemy, float deltaTime)
        {
            if (enemy == null || !enemy.IsAlive)
                return false;

            TimeSinceLastShot += deltaTime;
            float timeBetweenShots = 1.0f / FireRate;

            if (Position.DistanceTo(enemy.CurrentPosition) <= Range && TimeSinceLastShot >= timeBetweenShots)
            {
                TimeSinceLastShot = 0;
                return true;
            }

            return false;
        }

        /// <summary>
        /// F�gt ein Upgrade hinzu und aktualisiert die Stats
        /// </summary>
        public void AddUpgrade(string upgradeName)
        {
            if (!Upgrades.Contains(upgradeName))
            {
                Upgrades.Add(upgradeName);
                ApplyUpgrade(upgradeName);
                SellPrice = (int)(Cost / 2 + (Cost * 0.1f * Upgrades.Count));
            }
        }

        private void ApplyUpgrade(string upgradeName)
        {
            switch (upgradeName)
            {
                case "Damage I":
                    CurrentDamage += BaseDamage * 0.125f;
                    break;
                case "Damage II":
                    CurrentDamage += BaseDamage * 0.125f;
                    break;
                case "Damage III":
                    CurrentDamage += BaseDamage * 0.125f;
                    break;
                case "Range I":
                    Range += Range * 0.125f;
                    break;
                case "Range II":
                    Range += Range * 0.125f;
                    break;
                case "Range III":
                    Range += Range * 0.125f;
                    break;
                case "Luck I":
                case "Luck II":
                case "Luck III":
                    // Gl�ck wird sp�ter im Economy-System angewendet
                    break;
                case "Boost All":
                    CurrentDamage += BaseDamage * 0.25f;
                    Range += Range * 0.25f;
                    break;
            }
        }
    }
}
