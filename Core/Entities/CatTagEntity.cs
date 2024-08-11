
using System.Text.Json.Serialization;


namespace Core.Entities
{
    public class CatTagEntity
    {

        public CatTagEntity()
        {
            Cat = new CatEntity();
            Tag = new TagEntity();
        }
        public CatTagEntity(CatEntity cat, TagEntity tag)
        {
            Cat = cat ?? throw new ArgumentNullException(nameof(cat));
            Tag = tag ?? throw new ArgumentNullException(nameof(tag));
        }
        public int CatId { get; set; }
        [JsonIgnore]
        public CatEntity Cat { get; set; }
        public int TagId { get; set; }
        [JsonIgnore]
        public TagEntity Tag { get; set; }

     
    }
}
