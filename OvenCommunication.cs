using ESS_Controller.Helpers;
using ESS_Controller.Windows;
using LiveCharts.Wpf;
using System;
using System.Collections;
using System.Diagnostics;
using System.IO.Ports;
using System.Threading;
using System.Timers;
using System.Windows;

namespace ESS_Controller
{
    class OvenCommunication
    {
        /* Variables/members for LiveCharts chart/graph performance issues */

        ArrayList redundantList;
        ArrayList fullList;


        /* Serial port and oven related variables/members */

        static SerialPort _serialPort = new SerialPort();

        public const int MB_ADDRESS = 1;
        public const int MB_WRITE = 6;
        public const int MB_READ = 3;

        public const int MB_SP_REG_H = 1;
        public const int MB_SP_REG_L = 44;

        public const int MB_CURRENT_TEMP_REG_H = 0;
        public const int MB_CURRENT_TEMP_REG_L = 100;



        /* ESS process related variables/members */

        public static bool ESSProcessIsRunning { get; set; }
        public static bool IsESSWindowActive { get; set; }
        public static bool waitForTwentyFiveDegrees = true;

        private int maxTemp { get; set; }
        private int minTemp { get; set; }
        private int cycles { get; set; }
        private int stayTime { get; set; }

        public static ArrayList serialNumbers;
        public static String productType;

        public static String fileName;

        public static DateTime currentDate;

        /* Functions and methods */

        // Set up all of the serial port parameters according to the oven's parameters.
        public void SetupSerialPort(String portName)
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort = new SerialPort();

                _serialPort.PortName = portName;
                _serialPort.BaudRate = 9600;
                _serialPort.Parity = Parity.None;
                _serialPort.DataBits = 8;
                _serialPort.StopBits = StopBits.One;
                _serialPort.ReadBufferSize = 4096;

                _serialPort.ReadTimeout = 2000;
                _serialPort.WriteTimeout = 2000;

                OpenPort();
                _serialPort.DiscardInBuffer();
            }
            else
            {
                ClosePort();

                _serialPort = new SerialPort();

                _serialPort.PortName = portName;
                _serialPort.BaudRate = 9600;
                _serialPort.Parity = Parity.None;
                _serialPort.DataBits = 8;
                _serialPort.StopBits = StopBits.One;
                _serialPort.ReadBufferSize = 4096;

                _serialPort.ReadTimeout = 2000;
                _serialPort.WriteTimeout = 2000;

                OpenPort();
                _serialPort.DiscardInBuffer();
            }
        }

        // Open the serial port.
        public void OpenPort()
        {
            if (!_serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Open();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Please choose a valid COM port.\n\n" + ex.Message);
                }
            }
        }

        // Close the serial port.
        public void ClosePort()
        {
            if (_serialPort.IsOpen)
                _serialPort.Close();
        }

        // Send a command to the oven via the serial port.
        public bool WriteCommand(byte Address, byte Command, byte RegH, byte RegL, Int32 Data)
        {
            if (!_serialPort.IsOpen)
                return false;

            int CRC = 65535;
            UInt16 CRC_H = 0;
            UInt16 CRC_L = 0;
            ushort Poly = 40961;
            byte payl_H = 0; // 0x0
            byte payl_L = 0; // 0x0
            int sendData = Data; // double sendData = Data;

            if (Data < 0)
                sendData = -Data;

            payl_H = (byte)(sendData >> 8);
            payl_L = (byte)(sendData - (payl_H * 256));

            if (Data < 0)
            {
                payl_H = (byte)~payl_H;
                payl_L = (byte)((~payl_L) + 1);
            }

            byte[] MBmessage = new byte[8];

            MBmessage[0] = Address;
            MBmessage[1] = Command;
            MBmessage[2] = RegH;
            MBmessage[3] = RegL;
            MBmessage[4] = payl_H;
            MBmessage[5] = payl_L;

            for (int j = 0; j < 6; j++)
            {
                CRC = CRC ^ MBmessage[j];

                for (int i = 0; i < 8; i++)
                {
                    if ((CRC & 1) == 1)
                    {
                        CRC = CRC >> 1;
                        CRC = CRC ^ Poly;
                    }
                    else CRC = CRC >> 1;
                }
            }

            CRC_H = (ushort)(CRC >> 8);
            CRC_L = (ushort)(CRC - (CRC_H << 8));

            MBmessage[6] = (byte)CRC_L;
            MBmessage[7] = (byte)CRC_H;

            _serialPort.Write(MBmessage, 0, MBmessage.Length);

            String tempStr = "00" + MBmessage[0].ToString();
            String strCommand = tempStr.Substring(tempStr.Length - 2);

            for (int i = 1; i < MBmessage.Length; i++)
            {
                tempStr = "00" + MBmessage[i].ToString();
                strCommand = strCommand + "," + tempStr.Substring(tempStr.Length - 2);
            }

            return true;
        }

        // Read the oven's response via the serial port.
        public byte[] ReadResponse()
        {
            bool noneZeroFoundFlag = false;
            byte[] readBytes = new byte[8];

            if (_serialPort.IsOpen)
                _serialPort.Read(readBytes, 0, 8);

            for(int i = 0; i < readBytes.Length; i++)
            {
                if (readBytes[i] != 0)
                    noneZeroFoundFlag = true;
            }

            if(noneZeroFoundFlag)
            {
                // Set a global bool to indicate lack of communication and stop the process and whatever else that has to be done.
            }

            return readBytes;
        }

        // Read the oven's temperature register via the serial port.
        public double readTempRegister(ushort address)
        {
            if (!_serialPort.IsOpen)
                return -9999;

            byte[] bytesRead;

            ushort addressHigh = 0;
            ushort addressLow = 0;

            addressHigh = (ushort)(address >> 8);
            addressLow = (ushort)(address - (addressHigh << 8));

            _serialPort.DiscardInBuffer();

            WriteCommand(1, 3, (byte)addressHigh, (byte)addressLow, 1);

            Thread.Sleep(500);
            bytesRead = ReadResponse();
            Thread.Sleep(100);

            double temp = 0;

            temp = (bytesRead[3] * 256) + bytesRead[4];

            if (temp > 32768)
                temp = -(65536 - temp);

            temp /= 10;

            return temp;
        }

        // Set the oven's temperature via the serial port.
        public void SetTemp(int temp)
        {
            try
            {
                // Temp is multiplied by 10 because the oven reads "234" as 23.4
                WriteCommand(MB_ADDRESS, MB_WRITE, MB_SP_REG_H, MB_SP_REG_L, temp * 10);
            }
            catch (Exception ex)
            {
                StopESSProcess();
                waitForTwentyFiveDegrees = false;

                MessageBox.Show("Error: Communication to the oven was lost, stopping the process. Please manually set the oven to 25C°.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Get the oven's curent temperature via the serial port.
        public double GetTemp()
        {
            double temp = -69;

            try
            {
                temp = readTempRegister(100);

                Debug.WriteLine("Inside GetTemp(): " + temp.ToString());
            }
            catch(Exception ex)
            {
                StopESSProcess();
                waitForTwentyFiveDegrees = false;

                MessageBox.Show("Error: Communication to the oven was lost, stopping the process. Please manually set the oven to 25C°.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            return temp;
        }



        /* ESS process functions/methods */

        public async void StartESSProcess(int maxTemp, int minTemp, int cycles, int stayTime, ArrayList snList, String productName)
        {
            fullList = new ArrayList();

            ESSProcessIsRunning = true;

            this.maxTemp = maxTemp;
            this.minTemp = minTemp;
            this.cycles = cycles;
            this.stayTime = stayTime;
            serialNumbers = snList;
            productType = productName;
            currentDate = DateTime.Now;

            fileName = @"R:\Analog Links\ESS Reports\" + DateTime.Now.ToString("MMMM dd");

            //await MongoDBHelper.InitiateESSProcess(maxTemp, minTemp, cycles, stayTime);

            Database.SetupESSDatabase(productType, maxTemp, minTemp, cycles, stayTime);

            Database.InitiateESSProcess(maxTemp, minTemp, cycles, stayTime);

            for (int i = 0; i < serialNumbers.Count; i++)
            {
                Database.AddSerialNumber(serialNumbers[i].ToString(), Database.tID);

                
                //await MongoDBHelper.InitiateESSProcessOnOpsysMongoDBDatabaseBySerialNumber(serialNumbers[i].ToString(), ESSWindow.productTypes[i]);

                Database.LinkUnitToESS(serialNumbers[i].ToString(), productName, Database.currentESSProcessID);
            }
            Thread threadESSProcess = new Thread(new ThreadStart(ESSProcess));
            threadESSProcess.Start();
        }

        public void StopESSProcess()
        {
            waitForTwentyFiveDegrees = false;
            ESSProcessIsRunning = false;

            SetTemp(25);

            //Database.CreateExcelFile();

            if (_serialPort.IsOpen)
                _serialPort.Close();

            Application.Current.Dispatcher.Invoke(delegate {

                foreach (Window item in Application.Current.Windows)
                {
                    if (!IsESSWindowActive)
                        return;

                    DateTime currentTime = new DateTime();
                    currentTime = DateTime.Now;

                    if (item.Name == "Main")
                    {
                        ((Windows.ESSWindow)item).btnStartStopESS.IsEnabled = false;
                        ((Windows.ESSWindow)item).txtboxCycles.IsEnabled = true;
                        ((Windows.ESSWindow)item).txtboxMaxTemp.IsEnabled = true;
                        ((Windows.ESSWindow)item).txtboxMinTemp.IsEnabled = true;
                        ((Windows.ESSWindow)item).txtboxStayTime.IsEnabled = true;
                        ((Windows.ESSWindow)item).comboBoxPortsList.IsEnabled = true;
                    }

                    if (!IsESSWindowActive)
                        return;
                }
            });
        }


        public async void ESSProcess()
        {
            int lastFinishedCycle = 0;

            int halfCycle = stayTime;

            SetTemp(25);

            Thread.Sleep(300);

            while (!DidOvenReachTempWithoutLogging(25)) { Thread.Sleep(1000); }

            for (int currentCycle = 0; currentCycle < cycles; currentCycle++)
            {
                int halfCycleInSeconds = halfCycle * 60; // Convert stayTime from minutes to seconds.

                SetTemp(minTemp);

                Thread.Sleep(300);

                while (!DidOvenReachTemp(minTemp)) { Thread.Sleep(1000 * 10); }

                Thread.Sleep(300);

                if (!ESSProcessIsRunning)
                    break;

                while (halfCycleInSeconds > 0)
                {
                    if (!ESSProcessIsRunning)
                        break;

                    Thread.Sleep(1000 * 10);

                    LogTemp(GetTemp());

                    halfCycleInSeconds -= 10;
                }

                if (!ESSProcessIsRunning)
                    break;

                halfCycleInSeconds = halfCycle * 60; // Convert stayTime from minutes to seconds for second half cycle.

                SetTemp(maxTemp);

                Thread.Sleep(300);

                while (!DidOvenReachTemp(maxTemp)) { Thread.Sleep(1000 * 10); }

                Thread.Sleep(300);

                if (!ESSProcessIsRunning)
                    break;

                while (halfCycleInSeconds > 0)
                {
                    if (!ESSProcessIsRunning)
                        break;

                    Thread.Sleep(1000 * 10);

                    LogTemp(GetTemp());

                    halfCycleInSeconds -= 10;
                }

                lastFinishedCycle = currentCycle + 1;

                if (!ESSProcessIsRunning)
                    break;
            }

            SetTemp(25);

            Thread.Sleep(300);

            //if(!ESSProcessIsRunning)
            //{
            //    MessageBox.Show("Info: You have stopped the process while it was running. Completed " + lastFinishedCycle.ToString() + " cycles.\nPlease wait until the oven reaches 25C°.", "Stopped", MessageBoxButton.OK, MessageBoxImage.Information);
            //}

            while (!DidOvenReachTemp(25)) { Thread.Sleep(1000 * 10); }

            if (ESSProcessIsRunning)
            {
                //await MongoDBHelper.FinishESSProcess(MongoDBHelper.currentESSProcessID, lastFinishedCycle);
                Database.FinishESSProcess(Database.currentESSProcessID, lastFinishedCycle);
                //await MongoDBHelper.FinishESSProcessOnOpsysMongoDBDatabaseForAllUnitsOnThisESSProcess();

                MessageBox.Show("Info: ESS process finished successfully! " + lastFinishedCycle.ToString() + " cycles finished.", "Finished", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                //await MongoDBHelper.PauseESSProcess(MongoDBHelper.currentESSProcessID, lastFinishedCycle);
                Database.PauseESSProcess(Database.currentESSProcessID, lastFinishedCycle);
                //await MongoDBHelper.PauseESSProcessOnOpsysMongoDBDatabaseForAllUnitsOnThisESSProcess();

                MessageBox.Show("Info: ESS process stopped by the user. Completed " + lastFinishedCycle.ToString() + " cycles.", "Stopped", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            ESSProcessIsRunning = false;

            StopESSProcess();

            return;
        }

        public void LogTemp(double temp)
        {
            if (!ESSProcessIsRunning)
                return;

            if (!IsESSWindowActive)
                return;

            Application.Current.Dispatcher.Invoke(delegate {

                foreach (Window item in Application.Current.Windows)
                {
                    if (!IsESSWindowActive)
                        return;

                    DateTime currentTime = new DateTime();
                    currentTime = DateTime.Now;

                    if (item.Name == "Main")
                    {
                        fullList.Add(temp);

                        RefactorLineSeriesForPerformance();
                    }

                    if (!IsESSWindowActive)
                        return;
                }
            });

            Database.AddTemperature(temp, Database.tID);
        }

        public void RefactorLineSeriesForPerformance()
        {
            redundantList = new ArrayList();

            int maxAllowedPoints = 500;
            int skips = 1;

            Application.Current.Dispatcher.Invoke(delegate {

                foreach (Window item in Application.Current.Windows)
                {
                    if (!IsESSWindowActive)
                        return;

                    if (item.Name == "Main")
                    {
                        //int count = ((Windows.ESSWindow)item).mySeries.Values.Count;
                        int count = fullList.Count;

                        if (count > maxAllowedPoints)
                            skips = (int)Math.Ceiling(count / (float)maxAllowedPoints);

                        for(int i = 0; i < count; i += skips)
                        {
                            //var valueToAdd = ((Windows.ESSWindow)item).mySeries.Values[i];
                            var valueToAdd = fullList[i];

                            redundantList.Add(valueToAdd);
                        }

                        ((Windows.ESSWindow)item).mySeries.Values.Clear();

                        for(int iteration = 0; iteration < redundantList.Count; iteration++)
                        {
                            ((Windows.ESSWindow)item).mySeries.Values.Add(redundantList[iteration]);
                        }
                    }

                    if (!IsESSWindowActive)
                        return;
                }
            });
        }

        public bool DidOvenReachTemp(double temp)
        {
            if (!ESSProcessIsRunning && !waitForTwentyFiveDegrees)
                return true;

            double currentTemp = GetTemp();

            LogTemp(currentTemp);

            if (currentTemp > temp)
            {
                if (currentTemp - temp > 2)
                    return false;

                else return true;
            }
            else if (currentTemp < temp)
            {
                if (temp - currentTemp > 2)
                    return false;

                else return true;
            }

            return true;
        }

        public bool DidOvenReachTempWithoutLogging(double temp)
        {
            if (!ESSProcessIsRunning && !waitForTwentyFiveDegrees)
                return true;

            double currentTemp = GetTemp();

            //LogTemp(currentTemp);

            if (currentTemp > temp)
            {
                if (currentTemp - temp > 2)
                    return false;

                else return true;
            }
            else if (currentTemp < temp)
            {
                if (temp - currentTemp > 2)
                    return false;

                else return true;
            }

            return true;
        }
    }
}
