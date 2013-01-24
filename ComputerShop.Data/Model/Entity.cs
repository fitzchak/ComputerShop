namespace ComputerShop.Data.Model
{
    public class Entity : IHaveId, IHaveTimestamp
    {
        public virtual int Id { get; set; }
        public byte[] Timestamp { get; set; }
    }
}