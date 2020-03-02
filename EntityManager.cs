using Microsoft.Azure.Cosmos.Table;
using System;
using System.Threading.Tasks;

namespace processcsvdata
{
    public class EntityManager
    {
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

        public static async Task<Session> RetrieveEntityAsync(CloudTable table, string partitionKey, string rowKey)
        {
            try
            {
                TableOperation tableOperation = TableOperation.Retrieve<Session>(partitionKey, rowKey);
                TableResult result = await table.ExecuteAsync(tableOperation);
                Session session = result.Result as Session;
                if (session != null)
                {
                    Console.WriteLine("\t{0}\t{1}\t{2}\t{3}", session.PartitionKey, session.RowKey, session.Points, session.IsWeekend);
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
    }
}
