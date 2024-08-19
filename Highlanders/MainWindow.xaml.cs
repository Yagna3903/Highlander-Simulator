using Highlander_Components.lander;
using Highlander_Component.GameBoard;
using Highlander_Components.GameStimulation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;

namespace Highlanders
{
    public partial class MainWindow : Window
    {
        private GameStimulation gamePlay;
        private List<Highlander> highlanders;
        private IGameBoard<Highlander> gameBoard;
        private bool isSimulationRunning = false;

        public MainWindow()
        {
            InitializeComponent();
            HighlandersSlider.ValueChanged += HighlandersSlider_ValueChanged;
            HighlandersLabel.Text = HighlandersSlider.Value.ToString();
        }

        private void HighlandersSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            HighlandersLabel.Text = ((int)e.NewValue).ToString();
        }

        private void StartSimulation_Click(object sender, RoutedEventArgs e)
        {
            if (isSimulationRunning) return;

            int rows = int.Parse(RowsTextBox.Text);
            int columns = int.Parse(ColumnsTextBox.Text);
            int highlanderCount = (int)HighlandersSlider.Value;

            gameBoard = new GameBoard<Highlander>(rows, columns);
            highlanders = HighlanderFactory.generateRandomHighlanders(highlanderCount, rows, columns);
            gamePlay = new GameStimulation(gameBoard, highlanders);

            isSimulationRunning = true;
            GameBoardCanvas.Children.Clear();
            UpdateCanvas();
        }

        private void NextStep_Click(object sender, RoutedEventArgs e)
        {
            if (isSimulationRunning)
            {
                gamePlay.RunSimulationStep();
                GameBoardCanvas.Children.Clear();
                UpdateCanvas();

                // Check if the simulation has ended
                var aliveHighlanders = highlanders.Where(h => h.IsAlive).ToList();
                if (aliveHighlanders.Count == 1)
                {
                    var lastHighlander = aliveHighlanders.First();
                    if (lastHighlander is BadHighlander)
                    {
                        MessageBox.Show("Game over! Bad Highlander wins.");
                    }
                    else if (lastHighlander is GoodHighlander)
                    {
                        MessageBox.Show("Game over! Good Highlander wins.");
                    }
                    isSimulationRunning = false;
                }
                else if (!highlanders.Any(h => h is BadHighlander && h.IsAlive))
                {
                    MessageBox.Show("Game over! Good Highlanders win.");
                    isSimulationRunning = false;
                }
            }
        }

        private void RestartSimulation_Click(object sender, RoutedEventArgs e)
        {
            // Reset the simulation and UI
            HighlandersSlider.Value = 5;
            RowsTextBox.Text = "5";
            ColumnsTextBox.Text = "5";
            HighlandersLabel.Text = "5";
            GameBoardCanvas.Children.Clear();
            isSimulationRunning = false;
        }

        private void UpdateCanvas()
        {
            DrawGameBoard(gameBoard.Rows, gameBoard.Columns);

            foreach (var highlander in highlanders)
            {
                if (highlander.IsAlive)
                {
                    var (row, col) = highlander.GetPosition();
                    double cellWidth = GameBoardCanvas.ActualWidth / gameBoard.Columns;
                    double cellHeight = GameBoardCanvas.ActualHeight / gameBoard.Rows;

                    Ellipse ellipse = new Ellipse
                    {
                        Width = cellWidth * 0.8, // Adjust size as needed
                        Height = cellHeight * 0.8, // Adjust size as needed
                        Fill = highlander is GoodHighlander ? Brushes.Green : Brushes.Red
                    };

                    Canvas.SetLeft(ellipse, col * cellWidth + (cellWidth - ellipse.Width) / 2);
                    Canvas.SetTop(ellipse, row * cellHeight + (cellHeight - ellipse.Height) / 2);
                    GameBoardCanvas.Children.Add(ellipse);
                }
            }
        }

        private void DrawGameBoard(int rows, int columns)
        {
            double cellWidth = GameBoardCanvas.ActualWidth / columns;
            double cellHeight = GameBoardCanvas.ActualHeight / rows;

            // Draw vertical grid lines
            for (int col = 0; col <= columns; col++)
            {
                Line line = new Line
                {
                    X1 = col * cellWidth,
                    Y1 = 0,
                    X2 = col * cellWidth,
                    Y2 = GameBoardCanvas.ActualHeight,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                GameBoardCanvas.Children.Add(line);
            }

            // Draw horizontal grid lines
            for (int row = 0; row <= rows; row++)
            {
                Line line = new Line
                {
                    X1 = 0,
                    Y1 = row * cellHeight,
                    X2 = GameBoardCanvas.ActualWidth,
                    Y2 = row * cellHeight,
                    Stroke = Brushes.Black,
                    StrokeThickness = 1
                };
                GameBoardCanvas.Children.Add(line);
            }
        }
    }
}
