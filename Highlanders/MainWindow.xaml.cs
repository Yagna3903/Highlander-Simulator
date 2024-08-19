using Highlander_Component.GameBoard;
using Highlander_Components.GameStimulation;
using Highlander_Components.lander;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
            UpdateGrid();
        }

        private void NextStep_Click(object sender, RoutedEventArgs e)
        {
            if (isSimulationRunning)
            {
                gamePlay.RunSimulationStep();
                UpdateGrid();

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
            // Reset the simulation and UI
            HighlandersSlider.Value = 5;
            RowsTextBox.Text = "5";
            ColumnsTextBox.Text = "5";
            HighlandersLabel.Text = "5";
            GameBoardGrid.Children.Clear();
            GameBoardGrid.RowDefinitions.Clear();
            GameBoardGrid.ColumnDefinitions.Clear();
            isSimulationRunning = false;
        }

        private void InitializeGameBoardGrid(int rows, int columns)
        {
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
                    GameBoardGrid.Children.Add(border);
                }
            }
        }

        private void UpdateGrid()
        {
            // Clear previous Highlander positions
            foreach (UIElement child in GameBoardGrid.Children)
            {
                if (child is Border border && border.Child is Ellipse)
                {
                    border.Child = null; // Remove previous Ellipse
                }
            }

            // Place Highlanders on the grid
            foreach (var highlander in highlanders)
            {
                if (highlander.IsAlive)
                {

                    var (row, col) = highlander.GetPosition(); // Get Highlander position


                    Ellipse ellipse = new Ellipse
                    {
                        Width = 20, // Adjust size as needed
                        Height = 20, // Adjust size as needed
                        Fill = highlander is GoodHighlander ? Brushes.Green : Brushes.Red
                    };

                    // Find the corresponding border in the grid
                    Border cellBorder = GameBoardGrid.Children
                        .Cast<UIElement>()
                        .First(e => Grid.GetRow(e) == row && Grid.GetColumn(e) == col) as Border;

                    // Place the ellipse in the grid cell
                    cellBorder.Child = ellipse;
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
    }
}
