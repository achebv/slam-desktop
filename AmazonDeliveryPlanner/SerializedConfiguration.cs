using Newtonsoft.Json;

namespace AmazonDeliveryPlanner
{
    public class SerializedConfiguration
    {
        string adminURL;
        string driverListURL;
        string[] defaultTabs;
        string apiBaseURL;
        string plannerListURL;
        int autoDownloadExportFileRandomMinInterval;
        int autoDownloadExportFileRandomMaxInterval;
        string downloadDirectoryPath;
        string fileUploadURL;
        string planningOverviewURL;
        bool debug;

        [JsonProperty("admin_url")]
        public string AdminURL { get => adminURL; set => adminURL = value; }
        [JsonProperty("driver_list_url")]
        public string DriverListURL { get => driverListURL; set => driverListURL = value; }
        [JsonProperty("default_tabs")]
        public string[] DefaultTabs { get => defaultTabs; set => defaultTabs = value; }
        [JsonProperty("api_base_url")]
        public string ApiBaseURL { get => apiBaseURL; set => apiBaseURL = value; }
        [JsonProperty("planner_list_url")]
        public string PlannerListURL { get => plannerListURL; set => plannerListURL = value; }
        [JsonProperty("auto_download_export_file_random_min_interval")]
        public int AutoDownloadExportFileRandomMinInterval { get => autoDownloadExportFileRandomMinInterval; set => autoDownloadExportFileRandomMinInterval = value; }
        [JsonProperty("auto_download_export_file_random_max_interval")]
        public int AutoDownloadExportFileRandomMaxInterval { get => autoDownloadExportFileRandomMaxInterval; set => autoDownloadExportFileRandomMaxInterval = value; }
        [JsonProperty("download_directory_path")]
        public string DownloadDirectoryPath { get => downloadDirectoryPath; set => downloadDirectoryPath = value; }
        [JsonProperty("file_upload_url")]
        public string FileUploadURL { get => fileUploadURL; set => fileUploadURL = value; }
        [JsonProperty("debug")]
        public bool Debug { get => debug; set => debug = value; }
        [JsonProperty("planning_overview_url")]
        public string PlanningOverviewURL { get => planningOverviewURL; set => planningOverviewURL = value; }
    }

}
