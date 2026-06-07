using System;
using System.Collections.Generic;

namespace TowersOfSchool.Systems
{
    /// <summary>
    /// Verwaltet Effekte, die von Gegnern (bösen Lehrern) verursacht werden
    /// </summary>
    public class EnemyEffectsSystem
    {
        /// <summary>
        /// Mögliche negative Effekte von Gegnern:
        /// - Unangekündigte Tests
        /// - 4h Klassenarbeiten
        /// - Handygarage
        /// - Vertretung 7/8 Stunde
        /// - KI-generierte Aufgaben
        /// - Schlechte Noten
        /// - Arbeiten nach Schule (Nachschreiben)
        /// </summary>
        public enum EnemyAttackType
        {
            UnannounceTest,      // 10% Timeout auf Türme
            LongExam,            // Reduziert Einkommen
            PhoneGarage,         // Reduziert Turm-Range
            SubstitutionClass,   // Reduziert Turm-Schaden
            AIGeneratedTasks,    // Langsame Gegner-Bewegung
            BadGrades,           // Nächste Welle wird stärker
            AfterSchoolRetake    // Gegner heilt sich selbst
        }

        /// <summary>
        /// Spezial-Gegner: Herr Baumstamm (der schlimmste Lehrer)
        /// Hat 0.1% Chance, einen Turm für 10 Minuten zu pausieren
        /// </summary>
        public class HerBaumstammAbility
        {
            public float TowerPauseProbability { get; set; } = 0.001f; // 0.1%
            public float PauseDuration { get; set; } = 600.0f; // 10 Minuten in Sekunden
        }

        /// <summary>
        /// Positive Effekte von guten Lehrern (Tower):
        /// - 1-2 Stunde Frei (Erhöhte Turm-Effektivität)
        /// - Gemeinsames Frühstück (Schnelleres Einkommen)
        /// </summary>
        public enum TowerBenefitType
        {
            FreeHour,            // +25% Schaden und +25% Range für 60 Sekunden
            SharedBreakfast      // +50% Einkommen für 30 Sekunden
        }

        /// <summary>
        /// Umgebungs-Effekte:
        /// Negativ: Schlechtes Netz, kein Netz, kein Strom, Heizung immer an
        /// Positiv: Gutes Netz, Mobiler Hotspot
        /// </summary>
        public enum EnvironmentEffect
        {
            // Negative
            PoorNetwork,         // -20% Turm-Geschwindigkeit
            NoNetwork,           // -50% Turm-Geschwindigkeit
            NoPower,             // -30% Turm-Schaden
            ExcessiveHeating,    // Gegner erhalten +10% Geschwindigkeit

            // Positive
            GoodNetwork,         // +20% Turm-Geschwindigkeit
            MobileHotspot        // +50% Turm-Geschwindigkeit
        }
    }
}
