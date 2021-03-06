﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fancy.Data.Repositories
{
    public interface IEfGenericRepository<T> where T : class
    {
        T GetById(object id);

        IQueryable<T> All { get; }

        T GetSingle(Expression<Func<T, bool>> filterExpression);

        T GetSingleOrDefault(Expression<Func<T, bool>> filterExpression);

        T GetFirst(Expression<Func<T, bool>> filterExpression);

        T GetFirstOrDefault(Expression<Func<T, bool>> filterExpression);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(Expression<Func<T, bool>> filterExpression);

        IEnumerable<T> GetAll<T1>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, T1>> sortExpression);

        IEnumerable<T2> GetAll<T1, T2>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, T1>> sortExpression, Expression<Func<T, T2>> selectExpression);

        void Add(T entity);

        void Delete(T entity);
    }
}
