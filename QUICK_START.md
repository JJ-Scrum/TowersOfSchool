# Towers of School - Quick Start Guide

## ?? Projekt-Struktur

```
TowersOfSchool/
??? Models/
?   ??? Point.cs              # Punkt-Struktur mit Distanzen-Berechnung
?   ??? Enemy.cs              # Gegner-Klasse mit Bewegungs-Logik
?   ??? Tower.cs              # Turm-Klasse mit Upgrades & Fähigkeiten
??? Systems/
?   ??? GameController.cs          # Haupt-Game-Loop
?   ??? PathSystem.cs              # Kreis-Pfad für Gegner
?   ??? TowerPlacementSystem.cs     # Slot-Management für Türme
?   ??? WaveSystem.cs              # Gegner-Wellen & Spawning
?   ??? EconomySystem.cs           # Geld & Finanzamt
?   ??? TaxOfficeSystem.cs         # Lohnschein-Verwaltung
?   ??? PrestigeSystem.cs          # Permanente Upgrades (Diamanten)
?   ??? TowerAbilitiesSystem.cs     # Tower-Fähigkeiten
?   ??? EnemyEffectsSystem.cs       # Gegner-Effekte
?   ??? SpecialEnemySystem.cs       # Spezielle Gegner (z.B. Herr Baumstamm)
??? MainWindow.xaml                 # UI-Layout
??? MainWindow.xaml.cs              # UI-Logic & Game-Loop Handler
??? App.xaml / App.xaml.cs          # WPF App-Einstieg
??? Upgrades.cs                     # Upgrade-Listen
??? spawn.cs                        # Legacy-Spawn-Logik
??? IMPLEMENTATION_SUMMARY.md       # Detaillierte Feature-Dokumentation
??? LAYOUT_COORDINATES.md           # Koordinaten-Referenz
```

## ?? Spielmechaniken

### Gegner-System
1. **Spawning**: Gegner spawnen alle 1 Sekunde am `enemy_spawn` (Höhle)
2. **Bewegung**: Gegner folgen dem Kreis-Pfad (rechts ? oben ? links ? unten)
3. **Speed**: 2 Pixel pro Frame
4. **Tiers**: Gegner werden mit jeder Welle stärker
5. **Ziel**: Gegner verursachen Schaden am Schloss beim Erreichen von `enemy_dest`

### Tower-System
1. **Platzierung**: Türme können nur an festen Slots neben dem Pfad platziert werden
2. **Kosten**: Standard Tower kostet 100 Geld
3. **Verkauf**: Türme können für 50% des Kaufpreises verkauft werden
4. **Upgrades**: 9 verschiedene Upgrades möglich (Damage I-III, Range I-III, Luck I-III)
5. **Fähigkeiten**: 6 spezielle Fähigkeiten (SlowField, AOE, etc.)
6. **Pause**: Können von Herr Baumstamm für 10 Minuten pausiert werden

### Wirtschafts-System
1. **Geld**: Spieler erhält Lohnscheine statt direkt Geld
2. **Steuern**: 15% Standard-Steuersatz beim Einlösen
3. **Diamanten**: Verdient durch Wellen, ausgegeben für Prestige-Upgrades
4. **Prestige**: 7 verschiedene dauerhafte Upgrades möglich

### Gegner-Effekte
- **Herr Baumstamm** (0.1% Chance): Pausiert nächsten Turm für 10 Minuten
- **StrictTeacher**: Spawnt extra Gegner
- **TechTeacher**: Verursacht Netzwerk-Probleme (-Geschwindigkeit)

## ?? Wichtigste Code-Entry-Points

### Game Loop starten
```csharp
// MainWindow.xaml.cs
gameController = new GameController();
gameLoop = new DispatcherTimer();
gameLoop.Tick += GameLoop_Tick;  // Ruft gameController.Update() auf
```

### Gegner bewegen
```csharp
// GameController.Update()
enemy.MoveAlongPath(PathSystem, deltaTime);
if (enemy.HasReachedGoal(PathSystem))
{
    CastleHealth -= (int)enemy.CastleDamage;
}
```

### Turm schießen
```csharp
// GameController.Update()
if (tower.CanShoot(target, deltaTime))
{
    target.TakeDamage(tower.CurrentDamage);
}
```

### Turm platzieren
```csharp
// GameController.PlaceTower()
var slot = TowerPlacementSystem.TryPlaceTower(position);
if (slot != null && EconomySystem.TryBuyTower(cost))
{
    tower.Slot = slot;
    PlacedTowers.Add(tower);
}
```

## ?? Wichtige Konstanten

```csharp
// Gegner
Enemy.Speed = 2.0f;                    // Pixel pro Frame
Enemy.CastleDamage = 2.0f;            // Standardschaden
WaveSystem.SpawnInterval = 1.0f;      // Sekunden zwischen Spawns

// Türme
Tower.BaseDamage = 10.0f;
Tower.Range = 100.0f;
Tower.FireRate = 1.0f;                // Schüsse pro Sekunde
Tower.Cost = 100;

// Pfad
PathSystem.TILE_SIZE = 45;
PathSystem.Kreis-Umfang ? 1000 Pixel   // 4 Phasen à ~250px

// Prestige
PrestigeSystem min. 40 Diamanten für schwächstes Upgrade
PrestigeSystem max. 100 Diamanten für ExtraStartingMoney

// Finanzamt
TaxOfficeSystem.TaxRate = 15%          // Standard-Steuersatz
```

## ?? Für die Weiterentwicklung

### Visuelle Elemente hinzufügen
1. Gegner-Rendering basierend auf `enemy.CurrentPosition`
2. Tower-Rendering an `tower.Position` (von `tower.Slot.Position`)
3. Health-Bars für Gegner und Schloss

### Interaktivität hinzufügen
1. Maus-Klick-Handling in MainWindow.xaml.cs
2. Durchlauf PathSystem.GetAvailableSlots() um freie Plätze zu zeigen
3. Tower-Shop UI für Türme kaufen/verkaufen/upgraden

### Effekt-Systeme
1. Visuelle Effekte für Tower-Fähigkeiten
2. Audio für Explosionen, Kills, etc.
3. Animations für Gegner-Tier-Upgrades

### Spezielle Gegner implementieren
1. HerBaumstamm aktivieren in WaveSystem
2. Tower-Pause in GameController (bereits implementiert)
3. UI für Pause-Status anzeigen

## ?? UI-Updates bereits vorhanden

- Money Display
- Castle Health
- Wave Counter
- Enemy Counter
- Game Over Status

Alle werden automatisch aktualisiert in `MainWindow.xaml.cs::UpdateUI()`

## ? Checkliste für erste funktionierende Demo

- [ ] Gegner spawnen und auf Pfad bewegen
- [ ] Gegner-Render an Position
- [ ] Türme platzieren an Slots
- [ ] Tower-Render an Position
- [ ] Türme schießen auf Gegner
- [ ] Gegner sterben wenn Hit Points = 0
- [ ] Geld-System funktioniert
- [ ] Wellen erhöhen sich
- [ ] Schloss nimmt Schaden bei Gegner-Ankunft

---

**Projekt Status**: ? Alle Systeme implementiert und compiliert
**Nächster Schritt**: Visuelle Elemente und Interaktivität hinzufügen
