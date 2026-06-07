using System.Collections.Generic;
using TowersOfSchool.Models;

namespace TowersOfSchool.Systems
{
    /// <summary>
    /// Verwaltet den Pfad, den Gegner folgen.
    /// Der Pfad bildet einen Kreis: Start -> Rechts -> Oben -> Links -> Unten -> Ziel
    /// </summary>
    public class PathSystem
    {
        public List<Point> PathPoints { get; private set; } = new List<Point>();
        private const double TILE_SIZE = 45;

        public PathSystem()
        {
            BuildCirclePath();
        }

        /// <summary>
        /// Baut einen Kreis-Pfad auf dem Spielfeld
        /// Start bei (65, 130) - H�hle
        /// Phase 1: Rechts gehen von X=65 bis X=400, Y=130
        /// Phase 2: Oben gehen von Y=130 bis Y=85, X=400
        /// Phase 3: Links gehen von X=400 bis X=65, Y=85
        /// Phase 4: Unten gehen von Y=85 bis Y=220, X=65
        /// Ziel: (65, 220) - Burg
        /// </summary>
        private void BuildCirclePath()
        {
            // Startpunkt (enemy_spawn)
            double startX = 65;
            double startY = 130;

            // Phase 1: Rechts gehen (x erh�hen, y konstant)
            double rightY = startY;
            for (double x = startX; x <= 400; x += TILE_SIZE * 0.25)
            {
                PathPoints.Add(new Point(x, rightY));
            }

            // Phase 2: Oben gehen (y verringern, x konstant) - starte nach dem letzten Punkt von Phase 1
            double topX = 400;
            for (double y = startY - TILE_SIZE * 0.25; y >= 85; y -= TILE_SIZE * 0.25)
            {
                PathPoints.Add(new Point(topX, y));
            }

            // Phase 3: Links gehen (x verringern, y konstant) - starte nach dem letzten Punkt von Phase 2
            double leftY = 85;
            for (double x = 400 - TILE_SIZE * 0.25; x >= startX; x -= TILE_SIZE * 0.25)
            {
                PathPoints.Add(new Point(x, leftY));
            }

            // Phase 4: Unten gehen (y erh�hen, x konstant) - starte nach dem letzten Punkt von Phase 3
            double bottomX = startX;
            for (double y = 85 + TILE_SIZE * 0.25; y <= 220; y += TILE_SIZE * 0.25)
            {
                PathPoints.Add(new Point(bottomX, y));
            }

            // Finale Zielposition
            PathPoints.Add(new Point(startX, 220));
        }

        /// <summary>
        /// Gibt den n�chsten Punkt auf dem Pfad basierend auf dem aktuellen Index
        /// </summary>
        public Point GetNextPathPoint(int currentPathIndex)
        {
            if (currentPathIndex >= PathPoints.Count)
                return PathPoints[PathPoints.Count - 1];
            return PathPoints[currentPathIndex];
        }

        /// <summary>
        /// Gibt die n�chste Position f�r einen Gegner basierend auf fortschritt zur�ck
        /// </summary>
        public Point GetPositionAlongPath(double progress)
        {
            if (PathPoints.Count == 0)
                return new Point(0, 0);

            progress = System.Math.Max(0, System.Math.Min(1, progress));
            int pointIndex = (int)(progress * (PathPoints.Count - 1));
            return PathPoints[pointIndex];
        }
    }
}
