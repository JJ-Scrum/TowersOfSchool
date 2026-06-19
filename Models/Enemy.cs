using System.Collections.Generic;
using TowersOfSchool.Models;
using TowersOfSchool.Systems;

namespace TowersOfSchool
{
    /// <summary>
    /// Repr�sentiert einen Gegner im Spiel
    /// </summary>
    public class Enemy
    {
        public string Name { get; set; } = "Enemy";
        public string SpriteImage { get; set; } = "Enemy1.png";
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        public int Tier { get; set; }
        public float CastleDamage { get; set; }
        public double PathProgress { get; set; } // 0.0 bis 1.0
        public int CurrentPathIndex { get; set; }
        public Point CurrentPosition { get; set; }
        public float Speed { get; set; } // Pixel pro Frame
        public bool IsAlive { get; set; }
        private float distanceAlongCurrentSegment = 0f; // Distanz bereits auf diesem Segment zurückgelegt

        public Enemy()
        {
            Health = 100.0f;
            MaxHealth = 100.0f;
            Tier = 1;
            CastleDamage = 2.0f;
            Speed = 100.0f; // Pixel pro Sekunde
            IsAlive = true;
            PathProgress = 0;
            CurrentPathIndex = 0;
            CurrentPosition = new Point(65, 130);
        }

        /// <summary>
        /// Erh�ht den Tier und damit die Statistiken des Gegners
        /// </summary>
        public void SetTier(int newTier)
        {
            Tier = newTier;
            
            switch (Tier)
            {
                case 1:
                    // Standard
                    CastleDamage = 2.0f;
                    break;
                case 2:
                    // 50% mehr Schaden
                    CastleDamage = 2.0f + (2.0f * 0.5f);
                    Health = MaxHealth * 1.2f;
                    break;
                case 3:
                    // 60% mehr Schaden
                    CastleDamage = 2.0f + (2.0f * 0.6f);
                    Health = MaxHealth * 1.5f;
                    break;
                default:
                    CastleDamage = 2.0f + (2.0f * 0.3f * (Tier - 1));
                    Health = MaxHealth * (1.0f + (0.2f * (Tier - 1)));
                    break;
            }
            MaxHealth = Health;
        }

        /// <summary>
        /// Reduziert die Gesundheit des Gegners
        /// </summary>
        public void TakeDamage(float damage)
        {
            Health -= damage;
            if (Health <= 0)
                IsAlive = false;
        }

        /// <summary>
        /// Bewegt den Gegner entlang des Pfads
        /// </summary>
        public void MoveAlongPath(TowersOfSchool.Systems.PathSystem pathSystem, float deltaTime)
        {
            if (!IsAlive || CurrentPathIndex >= pathSystem.PathPoints.Count - 1)
                return;

            // Berechne die Bewegungsdistanz basierend auf Speed und deltaTime
            float distanceToMove = Speed * deltaTime;
            
            System.Diagnostics.Debug.WriteLine($"[MOVE] distanceToMove={distanceToMove:F2}, PathIndex={CurrentPathIndex}, MaxIndex={pathSystem.PathPoints.Count - 1}");

            // Bewege den Gegner schrittweise auf dem Pfad
            while (distanceToMove > 0 && CurrentPathIndex < pathSystem.PathPoints.Count - 1)
            {
                Point currentPoint = pathSystem.PathPoints[CurrentPathIndex];
                Point nextPoint = pathSystem.PathPoints[CurrentPathIndex + 1];
                
                double distanceToNext = currentPoint.DistanceTo(nextPoint);
                
                // Wie viel Distanz ist noch auf diesem Segment übrig?
                float remainingDistanceOnSegment = (float)(distanceToNext - distanceAlongCurrentSegment);
                
                if (distanceToMove >= remainingDistanceOnSegment)
                {
                    // Gehe zum nächsten Punkt
                    distanceToMove -= remainingDistanceOnSegment;
                    CurrentPathIndex++;
                    distanceAlongCurrentSegment = 0f; // Zurücksetzen für das neue Segment
                    
                    if (CurrentPathIndex < pathSystem.PathPoints.Count)
                    {
                        CurrentPosition = pathSystem.PathPoints[CurrentPathIndex];
                    }
                }
                else
                {
                    // Bewege dich teilweise weiter auf diesem Segment
                    distanceAlongCurrentSegment += distanceToMove;
                    double ratio = distanceAlongCurrentSegment / distanceToNext;
                    CurrentPosition = new Point(
                        currentPoint.X + (nextPoint.X - currentPoint.X) * ratio,
                        currentPoint.Y + (nextPoint.Y - currentPoint.Y) * ratio
                    );
                    distanceToMove = 0;
                }
            }

            PathProgress = CurrentPathIndex / (double)(pathSystem.PathPoints.Count - 1);
        }

        /// <summary>
        /// �berpr�ft, ob der Gegner das Ziel erreicht hat
        /// </summary>
        public bool HasReachedGoal(TowersOfSchool.Systems.PathSystem pathSystem)
        {
            return CurrentPathIndex >= pathSystem.PathPoints.Count - 1;
        }
    }
}
