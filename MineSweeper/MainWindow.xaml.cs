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

        const int BoardSize = 10;

        const int CellSize = 17;

        public void CreateGameBoard()
        {
            // adjust main window size
            mainWindow.Height = BoardSize * (CellSize - 1) + 133;
            mainWindow.Width = BoardSize * (CellSize - 1) + 37;

            // create overlapping horizontal rectangles (for gridlines)
            for (int i = 0; i < BoardSize; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B7B7B"));
                rect.Width = CellSize * BoardSize - (BoardSize - 1);  
                rect.Height = CellSize;
                Canvas.SetLeft(rect, 0);
                Canvas.SetTop(rect, i * CellSize - i - 1);
                gridBoard.Children.Add(rect);
            }

            // create overlapping vertical rectangles (for gridlines)
            for (int i = 0; i < BoardSize; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B7B7B"));
                rect.Width = CellSize;
                rect.Height = CellSize * BoardSize - (BoardSize - 1); 
                Canvas.SetLeft(rect, i * CellSize - i );
                Canvas.SetTop(rect, - 1);
                gridBoard.Children.Add(rect);
            }

            // Add mines
            AddMines();

            // Buttons
            AddButtons();

        }

        private void AddButtons()
        {
            // Create buttons
            for (int c = 0; c < BoardSize; c++)
            {
                for (int r = 0; r < BoardSize; r++)
                {
                    // create button, remove border, set image
                    Button button = new Button();
                    button.BorderThickness = new Thickness(0);
                    button.Height = CellSize - 1;
                    button.Width = CellSize - 1;
                    BitmapImage bmp = new BitmapImage(new Uri("Assets/ButtonForeground.png", UriKind.Relative));
                    Image image = new Image();
                    image.Source = bmp;
                    button.Content = image;

                    // add click handler to button
                    button.Click += new RoutedEventHandler(button_Click);

                    // add button to canvas
                    Canvas.SetLeft(button, r * (CellSize - 1) + 1);
                    Canvas.SetTop(button, c * (CellSize - 1));
                    gridBoard.Children.Add(button);
                }
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            button.Visibility = Visibility.Hidden;
        }

        private void AddMines()
        {
            int mineCount = 10;

            int[,] mineGrid = new int[mineCount, mineCount];

            Random rnd = new Random();
            int i = mineCount;
            do
            {
                int row = rnd.Next(BoardSize);
                int col = rnd.Next(BoardSize);
                if (mineGrid[col, row] == 0)
                {
                    mineGrid[col, row] = 1;

                    // Create image and add to first cell, overlayed by button
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri("assets/MineNoBorder.png", UriKind.Relative));
                    image.Stretch = Stretch.None;
                    Canvas.SetLeft(image, 1 + col * (CellSize - 1) );
                    Canvas.SetTop(image, 0 + row * (CellSize - 1));
                    gridBoard.Children.Add(image);

                    i--;
                }
            } while (i > 0);

        }
    }
}
