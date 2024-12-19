using AmazonDeliveryPlanner.API.data;
using Newtonsoft.Json;
using System.Net;

namespace AmazonDeliveryPlanner.API
{
    public class InitAppApi
    {
        public PlannerEntity[] planners { get; set; }
        public ApiConfig configuration { get; set; }


        public static InitAppApi GetAppInit()
        {
            using (WebClient wc = new WebClient())
            {
                string getPlannersURL = GlobalContext.SerializedConfiguration.AdminURL + GlobalContext.SerializedConfiguration.APIBaseURL;
                    // + GlobalContext.SerializedConfiguration.PlannerListURL;
                // MessageBox.Show(getPlannersURL);
                GlobalContext.Log("Getting planners from  '{0}'", getPlannersURL);
                var jsonResponse = wc.DownloadString(getPlannersURL);
                InitAppApi initAppConfig = JsonConvert.DeserializeObject<InitAppApi>(jsonResponse);
                // GlobalContext.ApiConfig = initAppConfig.configuration;
                return initAppConfig;
            }
        }

    }

}
