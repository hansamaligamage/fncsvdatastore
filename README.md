# Azure function to store csv data in Cosmos DB

This is a http trigger function written in C# - .NET Core 3.1. It processed a csv file and store the data on Cosmos DB

## Technology stack  
* .NET Core 3.1 on Visual Studio 2019
* Azure Cosmos DB table API

## Code snippets
### Retrieve the database storage account
`
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
`
### Create a table in Cosmos DB
`
public static string LoadConnectionDetails()
{
    return Environment.GetEnvironmentVariable("StorageConnectionString");
}
`


` 
public static async Task<CloudTable> CreateTableAsync(string tableName)
{
    CloudTable table;
    string storageConnectionString = LoadConnectionDetails();
}
 `
