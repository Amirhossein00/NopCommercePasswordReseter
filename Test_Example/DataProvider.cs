using System;
using System.Data.SqlClient;

namespace Test_Example
{
    public class DataProvider
    {
        #region Fields

        private SqlConnection _sqlConnection;
        private SqlCommand _sqlCommand;
        private string DatabaseName;
        private string SqlServerName;
        private string UserId;
        private string Password;
        #endregion

        #region Ctor
        public DataProvider(string sqlServerName, string databaseName, string userId = null, string password = null)
        {
            this.DatabaseName = databaseName;
            this.SqlServerName = sqlServerName;
            this.UserId = userId;
            this.Password = password;
        }

        public DataProvider(string sqlServerName, string databaseName)
        {
            this.DatabaseName = databaseName;
            this.SqlServerName = sqlServerName;
        }
        #endregion

        public void UpdateAdminPassword(string password, string saltKey, out string errorMessage)
        {
            try
            {
                if (!string.IsNullOrWhiteSpace(UserId) && !string.IsNullOrWhiteSpace(Password))
                {
                    _sqlConnection = new SqlConnection($"Data Source={SqlServerName}; Initial Catalog={DatabaseName}; User Id={UserId}; Password={Password};");
                    _sqlConnection.Open();
                    _sqlCommand = new SqlCommand($"update [dbo].[CustomerPassword] set Password='{password}', PasswordSalt='{saltKey}' where Id=1", _sqlConnection);
                    _sqlCommand.ExecuteNonQuery();
                    _sqlConnection.Close();
                    errorMessage = string.Empty;
                    return;
                }

                _sqlConnection = new SqlConnection($"Data Source={SqlServerName}; Initial Catalog={DatabaseName}; Integrated Security=True;");
                _sqlConnection.Open();
                _sqlCommand = new SqlCommand($"update [dbo].[CustomerPassword] set Password='{password}', PasswordSalt='{saltKey}' where Id=1", _sqlConnection);
                _sqlCommand.ExecuteNonQuery();
                _sqlConnection.Close();
                errorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }
        }

        public void UpdateUserPasswordByUserId(string password, string saltKey, int userId, out string errorMessage)
        {
            try
            {
                _sqlConnection = new SqlConnection($"Data Source={SqlServerName}; Initial Catalog={DatabaseName}; User Id={UserId}; Password={Password};");
                _sqlConnection.Open();
                _sqlCommand = new SqlCommand($"update [dbo].[CustomerPassword] set Password='{password}', PasswordSalt='{saltKey}' where Id={userId}", _sqlConnection);
                _sqlCommand.ExecuteNonQuery();
                _sqlConnection.Close();
                errorMessage = string.Empty;
            }
            catch (Exception ex)
            {
                errorMessage = ex.ToString();
            }
        }
    }
}
