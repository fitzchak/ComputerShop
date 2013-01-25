
namespace ComputerShop.Data.Model
{
    public class ComputerBrand : Entity, IHaveName, IHaveDescription
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }
}