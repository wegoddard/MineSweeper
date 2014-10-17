﻿using System;
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

            // Height and Width
            gridBoard.Width = 160;
            gridBoard.Height = 160;

            // Create  grid rows and columns
            for (int c = 0; c < columns; c++)
            {
                ColumnDefinition col = new ColumnDefinition();
                col.Width = new GridLength(16);
                gridBoard.ColumnDefinitions.Add(col);
            }

            for (int r = 0; r < rows; r++)
            {
                RowDefinition row = new RowDefinition();
                row.Height = new GridLength(16);
                gridBoard.RowDefinitions.Add(row);
            }



            // Create buttons
            for (int c = 0; c < columns; c++)
            {
                for (int r = 0; r < rows; r++)
                {
                    // Create image and add to first cell, overlayed by button
                    Image image = new Image();
                    image.Source = new BitmapImage(new Uri("assets/Mine1pxBorder.png", UriKind.Relative));
                    image.Stretch = Stretch.None;
                    Grid.SetRow(image, r);
                    Grid.SetColumn(image, c);
                    gridBoard.Children.Add(image);

                    Button button = new Button();
                    button.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#7B7B7B"));
                    button.BorderThickness = new Thickness(1);
                    button.Height = 16;
                    button.Height = 16;

                    // add click handler to button
                    button.Click += new RoutedEventHandler(button_Click);

                    // add button to grid
                    Grid.SetRow(button, r);
                    Grid.SetColumn(button, c);
                    gridBoard.Children.Add(button);
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
