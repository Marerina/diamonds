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
            adc = new AuxiliaryDiamondClass();
            adc.FileLoader("../../diamonds.csv");
            foreach (var v in adc.cuts)
            {
                comboBox1.Items.Add(v.name);
            }
            foreach (var v in adc.colors)
            {
                comboBox2.Items.Add(v.name);
            }
            foreach (var v in adc.clarities)
            {
                comboBox3.Items.Add(v.name);
            }
        }
        AuxiliaryDiamondClass adc;
        Random R;
        private void button_Click(object sender, RoutedEventArgs e)
        {
            int[] a = new int[listBox.SelectedItems.Count];
            int i = 0;
            foreach (var v in listBox.SelectedItems)
            {
                a[i] = listBox.Items.IndexOf(v);
                i++;
            }

            adc.SetF(a, comboBox.SelectedIndex);

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            int count = 20;
            R = new Random();
            int tmp = R.Next(50000);
            adc.Start(tmp, tmp + count, int.Parse(textBox1.Text), 1, true, decimal.Parse(textBox2.Text));     
            NeiroDiamonds.WriteW();
            adc.Start(tmp, tmp + count, int.Parse(textBox1.Text), 1, false, decimal.Parse(textBox2.Text));
            NeiroDiamonds.WriteW();
            stopwatch.Stop();
            textBlock.Text = stopwatch.ElapsedTicks.ToString();
            textBox.Text = Math.Round(adc.Rkvadrat(count), 3).ToString();
            textBox_Copy.Text = Math.Round(adc.Rskorrect(count, a.Count()), 3).ToString();
            button2.IsEnabled = true;
        }


        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //int[] x = new int[26];
            int indexcut = adc.IndexCut(comboBox1.SelectedItem.ToString());
            int indexcolor = adc.IndexColor(comboBox2.SelectedItem.ToString());
            int indexclarity = adc.IndexClarity(comboBox3.SelectedItem.ToString());
            decimal[] x = new decimal[adc.cuts.Count + adc.colors.Count + adc.clarities.Count + 6];
            int i = 0; int tmp = 0;
            x[i] = decimal.Parse(textBox3.Text); i++;

            tmp = i + adc.cuts.Count;
            for (int j = i; j < tmp; j++)
                x[j] = 0;
            x[i+ indexcut] = 1;


            i = tmp;
            tmp = i + adc.colors.Count;
            for (int j = i; j < tmp; j++)
                x[j] = 0;
            x[i + indexcolor] = 1;


            i = tmp;
            tmp = i + adc.clarities.Count;
            for (int j = i; j < tmp; j++)
                x[j] = 0;
            x[i + indexclarity] = 1;

            i = tmp;
            try
            {
                x[i] = decimal.Parse(textBox4.Text); i++;
                x[i] = decimal.Parse(textBox5.Text); i++;
                x[i] = decimal.Parse(textBox6.Text); i++;
                x[i] = decimal.Parse(textBox7.Text); i++;
                x[i] = decimal.Parse(textBox8.Text); i++;
                NeiroDiamonds.ReadW("W1true.csv", "W2true.csv");
                decimal Out = NeiroDiamonds.StraightPass(x, NeiroDiamonds.W1, NeiroDiamonds.W2) / 100 - 155;

                label1.Content = Math.Round(Out, 3).ToString();
            }
            catch { MessageBox.Show("Данные введены некорректно"); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            //System.Windows.Data.CollectionViewSource clarityViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clarityViewSource")));
            //// Load data by setting the CollectionViewSource.Source property:
            //// clarityViewSource.Source = [generic data source]
            //System.Windows.Data.CollectionViewSource cutViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("cutViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // cutViewSource.Source = [generic data source]
        }
    }
}
