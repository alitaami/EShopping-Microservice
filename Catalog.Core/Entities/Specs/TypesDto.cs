using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Core.Entities.Models
{
    public class TypesDto
    {
        public string Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }
    }
}
