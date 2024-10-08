﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class PageResponseDto
    {
        public PageResponseDto() { }
        public int PageSize { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public string PageInfo { get; set; }
    }
}
