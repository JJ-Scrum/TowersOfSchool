using System.Collections.Generic;

namespace TowersOfSchool.Systems
{
    /// <summary>
    /// Verwaltet spezielle Fähigkeiten für Türme
    /// Basierend auf TextFile1.txt "Spezielle Ideen"
    /// </summary>
    public class TowerAbilitiesSystem
    {
        /// <summary>
        /// Verfügbare Turm-Fähigkeiten
        /// </summary>
        public enum TowerAbility
        {
            BasicAttack,         // Standard-Attacke
            TeleportAttack,      // Gegner wird verlangsamt
            AOEAttack,           // Schaden an mehreren Gegnern
            CriticalStrike,      // Erhöhter Schaden gegen einzelne Gegner
            SlowField,           // Verlangsamt Gegner im Bereich
            DefensiveWall,       // Reduziert Schaden
            ResourceGenerator    // Generiert Extra-Einkommen
        }

        /// <summary>
        /// Tower-Fähigkeits-Konfiguration
        /// </summary>
        public class AbilityConfig
        {
            public TowerAbility AbilityType { get; set; }
            public float CooldownTime { get; set; }     // Sekunden zwischen Aktivierungen
            public float Duration { get; set; }         // Wie lange der Effekt anhält
            public float EffectRadius { get; set; }     // Bereich der Fähigkeit
            public float EffectStrength { get; set; }   // Stärke des Effekts
            public int Cost { get; set; }               // Kosten zu kaufen
        }

        public Dictionary<TowerAbility, AbilityConfig> AbilityConfigs { get; private set; }

        public TowerAbilitiesSystem()
        {
            InitializeAbilities();
        }

        private void InitializeAbilities()
        {
            AbilityConfigs = new Dictionary<TowerAbility, AbilityConfig>
            {
                { 
                    TowerAbility.BasicAttack, 
                    new AbilityConfig 
                    { 
                        AbilityType = TowerAbility.BasicAttack,
                        CooldownTime = 1.0f,
                        Duration = 0.5f,
                        EffectRadius = 100f,
                        EffectStrength = 10f,
                        Cost = 0  // Standard
                    }
                },
                { 
                    TowerAbility.SlowField, 
                    new AbilityConfig 
                    { 
                        AbilityType = TowerAbility.SlowField,
                        CooldownTime = 2.0f,
                        Duration = 5.0f,
                        EffectRadius = 150f,
                        EffectStrength = 0.5f,  // 50% Geschwindigkeitsreduktion
                        Cost = 150
                    }
                },
                { 
                    TowerAbility.AOEAttack, 
                    new AbilityConfig 
                    { 
                        AbilityType = TowerAbility.AOEAttack,
                        CooldownTime = 3.0f,
                        Duration = 1.0f,
                        EffectRadius = 200f,
                        EffectStrength = 15f,  // Schaden
                        Cost = 250
                    }
                },
                { 
                    TowerAbility.CriticalStrike, 
                    new AbilityConfig 
                    { 
                        AbilityType = TowerAbility.CriticalStrike,
                        CooldownTime = 1.5f,
                        Duration = 0.5f,
                        EffectRadius = 100f,
                        EffectStrength = 25f,  // 25 Schaden pro Critical
                        Cost = 200
                    }
                },
                { 
                    TowerAbility.DefensiveWall, 
                    new AbilityConfig 
                    { 
                        AbilityType = TowerAbility.DefensiveWall,
                        CooldownTime = 5.0f,
                        Duration = 10.0f,
                        EffectRadius = 300f,
                        EffectStrength = 0.5f,  // 50% Schadensreduktion
                        Cost = 300
                    }
                },
                { 
                    TowerAbility.ResourceGenerator, 
                    new AbilityConfig 
                    { 
                        AbilityType = TowerAbility.ResourceGenerator,
                        CooldownTime = 3.0f,
                        Duration = 3.0f,
                        EffectRadius = 0f,  // Keine Radius
                        EffectStrength = 5f,  // 5 Einkommen pro Aktivierung
                        Cost = 200
                    }
                }
            };
        }
    }
}
