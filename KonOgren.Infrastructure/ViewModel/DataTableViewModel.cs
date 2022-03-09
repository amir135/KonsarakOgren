using System;
using System.Collections.Generic;
using System.Text;

namespace KonOgren.Infrastructure.ViewModel
{
    public class DataTableViewModel
    {
        public string sortColumn { get; set; }
        public string sortColumnDirection { get; set; }
        public string searchValue { get; set; }
        public int skip { get; set; }
        public int pageSize { get; set; }
        public string draw { get; set; }
        public List<ColumnFiltering> columns { get; set; }

        public DataTableViewModel()
        {
            columns = new List<ColumnFiltering>();
        }
    }

    public class ColumnFiltering
    {
        public int index { get; set; }
        public string value { get; set; }
    }
}
