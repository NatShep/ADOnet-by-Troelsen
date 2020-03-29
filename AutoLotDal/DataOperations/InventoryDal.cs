using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using AutoLotDal.Models;

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

        public List<Car> GetAllInventory()
        {
            OpenConnection();
            List<Car> inventory = new List<Car>();
            string sql = "Select * from Inventory";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (dataReader.Read())
                {
                    inventory.Add(new Car
                    {
                        CarId = (int)dataReader["CarId"],
                        Color= (string)dataReader["Color"],
                        Make = (string)dataReader["Make"],
                        PetName = (string)dataReader["PetName"]
                    });
                }
                dataReader.Close();
            }

            return inventory;
        }
    }
}