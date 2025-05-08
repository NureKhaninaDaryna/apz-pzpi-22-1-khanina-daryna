namespace DineMetrics.Core.Models
{
    public class CustomerMetric : BaseEntity
    {
        public int Count { get; set; } 

        public DateTime Time { get; set; }

        public virtual Device Device { get; set; } = null!;

        public virtual Report Report { get; set; } = null!;
    }
}
