using Highlander_Component.GameBoard;
using Highlander_Components.GameStimulation;
using Highlander_Components.lander;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            InitializeGameBoardGrid(rows, columns);
            UpdateGrid(gameBoard);
        }

        private async void NextStep_Click(object sender, RoutedEventArgs e)
        {
            if (isSimulationRunning)
            {
                gamePlay.RunSimulationStep();
                UpdateGrid(gameBoard);

                await Task.Delay(500); // Delay for 500 milliseconds 

                gamePlay.HandleInteractions();
                UpdateGrid(gameBoard);

                // Check if the simulation has ended
                var aliveHighlanders = highlanders.Where(h => h.IsAlive).ToList();
                if (aliveHighlanders.Count == 1)
                {
                    var lastHighlander = aliveHighlanders.First();
                    MessageBox.Show(lastHighlander is BadHighlander ? "Game over! Bad Highlander wins." : "Game over! Good Highlander wins.");
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
            // Reset the simulation state
            isSimulationRunning = false;

            // Reset UI controls to their initial state
            HighlandersSlider.Value = 5;
            RowsTextBox.Text = "5";
            ColumnsTextBox.Text = "5";
            HighlandersLabel.Text = "5";

            // Clear the GameBoardGrid
            GameBoardGrid.Children.Clear();
            GameBoardGrid.RowDefinitions.Clear();
            GameBoardGrid.ColumnDefinitions.Clear();

            // Reinitialize the game board and Highlander list
            int rows = int.Parse(RowsTextBox.Text);
            int columns = int.Parse(ColumnsTextBox.Text);
            gameBoard = new GameBoard<Highlander>(rows, columns);
            highlanders = HighlanderFactory.generateRandomHighlanders((int)HighlandersSlider.Value, rows, columns);

            // Optionally, re-enable the Start button or other controls if needed
            StartSimulationButton.IsEnabled = true;

            // Reinitialize the grid on the UI
            InitializeGameBoardGrid(rows, columns);
            UpdateGrid(gameBoard);

            // Optionally, clear the Highlander stats list and simulation log
            HighlanderStatsListBox.Items.Clear();
            SimulationLogTextBox.Clear();
        }

        private void InitializeGameBoardGrid(int rows, int columns)
        {
            // Clear any existing rows, columns, and children
            GameBoardGrid.RowDefinitions.Clear();
            GameBoardGrid.ColumnDefinitions.Clear();
            GameBoardGrid.Children.Clear();

            // Define rows and columns
            for (int i = 0; i < rows; i++)
            {
                GameBoardGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int i = 0; i < columns; i++)
            {
                GameBoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            // Add borders to each cell
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < columns; col++)
                {
                    Border border = new Border
                    {
                        BorderBrush = Brushes.Black,
                        BorderThickness = new Thickness(1)
                    };
                    Grid.SetRow(border, row);
                    Grid.SetColumn(border, col);

                    // Capture the current row and column values for the lambda expression
                    int capturedRow = row;
                    int capturedCol = col;

                    // Show Highlander stats on mouse enter
                    border.MouseEnter += (s, e) => ShowHighlanderStats(capturedRow, capturedCol);
                    GameBoardGrid.Children.Add(border);
                }
            }
        }


        private void UpdateGrid(IGameBoard<Highlander> gameBoard)
        {
            // Clear previous Highlander positions
            foreach (UIElement child in GameBoardGrid.Children)
            {
                if (child is Border border && border.Child is Canvas)
                {
                    border.Child = null; // Remove previous Canvas (containing Images)
                }
            }

            // Place Highlanders on the grid based on the gameBoard
            for (int row = 0; row < gameBoard.Rows; row++)
            {
                for (int col = 0; col < gameBoard.Columns; col++)
                {
                    // Get all Highlanders at the current position (row, col)
                    List<Highlander> highlandersAtPosition = gameBoard.Board[row, col];

                    if (highlandersAtPosition.Count > 0)
                    {
                        // Calculate the size of the cell
                        double cellWidth = GameBoardGrid.ActualWidth / gameBoard.Columns;
                        double cellHeight = GameBoardGrid.ActualHeight / gameBoard.Rows;

                        // Create a Canvas to hold multiple Images
                        Canvas canvas = new Canvas
                        {
                            Width = cellWidth,
                            Height = cellHeight
                        };

                        // Calculate the size of each image relative to the cell size
                        double imageSize = Math.Min(cellWidth, cellHeight) / Math.Max(1, highlandersAtPosition.Count) * 0.6; // Adjust the multiplier to control image size

                        // Calculate the spacing between images
                        double spacing = (cellWidth - imageSize) / Math.Max(1, highlandersAtPosition.Count);

                        // Add images to the canvas
                        for (int i = 0; i < highlandersAtPosition.Count; i++)
                        {
                            Highlander highlander = highlandersAtPosition[i];

                            if (highlander.IsAlive)
                            {
                                // Create an Image control representing the Highlander
                                Image image = new Image
                                {
                                    Width = imageSize,
                                    Height = imageSize,
                                    Source = new BitmapImage(new Uri(
                                        highlander is GoodHighlander ? "pack://application:,,,/Images/good.png" : "pack://application:,,,/Images/bad.png"))
                                };

                                // Set position of the image within the canvas
                                Canvas.SetLeft(image, i * spacing + (spacing / 2));
                                Canvas.SetTop(image, (cellHeight - image.Height) / 2);

                                canvas.Children.Add(image);
                            }
                        }

                        // Find the corresponding border in the grid
                        Border cellBorder = GameBoardGrid.Children
                            .Cast<UIElement>()
                            .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col) as Border;

                        // Place the canvas in the grid cell
                        cellBorder.Child = canvas;
                    }
                }
            }
        }



        private void HighlandersSlider_ValueChanged_1(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            int rows = int.Parse(RowsTextBox.Text);
            int columns = int.Parse(ColumnsTextBox.Text);

            // Set the slider's maximum value based on the grid size
            int maxHighlanders = rows * columns; // Maximum number of Highlanders is the total number of grid cells - 1 
            HighlandersSlider.Maximum = maxHighlanders - 1;
        }

        private void ShowHighlanderStats(int row, int col)
        {
            // Debug: Log the row and col values
            Console.WriteLine($"Row: {row}, Col: {col}, Max Rows: {gameBoard.Rows}, Max Columns: {gameBoard.Columns}");

            // Ensure the row and col are within bounds
            if (row < 0 || row >= gameBoard.Rows || col < 0 || col >= gameBoard.Columns)
            {
                HighlanderStatsListBox.Items.Clear();
                HighlanderStatsListBox.Items.Add("Invalid position.");
                return;
            }

            // Clear the ListBox before showing new stats
            HighlanderStatsListBox.Items.Clear();

            // Get Highlanders at the specified position
            List<Highlander> highlandersAtPosition = gameBoard.Board[row, col];

            if (highlandersAtPosition == null || highlandersAtPosition.Count == 0)
            {
                HighlanderStatsListBox.Items.Add("No Highlanders at this position.");
            }
            else
            {
                foreach (Highlander highlander in highlandersAtPosition)
                {
                    string stats = $"ID: {highlander.Id}, Power: {highlander.Power}, Age: {highlander.Age}, Alive: {highlander.IsAlive}";
                    HighlanderStatsListBox.Items.Add(stats);
                }
            }
        }
    }
}
