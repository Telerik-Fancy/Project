﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.Entity;
using Fancy.Data.Contexts;
using Fancy.Common.Validator;

namespace Fancy.Data.Repositories
{
    public class EfGenericRepository<T> : IEfGenericRepository<T> where T : class
    {
        private IFancyDbContext context;
        private IDbSet<T> dbSet;

        public EfGenericRepository(IFancyDbContext context)
        {
            Validator.ValidateNullArgument(context, "context");

            this.context = context;
            this.dbSet = this.context.Set<T>();

        }
        public IFancyDbContext Context
        {
            get { return this.context; }
        }

        protected IDbSet<T> DbSet
        {
            get  { return this.dbSet; }
        }

        public IQueryable<T> All
        {
            get { return this.dbSet; }
        }

        public T GetById(object id)
        {
            return this.dbSet.Find(id);
        }

        public T GetSingle(Expression<Func<T, bool>> filterExpression)
        {
            return this.All.Single(filterExpression);
        }

        public T GetSingleOrDefault(Expression<Func<T, bool>> filterExpression)
        {
            return this.All.SingleOrDefault(filterExpression);
        }

        public T GetFirst(Expression<Func<T, bool>> filterExpression)
        {
            return this.All.First(filterExpression);
        }

        public T GetFirstOrDefault(Expression<Func<T, bool>> filterExpression)
        {
            return this.All.FirstOrDefault(filterExpression);
        }

        public IEnumerable<T> GetAll()
        {
            return this.GetAll(null);
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> filterExpression)
        {
            return this.GetAll<object>(filterExpression, null);
        }

        public IEnumerable<T> GetAll<T1>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, T1>> sortExpression)
        {
            return this.GetAll<T1, T>(filterExpression, sortExpression, null);
        }

        public IEnumerable<T2> GetAll<T1, T2>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, T1>> sortExpression, Expression<Func<T, T2>> selectExpression)
        {
            IQueryable<T> result = this.DbSet;

            if (filterExpression != null)
            {
                result = result.Where(filterExpression);
            }

            if (sortExpression != null)
            {
                result = result.OrderBy(sortExpression);
            }

            if (selectExpression != null)
            {
                return result.Select(selectExpression).ToList();
            }
            else
            {
                return result.OfType<T2>().ToList();
            }
        }

        public void Add(T entity)
        {
            Validator.ValidateNullArgument(entity, "Entity");

            this.DbSet.Add(entity);
        }

        public void Delete(T entity)
        {
            Validator.ValidateNullArgument(entity, "Entity");

            this.DbSet.Remove(entity);
        }
    }
}
