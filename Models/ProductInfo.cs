using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESS_Controller.Models
{
    public class ProductInfo
    {
        [BsonElement("_id")]
        public int ID { get; set; }

        [BsonElement("type")]
        public string type { get; set; }

        [BsonElement("name")]
        public string name { get; set; }

        [BsonElement("maxTemp")]
        public string maxTemp { get; set; }

        [BsonElement("minTemp")]
        public string minTemp { get; set; }

        [BsonElement("cycles")]
        public string cycles { get; set; }

        [BsonElement("dwellTime")]
        public string dwellTime { get; set; }
    }
}
