using System;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls;

namespace ESS_Controller.Windows
{
    public partial class HistoryWindow : MetroWindow
    {
        public static bool isDisplayHistoryWindowActive;
        public static DisplayHistoryWindow displayHistoryWindow;

        public static int index = -1;

        public HistoryWindow()
        {
            InitializeComponent();

            displayHistoryWindow = new DisplayHistoryWindow();
            //Database.InitHistoryWindow();
        }

        // Prevent window from closing and hide it.
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            e.Cancel = true;

            Hide();

            MainWindow.IsHistoryWindowActive = false;
        }

        // Event for double clicking a ListItem in the ListView.
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (isDisplayHistoryWindowActive) // If the display history window is active, apply new information to it from the database and re-focus it on screen.
            {
                index = listViewHistory.SelectedIndex;

                displayHistoryWindow.Focus();

                displayHistoryWindow.InitWindow();
            }
            else // If the display history window is not active, apply new information to it from the database and display it.
            {
                index = listViewHistory.SelectedIndex;

                displayHistoryWindow.Show();

                displayHistoryWindow.InitWindow();
            }
        }

        // Event which is called whenever a window is set to this.Hide(), this.Show().
        private void MetroWindow_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            Database.InitHistoryWindow();
        }
    }
}
