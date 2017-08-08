using System;
namespace PosWeb.Repositories.Infrastructure
{
	public interface IUnitOfWork : IDisposable
	{
		int Commit();
	}
}
