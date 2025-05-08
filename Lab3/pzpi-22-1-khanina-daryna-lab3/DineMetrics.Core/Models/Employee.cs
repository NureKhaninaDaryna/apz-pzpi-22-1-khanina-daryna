namespace DineMetrics.Core.Models
{
    public class Employee : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string Position { get; set; } = null!;

        public string PhoneNumber { get; set; } = null!;

        public DateOnly WorkStart { get; set; }

        public DateOnly? WorkEnd { get; set; } 

        public virtual User Manager { get; set; } = null!;
    }
}
