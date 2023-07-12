using System;
using System.Windows;
using LiveCharts.Wpf;
using MahApps.Metro.Controls;

namespace ESS_Controller.Windows
{
    public partial class DisplayHistoryWindow : MetroWindow
    {
        public DisplayHistoryWindow()
        {
            InitializeComponent();

            Func<double, string> formatFunc = (x) => string.Format("{0:0.00}", x);

            ovenGraph.AxisY.Add(new Axis
            {
                LabelFormatter = formatFunc
            });
            ovenGraph.AxisX.Add(new Axis
            {
                LabelFormatter = formatFunc
            });
        }

        // Prevent window from closing and hide it. In order to have only 1 object per window.
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            e.Cancel = true;

            Hide();

            HistoryWindow.isDisplayHistoryWindowActive = false;
        }

        // This function retrieves information from the SQLite database and loads it into the window controls.
        public void InitWindow()
        {
            Database.InitDisplayWindow(Database.listOfIDs[HistoryWindow.index], Database.listOfDates[HistoryWindow.index]);
        }

        private void ExportToExcel_Click(object sender, RoutedEventArgs e)
        {
            Database.CreateExcelFile(Database.listOfIDs[HistoryWindow.index], Database.listOfDates[HistoryWindow.index]);
        }
    }
}
