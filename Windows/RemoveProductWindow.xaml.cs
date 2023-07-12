using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace ESS_Controller.Windows
{
    /// <summary>
    /// Interaction logic for RemoveProductWindow.xaml
    /// </summary>
    public partial class RemoveProductWindow : MetroWindow
    {
        public RemoveProductWindow()
        {
            InitializeComponent();

            UpdateComboBox();
        }

        public void UpdateComboBox()
        {
            comboboxProducts.Items.Clear();

            var productsList = Database.GetProductList();

            foreach (var product in productsList)
                comboboxProducts.Items.Add(product.name);
        }

        private void comboboxProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if(comboboxProducts.SelectedItem == null)
            {
                MessageBox.Show("Error: Please select a product from the list.");
                return;
            }

            if (Database.RemoveProductByName(comboboxProducts.SelectedItem.ToString()))
                MessageBox.Show("Info: Successfully removed the product from the database.");

            UpdateComboBox();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(IntroNewESSWindow))
                {
                    (window as IntroNewESSWindow).UpdateComboBox();
                }
            }
        }

        private void MetroWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateComboBox();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            e.Cancel = true;

            SettingsWindow.IsRemoveProductWindowActive = false;
            Hide();
        }
    }
}
