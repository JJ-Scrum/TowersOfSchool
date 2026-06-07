# Towers of School - Layout & Koordinaten Referenz

## ??? Spielfeld-Layout

```
??????????????????????????????????????????????????????
?                                         ?          ?
?  Game Area                              ?   UI     ?
?  (543px breit × 450px hoch)            ? Panel    ?
?                                         ?          ?
??????????????????????????????????????????????????????
  Spalte 1               Spalte 2         Spalte 3
  (3px)               (543px)            (254px)
```

## ?? Kreis-Pfad Details

### Start & Ende Positionen
- **Start (enemy_spawn)**: X=65, Y=130
- **Ende (enemy_dest)**: X=65, Y=220

### Kreis-Phasen

```
         ????????????
         ?   Phase 2 ?
         ?  (Oben)   ?
         ?           ?
    Phase 1  ???????????????  Phase 3
    (Rechts) ?             ?  (Links)
         X   ?             ?
(65,130)?????             ?
         ?   ???????????????  (400, 85)
         ?   Phase 4
         ?   (Unten)
         ?
    (65,220)
```

### Koordinaten-Bereiche

| Phase | Achse | Von | Bis | Konstante |
|-------|-------|-----|-----|-----------|
| 1 (Rechts) | X | 65 | 400 | Y=130 |
| 2 (Oben) | Y | 130 | 85 | X=400 |
| 3 (Links) | X | 400 | 65 | Y=85 |
| 4 (Unten) | Y | 85 | 220 | X=65 |

## ?? Path-Tiles Positionen

```
path_tile1: Y=105, Height=45
path_tile2: Y=150, Height=45
path_tile3: Y=195, Height=45
path_tile4: Y=240, Height=45
path_tile5: Y=285, Height=45
```

Alle Path-Tiles: `Margin="30,Y,468,Z"` wobei Y die Y-Position und Z wird automatisch berechnet.

## ?? Tower-Slot Positionen

### Horizontale Slots (oben/unten von Pfad)
- **Oben**: Y = PathTile.Y - 55 (10px Abstand + 45px Tile)
- **Unten**: Y = PathTile.Y + 55

### Vertikale Slots (links/rechts)
- **Links**: X = 30 - 55 = -25
- **Rechts**: X = 30 + 438 + 55 = 523

## ?? UI Panel Positionen

```
????????????????????????
? Player Info          ? (X=10, Y=10)
????????????????????????
? Money: 200           ?
? Castle HP: 20        ?
? Wave: 0              ?
? Enemies: 0           ?
????????????????????????
```

## ?? Dekoration Positionen

**tree1**: `Margin="271,186,184,138"` (Mitte des Spielfeldes)

## ?? Berechnung der Y-Margin Werte

Für **Bottom-Margin** (Rechts bei Margin):

```
BottomMargin = TotalHeight - TopMargin - ElementHeight
BottomMargin = 450 - TopMargin - 45
```

Beispiel für path_tile1 (TopMargin=105):
```
BottomMargin = 450 - 105 - 45 = 300
Aber: Margin="30,105,468,284" ? 284 (andere Berechnung basierend auf Spalten-Width)
```

## ?? Konstanten im Code

```csharp
// PathSystem
private const double TILE_SIZE = 45;

// TowerPlacementSystem
private const double TILE_SIZE = 45;
private const double GRID_OFFSET_X = 30;
private const double OFFSET_Y_START = 105;
private const double PATH_WIDTH = 438;  // 800 - 30 - 332

// Pfad-Punkte werden in 0.25 TILE_SIZE Schritten hinzugefügt
for (double x = startX; x <= 400; x += TILE_SIZE * 0.25)
```

## ?? Distanzen & Abstände

- **Zwischen Path-Tiles**: 45px (ein Tile-Size)
- **Turm zu Pfad Abstand**: 10px (+ 45px für Slot-Größe)
- **Gegner-Spawn zu erstem Pfad**: X=65, Y=130
- **Letzter Pfad zu Ziel**: X=65, Y=220

## ?? Wichtige Referenzen für Neue Features

```csharp
// Gegner-Position aus Pfad
public Point GetPositionAlongPath(double progress)  // progress 0.0 bis 1.0

// Turm-Platzierung
public TowerSlot TryPlaceTower(Point position, double tolerance = 20)

// Gegner-Bewegung
public void MoveAlongPath(PathSystem pathSystem, float deltaTime)

// Distanzen-Berechnung
public double DistanceTo(Point other)
```

---

**Letztes Update**: Bei Kreis-Pfad Implementierung
