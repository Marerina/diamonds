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

namespace diamonds
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int i = 0;
        public MainWindow()
        {
            InitializeComponent();
            i = 5;
            
            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            AuxiliaryDiamondClass hueta = new AuxiliaryDiamondClass();
            hueta.FileLoader("../../diamonds.csv");
            hueta.Start(0, 10, 13, 1, true);
            hueta.Start(0, 10, 13, 1, false);
            textBlock.Text = "Ну типа МАГИЯ БЛЯ";
        }
    }
}
