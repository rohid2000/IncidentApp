using Microsoft.Maui.Devices.Sensors;

public static class LocationFetcher
{
    public static async Task<Location> GetCurrentLocation()
    {
        try
        {
            // Request permission if needed
            var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            if (status != PermissionStatus.Granted)
            {
                status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
                if (status != PermissionStatus.Granted)
                {
                    // Permission not granted
                    return null;
                }
            }

            var request = new GeolocationRequest(GeolocationAccuracy.Best);
            var location = await Geolocation.GetLocationAsync(request);

            if (location != null)
            {
                return location;
            }
        }
        catch (Exception ex)
        {
            // Handle exception
            Console.WriteLine($"Error getting location: {ex.Message}");
        }

        return null;
    }
}