using System;
using System.Collections.Generic;
using AutoLotDal.BulkImport;
using AutoLotDal.DataOperations;
using AutoLotDal.Models;

namespace AutoLotClient
{
    internal class Program
    {
        public static void Main(string[] args)
        {
           GetAll();
           //       MoveCustomer();
     
           Console.ReadLine();   
     
      //     DoBulkCopy();

            Console.ReadLine();
        }

        public static void GetAll()
        {
            InventoryDal dal = new InventoryDal();
            var list = dal.GetAllInventory();

            Console.WriteLine("*************All cars***********");
            Console.WriteLine("CarId\tMake\tColor\tPetName");
            foreach (var car in list)
            {
                Console.WriteLine($"{car.Id}\t{car.Make}\t{car.Color}\t{car.PetName}");
            }
        }
        public static void MoveCustomer()
        {
            Console.WriteLine("Simple Transaction Example..");
            bool throwEx = true;
            
            Console.Write("Do you want to throw an exception?(Y or N): ");
            var userAnswer = Console.ReadLine();

            if (userAnswer?.ToLower() == "n")
                throwEx = false;
            
            var dal = new InventoryDal();
            dal.ProcessCreditRisk(throwEx,1);
        }

        public static void DoBulkCopy()
        {
            Console.WriteLine("----------Do Bulk Copy-----------");
            var cars = new List<Car>
            {
                new Car(){Color = "Blue",Make="Honda",PetName = "MyCar"},
                new Car(){Color = "Red",Make="Volvo",PetName = "MyCar2"}
            };
            ProcessBulkImport.ExecuteBulkImport(cars,"Inventory");
            InventoryDal dal = new InventoryDal();
            var list = dal.GetAllInventory();
            
            Console.WriteLine("*************All cars***********");
            Console.WriteLine("CarId\tMake\tColor\tPetName");
            foreach (var car in list)
            {
                Console.WriteLine($"{car.Id}\t{car.Make}\t{car.Color}\t{car.PetName}");
            }
            
        }
    }
}