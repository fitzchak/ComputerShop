using System;
using System.ComponentModel.DataAnnotations;

namespace ComputerShop.Data.Model
{
    public class Computer : Entity, IHaveDescription, IHaveName
    {
        public virtual ComputerBrand ComputerBrand { get; set; }

        public virtual Processor Processor { get; set; }

        public decimal RamCapacity { get; set; }

        public CapacityUnitEnum RamUnit { get; set; }

        public decimal HarddiskCapacity { get; set; }

        public CapacityUnitEnum HarddiskCapacityUnit { get; set; }

        public virtual ComputerModel ComputerModel { get; set; }
        
        public string Description { get; set; }
        
        public string Name { get; set; }
    }

    public enum CapacityUnitEnum
    {
        kB, MB, GB
    }
}