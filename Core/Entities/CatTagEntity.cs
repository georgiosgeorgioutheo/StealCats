using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class CatTagEntity
    {
        public int CatId { get; set; }
        public CatEntity Cat { get; set; }
        public int TagId { get; set; }
        public TagEntity Tag { get; set; }
    }
}
