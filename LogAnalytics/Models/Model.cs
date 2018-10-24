

namespace LogAnalytics.Models
{
    public class Logs
    {
        public LogResultTable[] Tables { get; set; }
    }

    public class LogResultTable
    {
        public string Name { get; set; }
        public LogResultColumn[] Columns { get; set; }
        public object[][] Rows { get; set; }
    }

    public class LogResultColumn
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }
}
