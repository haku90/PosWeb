using System;
namespace PosWeb.Repositories.Infrastructure
{
	public class UnitOfWork : IUnitOfWork
	{
        private PosWebDbContext _dbContext;

        public UnitOfWork(PosWebDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public int Commit()
		{
			return _dbContext.SaveChanges();
		}

		public void Dispose()
		{
			if (_dbContext != null)
			{
				_dbContext.Dispose();
				_dbContext = null;
			}
			GC.SuppressFinalize(this);
		}
	}
}
