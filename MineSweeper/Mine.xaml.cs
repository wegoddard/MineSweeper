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
    /// Interaction logic for Mine.xaml
    /// </summary>
    public partial class Mine : UserControl
    {
        public static readonly DependencyProperty XCoordinateProperty = DependencyProperty.Register("XCoordinate", typeof(int), typeof(Mine), new PropertyMetadata());

        public static readonly DependencyProperty YCoordinateProperty = DependencyProperty.Register("YCoordinate", typeof(int), typeof(Mine), new PropertyMetadata());

        public int XCoordinate
        {
            get { return (int)GetValue(XCoordinateProperty); }
            set { SetValue(XCoordinateProperty, value); }
        }

        public int YCoordinate
        {
            get { return (int)GetValue(YCoordinateProperty); }
            set { SetValue(YCoordinateProperty, value); }
        }

        public Mine()
        {
            InitializeComponent();
        }
    }
}
