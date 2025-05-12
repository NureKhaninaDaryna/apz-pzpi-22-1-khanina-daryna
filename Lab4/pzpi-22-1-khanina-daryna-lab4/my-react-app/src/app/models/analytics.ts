export interface DashboardDataDto {
   averageTemperature: number;
   totalMetrics: number;
}

export interface TrendAnalysisDto {
   date: string; // ISO date string
   averageValue: number;
}