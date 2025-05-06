
using Microsoft.EntityFrameworkCore;

namespace Minutas.Repositories
{
    public class Repository<M> where M : class
    {
        private readonly DbContext context;

        public Repository(DbContext context)
        {
            this.context = context;
        }

        public void Insert(M entity)
        {
            context.Add(entity);
            context.SaveChanges();
        }

        public IEnumerable<M> GetAll()
        {
            return context.Set<M>().ToList();
        }
        public M? Get(object id)
        {
            return context.Find<M>(id);
        }

        public void Update(M entity)
        {
            context.Update(entity);
            context.SaveChanges();
        }

        public void Delete(M entity)
        {
            context.Remove(entity);
            context.SaveChanges();
        }
    }
}
