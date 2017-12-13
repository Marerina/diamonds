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
        public MainWindow()
        {
            InitializeComponent();         
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            int[] a = new int[listBox.SelectedItems.Count];
            int i = 0;
            foreach(var v in listBox.SelectedItems)
            {
                a[i] = listBox.Items.IndexOf(v);
                i++; 
            }
            AuxiliaryDiamondClass adc = new AuxiliaryDiamondClass(comboBox.SelectedIndex,int.Parse(textBox1.Text), a);
           
            adc.FileLoader("../../diamonds.csv");
            adc.Start(0, 0, 0, 0, true, 0);
            adc.Start(0, 5000, 26, 1, true, decimal.Parse(textBox2.Text));
            textBlock.Text = "Ну типа МАГИЯ";
        }
    }
}
