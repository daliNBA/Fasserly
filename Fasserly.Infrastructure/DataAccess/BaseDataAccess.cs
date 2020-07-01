using Fasserly.Database;
using Microsoft.EntityFrameworkCore;
using System;

namespace Fasserly.Infrastructure.DataAccess
{
    public class BaseDataAccess : IDisposable
    {
        protected readonly DatabaseContext context;

        public BaseDataAccess(DbContextOptions<DatabaseContext> databaseOptions)
        {
            context = new DatabaseContext(databaseOptions);
        }

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                    context.Dispose();
                disposedValue = true;
            }
        }

        void IDisposable.Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
