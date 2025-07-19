using Ioc.Core.DbModel.Models;

namespace Ioc.ObjModels.Model.DataTableModel
{


    public class DataTableParameters
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public string? Search { get; set; }
        public List<DataTableOrder> Order { get; set; }
    }

    public class DataTableSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

    public class DataTableOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class DataTableResponse
    {
        public int Draw { get; set; }
        public int RecordsTotal { get; set; }
        public int RecordsFiltered { get; set; }
        public List<object> Data { get; set; }
    }
    public class FilteredUsers
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }
        public IEnumerable<object>? Data { get; set; }
    }
}

