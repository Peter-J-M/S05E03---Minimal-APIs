using System.Data;

namespace iRosterApi.Common
{
    public class ParameterInfo
    {
        public string VariableName { get; set; }
        public string Value { get; set; }
        public DbType? DataType { get; set; }
        public ParameterDirection Direction { get; set; } = ParameterDirection.Input;
        public int? Size { get; set; }
    }
}
