using System;

namespace TowersOfSchool.Systems
{
    /// <summary>
    /// Verwaltet spezielle Gegner und ihre F�higkeiten
    /// Insbesondere: Herr Baumstamm (der schlimmste Lehrer) mit seiner spezialen F�higkeit
    /// </summary>
    public class SpecialEnemySystem
    {
        private Random random = new Random();

        /// <summary>
        /// Typen von speziellen Gegnern
        /// </summary>
        public enum SpecialEnemyType
        {
            Regular,
            HerBaumstamm,     // Der schlimmste Lehrer - hat speziale F�higkeit
            StrictTeacher,     // Erzeugt zus�tzliche Gegner
            TechTeacher        // Verursacht Netzwerk-Probleme
        }

        /// <summary>
        /// Pr�ft, ob Herr Baumstamm einen Turm pausiert
        /// 0.1% Wahrscheinlichkeit
        /// </summary>
        public Tower? TryActivateBaumstammAbility(System.Collections.Generic.List<Tower> towers)
        {
            if (towers.Count == 0)
                return null;

            // 0.1% Chance = 0.001
            if (random.NextDouble() < 0.001)
            {
                // W�hle den n�chsten Turm
                Tower closestTower = towers[0];
                return closestTower;
            }

            return null;
        }

        /// <summary>
        /// Gibt die Wahrscheinlichkeit an, dass ein spezieller Gegner spawnt
        /// </summary>
        public SpecialEnemyType DetermineSpecialEnemyType(int waveNumber)
        {
            double rand = random.NextDouble();

            // Ab Welle 3: 5% Chance auf einen speziellen Gegner
            if (waveNumber >= 3)
            {
                if (rand < 0.05) // 5%
                {
                    // Ab Welle 5: 2% Chance auf Herr Baumstamm
                    if (waveNumber >= 5 && random.NextDouble() < 0.4) // 2% von den 5%
                    {
                        return SpecialEnemyType.HerBaumstamm;
                    }

                    if (random.NextDouble() < 0.5)
                        return SpecialEnemyType.StrictTeacher;
                    else
                        return SpecialEnemyType.TechTeacher;
                }
            }

            return SpecialEnemyType.Regular;
        }
    }
}
