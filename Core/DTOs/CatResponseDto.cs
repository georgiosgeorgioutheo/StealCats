using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CatResponseDto
    {
        public int Id { get; set; }
        public string CatId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public DateTime Created { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Tags { get; set; }
    }
}
