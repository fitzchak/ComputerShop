using System.ComponentModel.DataAnnotations;

namespace ComputerShop.Data.Model
{
    public class ComputerBrand : Entity, IHaveName
    {
        public string Name { get; set; }
    }
}