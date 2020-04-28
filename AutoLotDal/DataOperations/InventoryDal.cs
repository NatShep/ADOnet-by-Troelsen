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
            this(@"Server=desktop-l8k07nf\sqlexpress;Initial Catalog=AutoLot;User Id=sa;Password=123")
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


        private Car ProjectionFromDataReaderToCar(SqlDataReader dataReader)
        {
            return new Car
            {
                Id = (int) dataReader["Id"],
                Color = (string) dataReader["Color"],
                Make = (string) dataReader["Make"],
                PetName = (string) dataReader["PetName"],
            };
        }

        public List<Car> GetAllInventory()
        {
            OpenConnection();  
            List<Car> inventory = new List<Car>();
            string sql = "Select * from Inventory";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                // commandBehavior.CloseConnection == autoClosing Connection
                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (dataReader.Read())
                {
                    inventory.Add(ProjectionFromDataReaderToCar(dataReader));
                }
                dataReader.Close();
            }

            return inventory;
        }

        public Car GetCar(int id)
        {
            OpenConnection();  
            Car car = new Car();
            string sql = $"Select * From Inventory where CarId = {id}";
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                // commandBehavior.CloseConnection == autoClosing Connection
                SqlDataReader dataReader = command.ExecuteReader(CommandBehavior.CloseConnection);
                while (dataReader.Read())
                    car = ProjectionFromDataReaderToCar(dataReader);
                dataReader.Close();
            }
            return car;
        }

        public void InsertAuto(string color, string make, string petName)
        {
            OpenConnection(); 
            string sql = $"Insert Into Inventory (Make,Color,PetName) Values ('{make}','{color}','{petName}'";

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.CommandType = CommandType.Text;
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }
        
        // using parameters
        public void InsertAuto(Car car)
        {
            OpenConnection(); 
            string sql = $"Insert Into Inventory (Make,Color,PetName) Values (@Make,@Color,@PetName)";

            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@Make",
                    Value = car.Make,
                    SqlDbType = SqlDbType.Char,
                    Size = 10
                };
                command.Parameters.Add(parameter);
                
                parameter = new SqlParameter()
                {
                    ParameterName = "@Color",
                    Value = car.Color,
                    SqlDbType = SqlDbType.Char,
                    Size = 10
                };
                command.Parameters.Add(parameter);
                
                parameter = new SqlParameter()
                {
                    ParameterName = "@PetName",
                    Value = car.PetName,
                    SqlDbType = SqlDbType.Char,
                    Size = 10
                };
                command.Parameters.Add(parameter);
                
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        public void DeleteCar(int id)
        {
            OpenConnection();
            string sql = $"Delete From Inventory where CarId='{id}'";
            using (SqlCommand command = new SqlCommand(sql,_sqlConnection))
            {
                try
                {
                    command.CommandType = CommandType.Text;
                    command.ExecuteNonQuery();
                }   
                catch (SqlException e)
                {
                    Exception error = new Exception("Sorry! That car is on erder!",e);
                    throw error;
                }
            }
            CloseConnection();
        }

        public void UpdateCarPetName(int id, string newPetName)
        {
            OpenConnection();
            string sql = $"Update Inventory Set PetName = '{newPetName}' " +
                         $"Where CarId = '{id}'";
            using (SqlCommand command =new SqlCommand(sql,_sqlConnection))
            {
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }
        
        //using stored procedure
        public string LookUpPetName(int carId)
        {
            OpenConnection();
            string carPetName;

            using (SqlCommand command = new SqlCommand("GetPetName", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
                SqlParameter param = new SqlParameter()
                {
                    ParameterName = "@carId",
                    SqlDbType = SqlDbType.Int,
                    Value = carId,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(param);

                param = new SqlParameter()
                {
                    ParameterName = "@PetName",
                    SqlDbType = SqlDbType.Char,
                    Size = 10,
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(param);

                command.ExecuteNonQuery();
                carPetName = (string) command.Parameters["@petName"].Value;
            }

            return carPetName;
        }

        public void ProcessCreditRisk(bool throwEx, int custId)
        {
            OpenConnection();
            string fName;
            string lName;
            var cmdSelect = new SqlCommand($"Select * from Customers Where CustId={custId}",_sqlConnection);
            using (var datareader = cmdSelect.ExecuteReader())
            {
                if (datareader.HasRows)
                {
                    datareader.Read();
                    fName = (string) datareader["FirstName"];
                    lName = (string) datareader["LastName"];             
                }
                else
                {
                    CloseConnection();
                    Console.WriteLine("There is no customer with this ID");
                    return;
                }
                datareader.Close();

                
                var cmdRemove = new SqlCommand($"Delete from Customers where CustId={custId}",_sqlConnection);
                var cmdInsert = new SqlCommand($"Insert Into CreditRisks (FirstName,LastName) Values('{fName}','{lName}')",_sqlConnection);

                SqlTransaction tx = null;
                try
                {
                    tx = _sqlConnection.BeginTransaction();

                    cmdInsert.Transaction = tx;
                    cmdRemove.Transaction = tx;

                    cmdInsert.ExecuteNonQuery();
                    cmdRemove.ExecuteNonQuery();

                    if (throwEx)
                        throw new Exception("DatabaseError! Tx failed...");
                    tx.Commit();
                    Console.WriteLine();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    tx?.Rollback();
                }
                finally{CloseConnection();}
            }
        }
        
    }
}