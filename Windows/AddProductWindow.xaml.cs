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
    /// Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddProductWindow : MetroWindow
    {
        public AddProductWindow()
        {
            InitializeComponent();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            e.Cancel = true;

            SettingsWindow.IsAddProductWindowActive = false;
            Hide();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtboxName.Text) || String.IsNullOrWhiteSpace(txtboxName.Text) || txtboxName.Text.Length == 0)
            {
                MessageBox.Show("Error: Please enter a product name.");
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
                MessageBox.Show("Error: Please choose how many cycles.");
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
                MessageBox.Show("Error: Please choose how many cyles.");
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

            productInfo.name = txtboxName.Text;
            productInfo.maxTemp = maxTemp.ToString();
            productInfo.minTemp = minTemp.ToString();
            productInfo.cycles = cycles.ToString();
            productInfo.dwellTime = stayTime.ToString();

            if(Database.AddProduct(productInfo))
                MessageBox.Show("Info: Successfully added " + productInfo.name + " into the database.");

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
        }
    }
}
