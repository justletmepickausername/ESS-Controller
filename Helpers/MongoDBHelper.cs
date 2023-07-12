using ESS_Controller.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ESS_Controller.Helpers
{
    public static class MongoDBHelper
    {
        private static MongoClientSettings settings = MongoClientSettings.FromConnectionString("mongodb+srv://ESS:3450337@cluster0.s89wf.mongodb.net/ESS?retryWrites=true&w=majority");
        private static MongoClientSettings opsysSettings = MongoClientSettings.FromConnectionString("mongodb+srv://orxuser:c7a9acdd4eFg50@clusterd1.xqnwc.mongodb.net/orxu-tester3?retryWrites=true");
        //private static MongoClientSettings notORXUSettings = MongoClientSettings.FromConnectionString("mongodb+srv://essdebugusr:e33debug2070@cluster0.xqnwc.azure.mongodb.net/test");
        private static MongoClientSettings notORXUSettings = MongoClientSettings.FromConnectionString("mongodb+srv://essdebugusr:e33debug2070@cluster0.xqnwc.azure.mongodb.net/test?authSource=admin&replicaSet=Cluster0-shard-0&readPreference=primary&appname=MongoDB%20Compass&retryWrites=true&ssl=true");

        private static MongoClient client;
        private static MongoClient opsysClient;
        private static MongoClient notORXUDatabaseClient;

        private static IMongoDatabase database;
        private static IMongoDatabase opsysDatabase;
        private static IMongoDatabase notORXUDatabase;

        public static Guid currentESSProcessID;

        public static long lastUsedID = 149000;

        public static List<long> usedIDs = new List<long>();
        //public static List<string> productTypes = new List<string>();

        static MongoDBHelper()
        {
            BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));

            client = new MongoClient(settings);
            database = client.GetDatabase("ESS");

            if (client == null)
                throw new Exception("MongoDB cluster could not be accessed.");

            if(database == null)
                throw new Exception("MongoDB database could not be accessed.");

            opsysClient = new MongoClient(opsysSettings);
            opsysDatabase = opsysClient.GetDatabase("orxu-tester3");

            if (opsysClient == null)
                throw new Exception("MongoDB cluster could not be accessed.");

            if (opsysDatabase == null)
                throw new Exception("MongoDB database could not be accessed.");

            notORXUDatabaseClient = new MongoClient(notORXUSettings);
            notORXUDatabase = notORXUDatabaseClient.GetDatabase("ess-debug-db");

            if(notORXUDatabaseClient == null)
                throw new Exception("MongoDB cluster could not be accessed.");

            if (notORXUDatabase == null)
                throw new Exception("MongoDB database could not be accessed.");

            if(lastUsedID == 149000)
            {
                lastUsedID = GetNextAvailableID();
            }
        }

        public static long GetLastestUsedUnitID()
        {
            var col = database.GetCollection<BsonDocument>("Settings");

            var filter = Builders<BsonDocument>.Filter.Eq("type", "dbLastUsedID");

            var foundUnit = col.Find(filter).FirstOrDefault();

            var lastID = (long)foundUnit.GetElement("lastUsedID").Value;

            //var collection = notORXUDatabase.GetCollection<BsonDocument>

            return lastID;
        }

        public static async void SetLatestUsedUnitID(long lastUsedUnitID)
        {
            var col = database.GetCollection<BsonDocument>("Settings");

            var filter = Builders<BsonDocument>.Filter.Eq("type", "dbLastUsedID");

            var update = Builders<BsonDocument>.Update.Set("lastUsedID", lastUsedUnitID);

            await col.UpdateOneAsync(filter, update);
        }

        //public static async Task<bool> InitiateESSProcess(int maxTemp, int minTemp, int cyclesToPerform, int dwellTime)
        //{
        //    ESSEntry newESSEntry = new ESSEntry();

        //    newESSEntry.id = Guid.NewGuid();
        //    newESSEntry.maxTemp = maxTemp;
        //    newESSEntry.minTemp = minTemp;
        //    newESSEntry.cyclesToPerform = cyclesToPerform;
        //    newESSEntry.dwellTime = dwellTime;
        //    newESSEntry.startTime = DateTime.Now;
        //    newESSEntry.status = "ongoing";

        //    newESSEntry.endTime = null;
        //    newESSEntry.cyclesFinished = 0;

        //    IMongoCollection<ESSEntry> collection = database.GetCollection<ESSEntry>("Entries");

        //    await collection.InsertOneAsync(newESSEntry);

        //    currentESSProcessID = newESSEntry.id;

        //    return true;
        //}

        public static async Task<bool> InitiateESSProcessOnOpsysMongoDBDatabaseBySerialNumber(string serialNumber, string productType)
        {
            if (productType.Contains("ORXU"))
            {
                IMongoCollection<BsonDocument> collection = opsysDatabase.GetCollection<BsonDocument>("testentries");

                BsonDocument doc = new BsonDocument();

                //lastUsedID = GetNextAvailableID();
                lastUsedID++;

                doc.Add(new BsonElement("HostName", "ESS Controller"));
                doc.Add(new BsonElement("TechnicianName", "ESS Controller"));
                doc.Add(new BsonElement("SerialNumber", serialNumber));
                doc.Add(new BsonElement("PartNumber", "1278484"));
                doc.Add(new BsonElement("TestName", "Ess Process"));
                doc.Add(new BsonElement("StartTime", DateTime.Now));
                doc.Add(new BsonElement("EndTime", ""));
                doc.Add(new BsonElement("Status", "Running"));
                doc.Add(new BsonElement("AteVersion", "79"));
                doc.Add(new BsonElement("AteName", "ESS Controller"));
                doc.Add(new BsonElement("Id", lastUsedID));
                doc.Add(new BsonElement("__v", 0));

                usedIDs.Add(lastUsedID);
                //productTypes.Add(productType);

                await collection.InsertOneAsync(doc);
            }
            else
            {
                IMongoCollection<BsonDocument> collection = notORXUDatabase.GetCollection<BsonDocument>("ess-debug");

                BsonDocument doc = new BsonDocument();

                //lastUsedID = GetNextAvailableID();
                lastUsedID++;

                doc.Add(new BsonElement("HostName", "ESS Controller"));
                doc.Add(new BsonElement("TechnicianName", "ESS Controller"));
                doc.Add(new BsonElement("SerialNumber", serialNumber));
                doc.Add(new BsonElement("PartNumber", "1278484"));
                doc.Add(new BsonElement("TestName", "Ess Process"));
                doc.Add(new BsonElement("StartTime", DateTime.Now));
                doc.Add(new BsonElement("EndTime", ""));
                doc.Add(new BsonElement("Status", "Running"));
                doc.Add(new BsonElement("AteVersion", "79"));
                doc.Add(new BsonElement("AteName", "ESS Controller"));
                doc.Add(new BsonElement("Id", lastUsedID));
                doc.Add(new BsonElement("__v", 0));

                usedIDs.Add(lastUsedID);
                //productTypes.Add(productType);

                await collection.InsertOneAsync(doc);
            }

            return true;
        }

        //public static async Task<bool> PauseESSProcess(Guid ESS_ID, int finishedCycles)
        //{
        //    var filter = Builders<ESSEntry>.Filter.Eq("id", ESS_ID);

        //    var collection = database.GetCollection<ESSEntry>("Entries");

        //    var update = Builders<ESSEntry>.Update.Set("status", "partial");

        //    await collection.UpdateOneAsync(filter, update);

        //    update = Builders<ESSEntry>.Update.Set("endTime", DateTime.Now);

        //    await collection.UpdateOneAsync(filter, update);

        //    update = Builders<ESSEntry>.Update.Set("cyclesFinished", finishedCycles);

        //    await collection.UpdateOneAsync(filter, update);

        //    return true;
        //}

        public static async Task<bool> PauseESSProcessOnOpsysMongoDBDatabaseForAllUnitsOnThisESSProcess()
        {
            IMongoCollection<BsonDocument> collection = opsysDatabase.GetCollection<BsonDocument>("testentries");

            for(int i = 0; i < usedIDs.Count; i++)
            {
                var filter = Builders<BsonDocument>.Filter.Eq("Id", usedIDs[i]);

                var update = Builders<BsonDocument>.Update.Set("Status", "Paused");

                await collection.UpdateOneAsync(filter, update);

                update = Builders<BsonDocument>.Update.Set("EndTime", DateTime.Now);

                await collection.UpdateOneAsync(filter, update);
            }

            SetLatestUsedUnitID(lastUsedID);
            usedIDs = new List<long>();

            collection = notORXUDatabase.GetCollection<BsonDocument>("ess-debug");

            for (int i = 0; i < usedIDs.Count; i++)
            {
                var filter = Builders<BsonDocument>.Filter.Eq("Id", usedIDs[i]);

                var update = Builders<BsonDocument>.Update.Set("Status", "Paused");

                await collection.UpdateOneAsync(filter, update);

                update = Builders<BsonDocument>.Update.Set("EndTime", DateTime.Now);

                await collection.UpdateOneAsync(filter, update);
            }

            SetLatestUsedUnitID(lastUsedID);
            usedIDs = new List<long>();

            return true;
        }

        public static long GetNextAvailableID()
        {
            lastUsedID = GetLastestUsedUnitID();

            //var filter = Builders<ESSEntry>.Filter.Gte("id", lastUsedID);

            var collection = opsysDatabase.GetCollection<BsonDocument>("testentries");

            var res = collection.Aggregate().SortByDescending((a) => a["Id"]).FirstOrDefault();

            var theIDToReturn = res.GetValue("Id");

            if(!long.TryParse(theIDToReturn.ToString(), out long nextReturnID))
            {
                MessageBox.Show("Error: There was an error accessing the database. [ERROR-2052]");
                return 200000;
            }

            //DoThis();

            //var notORXUCollection = notORXUDatabase.GetCollection<BsonDocument>("ess-debug");

            //var result = notORXUCollection.Aggregate().SortByDescending((a) => a["Id"]).FirstOrDefault();

            //var otherID = result.GetValue("Id").AsInt64;

            //if(otherID > nextReturnID)
            //    nextReturnID = otherID;

            nextReturnID++;

            return nextReturnID;
        }

        private async static void DoThis()
        {
            notORXUDatabaseClient = new MongoClient(notORXUSettings);

            notORXUDatabase = notORXUDatabaseClient.GetDatabase("ess-debug-db");

            var coll = notORXUDatabase.GetCollection<BsonDocument>("debug-db");

            if (await coll.CountDocumentsAsync(new BsonDocument()) > 0)
            {
                var result = coll.Aggregate().SortByDescending((a) => a["Id"]).FirstOrDefault();

                var otherID = result.GetValue("Id").AsInt64;
            }
        }

        //public static async Task<bool> FinishESSProcess(Guid ESS_ID, int finishedCycles)
        //{
        //    var filter = Builders<ESSEntry>.Filter.Eq("id", ESS_ID);

        //    var collection = database.GetCollection<ESSEntry>("Entries");

        //    var update = Builders<ESSEntry>.Update.Set("status", "finished");

        //    await collection.UpdateOneAsync(filter, update);

        //    update = Builders<ESSEntry>.Update.Set("endTime", DateTime.Now);

        //    await collection.UpdateOneAsync(filter, update);

        //    update = Builders<ESSEntry>.Update.Set("cyclesFinished", finishedCycles);

        //    await collection.UpdateOneAsync(filter, update);

        //    return true;
        //}

        public static async Task<bool> FinishESSProcessOnOpsysMongoDBDatabaseForAllUnitsOnThisESSProcess()
        {
            IMongoCollection<BsonDocument> collection = opsysDatabase.GetCollection<BsonDocument>("testentries");

            for (int i = 0; i < usedIDs.Count; i++)
            {
                var filter = Builders<BsonDocument>.Filter.Eq("Id", usedIDs[i]);

                var update = Builders<BsonDocument>.Update.Set("Status", "Pass");

                await collection.UpdateOneAsync(filter, update);

                update = Builders<BsonDocument>.Update.Set("EndTime", DateTime.Now);

                await collection.UpdateOneAsync(filter, update);
            }

            //SetLatestUsedUnitID(lastUsedID);
            //usedIDs = new List<long>();

            collection = notORXUDatabase.GetCollection<BsonDocument>("ess-debug");

            for (int i = 0; i < usedIDs.Count; i++)
            {
                var filter = Builders<BsonDocument>.Filter.Eq("Id", usedIDs[i]);

                var update = Builders<BsonDocument>.Update.Set("Status", "Pass");

                await collection.UpdateOneAsync(filter, update);

                update = Builders<BsonDocument>.Update.Set("EndTime", DateTime.Now);

                await collection.UpdateOneAsync(filter, update);
            }

            SetLatestUsedUnitID(lastUsedID);
            usedIDs = new List<long>();

            return true;
        }


        //public static async Task<bool> LinkUnitToESS(string serialNumber, string unitType, Guid ess_id)
        //{
        //    UnitEntry unitEntry = new UnitEntry();

        //    unitEntry.serialNumber = serialNumber;
        //    unitEntry.unitType = unitType;
        //    unitEntry.essID = ess_id;
        //    unitEntry.myID = Guid.NewGuid();

        //    IMongoCollection<UnitEntry> collection = database.GetCollection<UnitEntry>("UnitEntry");

        //    await collection.InsertOneAsync(unitEntry);

        //    return true;
        //}

        //public static bool IsUnitOnDatabase(string serialNumber)
        //{
        //    IMongoCollection<UnitEntry> collection = database.GetCollection<UnitEntry>("UnitEntry");

        //    var filter = Builders<UnitEntry>.Filter.Eq("serialNumber", serialNumber);

        //    var foundUnit = collection.Find(filter).FirstOrDefault();

        //    if (foundUnit == null)
        //        return false;

        //    return true;
        //}

        //public static int GetUnitRemainingCycles(string serialNumber)
        //{
        //    IMongoCollection<UnitEntry> collection = database.GetCollection<UnitEntry>("UnitEntry");

        //    var filter = Builders<UnitEntry>.Filter.Eq("serialNumber", serialNumber);

        //    var foundUnit = collection.Find(filter).FirstOrDefault();

        //    if (foundUnit == null)
        //        return -1;

        //    var entry = GetESSInformationByESSID(foundUnit.essID);

        //    if (entry == null)
        //        return -1;

        //    return (entry.cyclesToPerform - entry.cyclesFinished);
        //}

        //public static ESSEntry GetESSInformationByESSID(Guid id)
        //{
        //    ESSEntry retrievedEntry;

        //    var filter = Builders<ESSEntry>.Filter.Eq("id", id);

        //    var collection = database.GetCollection<ESSEntry>("Entries");

        //    retrievedEntry = collection.Find(filter).FirstOrDefault();

        //    return retrievedEntry;
        //}

        public async static void AddProduct(ProductInfo newProductInfo)
        {
            if (CheckIfProductAlreadyExistsByName(newProductInfo.name))
            {
                MessageBox.Show("Error: Either the product already exists or the database could not be accessed.\n\nIf the product does not exist, please restart the application.");
                return;
            }

            newProductInfo.type = "productInfo";

            var collection = database.GetCollection<ProductInfo>("ProductInfo");

            await collection.InsertOneAsync(newProductInfo);
        }

        public async static void RemoveProductByName(string productName)
        {
            if (!CheckIfProductAlreadyExistsByName(productName))
            {
                MessageBox.Show("Error: The selected product does not exist.");
                return;
            }

            var collection = database.GetCollection<ProductInfo>("ProductInfo");

            var deleteFilter = Builders<ProductInfo>.Filter.Eq("name", productName);

            await collection.DeleteOneAsync(deleteFilter);
        }

        // TO DO
        // Fix this update function. It somehow changes the types from strings to int32 on the mongodb server.
        // Send this in the mongodb server chat
        /*
         * Hello. I've created a collection in my database. I've added a document to that collection, which adds just fine, with the proper types (all strings except for the id)
And when I go to update, also with strings, the document changes some types to ints by itself.

        Also, check stackoverflow, u asked a question there.
         * 
         */

        public static async void UpdateProduct(ProductInfo productInfo)
        {
            var filter = Builders<ESSEntry>.Filter.Eq("id", productInfo.ID);

            var collection = database.GetCollection<ESSEntry>("ProductInfo");

            var update = Builders<ESSEntry>.Update.Set("maxTemp", productInfo.maxTemp);

            await collection.UpdateOneAsync(filter, update);

            update = Builders<ESSEntry>.Update.Set("minTemp", productInfo.minTemp);

            await collection.UpdateOneAsync(filter, update);

            update = Builders<ESSEntry>.Update.Set("cycles", productInfo.cycles);

            await collection.UpdateOneAsync(filter, update);

            update = Builders<ESSEntry>.Update.Set("dwellTime", productInfo.dwellTime);

            await collection.UpdateOneAsync(filter, update);
        }

        public static ProductInfo GetProductInfoByName(string name)
        {
            var filter = Builders<ProductInfo>.Filter.Eq("name", name);

            var collection = database.GetCollection<ProductInfo>("ProductInfo");

            var result = collection.Find(filter).FirstOrDefault();

            return result;
        }

        public static bool CheckIfProductAlreadyExistsByName(string name)
        {
            var filter = Builders<ProductInfo>.Filter.Eq("name", name);

            var collection = database.GetCollection<ProductInfo>("ProductInfo");

            var result = collection.Find(filter).FirstOrDefault();

            if (result == null)
                return false;

            return true;
        }

        public static List<ProductInfo> GetProductList()
        {
            var collection = database.GetCollection<ProductInfo>("ProductInfo");

            List<ProductInfo> products = new List<ProductInfo>();

            var filter = Builders<ProductInfo>.Filter.Eq("type", "productInfo");

            var doc = collection.Find(filter);

            products = doc.ToList();

            return products;
        }

        public static bool DidUnitDoTester3B(string serialNumber)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("TestName", "Tester3-B") & Builders<BsonDocument>.Filter.Eq("Parameters.Value", serialNumber);

            var collection = opsysDatabase.GetCollection<BsonDocument>("testentries");

            var result = collection.Find(filter).FirstOrDefault();

            if (result == null)
                return false;

            return true;
        }

        public static bool DidUnitPassTester3B(string serialNumber)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("TestName", "Tester3-B") & Builders<BsonDocument>.Filter.Eq("Parameters.Value", serialNumber);

            var collection = opsysDatabase.GetCollection<BsonDocument>("testentries");

            var result = collection.Find(filter);

            if (result == null)
                return false;

            var found = false;

            foreach (var res in result.ToList())
            {
                var status = res.GetValue("Status").ToString();

                if (status.Equals("Pass", StringComparison.InvariantCultureIgnoreCase))
                    found = true;
            }

            return found;
        }
    }
}
