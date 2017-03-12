using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Fancy.Data.Repositories.Contracts
{
    public interface IRepository<T> where T : class
    {
        IQueryable<T> QueryAll();

        IEnumerable<T> All();

        ObservableCollection<T> Local { get; }

        IEnumerable<T> Search(Expression<Func<T, bool>> condition);

        T GetById(int id);

        T FirstOrDefault(Expression<Func<T, bool>> condition);

        bool Any();

        void Add(T entity);

        void Delete(T entity);

        void Delete(int id);
    }
}
