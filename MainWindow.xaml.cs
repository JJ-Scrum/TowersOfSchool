using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;
using System.Linq;
using TowersOfSchool.Systems;

namespace TowersOfSchool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameController gameController;
        private DispatcherTimer gameLoop;
        private Stopwatch frameTimer;
        private double fps = 60;
        private int frameCount = 0;
        private double lastFpsUpdate = 0;
        private bool isPlacingTower = false;
        private TowerSlot selectedSlot = null;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            gameController = new GameController();
            frameTimer = Stopwatch.StartNew();
            
            // Zeichne initiale Pfad-Elemente
            DrawPath();
            DrawTowerSlots();
            DrawCastleAndSpawn();

            // Starte erste Welle
            gameController.WaveSystem.StartNextWave();

            // Starte Game Loop
            gameLoop = new DispatcherTimer();
            gameLoop.Interval = System.TimeSpan.FromMilliseconds(16); // ~60 FPS
            gameLoop.Tick += GameLoop_Tick;
            gameLoop.Start();

            // Tower buy button - direkter Zugriff
            if (tower_buy_btn != null)
            {
                tower_buy_btn.Click += Tower_Buy_Click;
            }

            // Canvas Click Handler für Tower Platzierung
            gameCanvas.MouseLeftButtonDown += GameCanvas_MouseLeftButtonDown;
        }

        private void GameLoop_Tick(object sender, System.EventArgs e)
        {
            gameController.Update(0.016f); // 16ms = ~60 FPS
            UpdateCanvas();
            UpdateUI();

            // FPS Berechnung
            frameCount++;
            double elapsed = frameTimer.ElapsedMilliseconds / 1000.0;
            if (elapsed - lastFpsUpdate >= 1.0)
            {
                fps = frameCount / (elapsed - lastFpsUpdate);
                lastFpsUpdate = elapsed;
                frameCount = 0;
            }
            
            // DEBUG
            if (frameCount % 60 == 0)
            {
                System.Diagnostics.Debug.WriteLine($"Gegner: {gameController.WaveSystem.CurrentEnemies.Count}, Welle: {gameController.WaveSystem.CurrentWave}");
            }
        }

        private void DrawPath()
        {
            // Gegner-Pfad zeichnen (basierend auf PathSystem)
            var pathPoints = gameController.PathSystem.PathPoints;

            if (pathPoints.Count > 1)
            {
                for (int i = 0; i < pathPoints.Count - 1; i++)
                {
                    var p1 = pathPoints[i];
                    var p2 = pathPoints[i + 1];
                    
                    var line = new Line
                    {
                        X1 = p1.X,
                        Y1 = p1.Y,
                        X2 = p2.X,
                        Y2 = p2.Y,
                        Stroke = Brushes.SaddleBrown,
                        StrokeThickness = 3
                    };
                    line.SetValue(NameProperty, "path_line");
                    Canvas.SetZIndex(line, 0);
                    gameCanvas.Children.Add(line);
                }
            }

            // Zeichne Pfad-Punkte als kleine Kreise
            foreach (var point in pathPoints)
            {
                var circle = new Ellipse
                {
                    Width = 10,
                    Height = 10,
                    Fill = Brushes.SaddleBrown,
                    Stroke = Brushes.Black,
                    StrokeThickness = 0.5
                };
                circle.SetValue(NameProperty, "path_line");
                Canvas.SetLeft(circle, point.X - 5);
                Canvas.SetTop(circle, point.Y - 5);
                Canvas.SetZIndex(circle, 0);
                gameCanvas.Children.Add(circle);
            }
        }

        private void DrawTowerSlots()
        {
            // Zeichne verfügbare Tower-Slots mit Tag für Klick-Erkennung
            var slots = gameController.TowerPlacementSystem.AvailableSlots;
            int slotIndex = 0;
            foreach (var slot in slots)
            {
                // Leere Slots als helle Kreise, besetzte als dunkle
                var circle = new Ellipse
                {
                    Width = slot.Size,
                    Height = slot.Size,
                    Fill = slot.IsOccupied ? Brushes.Red : Brushes.LightGray,
                    Stroke = Brushes.DarkGray,
                    StrokeThickness = 2,
                    Opacity = 0.6,
                    Tag = slotIndex,
                    Cursor = System.Windows.Input.Cursors.Hand
                };
                circle.SetValue(NameProperty, "slot_visual");
                circle.MouseLeftButtonDown += SlotCircle_MouseLeftButtonDown;

                Canvas.SetLeft(circle, slot.Position.X - slot.Size / 2);
                Canvas.SetTop(circle, slot.Position.Y - slot.Size / 2);
                Canvas.SetZIndex(circle, 2);
                gameCanvas.Children.Add(circle);
                slotIndex++;
            }
        }

        private void DrawCastleAndSpawn()
        {
            // Spawn Punkt
            var spawnCircle = new Ellipse
            {
                Width = 40,
                Height = 40,
                Fill = Brushes.SaddleBrown,
                Stroke = Brushes.Black,
                StrokeThickness = 2
            };
            spawnCircle.SetValue(NameProperty, "spawn_visual");
            Canvas.SetLeft(spawnCircle, 65 - 20);
            Canvas.SetTop(spawnCircle, 130 - 20);
            Canvas.SetZIndex(spawnCircle, 1);
            gameCanvas.Children.Add(spawnCircle);

            // Spawn Text
            var spawnText = new TextBlock
            {
                Text = "SPAWN",
                FontSize = 10,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold
            };
            spawnText.SetValue(NameProperty, "spawn_visual");
            Canvas.SetLeft(spawnText, 65 - 20);
            Canvas.SetTop(spawnText, 130 + 5);
            Canvas.SetZIndex(spawnText, 2);
            gameCanvas.Children.Add(spawnText);

            // Castle/Destination
            var castleRect = new Rectangle
            {
                Width = 60,
                Height = 60,
                Fill = Brushes.Gold,
                Stroke = Brushes.DarkGoldenrod,
                StrokeThickness = 3
            };
            castleRect.SetValue(NameProperty, "castle_visual");
            Canvas.SetLeft(castleRect, 65 - 30);
            Canvas.SetTop(castleRect, 220 - 30);
            Canvas.SetZIndex(castleRect, 1);
            gameCanvas.Children.Add(castleRect);

            // Castle Text
            var castleText = new TextBlock
            {
                Text = "CASTLE",
                FontSize = 12,
                Foreground = Brushes.White,
                FontWeight = FontWeights.Bold
            };
            castleText.SetValue(NameProperty, "castle_visual");
            Canvas.SetLeft(castleText, 65 - 28);
            Canvas.SetTop(castleText, 220 - 10);
            Canvas.SetZIndex(castleText, 3);
            gameCanvas.Children.Add(castleText);
        }

        private void UpdateCanvas()
        {
            // Lösche alte Gegner- und Tower-Zeichnungen (aber nicht den Pfad und Slots)
            var toRemove = gameCanvas.Children
                .OfType<UIElement>()
                .Where(x => {
                    var name = (string)x.GetValue(NameProperty);
                    return name == "enemy_visual" || name == "tower_visual";
                })
                .ToList();

            foreach (var item in toRemove)
            {
                gameCanvas.Children.Remove(item);
            }

            // Zeichne alle Gegner
            foreach (var enemy in gameController.WaveSystem.CurrentEnemies)
            {
                if (enemy.IsAlive)
                {
                    var circle = new Ellipse
                    {
                        Width = 15,
                        Height = 15,
                        Fill = Brushes.Red,
                        Stroke = Brushes.DarkRed,
                        StrokeThickness = 1
                    };
                    circle.SetValue(NameProperty, "enemy_visual");

                    Canvas.SetLeft(circle, enemy.CurrentPosition.X - 7.5);
                    Canvas.SetTop(circle, enemy.CurrentPosition.Y - 7.5);
                    Canvas.SetZIndex(circle, 10);
                    gameCanvas.Children.Add(circle);

                    // Health Bar
                    double healthPercent = (double)enemy.Health / enemy.MaxHealth;
                    var healthBar = new Rectangle
                    {
                        Width = 15 * healthPercent,
                        Height = 3,
                        Fill = healthPercent > 0.5 ? Brushes.LimeGreen : healthPercent > 0.2 ? Brushes.Yellow : Brushes.Red
                    };
                    healthBar.SetValue(NameProperty, "enemy_visual");

                    Canvas.SetLeft(healthBar, enemy.CurrentPosition.X - 7.5);
                    Canvas.SetTop(healthBar, enemy.CurrentPosition.Y - 12);
                    Canvas.SetZIndex(healthBar, 11);
                    gameCanvas.Children.Add(healthBar);
                }
            }

            // Zeichne alle Türme
            foreach (var tower in gameController.PlacedTowers)
            {
                var rect = new Rectangle
                {
                    Width = 25,
                    Height = 25,
                    Fill = tower.IsPaused ? Brushes.Gray : Brushes.Blue,
                    Stroke = Brushes.DarkBlue,
                    StrokeThickness = 2
                };
                rect.SetValue(NameProperty, "tower_visual");

                Canvas.SetLeft(rect, tower.Position.X - 12.5);
                Canvas.SetTop(rect, tower.Position.Y - 12.5);
                Canvas.SetZIndex(rect, 5);
                gameCanvas.Children.Add(rect);

                // Tower Range Kreis (schwach sichtbar)
                var rangeCircle = new Ellipse
                {
                    Width = tower.Range * 2,
                    Height = tower.Range * 2,
                    Stroke = Brushes.CornflowerBlue,
                    StrokeThickness = 0.5,
                    Opacity = 0.3
                };
                rangeCircle.SetValue(NameProperty, "tower_visual");

                Canvas.SetLeft(rangeCircle, tower.Position.X - tower.Range);
                Canvas.SetTop(rangeCircle, tower.Position.Y - tower.Range);
                Canvas.SetZIndex(rangeCircle, 1);
                gameCanvas.Children.Add(rangeCircle);
            }
        }

        private void UpdateUI()
        {
            // Aktualisiere Geld-Anzeige
            money_display.Text = $"Money: {gameController.EconomySystem.PlayerMoney}";

            // Aktualisiere Schloss-Gesundheit
            castle_health_display.Text = $"Castle HP: {gameController.CastleHealth}";

            // Aktualisiere Wellen-Info
            wave_display.Text = $"Wave: {gameController.WaveSystem.CurrentWave}";

            // Aktualisiere Gegner-Anzahl
            enemies_display.Text = $"Enemies: {gameController.WaveSystem.CurrentEnemies.Count}";

            // FPS
            fps_display.Text = $"FPS: {fps:F1}";

            // Tower Info
            tower_info.Text = $"Towers placed: {gameController.PlacedTowers.Count}";

            // Wenn Spiel vorbei ist
            if (gameController.IsGameOver)
            {
                wave_display.Text = "GAME OVER!";
                gameLoop.Stop();
            }
        }

        private void Tower_Buy_Click(object sender, RoutedEventArgs e)
        {
            isPlacingTower = !isPlacingTower;
            if (isPlacingTower)
            {
                tower_buy_btn.Background = Brushes.LightGreen;
                tower_buy_btn.Content = "Click on slot to place...";
            }
            else
            {
                tower_buy_btn.Background = SystemColors.ControlBrush;
                tower_buy_btn.Content = "Buy Tower (100)";
                selectedSlot = null;
            }
        }

        private void SlotCircle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!isPlacingTower)
                return;

            var circle = sender as Ellipse;
            if (circle?.Tag is int slotIndex)
            {
                var slots = gameController.TowerPlacementSystem.GetAvailableSlots();
                if (slotIndex >= 0 && slotIndex < slots.Count)
                {
                    selectedSlot = slots[slotIndex];
                    PlaceTowerAtSlot(selectedSlot);
                }
            }
            e.Handled = true;
        }

        private void GameCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Wenn nicht im PlacingMode, ignorieren
            if (!isPlacingTower)
                return;

            var clickPos = e.GetPosition(gameCanvas);
            
            // Suche nach dem nächsten Slot zur Click-Position
            var slots = gameController.TowerPlacementSystem.GetAvailableSlots();
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                double distance = System.Math.Sqrt(
                    (clickPos.X - slot.Position.X) * (clickPos.X - slot.Position.X) +
                    (clickPos.Y - slot.Position.Y) * (clickPos.Y - slot.Position.Y)
                );
                
                // Wenn nah genug am Slot (30 Pixel Toleranz)
                if (distance <= 30)
                {
                    PlaceTowerAtSlot(slot);
                    e.Handled = true;
                    return;
                }
            }
        }

        private void PlaceTowerAtSlot(TowerSlot slot)
        {
            if (slot.IsOccupied)
            {
                MessageBox.Show("Dieser Slot ist bereits belegt!");
                return;
            }

            var tower = new Tower
            {
                Name = $"Tower_{gameController.PlacedTowers.Count}",
                SpriteImage = "tower.png"
            };

            if (gameController.PlaceTower(new TowersOfSchool.Models.Point(slot.Position.X, slot.Position.Y), tower))
            {
                MessageBox.Show($"Turm platziert! ({gameController.EconomySystem.PlayerMoney}$ übrig)");
                isPlacingTower = false;
                tower_buy_btn.Background = SystemColors.ControlBrush;
                tower_buy_btn.Content = "Buy Tower (100)";
            }
            else
            {
                MessageBox.Show("Nicht genug Geld!");
            }
        }
    }
}

