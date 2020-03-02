using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Microsoft.Azure.Cosmos.Table;
using System.Threading.Tasks;

namespace processcsvdata
{
    public class DBConnection
    {
        public string StorageConnectionString { get; set; }
        public static string LoadConnectionDetails()
        {
            return Environment.GetEnvironmentVariable("StorageConnectionString");
        }

        public static CloudStorageAccount RetieveStorageAccount(string storageConnectionString)
        {
            CloudStorageAccount storageAccount;
            try
            {
                storageAccount = CloudStorageAccount.Parse(storageConnectionString);
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid storage account information provided");
                throw;
            }
            catch (ArgumentException)
            {
                Console.WriteLine("Please check the storage account details");
                Console.ReadLine();
                throw;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return storageAccount;
        }

        public static async Task<CloudTable> CreateTableAsync(string tableName)
        {
            CloudTable table;
            string storageConnectionString = LoadConnectionDetails();

            CloudStorageAccount storageAccount = RetieveStorageAccount(storageConnectionString);

            CloudTableClient tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());

            Console.WriteLine("Table client is created");

            try
            {
                table = tableClient.GetTableReference(tableName);
                if (await table.CreateIfNotExistsAsync())
                {
                    Console.WriteLine("Table is created - {0}", tableName);
                }
                else
                {
                    Console.WriteLine("Table {0} already exists", tableName);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            Console.WriteLine();
            return table;
        }
    }
}
