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
            int count = 10;
            adc.Start(0, count, int.Parse(textBox1.Text), 1, true, decimal.Parse(textBox2.Text));
            stopwatch.Stop();
            textBlock.Text = stopwatch.ElapsedTicks.ToString();
            textBox.Text = Math.Round(adc.Rkvadrat(count),3).ToString();
            textBox_Copy.Text = Math.Round(adc.Rskorrect(count, a.Count()),3).ToString();
        }
        

        private void button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            System.Windows.Data.CollectionViewSource clarityViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("clarityViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // clarityViewSource.Source = [generic data source]
            System.Windows.Data.CollectionViewSource cutViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("cutViewSource")));
            // Load data by setting the CollectionViewSource.Source property:
            // cutViewSource.Source = [generic data source]
        }
    }
}
