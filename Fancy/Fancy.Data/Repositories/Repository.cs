using Fancy.Data.Contexts.Contracts;
using Fancy.Data.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Fancy.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        public Repository(IFancyDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentException("An instance of DbContext is required to use this repository.", "context");
            }

            this.Context = context;
            this.DbSet = this.Context.Set<T>();

        }
        protected IDbSet<T> DbSet { get; set; }

        protected IFancyDbContext Context { get; set; }

        public ObservableCollection<T> Local
        {
            get
            {
                return this.DbSet.Local;
            }
        }

        public IEnumerable<T> All()
        {
            return this.DbSet.ToList();
        }

        public IEnumerable<T> Search(Expression<Func<T, bool>> condition)
        {
            return this.DbSet.Where(condition).ToList();
        }

        public T GetById(int id)
        {
            return this.DbSet.Find(id);
        }

        public void Add(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Added);
        }

        public void Delete(T entity)
        {
            this.ChangeEntityState(entity, EntityState.Deleted);
        }

        public void Delete(int id)
        {
            var entity = this.GetById(id);

            if (entity != null)
            {
                this.Delete(entity);
            }
        }

        private void ChangeEntityState(T entity, EntityState newEntityState)
        {
            var entry = this.Context.Entry(entity);
            entry.State = newEntityState;
        }

        public T FirstOrDefault(Expression<Func<T, bool>> condition)
        {
            return this.DbSet.FirstOrDefault(condition);
        }

        public bool Any()
        {
            return this.DbSet.Any();
        }

        public IQueryable<T> QueryAll()
        {
            return this.DbSet;
        }
    }
}
