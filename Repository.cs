using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace Utility
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DbContext context;
        private DbSet<T> set;
        public Repository(DbContext context)
        {
            this.context = context;
            set = context.Set<T>();
        }

        public void Create(T entity)
        {
            set.Add(entity);
            context.SaveChanges();
        }

        public void Delete(object entityKey)
        {
            T entity = ReadOne(entityKey);
            set.Remove(entity);
            context.SaveChanges();
        }

        public void Delete(T t)
        {
            set.Remove(t);
            context.SaveChanges();
        }

        public bool Exists(object entityKey)
        {
            return set.Find(entityKey) != null;
        }

        public bool Exists(T t)
        {
            return set.Contains(t);
        }

        public IQueryable<T> ReadMany(Expression<Func<T, bool>> expression = null)
        {
            if (expression == null)
            {
                return set;
            }
            else
            {
                return set.Where(expression);
            }
        }

        public T ReadOne(object entityKey)
        {
            return set.Find(entityKey);
        }

        public void Update(T entity)
        {
            context.Entry<T>(entity).State = EntityState.Modified;
            context.SaveChanges();
        }
    }
}
