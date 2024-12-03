using AmazonDeliveryPlanner.API.data;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;

namespace AmazonDeliveryPlanner.API
{
    public class DriverList
    {
        public Driver[] drivers { get; set; }
    }

    //public class Root
    //{
    //    public List<Driver> drivers { get; set; }
    //}

    public class Driver
    {
        public static bool _ListModeToString = false;

        public long id { get; set; }
        public long driver_id { get; set; }
        public string fleet_id { get; set; }
        public string reg_plate { get; set; }
        public string user_id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public bool is_user_active { get; set; }
        public string driver_group_id { get; set; }
        public string group_name { get; set; }
        public string driver_status { get; set; }
        public int? break_interval { get; set; }
        public DateTime created_at { get; set; }
        public More_Info more_info { get; set; }

        public string message_id { get; set; }

        public override string ToString()
        {
            if (_ListModeToString)
                return (string.IsNullOrWhiteSpace(this.group_name) ? "" : this.group_name.Replace(this.first_name != null ? this.first_name.Trim() : "Z_8_1", "")).Replace(this.last_name != null ? this.last_name.Trim() : "Z_8_1", "") + "  "
                        + this.first_name + " " + this.last_name + "  (" + this.reg_plate + ")";
            else
                return string.IsNullOrWhiteSpace(this.group_name) ? this.first_name + " " + this.last_name : this.group_name;
        }
    }

    public class More_Info
    {
        public int km { get; set; }
        public string address { get; set; }
    }

    public class DriversAPI
    {
        public static DriverList GetDrivers()
        {
            using (WebClient wc = new WebClient())
            {
                string getDriversURL = GlobalContext.SerializedConfiguration.AdminURL + GlobalContext.SerializedConfiguration.ApiBaseURL + "/auth2/external/drivers";
                GlobalContext.Log("Getting drivers from  '{0}'", getDriversURL);
                var jsonResponse = wc.DownloadString(getDriversURL);
                DriverList driverList = JsonConvert.DeserializeObject<DriverList>(jsonResponse);
                return driverList;
            }
        }

        public static DriverRouteEntity /*async Task<DriverRouteEntity>*/ PostRoute(API.data.DriverRouteEntity driverEntity)
        {
            // ...

            string payload = JsonConvert.SerializeObject(new
            {
                driver_id = driverEntity.driver_id,
                plan_note = driverEntity.plan_note,
                op_note = driverEntity.op_note,
                vrid = driverEntity.vrid,
                loc1 = driverEntity.loc1,
                loc2 = driverEntity.loc2,
                loc3 = driverEntity.loc3,
                pick_up_date = driverEntity.pick_up_date.ToString("yyyy-MM-dd HH:mm:ss"),
                shift = driverEntity.shift
                //driver_id = driver_id,
                //plan_note = "text pentru planner",
                //op_note = "ceva text pr operator",
                //vrid = "AMZ12345",
                //loc1 = "BHX3",
                //loc2 = "BHX8",
                //loc3 = "",
                //pick_up_date = "2022-10-23 18:45:45",
                //shift = "D"
            });

            using (var client = new HttpClient())
            {
                var content = new StringContent(payload, Encoding.UTF8, "application/json");

                // HttpResponseMessage response = await client.PostAsync(Settings.Default.ApiPostEndPoint, content);

                throw new Exception("undefined api endpoint");
            }
        }
    }
}
