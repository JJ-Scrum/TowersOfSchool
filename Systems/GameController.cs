using System;
using System.Collections.Generic;
using TowersOfSchool.Systems;

namespace TowersOfSchool
{
    /// <summary>
    /// Hauptspiel-Controller, der alle Systeme verwaltet
    /// </summary>
    public class GameController
    {
        public PathSystem PathSystem { get; private set; }
        public TowerPlacementSystem TowerPlacementSystem { get; private set; }
        public EconomySystem EconomySystem { get; private set; }
        public WaveSystem WaveSystem { get; private set; }
        public PrestigeSystem PrestigeSystem { get; private set; }
        public TowerAbilitiesSystem TowerAbilitiesSystem { get; private set; }
        public List<Tower> PlacedTowers { get; private set; }
        public int CastleHealth { get; set; } = 20;
        public bool IsGameOver { get; set; }
        public bool IsGameWon { get; set; }

        public GameController()
        {
            PathSystem = new PathSystem();
            TowerPlacementSystem = new TowerPlacementSystem();
            EconomySystem = new EconomySystem();
            WaveSystem = new WaveSystem();
            PrestigeSystem = new PrestigeSystem();
            TowerAbilitiesSystem = new TowerAbilitiesSystem();
            PlacedTowers = new List<Tower>();
            
            // Anfangsgeld
            EconomySystem.PlayerMoney = 200;
        }

        /// <summary>
        /// Aktualisiert das Spiel-State (wird regelm��ig aufgerufen)
        /// </summary>
        public void Update(float deltaTime)
        {
            if (IsGameOver || IsGameWon)
                return;

            // Gegner spawnen
            var newEnemy = WaveSystem.TrySpawnEnemy(deltaTime);
            if (newEnemy != null)
            {
                WaveSystem.CurrentEnemies.Add(newEnemy);
            }

            // Gegner bewegen
            var enemiesToRemove = new List<Enemy>();
            foreach (var enemy in WaveSystem.CurrentEnemies)
            {
                enemy.MoveAlongPath(PathSystem, deltaTime);

                if (enemy.HasReachedGoal(PathSystem))
                {
                    CastleHealth -= (int)enemy.CastleDamage;
                    enemiesToRemove.Add(enemy);
                }
            }

            foreach (var enemy in enemiesToRemove)
            {
                WaveSystem.CurrentEnemies.Remove(enemy);
            }

            // T�rme schie�en
            foreach (var tower in PlacedTowers)
            {
                // Update Tower Pause
                if (tower.IsPaused)
                {
                    tower.PausedTimeRemaining -= deltaTime;
                    if (tower.PausedTimeRemaining <= 0)
                    {
                        tower.IsPaused = false;
                        tower.PausedTimeRemaining = 0;
                    }
                }

                // Update Ability Cooldown
                if (tower.AbilityCooldownTimer > 0)
                {
                    tower.AbilityCooldownTimer -= deltaTime;
                }

                // Nur schie�en wenn nicht pausiert
                if (!tower.IsPaused)
                {
                    var aliveEnemies = WaveSystem.GetAliveEnemies();
                    if (aliveEnemies.Count > 0)
                    {
                        var target = aliveEnemies[0];
                        if (tower.CanShoot(target, deltaTime))
                        {
                            target.TakeDamage(tower.CurrentDamage);
                            if (!target.IsAlive)
                            {
                                // Gegner get�tet - Belohnung
                                int reward = 20;
                                EconomySystem.RewardEnemyKill(reward);
                            }
                        }
                    }
                }
            }

            // Cleanup
            WaveSystem.CleanupEnemies(PathSystem);

            // Spielzustand �berpr�fen
            if (CastleHealth <= 0)
            {
                IsGameOver = true;
            }

            // Wellen-Verwaltung
            if (WaveSystem.IsWaveComplete())
            {
                WaveSystem.StartNextWave();
            }
        }

        /// <summary>
        /// Versucht, einen Turm zu platzieren
        /// </summary>
        public bool PlaceTower(Models.Point position, Tower tower)
        {
            var slot = TowerPlacementSystem.TryPlaceTower(position);
            if (slot != null && EconomySystem.TryBuyTower(tower.Cost))
            {
                tower.Slot = slot;
                tower.Position = slot.Position;
                PlacedTowers.Add(tower);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Verkauft einen Turm
        /// </summary>
        public bool SellTower(Tower tower)
        {
            if (PlacedTowers.Remove(tower))
            {
                TowerPlacementSystem.RemoveTower(tower.Slot);
                EconomySystem.SellTower(tower.SellPrice);
                return true;
            }
            return false;
        }

        /// <summary>
        /// L�dt ein Upgrade f�r einen Turm
        /// </summary>
        public bool UpgradeTower(Tower tower, string upgradeName, int upgradeCost)
        {
            if (EconomySystem.TryBuyUpgrade(upgradeCost))
            {
                tower.AddUpgrade(upgradeName);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Pausiert einen Turm f�r die angegebene Zeit (in Sekunden)
        /// Wird verwendet von Herr Baumstamm's F�higkeit
        /// </summary>
        public void PauseTower(Tower tower, float pauseDuration)
        {
            tower.IsPaused = true;
            tower.PausedTimeRemaining = pauseDuration;
        }

        /// <summary>
        /// Aktiviert eine Tower-F�higkeit
        /// </summary>
        public bool ActivateTowerAbility(Tower tower, TowerAbilitiesSystem.TowerAbility ability)
        {
            if (tower.AbilityCooldownTimer > 0)
                return false;

            var abilityConfig = TowerAbilitiesSystem.AbilityConfigs[ability];
            tower.AbilityCooldownTimer = abilityConfig.CooldownTime;
            
            // Hier k�nnten die spezifischen Effekte der F�higkeit implementiert werden
            return true;
        }
    }
}
