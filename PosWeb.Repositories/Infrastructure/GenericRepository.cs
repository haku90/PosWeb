using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PosWeb.Models.Infratructure;

namespace PosWeb.Repositories.Infrastructure
{
	public class GenericRepository<T> : ReadonlyRepository<T>, IGenericRepository<T>
	  where T : TrackedEntity, new()
	{
        public GenericRepository(PosWebDbContext dbContext) : base(dbContext)
		{

		}

		public T Add(T entity, HashSet<string> includedEntities = null)
		{
			RemoveSubEntities(entity, includedEntities);
			var result = DbSet.Add(entity);
			return result.Entity;
		}

		public T Edit(T entity, bool withReflection = true)
		{
			T entityDestination;
			if (!withReflection)
			{
				entityDestination = entity;
			}
			else
			{
				entityDestination = GetById(entity.Id);
				entityDestination = ReflectionMapper<T, T>.Map(entity, entityDestination);
			}

			RemoveSubEntities(entityDestination, null);
			AttachEntity(entityDestination);
			DbContext.Entry(entityDestination).State = EntityState.Modified;
			return entityDestination;
		}

		public void Delete(T entity)
		{
			AttachEntity(entity);
			DbContext.Remove(entity);
		}

		public void Delete(int id)
		{
			var dEntity = new T { Id = id };
			Delete(dEntity);
		}

		private void AttachEntity(T entity)
		{
			// ominiete sprawdzanie czy nie bylo przypadkiem wczesniej zataczowane
			// gdyby cos sie jebalo wrocic do tego i napisac tak jak bylo w DataInterfaces
			DbSet.Attach(entity);
		}

		private void RemoveSubEntities(T entity, HashSet<string> excludedEntities)
		{
			if (excludedEntities == null)
				excludedEntities = new HashSet<string>();

			foreach (var property in typeof(T).GetRuntimeProperties())
			{
				var propertyType = property.PropertyType;
				if (propertyType.GenericTypeArguments.Contains(typeof(IEntity)))
				{
					if (excludedEntities.Contains(property.Name))
						continue;

					property.SetValue(entity, null);
					continue;
				}

				if (!propertyType.IsGenericParameter || propertyType.GetGenericTypeDefinition() != typeof(ICollection<>))
					continue;
				var itemType = propertyType.GenericTypeArguments.First();
				if (!itemType.GenericTypeArguments.Contains(typeof(IEntity)))
					continue;
				if (excludedEntities.Contains(property.Name))
					continue;

				property.SetValue(entity, null);
			}
		}
	}
}
