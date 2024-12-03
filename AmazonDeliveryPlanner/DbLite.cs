using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;

namespace AmazonDeliveryPlanner
{
    internal class DbLite
    {
        static string configurationFilePath = Utilities.GetApplicationPath() + Path.DirectorySeparatorChar;

        private string connectionString = "Data Source=" + configurationFilePath + "planner.db;Version=3;";

        public DbLite()
        {
            InitializeDatabase();
        }

        private void InitializeDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("CREATE TABLE IF NOT EXISTS MyTable (ID INTEGER PRIMARY KEY)", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public void AddIdToList(int idToAdd)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("INSERT OR IGNORE INTO MyTable (ID) VALUES (@id)", connection))
                {
                    command.Parameters.AddWithValue("@id", idToAdd);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteIdFromList(int idToDelete)
        {
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("DELETE FROM MyTable WHERE ID = @id", connection))
                {
                    command.Parameters.AddWithValue("@id", idToDelete);
                    command.ExecuteNonQuery();
                }
            }
        }

        public List<int> GetListOfIds()
        {
            List<int> listOfIds = new List<int>();
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                using (SQLiteCommand command = new SQLiteCommand("SELECT ID FROM MyTable", connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            listOfIds.Add(id);
                        }
                    }
                }
            }
            return listOfIds;
        }
    }
}
