using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ESS_Controller.Helpers;
using ESS_Controller.Models;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace ESS_Controller.Windows
{
    public partial class IntroNewESSWindow : MetroWindow
    {
        ESSWindow essWindow;

        public static List<ProductInfo> allProductsList;
        public static string selectedProductName = null;
        public static List<String> productType = null;
        public static List<String> temporaryProductType = null;

        private List<UnverifiedSerialNumbers> unverifiedSerialNumbers;

        public static bool IsESSWindowActive { get; set; }
        public static bool IsScanActive { get; set; }

        public IntroNewESSWindow()
        {
            InitializeComponent();
            //InitGUI();

            IsScanActive = false;

            essWindow = new ESSWindow();

            productType = new List<String>();
            temporaryProductType = new List<String>();

            unverifiedSerialNumbers = new List<UnverifiedSerialNumbers>();

            allProductsList = Database.GetProductList();

            if (allProductsList != null && allProductsList.Count > 0)
            {
                comboboxProducts.Items.Clear();
                btnContinue.IsEnabled = true;

                foreach (var product in allProductsList)
                    comboboxProducts.Items.Add(product.name);
            }
            else
            {
                comboboxProducts.Items.Add("No products added.");
                btnContinue.IsEnabled = false;
            }
        }

        // Prevent window from closing and hide it. Clear all information displayed.
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            base.OnClosing(e);

            e.Cancel = true;

            MainWindow.IsIntroNewESSWindowActive = false;

            Hide();

            listViewSerialNumbers.Items.Clear();
            txtboxSerialNumber.Clear();
            comboboxProducts.SelectedIndex = -1;
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            if (IsESSWindowActive == true)
            {
                MessageBox.Show("Error: There may only be one ESS window active at a given time.");
                return;
            }

            if (comboboxProducts.SelectedItem == null)
            {
                MessageBox.Show("Error: Please select a product before moving on.");
                return;
            }

            if (listViewVerifiedSerialNumbers.Items.Count == 0)
            {
                MessageBox.Show("Error: There are no verified serial numbers.");
                return;
            }

            ESSWindow.productType = comboboxProducts.SelectedItem.ToString();
            ESSWindow.serialNumbers = new ArrayList();

            for (int i = 0; i < listViewVerifiedSerialNumbers.Items.Count; i++)
            {
                if (listViewVerifiedSerialNumbers.Items[i] != null)
                {
                    ESSWindow.serialNumbers.Add(listViewVerifiedSerialNumbers.Items[i].ToString());
                    ESSWindow.productTypes.Add(productType[i]);
                }
            }

            Hide();
            MainWindow.IsIntroNewESSWindowActive = false;
            txtboxSerialNumber.Clear();

            essWindow.Show();
            IsESSWindowActive = true;
            OvenCommunication.IsESSWindowActive = true;

            string? serialNumbersExportFilePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string serialNumbersExportFileName = "\\Serial Number Export";
            string serialNumbersExportFileExtension = ".txt";
            int counter = 0;

            if(File.Exists(serialNumbersExportFilePath + serialNumbersExportFileName + serialNumbersExportFileExtension))
            {
                counter++;
            }

        RetryExportFile:

            if (File.Exists(serialNumbersExportFilePath + serialNumbersExportFileName + "(" + counter.ToString() + ")" + serialNumbersExportFileExtension))
            {
                counter++;
                goto RetryExportFile;
            }

            string fileNameAndPath = string.Empty;

            if(counter == 0)
            {
                fileNameAndPath = serialNumbersExportFilePath + serialNumbersExportFileName + serialNumbersExportFileExtension;
            }
            else
            {
                fileNameAndPath = serialNumbersExportFilePath + serialNumbersExportFileName + "(" + counter.ToString() + ")" + serialNumbersExportFileExtension;
            }

            try
            {
                File.AppendAllText(fileNameAndPath, "Serial numbers\n");

                for (int i = 0; i < listViewVerifiedSerialNumbers.Items.Count; i++)
                {
                    File.AppendAllText(fileNameAndPath, listViewVerifiedSerialNumbers.Items[i].ToString() + "\n");
                }
            }
            catch (Exception ex)
            {
                goto RetryExportFile;
            }
        }

        // Event called whenever the ENTER key is pressed whenever the serial number textbox is in focus.
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (String.IsNullOrEmpty(txtboxSerialNumber.Text) || String.IsNullOrWhiteSpace(txtboxSerialNumber.Text) || txtboxSerialNumber.Text.Length == 0)
                {
                    MessageBox.Show("Please enter a serial number.");
                    return;
                }

                if (!comboboxProducts.SelectedItem.ToString().Contains("ORXU"))
                {
                    listViewVerifiedSerialNumbers.Items.Add(txtboxSerialNumber.Text);
                    productType.Add(comboboxProducts.SelectedItem.ToString());
                }

                else
                {
                    listViewSerialNumbers.Items.Add(txtboxSerialNumber.Text);
                    temporaryProductType.Add(comboboxProducts.SelectedItem.ToString());
                }

                if(listViewSerialNumbers.Items.Count >= 10)
                {
                    List<string> itemsToRemove = new List<string>();

                    foreach (string item in listViewSerialNumbers.Items)
                    {
                        bool allowUnitToContinue = true;

                        if (Database.IsUnitOnDatabase(item))
                        {
                            int remainingUnitCycles = Database.GetUnitRemainingCycles(item);

                            if (remainingUnitCycles == 0)
                            {
                                MessageBox.Show("Error: The scanned unit already finished the ESS process.\n\nSerial number: " + item);
                                allowUnitToContinue = false;
                            }
                            else
                            {
                                MessageBoxResult result = ContinueOrNot("Info: The scanned unit has " + remainingUnitCycles + " cycles left.\nKeep in mind if you click yes, it will go the full cycles you define in the next window.\n\nPress yes to continue with the unit.\nPress no to delete the unit's serial number.\n\nSerial number: " + item, "Continue?");

                                if (result == MessageBoxResult.No)
                                    allowUnitToContinue = false;
                            }

                            if (allowUnitToContinue)
                            {
                                var didDo = MongoDBHelper.DidUnitDoTester3B(item);

                                if (didDo)
                                {
                                    var response = MongoDBHelper.DidUnitPassTester3B(item);

                                    if (!response)
                                    {
                                        MessageBoxResult result = ContinueOrNot("Info: The scanned unit did NOT pass Tester3-B.\n\nPress yes to continue with the unit.\nPress no to delete the unit's serial number.\n\nSerial number: " + item, "Continue?");

                                        if (result == MessageBoxResult.No)
                                            allowUnitToContinue = false;
                                    }
                                }
                                else
                                {
                                    MessageBoxResult result = ContinueOrNot("Info: The scanned unit did NOT pass Tester3-B.\n\nPress yes to continue with the unit.\nPress no to delete the unit's serial number.\n\nSerial number: " + item, "Continue?");

                                    if (result == MessageBoxResult.No)
                                        allowUnitToContinue = false;
                                }
                            }
                        }
                        else
                        {
                            if (allowUnitToContinue)
                            {
                                var didDo = MongoDBHelper.DidUnitDoTester3B(item);

                                if (didDo)
                                {
                                    var response = MongoDBHelper.DidUnitPassTester3B(item);

                                    if (!response)
                                    {
                                        MessageBoxResult result = ContinueOrNot("Info: The scanned unit did NOT pass Tester3-B.\n\nPress yes to continue with the unit.\nPress no to delete the unit's serial number.\n\nSerial number: " + item, "Continue?");

                                        if (result == MessageBoxResult.No)
                                            allowUnitToContinue = false;
                                    }
                                }
                            }
                        }

                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            if (allowUnitToContinue)
                            {
                                listViewVerifiedSerialNumbers.Items.Add(item);
                                productType.Add(comboboxProducts.SelectedItem.ToString());
                                itemsToRemove.Add(item);

                                txtblockCountSerialNumbers.Text = "Count: " + listViewVerifiedSerialNumbers.Items.Count.ToString();
                            }
                            else
                            {
                                itemsToRemove.Add(item);
                            }
                        }));
                    }

                    foreach (var item in itemsToRemove)
                        listViewSerialNumbers.Items.Remove(item);

                    txtboxSerialNumber.IsEnabled = true;
                }

                txtboxSerialNumber.Clear();
            }
        }

        // Event called whenever a ListItem in the ListView is double clicked.
        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            listViewSerialNumbers.Items.Remove(listViewSerialNumbers.SelectedItem);
        }

        // Event called whenever the selection of the combobox is changed.
        private void comboboxProducts_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            if (!IsActive)
                return;

            if (comboboxProducts.SelectedItem == null)
                return;

            selectedProductName = comboboxProducts.SelectedItem.ToString();
        }

        // Center in screen.
        public void CenterInScreen()
        {
            Left = (SystemParameters.PrimaryScreenWidth - Width) / 2 + SystemParameters.WorkArea.Left;
            Top = (SystemParameters.PrimaryScreenHeight - Height) / 2 - ((SystemParameters.PrimaryScreenHeight - SystemParameters.WorkArea.Bottom) / 2);
        }

        private void txtboxSerialNumber_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtboxSerialNumber.Text.Length == 0)
                txtblockClickEnterHint.Opacity = 0;
            else txtblockClickEnterHint.Opacity = 0.75;

            if (txtboxSerialNumber.Text.Length == 5)
            {
                if (IsScanActive == true)
                    return;

                IsScanActive = true;

                bool wasScanned = false;
                string textBoxText = txtboxSerialNumber.Text;

                if (listViewSerialNumbers.Items.Count > 0)
                    for (int i = 0; i < listViewSerialNumbers.Items.Count; i++)
                    {
                        if ((string)listViewSerialNumbers.Items[i] == textBoxText)
                        {
                            MessageBox.Show("Error: You have already scanned this unit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            txtboxSerialNumber.Text = String.Empty;
                            wasScanned = true;
                        }
                    }

                if (listViewVerifiedSerialNumbers.Items.Count > 0)
                    for (int i = 0; i < listViewVerifiedSerialNumbers.Items.Count; i++)
                    {
                        if ((string)listViewVerifiedSerialNumbers.Items[i] == textBoxText)
                        {
                            MessageBox.Show("Error: You have already scanned this unit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            txtboxSerialNumber.Text = String.Empty;
                            wasScanned = true;
                        }

                    }

                if (wasScanned)
                {
                    IsScanActive = false;
                    return;
                }

                bool numeric = true;

                for (int index = 0; index < textBoxText.Length; index++)
                    if (textBoxText[index] < '0' || textBoxText[index] > '9')
                    {
                        numeric = false;
                        IsScanActive = false;
                        return;
                    }

                if (numeric)
                {
                    listViewSerialNumbers.Items.Add(textBoxText);
                    temporaryProductType.Add(comboboxProducts.SelectedItem.ToString());
                }

                if (listViewSerialNumbers.Items.Count == 10)
                    txtboxSerialNumber.IsEnabled = false;

                if (listViewSerialNumbers.Items.Count == 10)
                {
                    List<string> itemsToRemove = new List<string>();

                    foreach (string item in listViewSerialNumbers.Items)
                    {
                        bool allowUnitToContinue = true;

                        if (Database.IsUnitOnDatabase(item))
                        {
                            int remainingUnitCycles = Database.GetUnitRemainingCycles(item);

                            if (remainingUnitCycles == 0)
                            {
                                MessageBox.Show("Error: The scanned unit already finished the ESS process.\n\nSerial number: " + item);
                                allowUnitToContinue = false;
                            }
                            else
                            {
                                MessageBoxResult result = ContinueOrNot("Info: The scanned unit has " + remainingUnitCycles + " cycles left.\nKeep in mind if you click yes, it will go the full cycles you define in the next window.\n\nPress yes to continue with the unit.\nPress no to delete the unit's serial number.\n\nSerial number: " + item, "Continue?");

                                if (result == MessageBoxResult.No)
                                    allowUnitToContinue = false;
                            }

                            if (allowUnitToContinue)
                            {
                                var didDo = MongoDBHelper.DidUnitDoTester3B(item);

                                if (didDo)
                                {
                                    var response = MongoDBHelper.DidUnitPassTester3B(item);

                                    if (!response)
                                    {
                                        MessageBoxResult result = ContinueOrNot("Info: The scanned unit did NOT pass Tester3-B.\n\nPress yes to continue with the unit.\nPress no to delete the unit's serial number.\n\nSerial number: " + item, "Continue?");

                                        if (result == MessageBoxResult.No)
                                            allowUnitToContinue = false;
                                    }
                                }
                                else
                                {
                                    MessageBoxResult result = ContinueOrNot("Info: The scanned unit did NOT pass Tester3-B.\n\nPress yes to continue with the unit.\nPress no to delete the unit's serial number.\n\nSerial number: " + item, "Continue?");

                                    if (result == MessageBoxResult.No)
                                        allowUnitToContinue = false;
                                }
                            }
                        }
                        else
                        {
                            if (allowUnitToContinue)
                            {
                                var didDo = MongoDBHelper.DidUnitDoTester3B(item);

                                if (didDo)
                                {
                                    var response = MongoDBHelper.DidUnitPassTester3B(item);

                                    if (!response)
                                    {
                                        MessageBoxResult result = ContinueOrNot("Info: The scanned unit did NOT pass Tester3-B.\n\nPress yes to continue with the unit.\nPress no to delete the unit's serial number.\n\nSerial number: " + item, "Continue?");

                                        if (result == MessageBoxResult.No)
                                            allowUnitToContinue = false;
                                    }
                                }
                            }
                        }


                        if (allowUnitToContinue)
                        {
                            listViewVerifiedSerialNumbers.Items.Add(item);

                            productType.Add(temporaryProductType[0]);
                            temporaryProductType.RemoveAt(0);

                            itemsToRemove.Add(item);

                            txtblockCountSerialNumbers.Text = "Count: " + listViewVerifiedSerialNumbers.Items.Count.ToString();
                        }
                        else
                        {
                            itemsToRemove.Add(item);
                        }
                    }

                    foreach (var item in itemsToRemove)
                        listViewSerialNumbers.Items.Remove(item);

                    txtboxSerialNumber.IsEnabled = true;
                }

                IsScanActive = false;

                txtboxSerialNumber.Clear();
                txtboxSerialNumber.Focus();

                return;
            }

            if (txtboxSerialNumber.Text.Length != 19 && !comboboxProducts.SelectedItem.ToString().Contains("ORXU"))
                return;

            Thread thread = new Thread(HandlingTextBoxTextChanged);

            thread.Start();
        }

        private void HandlingTextBoxTextChanged()
        {
            IsScanActive = true;

            Thread.Sleep(250);

            string[] textWithoutSpaces = null;

            string textBoxText = "";

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                textBoxText = txtboxSerialNumber.Text;
            }));

            if (textBoxText.Length != 19)
                return;
         
            bool skipSerialNumberParsing = false;

            if (textBoxText.Length == 5)
                skipSerialNumberParsing = true;

            if (!skipSerialNumberParsing)
            {
                textWithoutSpaces = textBoxText.Split("  ");

                if (textWithoutSpaces.Length < 2)
                    return;
            }

            bool wasScanned = false;

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                if(!skipSerialNumberParsing)
                {
                    if (textWithoutSpaces.Length == 2)
                    {
                        txtboxSerialNumber.Text = textWithoutSpaces[1];
                        textBoxText = textWithoutSpaces[1];
                    }
                }
                else
                {
                    textBoxText = txtboxSerialNumber.Text;
                }

                if(listViewSerialNumbers.Items.Count > 0)
                    for (int i = 0; i < listViewSerialNumbers.Items.Count; i++)
                    {
                        if ((string)listViewSerialNumbers.Items[i] == textBoxText)
                        {
                            MessageBox.Show("Error: You have already scanned this unit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            txtboxSerialNumber.Text = String.Empty;
                            wasScanned = true;
                        }
                    }

                if(listViewVerifiedSerialNumbers.Items.Count > 0)
                    for(int i = 0; i < listViewVerifiedSerialNumbers.Items.Count; i++)
                    {
                        if((string)listViewVerifiedSerialNumbers.Items[i] == textBoxText)
                        {
                            MessageBox.Show("Error: You have already scanned this unit.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            txtboxSerialNumber.Text = String.Empty;
                            wasScanned = true;
                        }
                    }

            }));

            if (wasScanned)
            {
                IsScanActive = false;
                return;
            }

            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                txtboxSerialNumber.Clear();

                bool numeric = true;

                for (int index = 0; index < textBoxText.Length; index++)
                    if (textBoxText[index] < '0' || textBoxText[index] > '9')
                    {
                        IsScanActive = false;
                        numeric = false;
                        return;
                    }

                if (numeric)
                {
                    listViewSerialNumbers.Items.Add(textBoxText);
                    temporaryProductType.Add(comboboxProducts.SelectedItem.ToString());
                }


                if (listViewSerialNumbers.Items.Count == 10)
                    txtboxSerialNumber.IsEnabled = false;
            }));

            Thread.Sleep(100);

            App.Current.Dispatcher.Invoke(new Action(() =>
            {
                if (listViewSerialNumbers.Items.Count == 10)
                {
                    List<string> itemsToRemove = new List<string>();

                    foreach (string item in listViewSerialNumbers.Items)
                    {
                        bool allowUnitToContinue = true;

                        if (Database.IsUnitOnDatabase(item))
                        {
                            int remainingUnitCycles = Database.GetUnitRemainingCycles(item);

                            if (remainingUnitCycles == 0)
                            {
                                MessageBox.Show("Error: The scanned unit already finished the ESS process.\n\nSerial number: " + item);
                                allowUnitToContinue = false;
                            }
                            else
                            {
                                MessageBoxResult result = ContinueOrNot("Info: The scanned unit has " + remainingUnitCycles + " cycles left.\nKeep in mind if you click yes, it will go the full cycles you define in the next window.\n\nPress yes to continue with the unit.\nPress no to delete the unit's serial number.\n\nSerial number: " + item, "Continue?");

                                if (result == MessageBoxResult.No)
                                    allowUnitToContinue = false;
                            }

                            if (allowUnitToContinue)
                            {
                                var didDo = MongoDBHelper.DidUnitDoTester3B(item);

                                if (didDo)
                                {
                                    var response = MongoDBHelper.DidUnitPassTester3B(item);

                                    if (!response)
                                    {
                                        MessageBoxResult result = ContinueOrNot("Info: The scanned unit did NOT pass Tester3-B.\n\nPress yes to continue with the unit.\nPress no to delete the unit's serial number.\n\nSerial number: " + item, "Continue?");

                                        if (result == MessageBoxResult.No)
                                            allowUnitToContinue = false;
                                    }
                                }
                                else
                                {
                                    MessageBoxResult result = ContinueOrNot("Info: The scanned unit did NOT pass Tester3-B.\n\nPress yes to continue with the unit.\nPress no to delete the unit's serial number.\n\nSerial number: " + item, "Continue?");

                                    if (result == MessageBoxResult.No)
                                        allowUnitToContinue = false;
                                }
                            }
                        }
                        else
                        {
                            if (allowUnitToContinue)
                            {
                                var didDo = MongoDBHelper.DidUnitDoTester3B(item);

                                if (didDo)
                                {
                                    var response = MongoDBHelper.DidUnitPassTester3B(item);

                                    if (!response)
                                    {
                                        MessageBoxResult result = ContinueOrNot("Info: The scanned unit did NOT pass Tester3-B.\n\nPress yes to continue with the unit.\nPress no to delete the unit's serial number.\n\nSerial number: " + item, "Continue?");

                                        if (result == MessageBoxResult.No)
                                            allowUnitToContinue = false;
                                    }
                                }
                            }
                        }

                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            if (allowUnitToContinue)
                            {
                                listViewVerifiedSerialNumbers.Items.Add(item);
                                
                                productType.Add(temporaryProductType[0]);
                                temporaryProductType.RemoveAt(0);

                                itemsToRemove.Add(item);

                                txtblockCountSerialNumbers.Text = "Count: " + listViewVerifiedSerialNumbers.Items.Count.ToString();
                            }
                            else
                            {
                                itemsToRemove.Add(item);
                            }
                        }));
                    }

                    foreach (var item in itemsToRemove)
                        listViewSerialNumbers.Items.Remove(item);

                    txtboxSerialNumber.IsEnabled = true;
                }

                IsScanActive = false;
                txtboxSerialNumber.Focus();

            }));
        }      

        private MessageBoxResult ContinueOrNot(string message, string title)
        {
            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            return result;
        }

        private void Intro_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            UpdateComboBox();
        }

        public void UpdateComboBox()
        {
            allProductsList = Database.GetProductList();

            comboboxProducts.Items.Clear();

            if (allProductsList != null && allProductsList.Count > 0)
            {
                btnContinue.IsEnabled = true;

                foreach (var product in allProductsList)
                    comboboxProducts.Items.Add(product.name);
            }
            else
            {
                comboboxProducts.Items.Add("No products added.");
                btnContinue.IsEnabled = false;
            }
        }

        private void btnTransferToVerified_Click(object sender, RoutedEventArgs e)
        {
            List<string> itemsToRemove = new List<string>();

            foreach (string item in listViewSerialNumbers.Items)
            {
                bool allowUnitToContinue = true;

                if (Database.IsUnitOnDatabase(item))
                {
                    int remainingUnitCycles = Database.GetUnitRemainingCycles(item);

                    if (remainingUnitCycles == 0)
                    {
                        MessageBox.Show("Error: The scanned unit already finished the ESS process.\n\nSerial number: " + item);
                        allowUnitToContinue = false;
                    }
                    else
                    {
                        MessageBoxResult result = ContinueOrNot("Info: The scanned unit has " + remainingUnitCycles + " cycles left.\nKeep in mind if you click yes, it will go the full cycles you define in the next window.\n\nPress yes to continue with the unit.\nPress no to delete the unit's serial number.\n\nSerial number: " + item, "Continue?");

                        if (result == MessageBoxResult.No)
                            allowUnitToContinue = false;
                    }

                    if (allowUnitToContinue)
                    {
                        var didDo = MongoDBHelper.DidUnitDoTester3B(item);

                        if (didDo)
                        {
                            var response = MongoDBHelper.DidUnitPassTester3B(item);

                            if (!response)
                            {
                                MessageBoxResult result = ContinueOrNot("Info: The scanned unit did NOT pass Tester3-B.\n\nPress yes to continue with the unit.\nPress no to delete the unit's serial number.\n\nSerial number: " + item, "Continue?");

                                if (result == MessageBoxResult.No)
                                    allowUnitToContinue = false;
                            }
                        }
                        else
                        {
                            MessageBoxResult result = ContinueOrNot("Info: The scanned unit did NOT pass Tester3-B.\n\nPress yes to continue with the unit.\nPress no to delete the unit's serial number.\n\nSerial number: " + item, "Continue?");

                            if (result == MessageBoxResult.No)
                                allowUnitToContinue = false;
                        }
                    }
                }
                else
                {
                    if (allowUnitToContinue)
                    {
                        var didDo = MongoDBHelper.DidUnitDoTester3B(item);

                        if (didDo)
                        {
                            var response = MongoDBHelper.DidUnitPassTester3B(item);

                            if (!response)
                            {
                                MessageBoxResult result = ContinueOrNot("Info: The scanned unit did NOT pass Tester3-B.\n\nPress yes to continue with the unit.\nPress no to delete the unit's serial number.\n\nSerial number: " + item, "Continue?");

                                if (result == MessageBoxResult.No)
                                    allowUnitToContinue = false;
                            }
                        }
                    }
                }

                Application.Current.Dispatcher.Invoke(new Action(() =>
                {
                    if (allowUnitToContinue)
                    {
                        listViewVerifiedSerialNumbers.Items.Add(item);

                        productType.Add(temporaryProductType[0]);
                        temporaryProductType.RemoveAt(0);

                        itemsToRemove.Add(item);

                        txtblockCountSerialNumbers.Text = "Count: " + listViewVerifiedSerialNumbers.Items.Count.ToString();
                    }
                    else
                    {
                        itemsToRemove.Add(item);
                    }
                }));
            }

            foreach (var item in itemsToRemove)
                listViewSerialNumbers.Items.Remove(item);

            txtboxSerialNumber.IsEnabled = true;
            txtboxSerialNumber.Focus();
        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            if(listViewVerifiedSerialNumbers.Items.Count == 0)
            {
                MessageBox.Show("Error: The serial numbers list is empty.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();

            var result = saveFileDialog.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
                return;

            try
            {
                File.AppendAllText(saveFileDialog.FileName + ".txt", "Serial numbers\n");

                for (int i = 0; i < listViewVerifiedSerialNumbers.Items.Count; i++)
                {
                    File.AppendAllText(saveFileDialog.FileName + ".txt", listViewVerifiedSerialNumbers.Items[i].ToString() + "\n");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return;
        }

        private void btnImport_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();

            var result = openFileDialog.ShowDialog();

            if (result != System.Windows.Forms.DialogResult.OK)
                return;

            if(!File.Exists(openFileDialog.FileName))
            {
                MessageBox.Show("Error: File not found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var fileLines = File.ReadAllLines(openFileDialog.FileName);

            if (!fileLines[0].Contains("Serial numbers"))
            {
                MessageBox.Show("Error: File not supported by ESS Controller.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            for(int i = 1; i < fileLines.Length; i++)
            {
                listViewVerifiedSerialNumbers.Items.Add(fileLines[i]);
                productType.Add(fileLines[i]);
            }
        }
    }
}