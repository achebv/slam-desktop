using Newtonsoft.Json;

namespace AmazonDeliveryPlanner
{
    public class SerializedConfiguration
    {
        // Fields
        private string adminURL;
        private string apiBaseURL;
        private AmzTab[] amzTabs;
        private RelayAuthInt relayAuth;
        private string adminToken;
        private string adminTokenTpl;
        private string downloadDirectoryPath;
        private string fileUploadURL;
        private bool debug;

        // Properties

        [JsonProperty("admin_url")]
        public string AdminURL
        {
            get => adminURL;
            set => adminURL = value;
        }

        [JsonProperty("api_base_url")]
        public string APIBaseURL
        {
            get => apiBaseURL;
            set => apiBaseURL = value;
        }

        [JsonProperty("amz_tabs")]
        public AmzTab[] AmzTabs
        {
            get => amzTabs;
            set => amzTabs = value;
        }

        [JsonProperty("relay_auth")]
        public RelayAuthInt RelayAuth
        {
            get => relayAuth;
            set => relayAuth = value;
        }

        // sha1(email).sha1(password)
        [JsonProperty("admin_token")]
        public string AdminToken
        {
            get => adminToken;
            set => adminToken = value;
        }

        [JsonProperty("download_directory_path")]
        public string DownloadDirectoryPath
        {
            get => downloadDirectoryPath;
            set => downloadDirectoryPath = value;
        }

        [JsonProperty("file_upload_url")]
        public string FileUploadURL
        {
            get => fileUploadURL;
            set => fileUploadURL = value;
        }

        [JsonProperty("debug")]
        public bool Debug
        {
            get => debug;
            set => debug = value;
        }

        // Nested Class: AmzTab
        public class AmzTab
        {
            [JsonProperty("url")]
            public string URL { get; set; }

            [JsonProperty("min_min")]
            public int MinMin { get; set; }

            [JsonProperty("max_min")]
            public int MaxMin { get; set; }
        }

        // Nested Class: RelayAuth
        public class RelayAuthInt
        {
            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }
        }
    }
}
