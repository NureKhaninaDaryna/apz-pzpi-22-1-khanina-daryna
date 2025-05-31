using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DineMetrics.Mobile.ViewModels;

namespace DineMetrics.Mobile.Pages;

public partial class MetricsPage : ContentPage
{
    public MetricsPage(MetricsViewModel metricsViewModel)
    {
        InitializeComponent();
        BindingContext = metricsViewModel;
    }
}