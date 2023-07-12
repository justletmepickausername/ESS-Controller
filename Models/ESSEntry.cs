using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESS_Controller.Models
{
    public class ESSEntry
    {
        [BsonElement("id")]
        public Guid id { get; set; }

        [BsonElement("startTime")]
        public DateTime startTime { get; set; }

        [BsonElement("endTime")]
        public DateTime? endTime { get; set; }

        [BsonElement("cyclesToPerform")]
        public int cyclesToPerform { get; set; }

        [BsonElement("dwellTime")]
        public int dwellTime { get; set; }

        [BsonElement("cyclesFinished")]
        public int cyclesFinished { get; set; }

        [BsonElement("maxTemp")]
        public int maxTemp { get; set; }

        [BsonElement("minTemp")]
        public int minTemp { get; set; }

        [BsonElement("status")]
        public string status { get; set; }
    }
}
