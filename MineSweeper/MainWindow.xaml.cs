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
            // create overlapping horizontal rectangles
            int cellSize = 18;
            int numOfCells = 21;

            // adjust main window
            mainWindow.Height = numOfCells * (cellSize - 1) + 133;
            mainWindow.Width = numOfCells * (cellSize - 1) + 37;

            for (int i = 0; i < numOfCells; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B7B7B"));
                rect.Width = cellSize * numOfCells - (numOfCells - 1);  
                rect.Height = cellSize;
                Canvas.SetLeft(rect, 0);
                Canvas.SetTop(rect, i * cellSize - i - 1);
                gridBoard.Children.Add(rect);
            }

            // create overlapping vertical rectangles
            for (int i = 0; i < numOfCells; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B7B7B"));
                rect.Width = cellSize;
                rect.Height = cellSize * numOfCells - (numOfCells - 1); 
                Canvas.SetLeft(rect, i * cellSize - i);
                Canvas.SetTop(rect, -1);
                gridBoard.Children.Add(rect);
            }


            //int columns = 10;
            //int rows = 10;
            //int mines = 10;

            //// Height and Width
            //gridBoard.Width = 200;
            //gridBoard.Height = 200;

            //// Create  grid rows and columns
            //for (int c = 0; c < columns; c++)
            //{
            //    ColumnDefinition col = new ColumnDefinition();
            //    col.Width = new GridLength(17);
            //    gridBoard.ColumnDefinitions.Add(col);
            //}

            //for (int r = 0; r < rows; r++)
            //{
            //    RowDefinition row = new RowDefinition();
            //    row.Height = new GridLength(17);
            //    gridBoard.RowDefinitions.Add(row);
            //}

            // Add mines
            //AddMines();

            //// Create buttons
            //for (int c = 0; c < columns; c++)
            //{
            //    for (int r = 0; r < rows; r++)
            //    {


            //        //Button button = new Button();
            //        //button.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B7B7B"));
            //        //button.BorderThickness = new Thickness(1);
            //        //button.Height = 16;
            //        //button.Height = 16;

            //        //// add click handler to button
            //        //button.Click += new RoutedEventHandler(button_Click);

            //        //// add button to grid
            //        //Grid.SetRow(button, r);
            //        //Grid.SetColumn(button, c);
            //        //gridBoard.Children.Add(button);
            //    }
            //}
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.Visibility = Visibility.Hidden;
        }

        //private void AddMines()
        //{

            //int mineCount = 10;
            //int[,] mineGrid = new int[10, 10];

            //Random rnd = new Random();
            //int i = mineCount;
            //do
            //{
            //    int row = rnd.Next(10);
            //    int col = rnd.Next(10);
            //    if (mineGrid[col, row] == 0)
            //    {
            //        mineGrid[col, row] = 1;

            //        // Create image and add to first cell, overlayed by button
            //        Image image = new Image();
            //        image.Source = new BitmapImage(new Uri("assets/Mine1pxBorder.png", UriKind.Relative));
            //        image.Stretch = Stretch.None;
            //        Grid.SetRow(image, row);
            //        Grid.SetColumn(image, col);
            //        gridBoard.Children.Add(image);

            //        i--;
            //    }
            //} while (i > 0);

        //}
    }
}
