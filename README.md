# A HTTP trigger written in .NET Core to process a CSV file and store data into CosmosDB using Table API 

This is a http trigger function written in C# - .NET Core 3.1. It processes a csv file and store the data on Cosmos DB on Table API

## Technology stack  
* .NET Core 3.1 on Visual Studio 2019
* Azure functions v3 and Azure Cosmos DB table API

## How to run the solution
 * You have to create a Cosmos DB account with Table API and go to the Keys section and get the connectionstring to connect to the database
 * Open the solution file in Visual Studio and build the project

## Code snippets
### Retrieve the database storage account
```
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
```
### Create a table in Cosmos DB
```
public static string LoadConnectionDetails()
{
    return Environment.GetEnvironmentVariable("StorageConnectionString");
}
```

```
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
 ```
### Create a row in the Cosmos DB
```
  public static async Task<Session> InsertOrUpdateEntityAsync(CloudTable table, Session session)
        {
            if (session == null)
            {
                throw new ArgumentNullException("Session is empty");
            }
            try
            {
                TableOperation tableOperation = TableOperation.InsertOrMerge(session);

                TableResult result = await table.ExecuteAsync(tableOperation);
                Session newSession = result.Result as Session;

                if (result.RequestCharge.HasValue)
                {
                    Console.WriteLine("Request Charge of the operation: " + result.RequestCharge);
                }

                return newSession;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
```
### Read a row in the Cosmos DB
```
      public static async Task<Session> RetrieveEntityAsync(CloudTable table, string partitionKey, string rowKey)
        {
            try
            {
                TableOperation tableOperation = TableOperation.Retrieve<Session>(partitionKey, rowKey);
                TableResult result = await table.ExecuteAsync(tableOperation);
                Session session = result.Result as Session;
                if (session != null)
                {
                    Console.WriteLine("\t{0}\t{1}\t{2}\t{3}", session.PartitionKey, session.RowKey, 
                        session.Points, session.IsWeekend);
                }

                if (result.RequestCharge.HasValue)
                {
                    Console.WriteLine("Request Charge of Retrieve Operation: " + result.RequestCharge);
                }

                return session;
            }
            catch (StorageException e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
                throw;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
 ```
