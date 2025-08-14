using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InventoryManagementSystem
{
    
    public static class Logger
    {
        private static string connectionString = "Server=LAPTOP-OKO0VKK3;Database=MaintenanceInventoryDB;Trusted_Connection=True;";
        private static DateTime date = DateTime.Now;
        public static void AddLog(int userId, string action, string tableAffected, int recordId, string description)
        {
            string query = @"INSERT INTO Logs (UserId, Action_, TableAffected, RecordId, logDescription_, ActionDate) 
                         VALUES (@UserId, @Action, @TableAffected, @RecordId, @LogDescription, @date)";

            using (SqlConnection conn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@Action", action);
                cmd.Parameters.AddWithValue("@TableAffected", tableAffected);
                cmd.Parameters.AddWithValue("@RecordId", recordId);
                cmd.Parameters.AddWithValue("@LogDescription", description);
                cmd.Parameters.AddWithValue("@date", date);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
