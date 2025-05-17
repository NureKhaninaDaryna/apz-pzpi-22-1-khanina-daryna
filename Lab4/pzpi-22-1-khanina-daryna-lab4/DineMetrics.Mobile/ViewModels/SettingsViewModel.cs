using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DineMetrics.Core.Dto;
using DineMetrics.Mobile.State.Authenticators;

namespace DineMetrics.Mobile.ViewModels;

public partial class SettingsViewModel : ObservableObject
{
    private readonly HttpClient _httpClient;

    public ObservableCollection<EateryWithIdDto> Eateries { get; } = new();

    [ObservableProperty]
    private EateryWithIdDto selectedEatery;
    
    [ObservableProperty]
    private string fromTime = "08:00";

    [ObservableProperty]
    private string toTime = "17:00";

    [ObservableProperty]
    private int maximumCapacity;

    [ObservableProperty]
    private double temperatureThreshold;

    public IRelayCommand LoadCommand { get; }
    public IRelayCommand SaveCommand { get; }

    public SettingsViewModel(IHttpClientFactory clientFactory, IAuthenticator authenticator)
    {
        _httpClient = clientFactory.CreateClient();
        _httpClient.BaseAddress = new Uri("http://10.0.2.2:5048/");
        
        if (!authenticator.IsLoggedIn)
        {
            _ = Shell.Current.GoToAsync("login");
            return;
        }
        
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", authenticator.CurrentUser!.Token);

        LoadCommand = new RelayCommand(async () => await LoadEateries());
        SaveCommand = new RelayCommand(async () => await SaveUpdates());
    }

    partial void OnSelectedEateryChanged(EateryWithIdDto value)
    {
        if (value != null)
        {
            var hours = value.OperatingHours.Split('-');
            FromTime = hours[0].Trim();
            ToTime = hours[1].Trim();

            MaximumCapacity = value.MaximumCapacity;
            TemperatureThreshold = value.TemperatureThreshold;
        }
    }
    
    private async Task LoadEateries()
    {
        try
        {
            var response = await _httpClient.GetAsync("eateries");

            if (response.IsSuccessStatusCode)
            {
                var list = await response.Content.ReadFromJsonAsync<List<EateryWithIdDto>>();
                if (list != null)
                {
                    Eateries.Clear();
                    foreach (var e in list)
                        Eateries.Add(e);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Failed to load eateries: " + ex.Message);
        }
    }

    private async Task SaveUpdates()
    {
        if (SelectedEatery is null)
            return;

        var id = SelectedEatery.Id;

        var eatery = Eateries.FirstOrDefault(e => e.Id == id);
        if (eatery == null)
                return;

        await _httpClient.PutAsJsonAsync($"admins/{id}/operating-hours", new
        {
            From = FromTime,
            To = ToTime,
        });

        await _httpClient.PutAsJsonAsync($"admins/{id}/maximum-capacity", new
        {
            Capacity = MaximumCapacity
        });

        await _httpClient.PutAsJsonAsync($"admins/{id}/temperature-threshold", new
        {
            MinTemperature = TemperatureThreshold
        });

        eatery.OperatingHours = FromTime + "-" + ToTime;
        eatery.MaximumCapacity = MaximumCapacity;
        eatery.TemperatureThreshold = TemperatureThreshold;
    }
}