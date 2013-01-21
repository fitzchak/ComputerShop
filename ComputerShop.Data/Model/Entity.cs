using System.ComponentModel.DataAnnotations;

namespace ComputerShop.Data.Model
{
    public class Entity : IHaveTimestamp, IHaveId
    {
        public virtual int Id { get; set; }
        public byte[] Timestamp { get; set; }
    }
}