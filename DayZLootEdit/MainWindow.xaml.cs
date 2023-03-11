using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace DayZLootEdit {
	/// <summary>
	/// Interaktionslogik für MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		private LootTable LootTable;

		public MainWindow() {
			InitializeComponent();
		}

		private void LoadBtn_Click(object sender, RoutedEventArgs e) {
			OpenFileDialog dlg = new OpenFileDialog();
			dlg.Filter = "XML Files (*.xml)|*.xml|All Files (*.*)|*.*";
			if (dlg.ShowDialog().Value) {
				LootTable = new LootTable(dlg.FileName);
				LootTable.LoadFile();

				LootList.ItemsSource = LootTable.Loot;

				LootList.IsEnabled = true;
				SaveBtn.IsEnabled = true;
			}
		}

		private void LootList_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			LootPercBox.IsEnabled = LootList.SelectedItems.Count > 0;
		}

		private void PercBtn_Click(object sender, RoutedEventArgs e) {
			int percentage = 0;
			bool ok = int.TryParse(PercBox.Text.Replace("%", ""), out percentage);
			if (!ok) return;

			foreach (LootType loot in LootList.SelectedItems) {
				loot.SetNominal(percentage);
			}

			LootList.Items.Refresh();

			PercBox.Text = "0";
			UpdatePercValue();
		}

		private void PercSilder_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
			PercBox.Text = Math.Round(PercSilder.Value).ToString();
			UpdatePercValue();
		}

		private void PercBox_GotFocus(object sender, RoutedEventArgs e) {
			PercBox.SelectAll();
		}

		private void PercBox_LostFocus(object sender, RoutedEventArgs e) {
			UpdatePercValue();
		}

		private void PercBox_KeyUp(object sender, KeyEventArgs e) {
			if (e.Key == Key.Enter) {
				UpdatePercValue();
			}
		}

		private void UpdatePercValue() {
			int newval = 0;
			bool ok = int.TryParse(PercBox.Text.Replace("%", ""), out newval);

			if (ok) {
				PercSilder.Value = newval;
				PercBox.Text = String.Format("{0}%", newval);
				PercBtn?.Focus();
			}
		}

		private void SaveBtn_Click(object sender, RoutedEventArgs e) {
			LootTable.SaveFile();
		}

		private void PreviewKeyDownHandler(object sender, KeyEventArgs e) {
			switch (e.Key) {
				case Key.Delete:
				case Key.Back:
					foreach (LootType loot in LootList.SelectedItems) {
						loot.RemoveType();
					}

					break;
			}
		}

		private void ChangeSearchFilter(Object sender, TextChangedEventArgs e) {
			if (!(sender is TextBox textBox)) {
				Debug.WriteLine("What?");
				return;
			}

			LootList.ItemsSource = LootTable.Loot.Where(l => StrContains(l.Name, textBox.Text, StringComparison.CurrentCultureIgnoreCase));
			this.LootList.Items.Refresh();
			Debug.WriteLine("Updated");
		}

		public static bool StrContains(string source, string toCheck, StringComparison comp) {
			return source?.IndexOf(toCheck, comp) >= 0;
		}
	}
}