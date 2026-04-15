using Azure.Storage.Queues.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using TangyAzureFunc1.Data;
using TangyAzureFunc1.Models;

namespace TangyAzureFunc1;

public class OnQueueTriggerUpdateDatabase
{
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<OnQueueTriggerUpdateDatabase> _logger;


    public OnQueueTriggerUpdateDatabase(ILogger<OnQueueTriggerUpdateDatabase> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _dbContext = dbContext;
    }

    [Function(nameof(OnQueueTriggerUpdateDatabase))]
    public void Run([QueueTrigger("SalesRequestOutBound")] QueueMessage message)
    {
        string messageBody = message.Body.ToString();
        SalesRequest? salesRequest = JsonConvert.DeserializeObject<SalesRequest>(messageBody);

        if (salesRequest != null)
        {
            salesRequest.Status = "";
            _dbContext.SalesRequests.Add(salesRequest);
            _dbContext.SaveChanges();
        }
        else
        {
            _logger.LogWarning("Failed to deserialize the message body into a SalesRequest object.");
        }
    }
    //[Function(nameof(OnQueueTriggerUpdateDatabase))]
    //public async Task Run([QueueTrigger("SalesRequestOutBound", Connection = "AzureWebJobsStorage")] string messageBody)
    //{
    //    var salesRequest = JsonConvert.DeserializeObject<SalesRequest>(messageBody);

    //    if (salesRequest == null)
    //    {
    //        _logger.LogWarning("Failed to deserialize the message body into a SalesRequest object.");
    //        return;
    //    }

    //    try
    //    {
    //        salesRequest.Status = string.Empty;
    //        _dbContext.SalesRequests.Add(salesRequest);
    //        await _dbContext.SaveChangesAsync();
    //        _logger.LogInformation("SalesRequest saved to database (Id: {Id}).", salesRequest.Id);
    //        // Function completed successfully -> Functions runtime will delete the message from the queue.
    //    }
    //    catch (Exception ex)
    //    {
    //        // Rethrow so the Functions runtime treats this as a failed execution and keeps the message for retry.
    //        _logger.LogError(ex, "Failed to save SalesRequest. Letting the function fail so the message remains in the queue for retry.");
    //        throw;
    //    }
    //}
}