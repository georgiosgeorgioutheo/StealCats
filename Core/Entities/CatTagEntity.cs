using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CatTagEntity
    {
        public int CatId { get; set; }
        [JsonIgnore]
        public CatEntity Cat { get; set; }
        public int TagId { get; set; }
        [JsonIgnore]
        public TagEntity Tag { get; set; }
    }
}
