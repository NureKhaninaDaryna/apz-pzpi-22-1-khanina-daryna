using DineMetrics.Core.Enums;
using System.Text.Json.Serialization;

namespace DineMetrics.Core.Dto;

public class EateryWithIdDto : EateryDto
{
    public int Id { get; set; }
}

public class EateryDto
{
    public string Name { get; set; }

    public string Address { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public EateryType Type { get; set; }
        
    public DateOnly OpeningDay { get; set; }
    
    public string OperatingHours { get; set; }
    
    public int MaximumCapacity { get; set; }
    
    public double TemperatureThreshold { get; set; }
}