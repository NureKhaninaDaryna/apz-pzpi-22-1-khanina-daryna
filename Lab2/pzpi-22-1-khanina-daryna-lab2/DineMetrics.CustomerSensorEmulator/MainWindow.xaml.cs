using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DineMetrics.CustomerSensorEmulator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private readonly HttpClient _httpClient = new();
    private bool _isReading;
    private readonly Random _random = new();

    // Константи
    private const string CustomerMetricsUrl = "https://localhost:7239/CustomerMetrics";
    private const string DeviceId = "83242380-277b-40d5-82a4-cfc5663d4994";
    
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private async void StartStop_Click(object sender, RoutedEventArgs e)
    {
        if (!_isReading)
        {
            _isReading = true;
            StatusText.Text = "Reading started...";
                
            while (_isReading)
            {
                int customerCount = GetCustomerCount();
                CustomerCountText.Text = customerCount.ToString();

                await SendCustomerCountToServer(customerCount);

                await Task.Delay(5000); // Затримка 5 секунд
            }
        }
        else
        {
            _isReading = false;
            StatusText.Text = "Reading stopped.";
        }
    }
    
    // Метод для імітації кількості користувачів
    private int GetCustomerCount()
    {
        return _random.Next(-15, 16); // Значення від -15 до 15
    }

    // Метод для відправки даних на сервер
    private async Task SendCustomerCountToServer(int count)
    {
        try
        {
            var data = new
            {
                deviceId = DeviceId,
                count = count,
                time = DateTime.UtcNow
            };

            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(CustomerMetricsUrl, content);

            StatusText.Text = response.IsSuccessStatusCode ? 
                $"Sent: {count} users at {DateTime.Now:T}" : 
                "Failed to send data.";
        }
        catch (Exception ex)
        {
            StatusText.Text = $"Error: {ex.Message}";
        }
    }
}