namespace DineMetrics.Core.Enums
{
    public enum DeviceState
    {
        Offline,      // Device is turned off or not connected
        Online,       // Device is active and connected
        Maintenance,  // Device is under maintenance or requires attention
        Error,        // Device encountered an error or malfunction
        Standby,      // Device is on standby mode, awaiting action
        Updating,     // Device is in the process of updating its software or firmware
        Disabled      // Device is disabled by admin or system
    }
}
