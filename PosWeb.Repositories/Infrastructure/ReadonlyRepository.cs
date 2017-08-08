using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PosWeb.Models.Infratructure;

namespace PosWeb.Repositories.Infrastructure
{
	public class ReadonlyRepository<T> : IReadonlyRepository<T>
		where T : Entity, new()
	{
        protected PosWebDbContext DbContext;
		protected readonly DbSet<T> DbSet;

        public ReadonlyRepository(PosWebDbContext dbContext)
		{
			DbContext = dbContext;
			DbSet = dbContext.Set<T>();
		}

		public IEnumerable<T> GetAll()
		{
			IQueryable<T> items = DbSet;
			return items.AsEnumerable();
		}

		public IEnumerable<T> GetAll(List<Expression<Func<T, object>>> includes)
		{
			IQueryable<T> query = DbSet;
			query = GetIncluded(query, includes);
			return query.AsEnumerable();
		}

		//public IEnumerable<T> GetAllOrdered<TKey>(PageInfo pageInfo)
		//{
		//	IQueryable<T> query = DbSet;
		//	query = GetPaged<TKey>(query, pageInfo);
		//	return query.AsEnumerable();
		//}

		//public IEnumerable<T> GetAllOrdered<TKey>(List<Expression<Func<T, object>>> includes, PageInfo pageInfo)
		//{
		//	IQueryable<T> query = DbSet;
		//	query = GetIncluded(query, includes);
		//	query = GetPaged<TKey>(query, pageInfo);
		//	return query.AsEnumerable();
		//}

		public int GetItemsCount(Expression<Func<T, bool>> predicate)
		{
			IQueryable<T> items = DbSet;
			if (predicate != null)
				items = items.Where(predicate);
			return items.Count();
		}

		public T GetById(int id, List<Expression<Func<T, object>>> includes = null)
		{
			var query = DbSet.Where(c => c.Id == id);

			if (includes != null)
				query = GetIncluded(query, includes);

			var entity = query.FirstOrDefault();

			if (entity == null)
				throw new Exception($"Obiekt typu {typeof(T).Name} id: {id} nie został znaleziony!");
			return entity;
		}

		public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate)
		{
			var query = DbSet.Where(predicate);
			return query.AsEnumerable();
		}

		public IEnumerable<T> FindBy(Expression<Func<T, bool>> predicate,
			List<Expression<Func<T, object>>> includes)
		{
			var query = DbSet.Where(predicate);
			query = GetIncluded(query, includes);
			return query.AsEnumerable();
		}

		//public IEnumerable<T> FindByOrdered<TKey>(Expression<Func<T, bool>> predicate, PageInfo pageInfo)
		//{
		//	var query = DbSet.Where(predicate);
		//	query = GetPaged<TKey>(query, pageInfo);
		//	return query.AsEnumerable();
		//}

		//public IEnumerable<T> FindByOrdered<TKey>(Expression<Func<T, bool>> predicate,
		//	List<Expression<Func<T, object>>> includes, PageInfo pageInfo)
		//{
		//	var query = DbSet.Where(predicate);
		//	query = GetIncluded(query, includes);
		//	query = GetPaged<TKey>(query, pageInfo);
		//	return query.AsEnumerable();
		//}

		private IQueryable<T> GetIncluded(IQueryable<T> query, List<Expression<Func<T, object>>> includes)
		{
			if (includes == null) return query;
			foreach (var include in includes)
				query = query.Include(include);
			return query;
		}

		//private IQueryable<T> GetPaged<TKey>(IQueryable<T> query, PageInfo pageInfo)
		//{
		//	if (pageInfo == null)
		//		return query;
		//	var prop = string.IsNullOrEmpty(pageInfo.OrderByPropertyName) ?
		//		null : TypeDescriptor.GetConverter(typeof(T));
		//	if (prop != null)
		//	{
		//		query = GetOrderedQuery<TKey>(query, pageInfo)
		//			.Skip(pageInfo.CurrentPageNumber * pageInfo.ItemsPerPage).Take(pageInfo.ItemsPerPage);
		//	}
		//	else
		//		query = query.OrderBy(e => e.Id).Skip(pageInfo.CurrentPageNumber * pageInfo.ItemsPerPage).Take(pageInfo.ItemsPerPage);
		//	return query;
		//}

		//private IOrderedQueryable<T> GetOrderedQuery<TKey>(IQueryable<T> query, PageInfo pageInfo)
		//{
		//	return pageInfo.IsDescending ?
		//		query.OrderByDescending(ToLambda<TKey>(pageInfo.OrderByPropertyName)) :
		//		query.OrderBy(ToLambda<TKey>(pageInfo.OrderByPropertyName));
		//}

		private static Expression<Func<T, TKey>> ToLambda<TKey>(string propertyName)
		{
			var parameter = Expression.Parameter(typeof(T));
			var property = Expression.Property(parameter, propertyName);

			return Expression.Lambda<Func<T, TKey>>(property, parameter);
		}
	}
}
