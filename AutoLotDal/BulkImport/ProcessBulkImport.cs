using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace AutoLotDal.BulkImport
{
    public static class ProcessBulkImport
    {
        private static SqlConnection _sqlConnection = null;

        private static readonly string _connectionString =
            @"Server=desktop-l8k07nf\sqlexpress;Initial Catalog=AutoLot;User Id=sa;Password=123";
        
        private static void OpenConnection()
        {
            _sqlConnection=new SqlConnection(_connectionString);
            _sqlConnection.Open();
        }

        private static void CloseConnection() => _sqlConnection.Close();
        public static void ExecuteBulkImport<T>(IEnumerable<T> records, string tableName)
        {
            OpenConnection();
            using (SqlConnection conn = _sqlConnection)
            {
                SqlBulkCopy bc = new SqlBulkCopy(conn)
                {
                    DestinationTableName = tableName
                };
                var dataReader = new MyDataReader<T>(records.ToList());
                try
                {
                    bc.WriteToServer(dataReader);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                finally{CloseConnection();}
            }
        }

    }
}