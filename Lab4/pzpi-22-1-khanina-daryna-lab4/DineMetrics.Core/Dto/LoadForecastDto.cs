namespace DineMetrics.Core.Dto;

public class LoadForecastDto
{
    public List<ForecastDay> Forecast { get; set; } = new();
}

public class ForecastDay
{
    public DateTime Date { get; set; }
    public int ExpectedVisitors { get; set; }
}