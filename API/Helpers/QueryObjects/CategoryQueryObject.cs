using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers.QueryObjects
{
    public class CategoryQueryObject
    {
        public string? Name { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber{ get; set; }
    }
}