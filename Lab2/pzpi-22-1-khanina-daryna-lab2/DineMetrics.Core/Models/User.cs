using DineMetrics.Core.Enums;

namespace DineMetrics.Core.Models
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public UserRole Role { get; set; } = UserRole.Manager;
        
        public DateOnly? AppointmentDate { get; set; }
        
        public virtual ICollection<Employee>? Employees { get; set; } = new List<Employee>();
        
        public virtual Eatery? Eatery { get; set; }
    }
}
