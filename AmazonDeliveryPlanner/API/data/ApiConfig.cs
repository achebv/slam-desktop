using System.Collections.Generic;

namespace AmazonDeliveryPlanner.API.data
{
    public class ApiConfig
    {
        public List<TripPageConfiguration> tripPages { get; set; }
        public AuthConfigurations relayAuth { get; set; }

        public AuthConfigurations adminAuth { get; set; }
    }

    public class AuthConfigurations
    {
        public string username { get; set; }
        public string password { get; set; }
    }


    public class TripPageConfiguration
    {
        string url;
        int minRandomIntervalMinutes;
        int maxRandomIntervalMinutes;

        public string Url { get => this.url; set => this.url = value; }

        public int MinRandomIntervalMinutes { get => minRandomIntervalMinutes; set => minRandomIntervalMinutes = value; }

        public int MaxRandomIntervalMinutes { get => maxRandomIntervalMinutes; set => maxRandomIntervalMinutes = value; }
    }
}
