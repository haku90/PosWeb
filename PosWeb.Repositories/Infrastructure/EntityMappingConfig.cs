using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using PosWeb.Models.Infratructure;

namespace PosWeb.Repositories.Infrastructure
{
	public class EntityMappingConfig
	{
		public static void CreateMappings(ModelBuilder modelBuilder, List<AssemblyName> assemblyNames)
		{
			foreach (var name in assemblyNames)
			{
				var assemblyTypes = Assembly.Load(name).DefinedTypes;
				var entityTypes = assemblyTypes.
					Where(t => t.GetCustomAttributes<EntityMapping>().ToList().Count > 0).ToList();
				foreach (var entityType in entityTypes)
				{

					entityType.GetDeclaredMethod("CreateEntityMapping").
						Invoke(null, new object[] { modelBuilder });
				}
			}
		}
	}
}
