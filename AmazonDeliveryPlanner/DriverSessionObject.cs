using CefSharp;

namespace AmazonDeliveryPlanner
{
    public class DriverSessionObject
    {
        long driverId;
        RequestContextSettings reqContextSettings;

        DriverUserControl _driverUC;

        public long DriverId { get => driverId; set => driverId = value; }
        public RequestContextSettings ReqContextSettings { get => reqContextSettings; set => reqContextSettings = value; }
        public DriverUserControl DriverUC { get => _driverUC; set => _driverUC = value; }
    }
}
