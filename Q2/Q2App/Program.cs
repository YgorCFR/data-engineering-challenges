using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Q2App
{
    public class Program
    {
        public static void Main(string[] args)
        {

            string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Q2Db;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=False";

            string root = "..\\..\\..\\..\\Question2-Material";
            string mainFolder = "TestFiles";

            cleanUpDirectories(root, mainFolder);

            using (SqlConnection connection = new SqlConnection(
               connectionString))
            {
                // DELETING EXISTING TABLE SalesRecords
                SqlCommand deleteTable = new SqlCommand(@"
                    IF OBJECT_ID (N'dbo.SalesRecord', N'U') IS NOT NULL
                      DROP TABLE [dbo].[SalesRecord];", connection);
                deleteTable.Connection.Open();
                deleteTable.ExecuteNonQuery();
                deleteTable.Connection.Close();
                // CREATING TABLE Q2Tbl
                SqlCommand createTable = new SqlCommand(@"
                    CREATE TABLE dbo.SalesRecord (
		                    Region VARCHAR(255),
		                    Country VARCHAR(100),
		                    ItemType VARCHAR(100),
		                    SalesChannel VARCHAR(100),
		                    OrderPriority CHAR(1),
		                    OrderDate DATE,
		                    OrderID INT,
		                    ShipDate DATE,
		                    UnitsSold INT,
		                    UnitPrice FLOAT,
		                    UnitCost FLOAT,
		                    TotalRevenue FLOAT,
		                    TotalCost FLOAT,
		                    TotalProfit FLOAT
                    );
                ", connection);
                createTable.Connection.Open();
                createTable.ExecuteNonQuery();
                createTable.Connection.Close();

                
                string result = "";

                string[] fileEntries = Directory.GetFiles($"{root}\\{mainFolder}");
                foreach (string fileName in fileEntries)
                {
                    Console.WriteLine($"Treating file {fileName} ...");
                    var config = new CsvConfiguration(CultureInfo.GetCultureInfo("en-US"))
                    {
                        PrepareHeaderForMatch = header => Regex.Replace(header.Header, @"\s", string.Empty),
                        Delimiter = ","

                    };
                    using var streamReader = File.OpenText(fileName);
                    using var csvReader = new CsvReader(streamReader, config);
                    var salesRecord = csvReader.GetRecords<SalesRecord>().ToList();
                    SqlCommand queryInsert = new SqlCommand(@"",connection);
                    queryInsert.Connection.Open();
                    if (salesRecord.Any())
                    {
                        foreach (var sale in salesRecord)
                        {
                            List<string> values = new List<string>();
                            var props = sale.GetType().GetProperties();
                            foreach (PropertyInfo prop in props)
                            {
                                try
                                {


                                    object propValue = prop.GetValue(sale, null);
                                    if (propValue is string)
                                    {
                                        propValue = propValue.ToString().Replace("'", " ");
                                        propValue = "'" + propValue + "'";
                                    }
                                    if (propValue is DateTime)
                                    {
                                        propValue = "CONVERT(DATE,'" + propValue.ToString() + "',103)";
                                    }
                                    values.Add(propValue.ToString().Replace(",", ".").Replace("DATE.", "DATE,").Replace(".103", ",103"));
                                }
                                catch (Exception)
                                {
                                    result = "ERROR";
                                    moveToDirectory(Path.GetFileName(fileName), root, $"{root}\\{mainFolder}", result);
                                }
                            }
                            string stringValues = String.Join(",", values);
                            var qry = $@"INSERT INTO dbo.SalesRecord(Region,Country,ItemType,SalesChannel,OrderPriority,OrderDate,OrderID,ShipDate,UnitsSold,UnitPrice,UnitCost,TotalRevenue,TotalCost,TotalProfit) VALUES ({stringValues})";
                            queryInsert = new SqlCommand($@"INSERT INTO dbo.SalesRecord(Region,Country,ItemType,SalesChannel,OrderPriority,OrderDate,OrderID,ShipDate,UnitsSold,UnitPrice,UnitCost,TotalRevenue,TotalCost,TotalProfit) 
                                   VALUES (
                                     {stringValues})", connection);
                            queryInsert.ExecuteNonQuery();
                        }
                        result = "PROCESSED";
                        
                    } 
                    else
                    {
                        result = "ERROR";
                    }
                    queryInsert.Connection.Close();
                    moveToDirectory(Path.GetFileName(fileName), root, $"{root}\\{mainFolder}", result);
                }
            }

            cleanUpCurrentDir($"{root}\\{mainFolder}");

        }

        public static void moveToDirectory(string fileName, string root, string currentDirPath, string result)
        {
            string newDirectory = $"{root}\\{result}";
            if (!Directory.Exists(newDirectory))
            {
                Directory.CreateDirectory(newDirectory);
            }

            var file = new DirectoryInfo(currentDirPath).GetFiles().Where(x => x.Name == fileName).FirstOrDefault();

            file.CopyTo($@"{root}\\{result}\\{file.Name}");
        }

        public static void cleanUpDirectories(string root, string currentDir)
        {
            string[] runTimeDirs = { "PROCESSED", "ERROR" };
            foreach (string runTimeDir in runTimeDirs)
            {
                string runTimePath = $"{root}\\{runTimeDir}";
                if (Directory.Exists(runTimePath))
                {
                    foreach (var file in new DirectoryInfo(runTimePath).GetFiles())
                    {
                        file.CopyTo($@"{root}\\{currentDir}\\{file.Name}");
                    }

                    Directory.Delete($@"{root}\\{runTimeDir}", true);
                }
            }

        }

        public static void cleanUpCurrentDir(string currentDirPath)
        {
            foreach (var file in new DirectoryInfo(currentDirPath).GetFiles())
            {
                file.Delete();
            }
        }
    }
}
