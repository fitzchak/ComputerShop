using System;

namespace ComputerShop.Data.Model
{
    public class Computer : Entity, IHaveDescription
    {
        public virtual ComputerBrand ComputerBrand { get; set; }

        public virtual Processor Processor { get; set; }

        public decimal RamCapacity { get; set; }

        public CapacityUnitEnum RamUnit { get; set; }

        public decimal HarddiskCapacity { get; set; }

        public CapacityUnitEnum HarddiskCapacityUnit { get; set; }

        public string ComputerModel { get; set; }

        public string Description { get; set; }
        
        public decimal Price { get; set; }
    }

    public enum CapacityUnitEnum
    {
        kB = 0, MB = 1, GB = 2, TB = 3
    }
}