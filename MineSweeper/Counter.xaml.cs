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
    /// Interaction logic for Counter.xaml
    /// </summary>
    public partial class Counter : UserControl
    {
        public Counter()
        {
            InitializeComponent();
        }

        private BitmapImage GetImage(int digit)
        {
            return new BitmapImage(new Uri("assets/Digital" + digit.ToString() + ".png", UriKind.Relative));
        }

        public void SetCounter(int counter)
        {
            // get seconds
            imgOnes.Source = GetImage(counter % 10);

            // get tens of seconds
            int tens = ((counter % 100) - (counter % 10)) / 10;
            imgTens.Source = GetImage(tens);

            // hundreds of seconds
            int hundreds = (counter - (counter % 100)) / 100;
            imgHundreds.Source = GetImage(hundreds);
        }
    }
}
