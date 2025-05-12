namespace DineMetrics.Core.Models
{
    public class TemperatureMetric : BaseEntity
    {
        public double Value { get; set; }

        public DateTime Time { get; set; }

        public virtual Device Device { get; set; } = null!;

        public virtual Report Report { get; set; } = null!;
    }
}
