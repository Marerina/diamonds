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
using System.Diagnostics;

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
            foreach (var v in listBox.SelectedItems)
            {
                a[i] = listBox.Items.IndexOf(v);
                i++;
            }
            AuxiliaryDiamondClass adc = new AuxiliaryDiamondClass(a, comboBox.SelectedIndex);
            adc.FileLoader("../../diamonds.csv");
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int count = 500;
            adc.Start(0, count, int.Parse(textBox1.Text), 1, true, decimal.Parse(textBox2.Text));
            stopwatch.Stop();
            textBlock.Text = stopwatch.ElapsedTicks.ToString();
            textBox.Text = Math.Round(adc.Rkvadrat(count),3).ToString();
            textBox_Copy.Text = Math.Round(adc.Rskorrect(count, a.Count()),3).ToString();
        }
        

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
