using Microsoft.Data.SqlClient;
using System.Data;

namespace MiniAccountManagementSystem.Data
{
    public class DbHelper
    { 
    private readonly string _connectionString;

    public DbHelper(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }

        public List<(string Id, string Name)> GetIdNameList(string storedProcName)
        {
            var result = new List<(string, string)>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            using (SqlCommand cmd = new SqlCommand(storedProcName, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string id = reader[0].ToString();
                        string name = reader[1].ToString();
                        result.Add((id, name));
                    }
                }
            }

            return result;
        }


        public void AssignModuleAccess(string roleId, int moduleId)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand("sp_AssignModuleAccessRights", conn))
        {
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RoleID", roleId);
            cmd.Parameters.AddWithValue("@ModuleID", moduleId);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
}
