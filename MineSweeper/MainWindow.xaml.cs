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
using System.Windows.Threading;

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int MineCount = 99;

        const int CellSize = 16;

        int GameHeight = 16;

        int GameWidth = 30;

        GridButton[,] Buttons;

        Mine[,] Mines;

        int[,] Hints;

        bool IsGameInProgress = false;

        public MainWindow()
        {
            InitializeComponent();

            CreateGameBoard();
        }

        /// <summary>
        /// Determine the size of the gameboard, draw gridlines, draw mines, draw numbers and add buttons.
        /// </summary>
        public void CreateGameBoard()
        {
            // allocate data structures
            Buttons = new GridButton[GameWidth, GameHeight];
            Mines = new Mine[GameWidth, GameHeight];
            Hints = new int[GameWidth, GameHeight];

            // set title grid size
            TitleGrid.Width = GameWidth * CellSize + 20;

            // adjust main window size
            mainWindow.Height = GameHeight * CellSize + 150;
            mainWindow.Width = GameWidth * CellSize + 37;

            // create overlapping horizontal rectangles (for gridlines)
            for (int i = 0; i < GameHeight; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B7B7B"));
                rect.Width = GameWidth * CellSize + 1;
                rect.Height = CellSize + 1;
                Canvas.SetLeft(rect, 0);
                Canvas.SetTop(rect, i * CellSize - 1);
                gridBoard.Children.Add(rect);
            }

            // create overlapping vertical rectangles (for gridlines)
            for (int i = 0; i < GameWidth; i++)
            {
                Rectangle rect = new Rectangle();
                rect.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B7B7B"));
                rect.Width = CellSize + 1;
                rect.Height = GameHeight * CellSize + 1;
                Canvas.SetLeft(rect, i * CellSize);
                Canvas.SetTop(rect, -1);
                gridBoard.Children.Add(rect);
            }

            // Buttons
            AddButtons();
        }

        private void ClearGameBoard()
        {
            // remove all objects from the canvas
            gridBoard.Children.Clear();
        }

        /// <summary>
        /// Add a number of mines randomly to the gameboard, according to the total number Mines. X and Y represent
        /// the coordinates of the first button click.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void AddMines(GridButton button)
        {
            Random rnd = new Random();
            int i = MineCount;
            do
            {
                int row = rnd.Next(GameHeight);
                int col = rnd.Next(GameWidth);

                // if no mine is at the current position and this is NOT the position of the first button click
                if (Mines[col, row] == null && !(col == button.XCoordinate && row == button.YCoordinate))
                {
                    // add mine
                    Mine mine = new Mine();
                    mine.XCoordinate = col;
                    mine.YCoordinate = row;
                    Mines[col, row] = mine;

                    // Create image and add to first cell, overlayed by button
                    Canvas.SetLeft(mine, 1 + col * CellSize);
                    Canvas.SetTop(mine, 0 + row * CellSize);
                    gridBoard.Children.Add(mine);

                    i--;
                }
            } while (i > 0);
        }

        private void AddButtons()
        {
            // Create Buttons
            for (int c = 0; c < GameWidth; c++)
            {
                for (int r = 0; r < GameHeight; r++)
                {
                    // create button, remove border, set image
                    GridButton button = new GridButton();
                    button.Height = CellSize;
                    button.Width = CellSize;
                    button.XCoordinate = c;
                    button.YCoordinate = r;

                    // set z-index
                    Canvas.SetZIndex(button, 10);

                    // subscribe to event
                    button.ButtonClicked += ButtonClicked;

                    // right-click event
                    button.MouseRightButtonDown += RightClick;

                    // add button to canvas
                    Canvas.SetLeft(button, c * (CellSize) + 1);
                    Canvas.SetTop(button, r * (CellSize));
                    gridBoard.Children.Add(button);

                    // add button to array
                    Buttons[c, r] = button;
                }
            }
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            GridButton button = sender as GridButton;

            // is this a new game?
            if (!IsGameInProgress)
            {
                AddMines(button);
                AddHints();



                tmrGameTimer.StartTimer();
                IsGameInProgress = true;
            }

            ClearCells(button);

            // if button clicked is a mine, game is over
            if (Mines[button.XCoordinate, button.YCoordinate] != null)
            {
                ClearMines();
                FlagMine(button);
                DisableButtons();
                ChangeNewGameIcon();
                StopTimer();

                // for each marked button 
                foreach (GridButton gridButton in Buttons)
                {
                    // if no mine is underneath and button has been flagged
                    if (Mines[gridButton.XCoordinate, gridButton.YCoordinate] == null && Buttons[gridButton.XCoordinate, gridButton.YCoordinate].IsFlagged)
                    {
                        // hide button
                        gridButton.Visibility = Visibility.Hidden;

                        // place red X over mine
                        Image image = new Image();
                        image.Source = new BitmapImage(new Uri("assets/MineRedX.png", UriKind.Relative));
                        image.Stretch = Stretch.None;
                        Canvas.SetLeft(image, gridButton.XCoordinate * CellSize + 1);
                        Canvas.SetTop(image, gridButton.YCoordinate * CellSize);
                        gridBoard.Children.Add(image);
                    }
                }
            }
        }

        /// <summary>
        /// Replace the image located under the current button with a RedFlag image indicating that
        /// the user has clicked on a mine.
        /// </summary>
        /// <param name="button"></param>
        private void FlagMine(GridButton button)
        {
            Image image = new Image();
            image.Source = new BitmapImage(new Uri("assets/RedMine.png", UriKind.Relative));
            image.Stretch = Stretch.None;
            Canvas.SetLeft(image, button.XCoordinate * CellSize + 1);
            Canvas.SetTop(image, button.YCoordinate * CellSize);
            gridBoard.Children.Add(image);
        }

        /// <summary>
        /// Disables all buttons
        /// </summary>
        private void DisableButtons()
        {
            foreach(GridButton button in Buttons)
            {
                button.IsEnabled = false;
            }
        }

        /// <summary>
        /// Change happy face to not-so-happy face
        /// </summary>
        private void ChangeNewGameIcon()
        {
            Image image = new Image();
            image.Source = new BitmapImage(new Uri("assets/GameOver.png", UriKind.Relative));
            image.Stretch = Stretch.None;
            btnNewGame.Content = image;
        }

        private void StopTimer()
        {
            tmrGameTimer.StopTimer();
        }

        private void ResetTimer()
        {
            tmrGameTimer.ResetTimer();
        }

        /// <summary>
        /// Create a flag when the user right-clicks a button to flag that a mine may be located there
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightClick(object sender, EventArgs e)
        {
            GridButton button = sender as GridButton;
            Image image = new Image();

            // mark button in button array as marked
            if (button.IsFlagged == false)
            {
                button.IsFlagged = true;
                image.Source = new BitmapImage(new Uri("assets/MineFlag.png", UriKind.Relative));
            }
            else
            {
                button.IsFlagged = true;
                image.Source = new BitmapImage(new Uri("assets/ButtonForeground.png", UriKind.Relative));
            }

            button.Content = image;
        }

        /// <summary>
        /// AddNumbers places all the hint numbers on the board
        /// </summary>
        private void AddHints()
        {
            // for each element of the grid
            for (int i = 0; i < GameWidth; i++)
            {
                for (int j = 0; j < GameHeight; j++)
                {
                    // if the element is not a mine
                    if (Mines[i, j] == null)
                    {
                        // for each adjacent cell if the cell has a mine, add one to counter
                        int adjacentMines = CountAdjacentMines(i, j);

                        // assign counter to grid element
                        Hints[i, j] = adjacentMines;

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

        /// <summary>
        /// Given the coordinates of a cell, return the number of mines adjacent to that cell
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private int CountAdjacentMines(int x, int y)
        {
            // initialize counter
            int mineCounter = 0;

            // check upper left (x-1, y-1)
            // if not off grid
            if (x > 0 && y > 0)
            {
                // if has mine
                if (Mines[x - 1, y - 1] != null)
                    mineCounter++;
            }
                
            // check upper center (0, y-1)
            // if not off grid
            if (y > 0)
            {
                // if has mine
                if (Mines[x, y - 1] != null)
                    mineCounter++;
            }

            // check upper right (x+1, y-1)
            // if not off grid
            if (x < (GameWidth - 1) && y > 0)
            {
                // if has mine
                if (Mines[x + 1, y - 1] != null)
                    mineCounter++;
            }
                
            // check left center (x-1, 0)
            // if not off grid
            if (x > 0)
            {
                // if has mine
                if (Mines[x - 1, y] != null)
                    mineCounter++;
            }

            // check right center (x+1, 0)
            if (x < GameWidth - 1)
            {
                // if has mine
                if (Mines[x + 1, y] != null)
                    mineCounter++;
            }

            // check lower left (x-1, y+1)
            // if not off grid
            if (x > 0 && y < GameHeight - 1)
            {
                // if has mine
                if (Mines[x - 1, y + 1] != null)
                    mineCounter++;
            }

            // check lower center (0, y+1)
            // if not off grid
            if (y < GameHeight - 1)
            {
                // if has mine
                if (Mines[x, y + 1] != null)
                    mineCounter++;
            }

            // check lower right (x+1, y+1)
            if (x < GameWidth - 1 && y < GameHeight - 1)
            {
                // if has mine
                if (Mines[x + 1, y + 1] != null)
                    mineCounter++;
            }

            // return counter
            return mineCounter;
        }

        /// <summary>
        /// Given a number, return the image used to represent 
        /// that number of adjacent mines
        /// </summary>
        /// <param name="minesNearby"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get coordinates of current cell, and clear any adjacent and connected open cells
        /// </summary>
        /// <param name="x">Horizontal coordinate, as measured from the left side</param>
        /// <param name="y">Vertical coordinate, as measured from the top</param>
        private void ClearCells(GridButton button)
        { 
            // Hide button
            button.Visibility = Visibility.Hidden;

            // if cell is empty
            if (Mines[button.XCoordinate, button.YCoordinate] == null && Hints[button.XCoordinate, button.YCoordinate] == 0)
            {
                // for each adjacent cell
                foreach (GridButton adjacentButton in GetAdjacentButtons(button.XCoordinate, button.YCoordinate))
                {
                    // if button is visible
                    if (adjacentButton.Visibility != Visibility.Hidden)
                    {
                        // if cell is empty
                        if (Mines[adjacentButton.XCoordinate, adjacentButton.YCoordinate] == null && Hints[button.XCoordinate, button.YCoordinate] == 0)
                        {
                            // Clear() on adjacent cell
                            ClearCells(adjacentButton);
                        }
                        // else if cell value is 0 > x > 9
                        else if (Hints[adjacentButton.XCoordinate, adjacentButton.YCoordinate] > 0)
                        {
                            // hide button
                            adjacentButton.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
        }
    
        /// <summary>
        /// Return a list of adjacent buttons, given a grid coordinate
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private List<GridButton> GetAdjacentButtons(int x, int y)
        {
            List<GridButton> buttons = new List<GridButton>();

            // check upper left cell
            if (x > 0 && y > 0)
                buttons.Add(Buttons[x - 1, y - 1]);

            // upper center
            if (y > 0)
                buttons.Add(Buttons[x, y - 1]);

            // upper right
            if (x < GameWidth - 1 && y > 0)
                buttons.Add(Buttons[x + 1, y - 1]);

            // left center
            if (x > 0)
                buttons.Add(Buttons[x - 1, y]);

            // right center
            if (x < GameWidth - 1)
                buttons.Add(Buttons[x + 1, y]);

            // lower left
            if (x > 0 && y < GameHeight - 1)
                buttons.Add(Buttons[x - 1, y + 1]);

            // lower center
            if (y < GameHeight - 1)
                buttons.Add(Buttons[x, y + 1]);

            // lower right
            if (x < GameWidth - 1 && y < GameHeight - 1)
                buttons.Add(Buttons[x + 1, y + 1]);

            return buttons;
        }

        /// <summary>
        /// Create a new game by deleting and re-creating all the game elements 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 
            btnNewGame_Click(object sender, RoutedEventArgs e)
        {
            // stop game
            IsGameInProgress = false;
            ResetTimer();
            
            // restore happy face
            Image image = new Image();
            image.Source = new BitmapImage(new Uri("assets/Happy.png", UriKind.Relative));
            image.Stretch = Stretch.None;
            btnNewGame.Content = image;

            // reset board
            ClearBoard();

            // enable buttons
            foreach(GridButton button in Buttons)
            {
                button.Visibility = Visibility.Visible;
                button.IsEnabled = true;
                
                // remove flag from button
                if (button.IsFlagged)
                {
                    button.IsFlagged = false;
                    Image gridButtonImage = new Image();
                    gridButtonImage.Source = new BitmapImage(new Uri("assets/ButtonForeground.png", UriKind.Relative));
                    button.Content = gridButtonImage;
                }
            }
        }

        /// <summary>
        /// Clears all images of mines and numbers from the board
        /// </summary>
        private void ClearBoard()
        {
            // remove hints from gameboard
            List<Image> images = new List<Image>();

            // re-initialize Hint data structure
            Hints = new int[GameWidth, GameHeight];

            foreach (UIElement child in gridBoard.Children)
                if (child is Image)
                    images.Add((Image)child);

            foreach (Image image in images)
                gridBoard.Children.Remove(image);

            // remove mines
            foreach (Mine mine in Mines)
                gridBoard.Children.Remove(mine);
            
            Mines = new Mine[GameWidth, GameHeight];
        }

        /// <summary>
        /// Remove all buttons from canvas and renew list of buttons
        /// </summary>
        private void RemoveAllButtons()
        {
            // remove buttons from board
            foreach (GridButton button in Buttons)
                gridBoard.Children.Remove(button);

            // re-initialize array
            Buttons = new GridButton[GameHeight, GameWidth];
        }

        /// <summary>
        /// Remove buttons on gameboard covering mines.
        /// </summary>
        private void ClearMines()
        {
            for (int i = 0; i < GameWidth; i++)
            {
                for (int j = 0; j < GameHeight; j++)
                {
                    // if the current location is s a mine
                    if (Mines[i, j] != null)
                    {
                        // hide the button
                        Buttons[i, j].Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        #region Custom Popup

        private void mnu_Custom_Click(object sender, RoutedEventArgs e)
        {
            popCustom.IsOpen = true;
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            GameHeight = Convert.ToInt32(txtHeight.Text);
            GameWidth = Convert.ToInt32(txtWidth.Text);
            MineCount = Convert.ToInt32(txtMines.Text);

            // start new game
            ClearGameBoard();
            CreateGameBoard();
            btnNewGame_Click(sender, e);

            popCustom.IsOpen = false;
            txtHeight.Text = "";
            txtWidth.Text = "";
            txtMines.Text = "";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            popCustom.IsOpen = false;
            txtHeight.Text = "";
            txtWidth.Text = "";
            txtMines.Text = "";
        }

        #endregion
    }
}
