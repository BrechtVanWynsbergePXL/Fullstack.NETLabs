using System;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Polly;
using System.Collections.Generic;
using System.Text;

namespace HumanRelations.Infrastructure
{
    internal class HumanRelationsDbInitializer
    {
        private readonly HumanRelationsContext _context;
        private readonly ILogger<HumanRelationsDbInitializer> _logger;

        public HumanRelationsDbInitializer(HumanRelationsContext context, ILogger<HumanRelationsDbInitializer> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void MigrateDatabase()
        {
            _logger.LogInformation("Migrating database associated with HumanRelationsContext");

            try
            {
                var retry = Policy.Handle<SqlException>().WaitAndRetry(new TimeSpan[] 
                { 
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(8),
                });
                retry.Execute(() => _context.Database.Migrate());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while migrating the database used on HumanRelationsContext");
            }
        }
    }
}
