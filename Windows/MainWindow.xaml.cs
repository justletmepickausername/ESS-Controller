using System;
using System.Windows;
using ESS_Controller.Helpers;
using ESS_Controller.Models;
using MahApps.Metro.Controls;

namespace ESS_Controller.Windows
{
    public partial class MainWindow : MetroWindow
    {
        HistoryWindow historyWindow;
        public static bool IsHistoryWindowActive { get; set; }

        IntroNewESSWindow introNewESSWindow;
        public static bool IsIntroNewESSWindowActive { get; set; }

        SettingsWindow settingsWindow;
        public static bool IsSettingsWindowActive { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            Database.InitESSDatabase();

            historyWindow = new HistoryWindow();
            introNewESSWindow = new IntroNewESSWindow();
            settingsWindow = new SettingsWindow();
        }

        // If this window is closed, shutdown the entire application.
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            Application.Current.Shutdown();
        }

        // Event called when "New ESS Process" button is clicked.
        private void btnNewESSProcess_Click(object sender, RoutedEventArgs e)
        {
            if(OvenCommunication.IsESSWindowActive) // If the ESS window is already active, do not allow more. Only 1 ESS process can take place at a given time.
            {
                MessageBox.Show("Error: There can only be one ESS process active at a given time.");
                return;
            }

            if(!IsIntroNewESSWindowActive)
            {
                IsIntroNewESSWindowActive = true;
                introNewESSWindow.Show();

                introNewESSWindow.txtboxSerialNumber.Clear();
                introNewESSWindow.comboboxProducts.SelectedIndex = -1;
                introNewESSWindow.listViewSerialNumbers.Items.Clear();
            }
            else
            {
                IsIntroNewESSWindowActive = false;
                introNewESSWindow.Hide();
            }
        }

        // Event called when the "History" button is clicked.
        private void btnHistory_Click(object sender, RoutedEventArgs e)
        {
            if (IsHistoryWindowActive)
            {
                IsHistoryWindowActive = false;
                historyWindow.Hide();
            }
            else
            {
                IsHistoryWindowActive = true;
                historyWindow.Show();
            }
        }

        // Event called when the "Exit" button is clicked.
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            if (IsSettingsWindowActive)
            {
                IsSettingsWindowActive = false;
                settingsWindow.Hide();
            }
            else
            {
                IsSettingsWindowActive = true;
                settingsWindow.Show();
            }
        }
    }
}
