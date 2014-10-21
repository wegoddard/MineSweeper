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

        const int BoardSize = 12;

        const int CellSize = 16;

        int[,] MineGrid = new int[BoardSize, BoardSize];

        public void CreateGameBoard()
        {
            // set title grid size
            TitleGrid.Width = BoardSize * (CellSize) + 20;

            // adjust main window size
            mainWindow.Height = BoardSize * (CellSize) + 133;
            mainWindow.Width = BoardSize * (CellSize) + 37;

            // create overlapping horizontal rectangles (for gridlines)
            for (int i = 0; i < BoardSize; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B7B7B"));
                rect.Width = BoardSize * CellSize + 1;
                rect.Height = CellSize + 1;
                Canvas.SetLeft(rect, 0);
                Canvas.SetTop(rect, i * CellSize - 1);
                gridBoard.Children.Add(rect);
            }

            // create overlapping vertical rectangles (for gridlines)
            for (int i = 0; i < BoardSize; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B7B7B"));
                rect.Width = CellSize + 1;
                rect.Height = BoardSize * CellSize + 1;
                Canvas.SetLeft(rect, i *  CellSize);
                Canvas.SetTop(rect, -1);
                gridBoard.Children.Add(rect);
            }

            // Add mines
            AddMines();

            // Numbers
            AddNumbers();

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
                    button.Height = CellSize;
                    button.Width = CellSize;
                    BitmapImage bmp = new BitmapImage(new Uri("Assets/ButtonForeground.png", UriKind.Relative));
                    Image image = new Image();
                    image.Source = bmp;
                    button.Content = image;

                    // add click handler to button
                    button.Click += new RoutedEventHandler(button_Click);

                    // add button to canvas
                    Canvas.SetLeft(button, r * (CellSize) + 1);
                    Canvas.SetTop(button, c * (CellSize));
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
            int mineCount = 6;

            //int[,] mineGrid = new int[BoardSize, BoardSize];

            Random rnd = new Random();
            int i = mineCount;
            do
            {
                int row = rnd.Next(BoardSize);
                int col = rnd.Next(BoardSize);
                if (MineGrid[col, row] == 0)
                {
                    MineGrid[col, row] = 9;   // bomb is encoded as 9

                    // Create image and add to first cell, overlayed by button
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri("assets/MineNoBorder.png", UriKind.Relative));
                    image.Stretch = Stretch.None;
                    Canvas.SetLeft(image, 1 + col * (CellSize) );
                    Canvas.SetTop(image, 0 + row * (CellSize));
                    gridBoard.Children.Add(image);

                    i--;
                }
            } while (i > 0);
        }

        /// <summary>
        /// AddNumbers places all the hint numbers on the board
        /// </summary>
        private void AddNumbers()
        {
            // for each element of the grid
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    // if the element is not a mine
                    if (MineGrid[i, j] != 9)
                    {
                        // for each adjacent cell if the cell has a mine, add one to counter
                        int adjacentMines = CountAdjacentMines(i, j);

                        // assign counter to grid element
                        MineGrid[i, j] = adjacentMines;

                        // add corresponding image to canvas
                        Image image = GetNumberImage(adjacentMines);
                        image.Stretch = Stretch.None;

                        // set image if image is not empty
                        if (image.Source != null)
                        {
                            Canvas.SetLeft(image, 1 + i * (CellSize));
                            Canvas.SetTop(image, j * (CellSize));
                            gridBoard.Children.Add(image);
                        }
                    }
                }
            }
        }

        // if the adjacent cell has a mine, add one to the counter
        private int CountAdjacentMines(int x, int y)
        {
            // initialize counter
            int mineCounter = 0;

            // check upper left (x-1, y-1)
            // if not off grid
            if (x > 0 && y > 0)
            {
                // if has mine
                if (MineGrid[x - 1, y - 1] == 9)
                    mineCounter++;
            }
                
            // check upper center (0, y-1)
            // if not off grid
            if (y > 0)
            {
                // if has mine
                if (MineGrid[x, y - 1] == 9)
                    mineCounter++;
            }

            // check upper right (x+1, y-1)
            // if not off grid
            if (x < (BoardSize - 2) && y > 0)
            {
                // if has mine
                if (MineGrid[x + 1, y - 1] == 9)
                    mineCounter++;
            }
                
            // check left center (x-1, 0)
            // if not off grid
            if (x > 0)
            {
                // if has mine
                if (MineGrid[x - 1, y] == 9)
                    mineCounter++;
            }

            // check right center (x+1, 0)
            if (x < BoardSize - 2)
            {
                // if has mine
                if (MineGrid[x + 1, y] == 9)
                    mineCounter++;
            }

            // check lower left (x-1, y+1)
            // if not off grid
            if (x > 0 && y < BoardSize - 1)
            {
                // if has mine
                if (MineGrid[x - 1, y + 1] == 9)
                    mineCounter++;
            }

            // check lower center (0, y+1)
            // if not off grid
            if (y < BoardSize - 1)
            {
                // if has mine
                if (MineGrid[x, y + 1] == 9)
                    mineCounter++;
            }

            // check lower right (x+1, y+1)
            if (x < BoardSize - 1 && y < BoardSize - 1)
            {
                // if has mine
                if (MineGrid[x + 1, y + 1] == 9)
                    mineCounter++;
            }

            // return counter
            return mineCounter;
        }

        private Image GetNumberImage(int minesNearby)
        {
            // if number = x return corresponding image
            Image image = new Image();
            switch(minesNearby)
            {
                case 1:
                    image.Source = new BitmapImage(new Uri("Assets/One.png", UriKind.Relative));
                    break;
                case 2:
                    image.Source = new BitmapImage(new Uri("Assets/Two.png", UriKind.Relative));
                    break;
                case 3:
                    image.Source = new BitmapImage(new Uri("Assets/Three.png", UriKind.Relative));
                    break;
                case 4:
                    image.Source = new BitmapImage(new Uri("Assets/Four.png", UriKind.Relative));
                    break;
                case 5:
                    image.Source = new BitmapImage(new Uri("Assets/Five.png", UriKind.Relative));
                    break;
                case 6:
                    image.Source = new BitmapImage(new Uri("Assets/Six.png", UriKind.Relative));
                    break;
                case 7:
                    image.Source = new BitmapImage(new Uri("Assets/Seven.png", UriKind.Relative));
                    break;
                case 8:
                    image.Source = new BitmapImage(new Uri("Assets/Eight.png", UriKind.Relative));
                    break;
            }

            return image;
        }

        // clear() 
        // get coordinates of cell 
        // Hide button
        // for each adjacent cell
            
                // if button is visible
                    // if cell value is zero
                        // Clear() on adjacent cell
                    // else if cell value is 0 > x > 9
                        // hide button
                // end if
        // end for
            
    }
}
