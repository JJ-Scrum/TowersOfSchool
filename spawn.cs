using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowersOfSchool
{
    /// <summary>
    /// Verwaltet das Spawning von Gegnern und Türmen
    /// Diese Klasse wird durch das Welle- und Spiel-System ersetzt,
    /// aber behält die ursprünglichen Spawn-Logiken als Referenz
    /// </summary>
    public partial class Spawn
    {
        // Diese Logik wird nun in der WaveSystem-Klasse verwaltet
        public void Enemy()
        {
            string SpriteImage = "Enemy1.png";
            float EnemyHP = 100.00f;
            int Tier = 1;
            float CastleDmg = 2;

            // Die Enemy-Tier-Logik ist jetzt in der Enemy.SetTier() Methode implementiert
        }

        // Diese Logik wird nun in der Tower-Klasse verwaltet
        public void Tower()
        {
            string SpriteImage = "Tower1.png";

            List<string> TowerUpgrades = new List<string>();

            Upgrades alleVerfuegbarenUpgrades = new Upgrades();
            TowerUpgrades.AddRange(alleVerfuegbarenUpgrades.UpgradeList);

            Console.WriteLine($"Turm hat nun {TowerUpgrades.Count} Upgrades geladen.");
        }
    }
}

