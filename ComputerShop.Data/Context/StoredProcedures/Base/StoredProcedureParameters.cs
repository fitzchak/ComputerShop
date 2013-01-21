using System.Collections.Generic;

namespace ComputerShop.Data.Context.StoredProcedures.Base
{
    public class StoredProcedureParameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }

        public StoredProcedureParameter(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public StoredProcedureParameter(string name, string type, string value) : this(name, type)
        {
            Value = value;
        }
    }

    public class StoredProcedureParameters : Dictionary<string, StoredProcedureParameter>
    {
        
    }
}