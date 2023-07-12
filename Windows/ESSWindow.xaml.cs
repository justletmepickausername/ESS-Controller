using System;
using MahApps.Metro.Controls;
using LiveCharts.Wpf;
using System.Management;
using System.Collections;
using System.Windows;
using ESS_Controller.Models;
using System.Collections.Generic;

namespace ESS_Controller.Windows
{
    public partial class ESSWindow : MetroWindow
    {
        private OvenCommunication ovenComm;

        private ArrayList portNames;
        private ArrayList fullNames;

        public static List<String> productTypes = null;
        public static ArrayList serialNumbers;
        public static String productType;

        public static int maxTemp;
        public static int minTemp;
        public static int cycles;
        public static int stayTime;

        public ESSWindow()
        {
            portNames = new ArrayList();
            fullNames = new ArrayList();
            ovenComm = new OvenCommunication();
            productTypes = new List<string>();

            InitializeComponent();
            InitPortsComboBox();

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

        // Allow the window to close but make sure the set all relevant flags to false.
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            e.Cancel = true;

            Hide();

            btnStartStopESS.IsEnabled = true;
            mySeries.Values.Clear();

            OvenCommunication.ESSProcessIsRunning = false; // ESS process will stop if this flag is false.
            OvenCommunication.IsESSWindowActive = false;
            IntroNewESSWindow.IsESSWindowActive = false;
        }

        // Start/stop the ESS process by clicking a button on the GUI which calls this function.
        private void btnStartStopESS_Click(object sender, RoutedEventArgs e)
        {
            if (OvenCommunication.ESSProcessIsRunning) // If the process is already running... stop it.
            {
                ovenComm.StopESSProcess();

                btnStartStopESS.Content = "Start";
            }
            else // If the process is not running... start it.
            {
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

                if (String.IsNullOrEmpty(txtboxStayTime.Text) || String.IsNullOrWhiteSpace(txtboxStayTime.Text) || txtboxStayTime.Text.Length == 0)
                {
                    MessageBox.Show("Error: Please choose how long to stay on each temperature.");
                    return;
                }

                if (comboBoxPortsList.SelectedItem == null)
                {
                    MessageBox.Show("Error: Please select a COM port.");
                    return;
                }

                int maxTemp, minTemp, cycles, stayTime;

                if (!Int32.TryParse(txtboxCycles.Text, out cycles))
                {
                    MessageBox.Show("Error: Please choose how many cyles.");
                    return;
                }

                if (!Int32.TryParse(txtboxStayTime.Text, out stayTime))
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
                    MessageBox.Show("Error: Please enter a valid maximum temperature. (-39C - 82C)");
                    return;
                }

                if (minTemp > 82 || minTemp < -39)
                {
                    MessageBox.Show("Error: Please enter a valid minimum temperature. (-39C - 82C)");
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
                    MessageBox.Show("Error: Please enter a valid stay time number in minutes. (Must be bigger than 0)");
                    return;
                }

                try
                {
                    ovenComm.SetupSerialPort(extractCOMFromFullName(comboBoxPortsList.SelectedItem.ToString()));

                    ESSWindow.maxTemp = maxTemp;
                    ESSWindow.minTemp = minTemp;
                    ESSWindow.cycles = cycles;
                    ESSWindow.stayTime = stayTime;

                    txtboxCycles.IsEnabled = false;
                    txtboxMaxTemp.IsEnabled = false;
                    txtboxMinTemp.IsEnabled = false;
                    txtboxStayTime.IsEnabled = false;

                    comboBoxPortsList.IsEnabled = false;

                    btnStartStopESS.Content = "Stop";

                    ovenComm.StartESSProcess(maxTemp, minTemp, cycles, stayTime, serialNumbers, productType);
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        // Receives full port name like "ATEN Serial Port (COM1)" and returns "COM1"
        private String extractCOMFromFullName(String fullName)
        {
            int start = fullName.IndexOf("(") + 1;
            int end = fullName.IndexOf(")", start);

            string comName = fullName.Substring(start, end - start);

            return comName;
        }

        // Reads all COMs from Device Manager and lists them in the combobox.
        public void InitPortsComboBox()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PnPEntity");

            foreach (ManagementObject queryObj in searcher.Get())
            {
                if ((queryObj["Caption"] != null) && queryObj["Caption"].ToString().Contains("(COM"))
                {
                    fullNames.Add(queryObj["Caption"].ToString());
                    portNames.Add(extractCOMFromFullName(queryObj["Caption"].ToString()));

                    comboBoxPortsList.Items.Add(" " + queryObj["Caption"].ToString() + " ");
                }
            }
        }

        private void savePathButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog openFileDialog = new System.Windows.Forms.FolderBrowserDialog();

            System.Windows.Forms.DialogResult result = openFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                Database.savedFilePath = openFileDialog.SelectedPath + "\\";
                Database.updateSavePath(Database.savedFilePath);
            }
        }

        private void Main_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            ProductInfo productInfo = new ProductInfo();

            productInfo = Database.GetProductInfoByName(productType);

            if (productInfo != null)
            {
                txtboxMaxTemp.Text = productInfo.maxTemp;
                txtboxMinTemp.Text = productInfo.minTemp;
                txtboxCycles.Text = productInfo.cycles;
                txtboxStayTime.Text = productInfo.dwellTime;
            }
            else
            {
                txtboxMaxTemp.Text = "60";
                txtboxMinTemp.Text = "-30";
                txtboxCycles.Text = "9";
                txtboxStayTime.Text = "30";

                MessageBox.Show("Error: Could not retrieve product information from the database, default values used.");
            }
        }
    }
}
