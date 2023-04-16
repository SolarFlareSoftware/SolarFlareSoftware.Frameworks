namespace SolarFlareSoftware.Fw1.Core
{
    public class QueryParameter
    {   
        public string ParamName { get; set; }
        public QueryParameterTypeEnum ParamType { get; set; }
        public string ParamValue { get; set; }
        public int ParamLength { get; set; } = 0;
    }
}
