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
using System.Diagnostics;

namespace MineSweeper
{
    /// <summary>
    /// Interaction logic for Timer.xaml
    /// </summary>
    public partial class Timer : UserControl
    {
        Stopwatch stopWatch = new Stopwatch();

        public Timer()
        {
            InitializeComponent();

            // timer
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
        }

        private BitmapImage GetImage(int digit)
        {
            return new BitmapImage(new Uri("assets/Digital" + digit.ToString() + ".png", UriKind.Relative));
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            int seconds = (int)stopWatch.Elapsed.TotalSeconds;

            // get seconds
            imgOnes.Source = GetImage(seconds % 10);
            
            // get tens of seconds
            int tens = ((seconds % 100) - (seconds % 10)) / 10;
            imgTens.Source = GetImage(tens);

            // hundreds of seconds
            int hundreds = ((seconds % 100) - (seconds % 10)) / 100;
            imgHundreds.Source = GetImage(hundreds);
        }

        public void StartTimer()
        {
            stopWatch.Start();
        }

        public void StopTimer()
        {
            stopWatch.Stop();
        }

        public void ResetTimer()
        {
            stopWatch.Reset();
        }
    }
}
