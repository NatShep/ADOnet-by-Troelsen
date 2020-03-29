using System;
using System.Data;
using  System.Data.SqlClient;

namespace AutoLotDal.DataOperations
{
    public class InventoryDal
    {
        private readonly string _connectionString;
        private SqlConnection _sqlConnection = null;

        public InventoryDal() :
            this(@"СТРОКА ПОДКЛЮЧЕНИЯ")
        {
        }

        public InventoryDal(string connectionString)
        {
            _connectionString = connectionString;
        }

        private void OpenConnection()
        {
            _sqlConnection=new SqlConnection(_connectionString);
            _sqlConnection.Open();
        }

        private void CloseConnection() => _sqlConnection.Close();
    }
}