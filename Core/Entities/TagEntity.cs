using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class TagEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public DateTime Created { get; set; }
        [JsonIgnore]
        public List<CatTagEntity> CatTags { get; set; }
        public TagEntity()
        {
            Name = string.Empty;
            CatTags = new List<CatTagEntity>();
        }

        public TagEntity(string name, DateTime created)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Created = created;
            CatTags = new List<CatTagEntity>();
        }
    }
}
