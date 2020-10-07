using Fasserly.Database;
using Fasserly.Database.Entities;
using Fasserly.Database.Interface;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Fasserly.UnitTests
{
    /// <summary>Base class for tests that require the SQL Server engine.</summary>
    [TestClass]
    public abstract class DatabaseTestsBase : TestsBase
    {
        protected static DbContextOptions<DatabaseContext> options;
        protected static UserManager<UserFasserly> _userManager;
        protected static IJwtGenerator _jwtGenerator;
        protected static IMediator _mediator;

        /// <summary>Called at the start of a test run.</summary>
        [AssemblyInitialize]
        public static void AssemblyInitialize(TestContext testContext)
        {

            // Create a LocalDB database for each test run
            var databaseName = Guid.NewGuid().ToString();
            var connectionString = $"Server=(localdb)\\MSSQLLocalDB;Database={databaseName};Trusted_Connection=True;MultipleActiveResultSets=False";
            options = new DbContextOptionsBuilder<DatabaseContext>().UseSqlServer(connectionString).Options;
            using (var context = new DatabaseContext(options))
                context.Database.Migrate();
        }

        /// <summary>Called at the end of a test run.</summary>
        [AssemblyCleanup]
        public static void AssemblyCleanup()
        {
            // Delete the LocalDB database
            using (var context = new DatabaseContext(options))
                context.Database.EnsureDeleted();
        }
    }
}
