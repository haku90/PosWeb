using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PosWeb.Models.Infratructure;

namespace PosWeb.Repositories.Infrastructure
{
    public abstract class AbstractDbContext : DbContext
    {
		protected AbstractDbContext(DbContextOptions options) : base(options) { }
		protected abstract List<AssemblyName> GetModelMappingAssemblyNames();
		protected abstract string GetCurrentUserId();

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			EntityMappingConfig.CreateMappings(modelBuilder, GetModelMappingAssemblyNames());
			base.OnModelCreating(modelBuilder);
		}

		public override int SaveChanges()
		{
			var currentUserId = GetCurrentUserId();
			var modifiedEntries = ChangeTracker.Entries().Where(e => e.Entity is ITrackedEntity
				&& (e.State == EntityState.Added || e.State == EntityState.Modified));
			foreach (var entry in modifiedEntries)
			{
				var entity = entry.Entity as ITrackedEntity;
				if (entity == null)
					continue;
				var operationTime = DateTime.Now;
				if (entry.State == EntityState.Added)
				{
					entity.CreatedBy = currentUserId;
					entity.CreatedDate = operationTime;
				}
				else
				{
					Entry(entity).Property(e => e.CreatedBy).IsModified = false;
					Entry(entity).Property(e => e.CreatedDate).IsModified = false;
				}
				entity.UpdatedBy = currentUserId;
				entity.UpdatedDate = operationTime;
			}

			return base.SaveChanges();
		}
    }
}
