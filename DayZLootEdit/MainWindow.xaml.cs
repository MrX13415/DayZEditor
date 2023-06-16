using Microsoft.Win32;
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

namespace DayZLootEdit
{
    /// <summary>
    /// Interaction Logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private LootTable LootTable;


        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
            if (dlg.ShowDialog().Value)
            {
                LootTable = new LootTable(dlg.FileName);
                LootTable.LoadFile();

                LootList.ItemsSource = LootTable.Loot;

                LootList.IsEnabled = true;
                SaveBtn.IsEnabled = true;
            }
        }

        //Added "Nominal" prefix and then a new line for the Lifetime
        private void LootList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NominalLootPercBox.IsEnabled = LootList.SelectedItems.Count > 0;
            LifetimeLootPercBox.IsEnabled = LootList.SelectedItems.Count > 0;
            FillQtyBox.IsEnabled = LootList.SelectedItems.Count > 0;
        }

        // Added "Nominal" prefix to variables used in the Nominal dock panel
        private void NominalPercBtn_Click(object sender, RoutedEventArgs e)
        {
            int percentage = 0;
            bool ok = int.TryParse(NominalPercBox.Text.Replace("%", ""), out percentage);
            if (!ok) return;

            foreach (LootType loot in LootList.SelectedItems)
            {
                loot.SetNominal(percentage);
            }

            LootList.Items.Refresh();

            // NominalPercBox.Text = "0";
            UpdateNominalPercValue();
        }

        private void NominalPercSilder_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            NominalPercBox.Text = Math.Round(NominalPercSilder.Value).ToString();
            UpdateNominalPercValue();
        }
        
        private void NominalPercBox_GotFocus(object sender, RoutedEventArgs e)
        {
            NominalPercBox.SelectAll();
        }

        private void NominalPercBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateNominalPercValue();
        }

        private void NominalPercBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateNominalPercValue();
            }
        }
        
        private void UpdateNominalPercValue()
        {
            int newval = 0;
            bool ok = int.TryParse(NominalPercBox.Text.Replace("%", ""), out newval);

            if (ok)
            {
                NominalPercSilder.Value = newval;
                NominalPercBox.Text = String.Format("{0}%", newval);
                NominalPercBtn?.Focus();
            }
        }

        //New code for "Lifetime" manipulations
        private void LifetimePercBtn_Click(object sender, RoutedEventArgs e)
        {
            int percentage = 0;
            bool ok = int.TryParse(LifetimePercBox.Text.Replace("%", ""), out percentage);
            if (!ok) return;

            foreach (LootType loot in LootList.SelectedItems)
            {
                loot.SetLifetime(percentage);
            }

            LootList.Items.Refresh();

            // LifetimePercBox.Text = "0";
            UpdateLifetimePercValue();
        }

        private void LifetimePercSilder_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            LifetimePercBox.Text = Math.Round(LifetimePercSilder.Value).ToString();
            UpdateLifetimePercValue();
        }

        private void LifetimePercBox_GotFocus(object sender, RoutedEventArgs e)
        {
            LifetimePercBox.SelectAll();
        }

        private void LifetimePercBox_LostFocus(object sender, RoutedEventArgs e)
        {
            UpdateLifetimePercValue();
        }

        private void LifetimePercBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                UpdateLifetimePercValue();
            }
        }

        private void UpdateLifetimePercValue()
        {
            int newval = 0;
            bool ok = int.TryParse(LifetimePercBox.Text.Replace("%", ""), out newval);

            if (ok)
            {
                LifetimePercSilder.Value = newval;
                LifetimePercBox.Text = String.Format("{0}%", newval);
                LifetimePercBtn?.Focus();
            }
        }

        private void LifetimePercMaxBtn_Click(object sender, RoutedEventArgs e)
        {
            int time = 3888000;

            foreach (LootType loot in LootList.SelectedItems)
            {
                loot.SetLifetimeMax(time);
            }

            LootList.Items.Refresh();
        }

        // New code for Fill Quantity Min/Max Buttons

        private void FillMinQtyBtn_Click(object sender, RoutedEventArgs e)
        {
            int percentage = 100;

            foreach (LootType loot in LootList.SelectedItems)
            {
                loot.SetMinQuantity(percentage);
            }

            LootList.Items.Refresh();
        }

        private void FillMaxQtyBtn_Click(object sender, RoutedEventArgs e)
        {
            int percentage = 100;

            foreach (LootType loot in LootList.SelectedItems)
            {
                loot.SetMaxQuantity(percentage);
            }

            LootList.Items.Refresh();
        }

        // End of new code

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            LootTable.SaveFile();
        }

        private void PreviewKeyDownHandler(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                case Key.Back:
                    foreach (LootType loot in LootList.SelectedItems)
                    {
                        loot.RemoveType();
                    }
                    break;
            }
        }
    }
}
