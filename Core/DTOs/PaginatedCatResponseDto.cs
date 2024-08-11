using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class PaginatedCatResponseDTO
    {
        public PaginatedCatResponseDTO() { }

        public PageResponseDto pageResponseDto { get; set; }
        public  IEnumerable<CatResponseDto> CatResponses { get; set; }
       


    }
}
