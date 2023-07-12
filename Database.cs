using ESS_Controller.Helpers;
using ESS_Controller.Models;
using MongoDB.Driver;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;

namespace ESS_Controller
{
    class Database
    {
        public static long currentESSProcessID;

        private static SQLiteConnection sqConnection;
        private static SQLiteCommand sqCommand;

        private static String dbPath = System.Environment.CurrentDirectory;
        private static String dbFilePath = dbPath + "\\ESS Database.db";

        public static String savedFilePath;

        public static long tID;

        public static List<int> listOfIDs;
        public static List<string> listOfDates;
        private static ArrayList redundantList;
        private static ArrayList fullList;

        public static void CreateDBFile() // Create database file if it doesn't exist.
        {
            if (!String.IsNullOrEmpty(dbPath) && !Directory.Exists(dbPath))
            {
                Directory.CreateDirectory(dbPath);
            }

            dbFilePath = dbPath + "\\ESS Database.db";

            if (!File.Exists(dbFilePath))
            {
                SQLiteConnection.CreateFile(dbFilePath);
            }
        }

        public static void InitESSDatabase() // Initialize database when the app opens. This is called on MainWindow's constructor.
        {
            CreateDBFile();
            CreateTables();
        }

        public static void SetupESSDatabase(String productType, int maxTemp, int minTemp, int cycles, int stayTime) // Setup database if not set up already and start new ESS process.
        {
            StartNewESSProcess(productType, maxTemp, minTemp, cycles, stayTime);
        }

        public static void CreateTables() // Create tables if they don't exist.
        {
            if (!OpenConnection())
                return;

            String query = String.Format("CREATE TABLE IF NOT EXISTS SerialNumbers (SerialNumber VARCHAR(30), ID INTEGER)");

            sqCommand = sqConnection.CreateCommand();
            sqCommand.CommandText = query;
            sqCommand.ExecuteNonQuery();

            query = String.Format("CREATE TABLE IF NOT EXISTS ESSInfo (StartTime VARCHAR(40), ProductType VARCHAR(30), maxTemp INTEGER, minTemp INTEGER, cycles INTEGER, stayTime INTEGER, ID INTEGER PRIMARY KEY AUTOINCREMENT)");

            sqCommand = sqConnection.CreateCommand();
            sqCommand.CommandText = query;
            sqCommand.ExecuteNonQuery();

            query = String.Format("CREATE TABLE IF NOT EXISTS Temperatures (Temp VARCHAR(50), date VARCHAR(40), ID INTEGER, uniqueID INTEGER PRIMARY KEY AUTOINCREMENT)");

            sqCommand = sqConnection.CreateCommand();
            sqCommand.CommandText = query;
            sqCommand.ExecuteNonQuery();

            query = String.Format("CREATE TABLE IF NOT EXISTS ESSEntries (maxTemp INTEGER, minTemp INTEGER, cyclesToPerform INTEGER, cyclesFinished INTEGER, status VARCHAR(20), StartTime VARCHAR(40), EndTime VARCHAR(40), dwellTime INTEGER, ID INTEGER PRIMARY KEY AUTOINCREMENT)");

            sqCommand = sqConnection.CreateCommand();
            sqCommand.CommandText = query;
            sqCommand.ExecuteNonQuery();

            query = String.Format("CREATE TABLE IF NOT EXISTS UnitEntries (serialNumber VARCHAR(20), essID INTEGER, unitType VHARCHAR(100), ID INTEGER PRIMARY KEY AUTOINCREMENT)");

            sqCommand = sqConnection.CreateCommand();
            sqCommand.CommandText = query;
            sqCommand.ExecuteNonQuery();

            query = String.Format("CREATE TABLE IF NOT EXISTS ProductInfo (name VARCHAR(50), maxTemp INTEGER, minTemp INTEGER, cycles INTEGER, stayTime INTEGER, ID INTEGER PRIMARY KEY AUTOINCREMENT)");

            sqCommand = sqConnection.CreateCommand();
            sqCommand.CommandText = query;
            sqCommand.ExecuteNonQuery();

            query = String.Format("CREATE TABLE IF NOT EXISTS InternalInfo (filePath VARCHAR(100), ID INTEGER)");

            sqCommand = sqConnection.CreateCommand();
            sqCommand.CommandText = query;
            sqCommand.ExecuteNonQuery();

            string sql = String.Format("SELECT * FROM InternalInfo WHERE ID = '{0}'", 1);
            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                savedFilePath = reader["filePath"].ToString();
            }
            else
            {
                savedFilePath = @"R:\Analog Links\ESS Reports\";

                sql = String.Format("INSERT INTO InternalInfo (filePath, ID) values ('{0}', '{1}')", "R:\\Analog Links\\ESS Reports\\", 1);
                command = new SQLiteCommand(sql, sqConnection);
                command.ExecuteNonQuery();
            }

            reader.Close();

            CloseConnection();
        }

        public static bool OpenConnection() // Open connection to the database until explicitly closed.
        {
            bool openedSuccessfully = true;
            String dbConn = string.Format("Data Source={0};", dbFilePath);

            sqConnection = new SQLiteConnection(dbConn);

            try
            {
                sqConnection.Open();
            }
            catch (Exception ex)
            {
                openedSuccessfully = false;
                MessageBox.Show(ex.Message.ToString());
            }

            return openedSuccessfully;
        }

        public static void CloseConnection() // Close connection to the database.
        {
            if (sqConnection == null)
                return;

            sqConnection.Close();
            GC.Collect();
        }

        public static long StartNewESSProcess(String productType, int maxTemp, int minTemp, int cycles, int stayTime) // Insert new ESS process info into database.
        {
            CloseConnection();

            if (!OpenConnection())
                return -1;

            String sql = String.Format("INSERT INTO ESSInfo (ProductType, maxTemp, minTemp, cycles, stayTime, StartTime) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}')", productType, maxTemp, minTemp, cycles, stayTime, DateTime.Now.ToString("g"));

            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);

            command.ExecuteNonQuery();

            long ID = sqConnection.LastInsertRowId;
            tID = ID;

            CloseConnection();

            return ID;
        }

        public static void AddTemperature(double temp, long ID) // Add temperature sample to database. One sample at a time.
        {
            if (!OpenConnection())
                return;

            String sql = String.Format("INSERT INTO Temperatures (Temp, date, ID) values ('{0}', '{1}', '{2}')", temp.ToString(), DateTime.Now.ToString("g"), (int)ID);
            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            command.ExecuteNonQuery();

            CloseConnection();
        }

        public static void AddSerialNumber(String sn, long ID) // Add serial number to database. One serial number at a time.
        {
            if (!OpenConnection())
                return;

            String sql = String.Format("INSERT INTO SerialNumbers (SerialNumber, ID) values ('{0}', '{1}')", sn, (int)ID);
            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            command.ExecuteNonQuery();

            CloseConnection();
        }

        public static void InitiateESSProcess(int maxTemp, int minTemp, int cyclesToPerform, int dwellTime)
        {
            if (!OpenConnection())
                return;

            String sql = String.Format("INSERT INTO ESSEntries (maxTemp, minTemp, cyclesToPerform, cyclesFinished, status, StartTime, EndTime, dwellTime) values ('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')",
                maxTemp, minTemp, cyclesToPerform, 0, "ongoing", DateTime.Now.ToString("g"), "", dwellTime);

            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            command.ExecuteNonQuery();

            currentESSProcessID = sqConnection.LastInsertRowId;

            CloseConnection();
        }

        private static bool CheckIfESSProcessAlreadyExistsByID(long essID)
        {
            if (!OpenConnection())
            {
                MessageBox.Show("Error: The database could not be accessed.");
                return true;
            }

            string sql = String.Format("SELECT * FROM 'ESSEntries' WHERE ID = '{0}'", essID);

            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            if (reader.Read())
                return true;

            return false;
        }

        public static bool PauseESSProcess(long essID, int finishedCycles)
        {
            if (!OpenConnection())
                return false;

            if (!CheckIfESSProcessAlreadyExistsByID(essID))
            {
                MessageBox.Show("Error: The selected product does not exist.");
                return false;
            }

            String sql = String.Format("UPDATE ESSEntries SET status = '{0}', cyclesFinished = '{1}', EndTime = '{2}' WHERE ID = '{3}'",
                "partial", finishedCycles, DateTime.Now.ToString("g"), essID);

            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            command.ExecuteNonQuery();

            CloseConnection();

            return true;
        }

        public static bool DidUnitDoTester3B(string serialNumber)
        {


            return false;
        }

        public static bool IsUnitPassedTester3B(string serialNumber)
        {
            

            return true;
        }

        public static bool FinishESSProcess(long essID, int finishedCycles)
        {
            if (!OpenConnection())
                return false;

            if (!CheckIfESSProcessAlreadyExistsByID(essID))
            {
                MessageBox.Show("Error: The selected product does not exist.");
                return false;
            }

            String sql = String.Format("UPDATE ESSEntries SET status = '{0}', cyclesFinished = '{1}', EndTime = '{2}' WHERE ID = '{3}'",
                "finished", finishedCycles, DateTime.Now.ToString("g"), essID);

            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            command.ExecuteNonQuery();

            CloseConnection();

            return true;
        }

        public static void LinkUnitToESS(string serialNumber, string unitType, long essID)
        {
            if (!OpenConnection())
                return;

            String sql = String.Format("INSERT INTO UnitEntries (serialNumber, essID, unitType) values ('{0}', '{1}', '{2}')",
                serialNumber, essID, unitType);

            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            command.ExecuteNonQuery();

            CloseConnection();
        }

        public static bool IsUnitOnDatabase(string serialNumber)
        {
            if (!OpenConnection())
            {
                MessageBox.Show("Error: The database could not be accessed.");
                return true;
            }

            string sql = String.Format("SELECT * FROM 'UnitEntries' WHERE serialNumber = '{0}'", serialNumber);

            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            if (reader.Read())
                return true;

            CloseConnection();

            return false;
        }

        public static ESSEntry GetESSInformationByESSID(long essID)
        {
            if (!OpenConnection())
            {
                MessageBox.Show("Error: The database could not be accessed.");
                return null;
            }

            ESSEntry essEntry = null;

            string sql = String.Format("SELECT * FROM 'ESSEntries' WHERE ID = '{0}'", essID);
            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                essEntry = new ESSEntry();

                essEntry.status = reader["status"].ToString();

                essEntry.maxTemp = (int)(long)reader["maxTemp"];
                essEntry.minTemp = (int)(long)reader["minTemp"];
                essEntry.cyclesToPerform = (int)(long)reader["cyclesToPerform"];
                essEntry.cyclesFinished = (int)(long)reader["cyclesFinished"];
                string tStartTime = reader["StartTime"].ToString();
                string tEndTime = reader["EndTime"].ToString();
                essEntry.dwellTime = (int)(long)reader["dwellTime"];

                DateTime.TryParse(tStartTime, out DateTime parsedStartTime);
                DateTime.TryParse(tEndTime, out DateTime parsedEndTime);

                essEntry.startTime = parsedStartTime;
                essEntry.endTime = parsedEndTime;
            }

            CloseConnection();

            return essEntry;
        }

        public static int GetUnitRemainingCycles(string serialNumber)
        {
            if (!OpenConnection())
            {
                MessageBox.Show("Error: The database could not be accessed.");
                return -1;
            }

            ESSEntry essEntry = null;

            string sql = String.Format("SELECT * FROM 'UnitEntries' WHERE serialNumber = '{0}'", serialNumber);
            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            long essID = -1;

            if (reader.Read())
            {
                essID = (long)reader["essID"];
            }

            if (essID == -1)
                return -1;

            essEntry = new ESSEntry();

            essEntry = GetESSInformationByESSID(essID);

            CloseConnection();

            return (essEntry.cyclesToPerform - essEntry.cyclesFinished);
        }

        public static bool AddProduct(ProductInfo newProductInfo) // Add a new product via the settings page.
        {
            if(CheckIfProductAlreadyExistsByName(newProductInfo.name))
            {
                MessageBox.Show("Error: Either the product already exists or the database could not be accessed.\n\nIf the product does not exist, please restart the application.");
                return false;
            }

            if (!OpenConnection())
                return false;

            String sql = String.Format("INSERT INTO ProductInfo (name, maxTemp, minTemp, cycles, stayTime) values ('{0}', '{1}', '{2}', '{3}', '{4}')", newProductInfo.name, newProductInfo.maxTemp, newProductInfo.minTemp, newProductInfo.cycles, newProductInfo.dwellTime);

            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            command.ExecuteNonQuery();

            CloseConnection();

            return true;
        }

        public static bool UpdateProduct(ProductInfo productInfo) // Add a new product via the settings page.
        {
            if (!CheckIfProductAlreadyExistsByName(productInfo.name))
            {
                MessageBox.Show("Error: The selected product does not exist.");
                return false;
            }

            if (!OpenConnection())
                return false;

            String sql = String.Format("UPDATE ProductInfo SET maxTemp = '{0}', minTemp = '{1}', cycles = '{2}', stayTime = '{3}' WHERE name = '{4}'",
                productInfo.maxTemp, productInfo.minTemp, productInfo.cycles, productInfo.dwellTime, productInfo.name);

            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            command.ExecuteNonQuery();

            CloseConnection();

            return true;
        }

        public static bool RemoveProductByName(string productName)
        {
            if (!CheckIfProductAlreadyExistsByName(productName))
            {
                MessageBox.Show("Error: The selected product does not exist.");
                return false;
            }

            if (!OpenConnection())
                return false;

            String sql = String.Format("DELETE FROM 'ProductInfo' WHERE name = '{0}'", productName);

            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            command.ExecuteNonQuery();

            CloseConnection();

            return true;
        }

        private static bool CheckIfProductAlreadyExistsByName(string name)
        {
            if (!OpenConnection())
            {
                MessageBox.Show("Error: The database could not be accessed.");
                return true;
            }

            string sql = String.Format("SELECT * FROM 'ProductInfo' WHERE name = '{0}'", name);

            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            bool flag = false;

            if (reader.Read())
            {
                flag = true;
            }

            reader.Close();

            CloseConnection();

            return flag;
        }

        public static ProductInfo GetProductInfoByName(string productName)
        {
            if (!OpenConnection())
            {
                MessageBox.Show("Error: The database could not be accessed.");
                return null;
            }

            string sql = String.Format("SELECT * FROM 'ProductInfo' WHERE name = '{0}'", productName);
            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            ProductInfo productInfo = null;

            if (reader.Read())
            {
                productInfo = new ProductInfo();

                productInfo.name = reader["Name"].ToString();
                productInfo.maxTemp = reader["maxTemp"].ToString();
                productInfo.minTemp = reader["minTemp"].ToString();
                productInfo.cycles = reader["cycles"].ToString();
                productInfo.dwellTime = reader["stayTime"].ToString();
            }

            reader.Close();

            CloseConnection();

            return productInfo;
        }

        public static List<ProductInfo> GetProductList()
        {
            if (!OpenConnection())
            {
                MessageBox.Show("Error: The database could not be accessed.");
                return null;
            }

            string sql = String.Format("SELECT * FROM 'ProductInfo'");
            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            ProductInfo productInfo = new ProductInfo();

            List<ProductInfo> allProducts = new List<ProductInfo>();

            while (reader.Read())
            {
                allProducts.Add(new ProductInfo {

                    ID = (int)(long)reader["ID"],
                    name = reader["Name"].ToString(),
                    maxTemp = reader["maxTemp"].ToString(),
                    minTemp = reader["minTemp"].ToString(),
                    cycles = reader["cycles"].ToString(),
                    dwellTime = reader["stayTime"].ToString()

                });
            }

            reader.Close();

            CloseConnection();

            return allProducts;
        }

        public static void InitDisplayWindow(int id, string realdate)
        {
            List<MyItem> Items = new List<MyItem>();
            fullList = new ArrayList();
            redundantList = new ArrayList();

            if (!OpenConnection())
            {
                MessageBox.Show("Error: The database could not be accessed.");
                return;
            }

            string sql = String.Format("SELECT * FROM 'ESSInfo' WHERE ID = '{0}'", id);
            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            String productType = "-1", strMaxTemp = "-1", strMinTemp = "-1", date = "-1", strCycles = "-1", strStayTime = "-1";

            if (reader.Read())
            {
                productType = reader["ProductType"].ToString();
                strMaxTemp = reader["maxTemp"].ToString();
                strMinTemp = reader["minTemp"].ToString();
                strCycles = reader["cycles"].ToString();
                strStayTime = reader["stayTime"].ToString();
            }

            sql = String.Format("SELECT * FROM 'Temperatures' WHERE ID = '{0}' ORDER BY uniqueID ASC", id);
            command = new SQLiteCommand(sql, sqConnection);
            reader = command.ExecuteReader();

            int tRow = 0;

            ArrayList temperatures = new ArrayList();
            String temp = "-1";

            while (reader.Read())
            {
                temp = reader["Temp"].ToString();

                Double.TryParse(temp, out double parsedTemp);

                foreach (Window window in Application.Current.Windows)
                {
                    if (window.GetType() == typeof(Windows.DisplayHistoryWindow))
                    {
                        fullList.Add(parsedTemp);
                    }
                }

                if (tRow == 0)
                    date = reader["date"].ToString();

                tRow++;
            }

            RefactorLineSeriesForPerformance();

            date = realdate;

            sql = String.Format("SELECT * FROM 'Temperatures' WHERE ID = '{0}' ORDER BY uniqueID ASC", id);
            command = new SQLiteCommand(sql, sqConnection);
            reader = command.ExecuteReader();


            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Windows.DisplayHistoryWindow))
                {
                    (window as Windows.DisplayHistoryWindow).txtblockCycles.Text = "Cycles: " + strCycles;
                    (window as Windows.DisplayHistoryWindow).txtblockDate.Text = "Date: " + date;
                    (window as Windows.DisplayHistoryWindow).txtblockMaxTemp.Text = "Max Temp: " + strMaxTemp + "°";
                    (window as Windows.DisplayHistoryWindow).txtblockMinTemp.Text = "Min Temp: " + strMinTemp + "°";
                    (window as Windows.DisplayHistoryWindow).txtblockProduct.Text = "Product: " + productType;
                    (window as Windows.DisplayHistoryWindow).txtblockStayTime.Text = "Stay Time: " + strStayTime;

                    sql = String.Format("SELECT * FROM 'SerialNumbers' WHERE ID = '{0}'", id);
                    command = new SQLiteCommand(sql, sqConnection);
                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        (window as Windows.DisplayHistoryWindow).listViewSerialNumbers.Items.Add(reader["SerialNumber"].ToString());
                    }
                }
            }

            CloseConnection();
        }

        public static void RefactorLineSeriesForPerformance()
        {
            redundantList = new ArrayList();

            int maxAllowedPoints = 500;
            int skips = 1;

            Application.Current.Dispatcher.Invoke(delegate {

                foreach (Window item in Application.Current.Windows)
                {
                    if (item.Name == "Display")
                    {
                        //int count = ((Windows.ESSWindow)item).mySeries.Values.Count;
                        int count = fullList.Count;

                        if (count > maxAllowedPoints)
                            skips = (int)Math.Ceiling(count / (float)maxAllowedPoints);

                        for (int i = 0; i < count; i += skips)
                        {
                            //var valueToAdd = ((Windows.ESSWindow)item).mySeries.Values[i];
                            var valueToAdd = fullList[i];

                            redundantList.Add(valueToAdd);
                        }

                        ((Windows.DisplayHistoryWindow)item).mySeries.Values.Clear();

                        for (int iteration = 0; iteration < redundantList.Count; iteration++)
                        {
                            ((Windows.DisplayHistoryWindow)item).mySeries.Values.Add(redundantList[iteration]);
                        }
                    }
                }
            });
        }

        /* Initializing windows information via database methods/functions */

        // This method is responsible for loading all the information of a saved ESS process from the database into the DisplayHistory window.
        //public static void InitDisplayWindow(int id)
        //{
        //    String[] files = System.IO.Directory.GetFiles(savedFilePath, "*.xls");

        //    List<MyItem> Items = new List<MyItem>();

        //    String tempID = files[id].Split('(', ')')[1];

        //    bool extractedID = Int32.TryParse(tempID, out int myID);

        //    if (!extractedID)
        //    {
        //        MessageBox.Show("Error: There was an error with the excel files.");
        //        return;
        //    }

        //    if (!OpenConnection())
        //    {
        //        MessageBox.Show("Error: The database could not be accessed.");
        //        return;
        //    }

        //    string sql = String.Format("SELECT * FROM 'ESSInfo' WHERE ID = '{0}'", myID);
        //    SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
        //    SQLiteDataReader reader = command.ExecuteReader();

        //    String productType = "-1", strMaxTemp = "-1", strMinTemp = "-1", date = "-1", strCycles = "-1", strStayTime = "-1";

        //    if (reader.Read())
        //    {
        //        productType = reader["ProductType"].ToString();
        //        strMaxTemp = reader["maxTemp"].ToString();
        //        strMinTemp = reader["minTemp"].ToString();
        //        strCycles = reader["cycles"].ToString();
        //        strStayTime = reader["stayTime"].ToString();
        //    }

        //    sql = String.Format("SELECT * FROM 'Temperatures' WHERE ID = '{0}' ORDER BY uniqueID ASC", myID);
        //    command = new SQLiteCommand(sql, sqConnection);
        //    reader = command.ExecuteReader();

        //    int tRow = 0;

        //    ArrayList temperatures = new ArrayList();
        //    String temp = "-1";

        //    while (reader.Read())
        //    {
        //        temp = reader["Temp"].ToString();

        //        Double.TryParse(temp, out double parsedTemp);

        //        foreach (Window window in Application.Current.Windows)
        //        {
        //            if (window.GetType() == typeof(Windows.DisplayHistoryWindow))
        //            {
        //                if (tRow == 0)
        //                    (window as Windows.DisplayHistoryWindow).mySeries.Values.Clear();

        //                (window as Windows.DisplayHistoryWindow).mySeries.Values.Add(parsedTemp);
        //            }
        //        }

        //        if (tRow == 0)
        //            date = reader["date"].ToString();

        //        tRow++;
        //    }


        //    foreach (Window window in Application.Current.Windows)
        //    {
        //        if (window.GetType() == typeof(Windows.DisplayHistoryWindow))
        //        {
        //            (window as Windows.DisplayHistoryWindow).txtblockCycles.Text = "Cycles: " + strCycles;
        //            (window as Windows.DisplayHistoryWindow).txtblockDate.Text = "Date: " + date;
        //            (window as Windows.DisplayHistoryWindow).txtblockMaxTemp.Text = "Max Temp: " + strMaxTemp;
        //            (window as Windows.DisplayHistoryWindow).txtblockMinTemp.Text = "Min Temp: " + strMinTemp;
        //            (window as Windows.DisplayHistoryWindow).txtblockProduct.Text = "Product: " + productType;
        //            (window as Windows.DisplayHistoryWindow).txtblockStayTime.Text = "Stay Time: " + strStayTime;

        //            sql = String.Format("SELECT * FROM 'SerialNumbers' WHERE ID = '{0}'", myID);
        //            command = new SQLiteCommand(sql, sqConnection);
        //            reader = command.ExecuteReader();

        //            while (reader.Read())
        //            {
        //                (window as Windows.DisplayHistoryWindow).listViewSerialNumbers.Items.Add(reader["SerialNumber"].ToString());
        //            }
        //        }
        //    }

        //    CloseConnection();
        //}

        public static void InitHistoryWindow()
        {
            List<MyItem> Items = new List<MyItem>();
            listOfIDs = new List<int>();
            listOfDates = new List<string>();

            if (!OpenConnection())
            {
                MessageBox.Show("Error: The database could not be accessed.");
                return;
            }

            string sql = String.Format("SELECT * FROM 'ESSInfo'");
            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            int rows = 0;

            String productType, maxTemp, minTemp, date;
            int myID;

            while (reader.Read())
            {
                productType = reader["ProductType"].ToString();
                maxTemp = reader["maxTemp"].ToString();
                minTemp = reader["minTemp"].ToString();
                string justID = reader["ID"].ToString();
                date = reader["StartTime"].ToString();

                Int32.TryParse(justID, out myID);

                listOfIDs.Add(myID);
                listOfDates.Add(date);

                Int32.TryParse(maxTemp, out int maxTempInt);
                Int32.TryParse(minTemp, out int minTempInt);

                Items.Add(new MyItem() { dateTime = date, maxTemp = maxTempInt, minTemp = minTempInt, productName = productType });

                rows++;
            }

            reader.Close();

            CloseConnection();

            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType() == typeof(Windows.HistoryWindow))
                {
                    (window as Windows.HistoryWindow).listViewHistory.ItemsSource = Items;
                }
            }
        }

        // This method reads all files from the ESS Reports folder in R and lists them. It initializes the History window with all the information.
        //public static void InitHistoryWindow()
        //{
        //    String[] files = new String[2];

        //    if (Directory.Exists(savedFilePath))
        //    {
        //        files = System.IO.Directory.GetFiles(savedFilePath, "*.xls");
        //    }

        //    List<MyItem> Items = new List<MyItem>();

        //    foreach (String file in files)
        //    {
        //        String tempID = file.Split('(', ')')[1];

        //        bool extractedID = Int32.TryParse(tempID, out int myID);

        //        if (!extractedID)
        //        {
        //            MessageBox.Show("Error: There was an error with the excel files.");
        //            return;
        //        }

        //        if (!OpenConnection())
        //        {
        //            MessageBox.Show("Error: The database could not be accessed.");
        //            return;
        //        }

        //        string sql = String.Format("SELECT * FROM 'ESSInfo' WHERE ID = '{0}'", myID);
        //        SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
        //        SQLiteDataReader reader = command.ExecuteReader();

        //        String productType = "-1", maxTemp = "-1", minTemp = "-1", date = "-1";

        //        if (reader.Read())
        //        {
        //            productType = reader["ProductType"].ToString();
        //            maxTemp = reader["maxTemp"].ToString();
        //            minTemp = reader["minTemp"].ToString();
        //        }

        //        sql = String.Format("SELECT * FROM 'Temperatures' WHERE ID = '{0}' ORDER BY uniqueID ASC", myID);
        //        command = new SQLiteCommand(sql, sqConnection);
        //        reader = command.ExecuteReader();

        //        if (reader.Read())
        //        {
        //            date = reader["date"].ToString();
        //        }

        //        Int32.TryParse(maxTemp, out int maxTempInt);
        //        Int32.TryParse(minTemp, out int minTempInt);

        //        Items.Add(new MyItem() { dateTime = date, maxTemp = maxTempInt, minTemp = minTempInt, productName = productType });

        //        CloseConnection();
        //    }

        //    foreach (Window window in Application.Current.Windows)
        //    {
        //        if (window.GetType() == typeof(Windows.HistoryWindow))
        //        {
        //            (window as Windows.HistoryWindow).listViewHistory.ItemsSource = Items;
        //        }
        //    }
        //}

        /* Export excel file function/method */

        // For the current ESS process: read all relevant information from the database and export it to an excel file.
        public static void CreateExcelFile(int id, string testStartDate)
        {
            String tempFileName = OvenCommunication.fileName + " (" + id.ToString() + ")";

            string modifiedDateString = testStartDate.Replace('/', '.');
            modifiedDateString = modifiedDateString.Replace(':', '-');

            tempFileName = savedFilePath + modifiedDateString + " (" + id.ToString() + ")";

            if (Directory.Exists(savedFilePath))
                if (File.Exists(tempFileName + ".xls"))
                    return;

            if (!Directory.Exists(savedFilePath))
            {
                tempFileName = dbPath + modifiedDateString + " (" + id.ToString() + ")";

                if (File.Exists(tempFileName + ".xls"))
                    return;
            }

            object misValue = System.Reflection.Missing.Value;

            Microsoft.Office.Interop.Excel.Application xlApp = new Microsoft.Office.Interop.Excel.Application();

            if (xlApp == null)
            {
                MessageBox.Show("Error: Excel is not installed on this computer.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Microsoft.Office.Interop.Excel.Workbook xlWorkbook = xlApp.Workbooks.Add(misValue);
            Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkbook.Worksheets.get_Item(1);

            xlWorkSheet.Cells[1, 1] = "S/N:"; // Coloumn 1 is for serial numbers.
            xlWorkSheet.Cells[1, 2] = "Product:"; // Coloumn 2 is for product name.
            xlWorkSheet.Cells[1, 3] = "Max Temp:";
            xlWorkSheet.Cells[1, 4] = "Min Temp:";
            xlWorkSheet.Cells[1, 5] = "Cycles:";
            xlWorkSheet.Cells[1, 6] = "Stay Time:";
            xlWorkSheet.Cells[1, 10] = "Temp Measurements:";

            if (!OpenConnection())
            {
                MessageBox.Show("Error: The database could not be accessed.");
                return;
            }

            string sql = String.Format("SELECT * FROM 'SerialNumbers' WHERE ID = '{0}'", id);
            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            int row = 2;

            while (reader.Read())
            {
                xlWorkSheet.Cells[row, 1] = reader["SerialNumber"].ToString();
                row++;
            }

            sql = String.Format("SELECT * FROM 'ESSInfo' WHERE ID = '{0}'", id);
            command = new SQLiteCommand(sql, sqConnection);
            reader = command.ExecuteReader();

            if (reader.Read())
            {
                xlWorkSheet.Cells[2, 2] = reader["ProductType"].ToString();
                xlWorkSheet.Cells[2, 3] = reader["maxTemp"].ToString();
                xlWorkSheet.Cells[2, 4] = reader["minTemp"].ToString();
                xlWorkSheet.Cells[2, 5] = reader["cycles"].ToString();
                xlWorkSheet.Cells[2, 6] = reader["stayTime"].ToString();
            }

            sql = String.Format("SELECT * FROM 'Temperatures' WHERE ID = '{0}' ORDER BY uniqueID ASC", id);
            command = new SQLiteCommand(sql, sqConnection);
            reader = command.ExecuteReader();

            row = 2;

            while (reader.Read())
            {
                xlWorkSheet.Cells[row, 10] = reader["Temp"].ToString();
                xlWorkSheet.Cells[row, 9] = reader["date"].ToString();
                row++;
            }

            xlWorkbook.SaveAs(tempFileName + ".xls", Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue,
                Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);

            xlWorkbook.Close(true, misValue, misValue);
            xlApp.Quit();

            Marshal.ReleaseComObject(xlWorkSheet);
            Marshal.ReleaseComObject(xlWorkbook);
            Marshal.ReleaseComObject(xlApp);

            CloseConnection();
        }

        public static void updateSavePath(string newPath)
        {
            if (!OpenConnection())
            {
                MessageBox.Show("Error: The database could not be accessed.");
                return;
            }

            string sql = String.Format("UPDATE 'InternalInfo' SET filePath = '{0}' WHERE ID = {1}", @newPath, 1);
            SQLiteCommand command = new SQLiteCommand(sql, sqConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            CloseConnection();
        }
    }

    public class MyItem
    {
        public String dateTime { get; set; }
        public String productName { get; set; }
        public int maxTemp { get; set; }
        public int minTemp { get; set; }
    }
}
