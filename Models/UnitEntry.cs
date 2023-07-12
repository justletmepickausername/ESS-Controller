using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESS_Controller.Models
{
    class UnitEntry
    {
        [BsonElement("serialNumber")]
        public string serialNumber { get; set; }

        [BsonElement("_id")]
        public Guid myID { get; set; }

        [BsonElement("essID")]
        public Guid essID { get; set; }

        [BsonElement("unitType")]
        public string unitType { get; set; }
    }
}
