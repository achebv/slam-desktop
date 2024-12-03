namespace AmazonDeliveryPlanner.API.data
{
    public class PlannerEntity
    {
        public static bool _ListModeToString = false;

        public long id { get; set; }

        public string email { get; set; }
        public string password { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string phone { get; set; }
        public string role_name { get; set; }
        public bool is_active { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string[] roles { get; set; }
        // public string fire_id { get; set; } // string or?
        // public string app_version { get; set; } // string or?
        public string token { get; set; }

        public override string ToString()
        {
            return (
                    (string.IsNullOrWhiteSpace(this.last_name) ? "" : this.last_name.Trim()) + " " +
                    (string.IsNullOrWhiteSpace(this.first_name) ? "" : this.first_name.Trim()) + " " + "(" +
                    (this.roles != null ? "" + string.Join(", ", this.roles) : "") + ") - " +
                    (string.IsNullOrWhiteSpace(this.email) ? "" : this.email.Trim())
                   ).Trim();

        }
    }
}
