using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;

namespace DineMetrics.TemperatureSensorEmulator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly HttpClient _httpClient = new();
    private bool _isReading;
    private readonly Random _random = new();
    
    private const string TemperatureMetricsUrl = "https://localhost:7239/TemperatureMetrics";
    private const string DeviceGetSecondsDelay = $"https://localhost:7239/Devices/{DeviceId}/delay";
    private const string DeviceId = "83242380-277b-40d5-82a4-cfc5663d4994";
    
    private int _secondsDelay = 5;
    
    public MainWindow()
    {
        InitializeComponent();
        
        Loaded += async (sender, args) =>
        {
            var response = await _httpClient.GetAsync(DeviceGetSecondsDelay);

            if (!response.IsSuccessStatusCode) return;
            
            var jsonData = await response.Content.ReadAsStringAsync();
                
            _secondsDelay = int.Parse(jsonData);
        };
    }
    
    private async void StartStop_Click(object sender, RoutedEventArgs e)
    {
        if (!_isReading)
        {
            _isReading = true;
            StatusText.Text = "Reading started...";
            
            while (_isReading)
            {
                double temperature = GetTemperature();
                TemperatureText.Text = $"{temperature:F2} °C";

                await SendTemperatureToServer(temperature);

                await Task.Delay(_secondsDelay * 1000);
            }
        }
        else
        {
            _isReading = false;
            StatusText.Text = "Reading stopped.";
        }
    }
    
    // Метод для імітації зчитування температури
    private double GetTemperature()
    {
        return 20 + _random.NextDouble() * 15; // Випадкове значення від 20 до 35
    }

    // Метод для відправки температури на сервер
    private async Task SendTemperatureToServer(double temperature)
    {
        try
        {
            var data = new
            {
                deviceId = DeviceId,
                value = temperature,
                time = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Надсилаємо дані на сервер
            var response = await _httpClient.PostAsync(TemperatureMetricsUrl, content);

            StatusText.Text = response.IsSuccessStatusCode ? 
                "Temperature sent successfully." : 
                "Failed to send temperature data.";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Error: {ex.Message}";
        }
    }
}