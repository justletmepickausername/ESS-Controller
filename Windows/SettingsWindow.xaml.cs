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
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : MetroWindow
    {
        AddProductWindow addProductWindow;
        public static bool IsAddProductWindowActive { get; set; }

        RemoveProductWindow removeProductWindow;
        public static bool IsRemoveProductWindowActive { get; set; }

        EditProductWindow editProductWindow;
        public static bool IsEditProductWindowActive { get; set; }

        public SettingsWindow()
        {
            InitializeComponent();

            addProductWindow = new AddProductWindow();
            IsAddProductWindowActive = false;

            removeProductWindow = new RemoveProductWindow();
            IsRemoveProductWindowActive = false;

            editProductWindow = new EditProductWindow();
            IsEditProductWindowActive = false;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            e.Cancel = true;

            Hide();
            MainWindow.IsSettingsWindowActive = false;
        }

        private void btnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            if (IsAddProductWindowActive)
                return;

            addProductWindow.Show();
            IsAddProductWindowActive = true;
        }

        private void btnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            if (IsEditProductWindowActive)
                return;

            editProductWindow.Show();
            IsEditProductWindowActive = true;
        }

        private void btnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            if (IsRemoveProductWindowActive)
                return;

            removeProductWindow.Show();
            IsRemoveProductWindowActive = true;
        }
    }
}
