using System.Collections.Generic;
using System.Linq;
using TowersOfSchool.Systems;

namespace TowersOfSchool
{
    /// <summary>
    /// Verwaltet die Wellen von Gegnern
    /// </summary>
    public class WaveSystem
    {
        public int CurrentWave { get; set; }
        public List<Enemy> CurrentEnemies { get; set; }
        public int EnemiesSpawnedThisWave { get; set; }
        public int TotalEnemiesThisWave { get; set; }
        public float TimeSinceLastSpawn { get; set; }
        public float SpawnInterval { get; set; } = 1.0f; // Eine Gegner pro Sekunde

        public WaveSystem()
        {
            CurrentWave = 0;
            CurrentEnemies = new List<Enemy>();
        }

        /// <summary>
        /// Startet die n�chste Welle
        /// </summary>
        public void StartNextWave()
        {
            CurrentWave++;
            EnemiesSpawnedThisWave = 0;
            TotalEnemiesThisWave = 5 + CurrentWave; // 6 Gegner in Welle 1, 7 in Welle 2, usw.
            TimeSinceLastSpawn = 0;
            CurrentEnemies.Clear();
        }

        /// <summary>
        /// Versucht, einen neuen Gegner zu spawnen
        /// </summary>
        public Enemy? TrySpawnEnemy(float deltaTime)
        {
            if (EnemiesSpawnedThisWave >= TotalEnemiesThisWave)
                return null;

            TimeSinceLastSpawn += deltaTime;
            if (TimeSinceLastSpawn >= SpawnInterval)
            {
                TimeSinceLastSpawn = 0;
                var enemy = new Enemy
                {
                    Name = $"Enemy_{CurrentWave}_{EnemiesSpawnedThisWave}",
                    SpriteImage = "Enemy1.png"
                };

                // Tier basierend auf Welle
                int tier = 1;
                if (CurrentWave >= 2)
                    tier = System.Math.Min(2, (CurrentWave - 1) / 2 + 1);
                if (CurrentWave >= 5)
                    tier = 3;

                enemy.SetTier(tier);
                EnemiesSpawnedThisWave++;
                // Nicht hier hinzufügen - wird in GameController.Update() hinzugefügt
                return enemy;
            }

            return null;
        }

        /// <summary>
        /// Entfernt tote oder angekommen Gegner
        /// </summary>
        public void CleanupEnemies(TowersOfSchool.Systems.PathSystem pathSystem)
        {
            CurrentEnemies.RemoveAll(e => !e.IsAlive || e.HasReachedGoal(pathSystem));
        }

        /// <summary>
        /// �berpr�ft, ob die aktuelle Welle abgeschlossen ist
        /// </summary>
        public bool IsWaveComplete()
        {
            return EnemiesSpawnedThisWave >= TotalEnemiesThisWave && CurrentEnemies.Count == 0;
        }

        /// <summary>
        /// Gibt alle lebenden Gegner zur�ck
        /// </summary>
        public List<Enemy> GetAliveEnemies()
        {
            return CurrentEnemies.Where(e => e.IsAlive).ToList();
        }
    }
}
