using System.Collections.Generic;

namespace ComputerShop.Data.Context.StoredProcedures.Base
{
    public class StoredProcedureParameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public List<string> AdditionalTypeParams { get; set; }

        public StoredProcedureParameter(string name, string type)
        {
            Name = name;
            Type = type;

            AdditionalTypeParams = new List<string>();
        }

        public StoredProcedureParameter(string name, string type, string value) : this(name, type)
        {
            Value = value;
        }

        public string GetFormattedAdditionalParams()
        {
            if (AdditionalTypeParams.Count < 1)
            {
                return string.Empty;
            }

            var result = "(";

            foreach (var additionalTypeParam in AdditionalTypeParams)
            {
                result += additionalTypeParam + ",";
            }

            result = result.Substring(0, result.Length - 1);

            result += ")";

            return result;
        }
    }

    public class StoredProcedureParameters : Dictionary<string, StoredProcedureParameter>
    {
        
    }
}