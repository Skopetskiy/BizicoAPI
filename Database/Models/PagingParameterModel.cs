using System;
using System.Collections.Generic;
using System.Text;

namespace Database.Models
{
   
    public class PagingParameterModel
    {
        const int maxPageSize = 100;
        public int pageNumber { get; set; } = 1;
        private int _pageSize { get; set; } = 10;
        public string sorting { get; set; } = "";

        public int pageSize
        {
            get { return _pageSize; }
            set
            {
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
            }
        }
    }
}
