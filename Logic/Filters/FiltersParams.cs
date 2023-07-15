using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Filters
{
    public class FilterParams
    {
        public bool? Shine { get; set; }
        public OrderByParams OrderBy { get; set; }
        public string Searched { get; set; }
    }
}
