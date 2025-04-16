using DineMetrics.Core.Enums;

namespace DineMetrics.Core.Models
{
    public class Eatery : BaseEntity
    {
        public string Name { get; set; } = null!;

        public string Address { get; set; } = null!;

        public EateryType Type { get; set; }
        
        public DateOnly OpeningDay { get; set; }

        public virtual ICollection<Device> Devices { get; set; } = new List<Device>();

        public virtual ICollection<User> Managers { get; set; } = new List<User>();
        
        public string OperatingHours { get; set; } = "08:00-22:00"; // Формат HH:mm-HH:mm
        
        public int MaximumCapacity { get; set; } = 100; // Максимальна кількість відвідувачів
        
        public double TemperatureThreshold { get; set; } = 25.0; // Температурний поріг
    }
}
