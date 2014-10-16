using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            CreateGameBoard();
        }

        public void CreateGameBoard()
        {
            int columns = 10;
            int rows = 10;

            // Create grid
            Grid GameBoard = new Grid();
            GameBoard.Width = 400;
            GameBoard.Height = 400;
            GameBoard.HorizontalAlignment = HorizontalAlignment.Center;
            GameBoard.VerticalAlignment = VerticalAlignment.Bottom;
            GameBoard.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#C0C0C0"));

            // Create  grid rows and columns
            for (int c = 0; c < columns; c++)
            {
                ColumnDefinition col = new ColumnDefinition();
                GameBoard.ColumnDefinitions.Add(col);
            }

            for (int r = 0; r < rows; r++)
            {
                RowDefinition row = new RowDefinition();
                GameBoard.RowDefinitions.Add(row);
            }

            // Add gameboard to background grid
            BackgroundGrid.Children.Add(GameBoard);

            // Create buttons
 
            for (int c = 0; c < columns; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    Button button = new Button();
                    button.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));
                    button.BorderThickness = new Thickness(1);
                    button.Height = 40;
                    button.Height = 40;
                    // add click handler to button
                    button.Click += new RoutedEventHandler(button_Click);

                    // add button to grid
                    Grid.SetRow(button, r);
                    Grid.SetColumn(button, c);
                    GameBoard.Children.Add(button);


                }
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.Visibility = Visibility.Hidden;
        }
    }
}
