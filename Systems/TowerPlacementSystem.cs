using System.Collections.Generic;
using TowersOfSchool.Models;

namespace TowersOfSchool.Systems
{
    /// <summary>
        /// Verwaltet alle m�glichen Turm-Platzierungspositionen.
        /// T�rme d�rfen nur neben den Pfad-Kacheln platziert werden.
        /// Jeder Turm belegt einen Bereich der Gr��e eines Path-Tiles.
        /// </summary>
        public class TowerPlacementSystem
        {
            private const double TILE_SIZE = 45;
            private const double GRID_OFFSET_X = 30;
            private const double OFFSET_Y_START = 105;
            private const double PATH_WIDTH = 438; // Breite des Spielfeldes (ohne rechte UI-Spalte)

            public List<TowerSlot> AvailableSlots { get; private set; } = new List<TowerSlot>();

            public TowerPlacementSystem()
            {
                InitializeTowerSlots();
            }

            /// <summary>
            /// Initialisiert alle verf�gbaren Turm-Platzierungspositionen
            /// Die Slots sind in einem Kreis angeordnet, neben dem Pfad
            /// </summary>
            private void InitializeTowerSlots()
            {
                // Phase 1: Horizontale rechts Phase - oben und unten vom Pfad
                // Pfad: Y von 105 bis 285 (5 Tiles � 45px)
                for (int i = 0; i < 5; i++)
                {
                    double slotY = OFFSET_Y_START + (i * TILE_SIZE) + TILE_SIZE / 2;
                    
                    // Slot oben vom horizontalen Segment
                    AvailableSlots.Add(new TowerSlot
                    {
                        Position = new Point(GRID_OFFSET_X + PATH_WIDTH / 2, slotY - TILE_SIZE - 10),
                        IsOccupied = false,
                        Size = TILE_SIZE
                    });

                    // Slot unten vom horizontalen Segment
                    AvailableSlots.Add(new TowerSlot
                    {
                        Position = new Point(GRID_OFFSET_X + PATH_WIDTH / 2, slotY + TILE_SIZE + 10),
                        IsOccupied = false,
                        Size = TILE_SIZE
                    });
                }

                // Phase 2: Vertikale Segmente - links und rechts
                // Links vom Kreis (beim Start)
                for (int i = 0; i < 3; i++)
                {
                    double slotX = GRID_OFFSET_X - TILE_SIZE - 10;
                    double slotY = OFFSET_Y_START + (i * TILE_SIZE) + TILE_SIZE / 2;

                    AvailableSlots.Add(new TowerSlot
                    {
                        Position = new Point(slotX, slotY),
                        IsOccupied = false,
                        Size = TILE_SIZE
                    });
                }

                // Rechts vom Kreis (beim Ende)
                for (int i = 0; i < 3; i++)
                {
                    double slotX = GRID_OFFSET_X + PATH_WIDTH + TILE_SIZE + 10;
                    double slotY = OFFSET_Y_START + (i * TILE_SIZE) + TILE_SIZE / 2;

                    AvailableSlots.Add(new TowerSlot
                    {
                        Position = new Point(slotX, slotY),
                        IsOccupied = false,
                        Size = TILE_SIZE
                    });
                }

                // Phase 3: Oben und unten vom Kreis
                // Oben (bei X von 65 bis 400)
                for (int i = 0; i < 4; i++)
                {
                    double slotX = GRID_OFFSET_X + (i * TILE_SIZE * 2.5) + TILE_SIZE;
                    double slotY = OFFSET_Y_START - TILE_SIZE - 10;

                    AvailableSlots.Add(new TowerSlot
                    {
                        Position = new Point(slotX, slotY),
                        IsOccupied = false,
                        Size = TILE_SIZE
                    });
                }

                // Unten (bei X von 65 bis 400)
                for (int i = 0; i < 4; i++)
                {
                    double slotX = GRID_OFFSET_X + (i * TILE_SIZE * 2.5) + TILE_SIZE;
                    double slotY = OFFSET_Y_START + (TILE_SIZE * 5) + 10;

                    AvailableSlots.Add(new TowerSlot
                    {
                        Position = new Point(slotX, slotY),
                        IsOccupied = false,
                        Size = TILE_SIZE
                    });
                }
            }

        /// <summary>
        /// Versucht, einen Turm an der gegebenen Position zu platzieren
        /// </summary>
        public TowerSlot? TryPlaceTower(Point position, double tolerance = 20)
        {
            foreach (var slot in AvailableSlots)
            {
                if (!slot.IsOccupied && slot.Position.DistanceTo(position) <= tolerance)
                {
                    slot.IsOccupied = true;
                    return slot;
                }
            }
            return null;
        }

        /// <summary>
        /// Gibt einen Turm-Slot frei
        /// </summary>
        public void RemoveTower(TowerSlot slot)
        {
            if (slot != null)
                slot.IsOccupied = false;
        }

        /// <summary>
        /// Gibt alle verf�gbaren (freien) Slots zur�ck
        /// </summary>
        public List<TowerSlot> GetAvailableSlots()
        {
            var available = new List<TowerSlot>();
            foreach (var slot in AvailableSlots)
            {
                if (!slot.IsOccupied)
                    available.Add(slot);
            }
            return available;
        }
    }

    public class TowerSlot
    {
        public Point Position { get; set; }
        public bool IsOccupied { get; set; }
        public double Size { get; set; }
    }
}
