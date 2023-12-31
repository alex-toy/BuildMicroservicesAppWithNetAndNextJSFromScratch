﻿using MongoDB.Driver;
using MongoDB.Entities;
using Polly;

namespace BiddingService
{
    public static class WebApplicationHelper
    {
        public static async void ConfigureMongoDb(this WebApplication builder)
        {
            string connectionString = builder.Configuration.GetConnectionString("BidDbConnection");

            await Policy.Handle<TimeoutException>()
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(10))
                .ExecuteAndCaptureAsync(async () =>
                {
                    await DB.InitAsync("BidDb", MongoClientSettings.FromConnectionString(connectionString));
                });
        }
    }
}
