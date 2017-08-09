using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PosWeb.Models;

namespace PosWeb.Repositories.Infrastructure
{
    public class PosWebDbContext : AbstractDbContext
    {
		public PosWebDbContext(DbContextOptions options)
            : base(options) { }

        public DbSet<TestModel> TestModels
        {
            get;
            set;
        }
 
        protected override string GetCurrentUserId()
        {
			try
			{
				return "1";
			}
			catch (Exception)
			{
				return "Anonymous";
			}
        }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			EntityMappingConfig.CreateMappings(modelBuilder, GetModelMappingAssemblyNames());
			modelBuilder.HasDefaultSchema("PosWeb");
			base.OnModelCreating(modelBuilder);
		}

		protected override List<AssemblyName> GetModelMappingAssemblyNames()
		{
			var assemblyName = new AssemblyName("PosWeb.Models");
			return new List<AssemblyName> { assemblyName };
		}
    }
}
