using System;
using PosWeb.Models;
using PosWeb.Repositories.Infrastructure;

namespace PosWeb.Repositories
{
    public class TestRepository : GenericRepository<TestModel>
    {
        public TestRepository(PosWebDbContext dbContext) : base(dbContext)
        {
        }
    }
}
