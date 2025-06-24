using Microsoft.Data.SqlClient;
using MiniAccountManagementSystem.Entity;
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


        public void AssignUserRole(string userId, string roleId)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_AssignUserRole", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@RoleId", roleId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<RoleModuleAccess> GetModuleAccess(string roleId)
        {
            var list = new List<RoleModuleAccess>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_GetRoleModuleAccess", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoleId", roleId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new RoleModuleAccess
                        {
                            ModuleName = reader["ModuleName"].ToString(),
                            CanView = Convert.ToBoolean(reader["CanView"]),
                            CanEdit = Convert.ToBoolean(reader["CanEdit"])
                        });
                    }
                }
            }
            return list;
        }

        public void AssignModuleAccess(string roleId, string moduleName, bool canView, bool canEdit)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_AssignModuleAccess", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@RoleId", roleId);
                cmd.Parameters.AddWithValue("@ModuleName", moduleName);
                cmd.Parameters.AddWithValue("@CanView", canView);
                cmd.Parameters.AddWithValue("@CanEdit", canEdit);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public void ManageChartOfAccount(string action, int? id, string name, int? parentId, string type)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_ManageChartOfAccounts", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Action", action);
                cmd.Parameters.AddWithValue("@AccountID", (object)id ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AccountName", (object)name ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ParentAccountID", (object)parentId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@AccountType", (object)type ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Account> GetChartOfAccounts()
        {
            var list = new List<Account>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("sp_GetChartOfAccounts", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new Account
                        {
                            AccountID = reader.GetInt32(0),
                            AccountName = reader.GetString(1),
                            ParentAccountID = reader.IsDBNull(2) ? null : reader.GetInt32(2),
                            AccountType = reader.IsDBNull(3) ? null : reader.GetString(3)
                        });
                    }
                }
            }

            return list;
        }

        public void SaveVoucher(Voucher voucher)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                foreach (var entry in voucher.Entries)
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SaveVoucher", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@VoucherType", voucher.VoucherType);
                        cmd.Parameters.AddWithValue("@VoucherDate", voucher.VoucherDate);
                        cmd.Parameters.AddWithValue("@ReferenceNo", voucher.ReferenceNo ?? string.Empty);
                        cmd.Parameters.AddWithValue("@AccountId", entry.AccountId);
                        cmd.Parameters.AddWithValue("@Debit", entry.Debit);
                        cmd.Parameters.AddWithValue("@Credit", entry.Credit);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
