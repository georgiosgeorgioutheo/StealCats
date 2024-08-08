using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CatEntity
    {


        public int Id { get; set; }
        public string CatId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        [JsonIgnore]
        public byte[] Image { get; set; }
        public DateTime Created { get; set; }
        [JsonIgnore]
        public List<CatTagEntity> CatTags { get; set; }
        public CatEntity()
        {
            CatId = string.Empty;
            Image = Array.Empty<byte>();
            CatTags = new List<CatTagEntity>();
        }

        // Constructor with parameters to ensure non-nullable properties are initialized
        public CatEntity(string catId, int width, int height, byte[] image, DateTime created)
        {
            CatId = catId ?? throw new ArgumentNullException(nameof(catId));
            Width = width;
            Height = height;
            Image = image ?? throw new ArgumentNullException(nameof(image));
            Created = created;
            CatTags = new List<CatTagEntity>();
        }

        [NotMapped]
        public string? Base64Image => Image != null ? Convert.ToBase64String(Image) : null;
    }
       
}
