using ESS_Controller.Models;
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
    /// Interaction logic for EditProductWindow.xaml
    /// </summary>
    public partial class EditProductWindow : MetroWindow
    {
        public EditProductWindow()
        {
            InitializeComponent();
            
            UpdateComboBox();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            e.Cancel = true;

            SettingsWindow.IsEditProductWindowActive = false;
            Hide();
        }

        private void comboboxProducts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboboxProducts.SelectedItem != null)
            {
                ProductInfo productInfo = Database.GetProductInfoByName(comboboxProducts.SelectedItem.ToString());

                if (productInfo == null)
                {
                    MessageBox.Show("Error: The selected product does not exist in the database.");
                    return;
                }

                txtboxCycles.Text = productInfo.cycles;
                txtboxDwellTime.Text = productInfo.dwellTime;
                txtboxMaxTemp.Text = productInfo.maxTemp;
                txtboxMinTemp.Text = productInfo.minTemp;

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(IntroNewESSWindow))
                    {
                        (window as IntroNewESSWindow).UpdateComboBox();
                    }

                    if (window.GetType() == typeof(RemoveProductWindow))
                    {
                        (window as RemoveProductWindow).UpdateComboBox();
                    }
                }

                UpdateComboBox();
            }
            else
            {
                comboboxProducts.SelectedIndex = -1;

                txtboxCycles.Clear();
                txtboxDwellTime.Clear();
                txtboxMaxTemp.Clear();
                txtboxMinTemp.Clear();
            }
        }

        public void UpdateComboBox()
        {
            comboboxProducts.Items.Clear();

            var productsList = Database.GetProductList();

            foreach (var product in productsList)
                comboboxProducts.Items.Add(product.name);
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if(comboboxProducts.SelectedItem == null)
            {
                MessageBox.Show("Error: Please select a product from the list.");
                return;
            }

            if (String.IsNullOrEmpty(txtboxMaxTemp.Text) || String.IsNullOrWhiteSpace(txtboxMaxTemp.Text) || txtboxMaxTemp.Text.Length == 0)
            {
                MessageBox.Show("Error: Please enter a max temperature.");
                return;
            }

            if (String.IsNullOrEmpty(txtboxMinTemp.Text) || String.IsNullOrWhiteSpace(txtboxMinTemp.Text) || txtboxMinTemp.Text.Length == 0)
            {
                MessageBox.Show("Error: Please enter a min temperature.");
                return;
            }

            if (String.IsNullOrEmpty(txtboxCycles.Text) || String.IsNullOrWhiteSpace(txtboxCycles.Text) || txtboxCycles.Text.Length == 0)
            {
                MessageBox.Show("Error: Please choose how many cyles.");
                return;
            }

            if (String.IsNullOrEmpty(txtboxDwellTime.Text) || String.IsNullOrWhiteSpace(txtboxDwellTime.Text) || txtboxDwellTime.Text.Length == 0)
            {
                MessageBox.Show("Error: Please choose how long to stay on each temperature.");
                return;
            }

            int maxTemp, minTemp, cycles, stayTime;

            if (!Int32.TryParse(txtboxCycles.Text, out cycles))
            {
                MessageBox.Show("Error: Please choose how many cycles.");
                return;
            }

            if (!Int32.TryParse(txtboxDwellTime.Text, out stayTime))
            {
                MessageBox.Show("Error: Please choose how long to stay on each temperature.");
                return;
            }

            if (!Int32.TryParse(txtboxMaxTemp.Text, out maxTemp))
            {
                MessageBox.Show("Error: Please enter a max temperature.");
                return;
            }

            if (!Int32.TryParse(txtboxMinTemp.Text, out minTemp))
            {
                MessageBox.Show("Error: Please enter a min temperature.");
                return;
            }

            if (maxTemp > 82 || maxTemp < -39)
            {
                MessageBox.Show("Error: Please enter a valid maximum temperature. (-39C° - 82C°)");
                return;
            }

            if (minTemp > 82 || minTemp < -39)
            {
                MessageBox.Show("Error: Please enter a valid minimum temperature. (-39C° - 82C°)");
                return;
            }

            if (minTemp > maxTemp)
            {
                MessageBox.Show("Error: Minimum temperature cannot be higher than maximum temperature.");
                return;
            }

            if (cycles < 1)
            {
                MessageBox.Show("Error: Please enter a valid number of cycles. (Must be bigger than 0)");
                return;
            }

            if (stayTime < 1)
            {
                MessageBox.Show("Error: Please enter a valid dwell time number in minutes. (Must be bigger than 0)");
                return;
            }

            ProductInfo productInfo = new ProductInfo();

            productInfo.name = comboboxProducts.SelectedItem.ToString();
            productInfo.maxTemp = maxTemp.ToString();
            productInfo.minTemp = minTemp.ToString();
            productInfo.cycles = cycles.ToString();
            productInfo.dwellTime = stayTime.ToString();

            if (Database.UpdateProduct(productInfo))
                MessageBox.Show("Info: Successfully edited " + productInfo.name + " and updated the database.");

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(IntroNewESSWindow))
                {
                    (window as IntroNewESSWindow).UpdateComboBox();
                }

                if (window.GetType() == typeof(RemoveProductWindow))
                {
                    (window as RemoveProductWindow).UpdateComboBox();
                }
            }

            UpdateComboBox();
        }
    }
}
