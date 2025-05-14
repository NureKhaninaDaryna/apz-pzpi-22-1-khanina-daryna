using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using DineMetrics.Core.Dto;
using DineMetrics.Core.Models;
using DineMetrics.Mobile.State.Authenticators;

namespace DineMetrics.Mobile.ViewModels;

public partial class MetricsViewModel : ObservableObject
{
    private readonly HttpClient _httpClient;

    public MetricsViewModel(IHttpClientFactory httpClientFactory, IAuthenticator authenticator)
    {
        _httpClient = httpClientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("http://10.0.2.2:5048/");
        CustomerMetrics = [];
        TemperatureMetrics = [];

        if (!authenticator.IsLoggedIn)
        {
            _ = GoToLoginPage();
            return;
        }

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authenticator.CurrentUser!.Token);
        _ = LoadMetrics();
    }

    public ObservableCollection<TemperatureMetricDto> TemperatureMetrics { get; set; }
    public ObservableCollection<CustomerMetricDto> CustomerMetrics { get; set; }

    private async Task GoToLoginPage()
    {
        await Shell.Current.GoToAsync("login");
    }
    
    private async Task LoadMetrics()
    {
        await LoadTemperatureMetrics();
        await LoadCustomerMetrics();
    }

    private async Task LoadTemperatureMetrics()
    {
        try
        {
            var response = await _httpClient.GetAsync("TemperatureMetrics");

            if (response.IsSuccessStatusCode)
            {
                var metrics = await response.Content.ReadFromJsonAsync<List<TemperatureMetricDto>>();

                if (metrics != null)
                {
                    TemperatureMetrics.Clear();

                    foreach (var metric in metrics)
                    {
                        TemperatureMetrics.Add(metric);
                    }
                }
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to load temperature metrics: {error}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while loading temperature metrics: {ex.Message}");
        }
    }
    
    private async Task LoadCustomerMetrics()
    {
        try
        {
            var response = await _httpClient.GetAsync("CustomerMetrics");

            if (response.IsSuccessStatusCode)
            {
                var metrics = await response.Content.ReadFromJsonAsync<List<CustomerMetricDto>>();

                if (metrics != null)
                {
                    CustomerMetrics.Clear();

                    foreach (var metric in metrics)
                    {
                        CustomerMetrics.Add(metric);
                    }
                }
            }
            else
            {
                var error = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to load customer metrics: {error}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while loading customer metrics: {ex.Message}");
        }
    }
}