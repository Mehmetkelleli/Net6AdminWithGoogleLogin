namespace Backend.Domain.EntityClass.BaseClass
{
    public class Base
    {
        public int Id { get; set; }
        public DateTime? CreateTime { get; set; }
        public DateTime? UpdatedTime { get; set; }
        public string? OldObject { get; set; }
    }
}
